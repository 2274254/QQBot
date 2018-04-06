package com.binklac.qqbot.protocol.login;

import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.events.GetQRCodeEvent;
import com.binklac.qqbot.helper.TimeoutCallable;
import com.binklac.qqbot.helper.http.Respond;
import com.binklac.qqbot.helper.http.SimpleHttpSession;
import com.binklac.qqbot.protocol.hash.QQHash;
import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.List;

public class QunLogin {
    private final static Logger logger = LoggerFactory.getLogger(QunLogin.class);
    private final EventManager eventManager;
    private final SimpleHttpSession httpSession;
    private final LoginInfo loginInfo;


    public byte[] getQRCode() {
        String prepareUrl = "https://xui.ptlogin2.qq.com/cgi-bin/xlogin?appid=715030901&daid=73&pt_no_auth=1&s_url=http://qun.qq.com/member.html";
        String url = "https://ssl.ptlogin2.qq.com/ptqrshow?appid=715030901&e=2&l=M&s=3&d=72&v=4&t=0.1&daid=73&pt_3rd_aid=0";
        httpSession.doHttpGet(prepareUrl, "http://qun.qq.com/member.html", null);
        Respond respond = httpSession.doHttpGet(url, null, null);

        return respond.getBinaryData();
    }

    private QRStatus getQRCodeStatus() {
        String url = "https://ssl.ptlogin2.qq.com/ptqrlogin?u1=https%3A%2F%2Fqun.qq.com%2F&ptqrtoken=" + QQHash.BKNHash(httpSession.getCookieByName("qrsig"), 0) + "&ptredirect=1&h=1&t=1&g=1&from_ui=1&ptlang=2052&action=0-0-1522959728526&js_ver=10270&js_type=1&login_sig=&pt_uistyle=40&aid=715030901&daid=73&";
        String referer = "https://xui.ptlogin2.qq.com/cgi-bin/xlogin?appid=715030901&daid=73&pt_no_auth=1&s_url=https%3A%2F%2Fqun.qq.com%2F";

        Respond respond = httpSession.doHttpGet(url, referer, null);
        if (respond.getRespondCode() != 200) {
            return QRStatus.Unknown;
        }

        if (respond.getTextData().contains("二维码未失效")) {
            return QRStatus.WaitForScan;
        } else if (respond.getTextData().contains("二维码认证中")) {
            return QRStatus.WaitForAuthorization;
        } else if (respond.getTextData().contains("二维码已失效")) {
            return QRStatus.Invalid;
        } else if (respond.getTextData().contains("登录成功")) {
            loginInfo.setQunQrScannedResult(respond.getTextData());
            return QRStatus.LoginSuccess;
        } else {
            return QRStatus.Unknown;
        }
    }

    private void processQRSucceedRespond() {
        String[] succeedRespond = loginInfo.getQunQrScannedResult().split(",");
        String urlToGetPtWebQQ = succeedRespond[2];
        urlToGetPtWebQQ = urlToGetPtWebQQ.substring(0, urlToGetPtWebQQ.lastIndexOf("'")).substring(urlToGetPtWebQQ.indexOf("'") + 1);
        loginInfo.setQunUrlToGetPtWebQQ(urlToGetPtWebQQ);
    }

    public boolean login() {

        logger.info("尝试获取二维码...");
        byte[] qrCodeBuffer = getQRCode();
        if (qrCodeBuffer == null) {
            logger.error("尝试获取二维码失败!");
            return false;
        }

        logger.info("检测二维码的有效性...");
        if (getQRCodeStatus() != QRStatus.WaitForScan) {
            logger.error("二维码已经失效!");
            return false;
        }

        eventManager.dispatchAsyncEvent(new GetQRCodeEvent(qrCodeBuffer));

        TimeoutCallable<Boolean> qrCodeStatusChecker = new TimeoutCallable<>(() -> {
            QRStatus QRCodeStatus;
            while ((QRCodeStatus = getQRCodeStatus()) != QRStatus.LoginSuccess) {
                if (QRCodeStatus == QRStatus.Invalid || QRCodeStatus == QRStatus.Unknown) {
                    return Boolean.FALSE;
                } else {
                    Thread.sleep(1000);
                }
            }
            return Boolean.TRUE;
        }, 1000 * 60);

        try {
            logger.info("等待二维码扫描...");
            if (!qrCodeStatusChecker.call()) {
                logger.error("二维码在扫描时遇到异常!");
                return false;
            }
        } catch (Exception e) {
            logger.error("二维码在扫描时遇到异常!");
            return false;
        }


        processQRSucceedRespond();

        httpSession.doHttpGet(loginInfo.getQunUrlToGetPtWebQQ(), "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1", null);

        List<NameValuePair> parameter2 = new ArrayList<>();
        parameter2.add(new BasicNameValuePair("bkn", QQHash.BKNHash(loginInfo.getHttpSession().getCookieByName("skey"), 5381)));

        String grouplist = loginInfo.getHttpSession().doHttpPost(
                "http://qun.qq.com/cgi-bin/qun_mgr/get_group_list",
                "http://qun.qq.com/member.html",
                null,
                parameter2).getTextData();

        return true;
    }

    public QunLogin(LoginInfo loginInfo, EventManager eventManager) {
        this.loginInfo = loginInfo;
        this.httpSession = loginInfo.getHttpSession();
        this.eventManager = eventManager;
    }

    private enum QRStatus {WaitForScan, WaitForAuthorization, Invalid, LoginSuccess, Unknown}
}
