package com.binklac.qqbot.protocol.login;

import com.alibaba.fastjson.JSONObject;
import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.events.GetQRCodeEvent;
import com.binklac.qqbot.helper.JsonHelper;
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

public class Web2Login {
    private final static Logger logger = LoggerFactory.getLogger(Web2Login.class);
    private final EventManager eventManager;
    private final SimpleHttpSession httpSession;
    private final LoginInfo loginInfo;

    private byte[] getQRCode() {
        String url = "https://ssl.ptlogin2.qq.com/ptqrshow?appid=501004106&e=0&l=M&s=5&d=72&v=4&t=0.1";

        Respond respond = httpSession.doHttpGet(url, null, null);
        return respond.getBinaryData();
    }

    private QRStatus getQRCodeStatus() {
        String url = "https://ssl.ptlogin2.qq.com/ptqrlogin?ptqrtoken=" + QQHash.BKNHash(httpSession.getCookieByName("qrsig"), 0) + "&webqq_type=10&remember_uin=1&login2qq=1&aid=501004106&u1=http%3A%2F%2Fw.qq.com%2Fproxy.html%3Flogin2qq%3D1%26webqq_type%3D10&ptredirect=0&ptlang=2052&daid=164&from_ui=1&pttype=1&dumy=&fp=loginerroralert&action=0-0-157510&mibao_css=m_webqq&t=1&g=1&js_type=0&js_ver=10143&login_sig=&pt_randsalt=0";
        String referer = "https://ui.ptlogin2.qq.com/cgi-bin/login?daid=164&target=self&style=16&mibao_css=m_webqq&appid=501004106&enable_qlogin=0&no_verifyimg=1&s_url=http%3A%2F%2Fw.qq.com%2Fproxy.html&f_url=loginerroralert&strong_login=1&login_state=10&t=20131024001";

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
            loginInfo.setWeb2QrScannedResult(respond.getTextData());
            return QRStatus.LoginSuccess;
        } else {
            return QRStatus.Unknown;
        }
    }

    private void processQRSucceedRespond() {
        String[] succeedRespond = loginInfo.getWeb2QrScannedResult().split(",");
        String urlToGetPtWebQQ = succeedRespond[2];
        urlToGetPtWebQQ = urlToGetPtWebQQ.substring(0, urlToGetPtWebQQ.lastIndexOf("'")).substring(urlToGetPtWebQQ.indexOf("'") + 1);
        loginInfo.setWeb2UrlToGetPtWebQQ(urlToGetPtWebQQ);
    }

    private boolean getPtWebQQ() {
        String referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
        httpSession.doHttpGet(loginInfo.getWeb2UrlToGetPtWebQQ(), referer, null);
        loginInfo.setPtWebQQ(httpSession.getCookieByName("ptwebqq"));

        return !loginInfo.getPtWebQQ().isEmpty();
    }

    private boolean getVfWebQQ() {
        String url = "http://s.web2.qq.com/api/getvfwebqq?ptwebqq=" + loginInfo.getPtWebQQ() + "&clientid=53999199&psessionid=&t=0.1";
        String referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";

        Respond respond = httpSession.doHttpGet(url, referer, null);

        String responseString = respond.getTextData();
        String vfWebQQ = "";
        if (JsonHelper.getRetcodeFromJsonString(responseString) == 0) {
            vfWebQQ = JsonHelper.getResultJsonObjectFromString(responseString).getString("vfwebqq");
        }

        if (!vfWebQQ.isEmpty()) {
            loginInfo.setVfWebQQ(vfWebQQ);
            return true;
        }

        return false;
    }

    private boolean getUinAndPsessionid() {
        String url = "http://d1.web2.qq.com/channel/login2";
        String referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";

        JSONObject r = new JSONObject();
        r.put("ptwebqq", loginInfo.getPtWebQQ());
        r.put("clientid", Long.parseLong(loginInfo.getClientId()));
        r.put("psessionid", "");
        r.put("status", "online");

        List<NameValuePair> parameter = new ArrayList<>();
        parameter.add(new BasicNameValuePair("r", r.toJSONString()));

        Respond respond = httpSession.doHttpPost(url, referer, null, parameter);

        String responseString = respond.getTextData();

        if (responseString != null && JsonHelper.getRetcodeFromJsonString(responseString) == 0) {
            JSONObject result = JsonHelper.getResultJsonObjectFromString(responseString);
            loginInfo.setPSessionId(result.getString("psessionid"));
            loginInfo.setUin(Long.parseLong(result.getString("uin")));
            return true;
        } else {
            return false;
        }
    }

    public Web2Login(LoginInfo loginInfo, EventManager eventManager) {
        this.loginInfo = loginInfo;
        this.httpSession = loginInfo.getHttpSession();
        this.eventManager = eventManager;
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

        logger.info("尝试获取PtWebQQ...");
        if (!getPtWebQQ()) {
            logger.error("获取PtWebQQ遇到异常!");
            return false;
        }


        logger.info("尝试获取VfWebQQ...");
        if (!getVfWebQQ()) {
            logger.error("获取VfWebQQ遇到异常!");
            return false;
        }


        logger.info("尝试获取Uin与Psessionid...");
        return getUinAndPsessionid();
    }

    private enum QRStatus {WaitForScan, WaitForAuthorization, Invalid, LoginSuccess, Unknown}

}
