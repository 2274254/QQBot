package com.binklac.qqbot.protocol.login;

import com.alibaba.fastjson.JSONObject;
import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.events.GetQRCodeEvent;
import com.binklac.qqbot.helper.JsonHelper;
import com.binklac.qqbot.helper.TimeoutCallable;
import com.binklac.qqbot.helper.WebHelper;
import com.binklac.qqbot.protocol.hash.QQHash;
import org.apache.http.Header;
import org.apache.http.HttpHeaders;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.CookieStore;
import org.apache.http.client.methods.HttpUriRequest;
import org.apache.http.client.methods.RequestBuilder;
import org.apache.http.client.protocol.HttpClientContext;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.cookie.Cookie;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.List;

public class QRLogin {
    private final static Logger logger = LoggerFactory.getLogger(QRLogin.class);
    private final LoginInfo loginInfo;


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

        String responseString = WebHelper.getPostString(loginInfo, url, referer, parameter);

        if (responseString != null && JsonHelper.getRetcodeFromJsonString(responseString) == 0) {
            JSONObject result = JsonHelper.getResultJsonObjectFromString(responseString);
            loginInfo.setPSessionId(result.getString("psessionid"));
            loginInfo.setUin(Long.parseLong(result.getString("uin")));
            return true;
        } else {
            return false;
        }


    }

    private boolean getVfWebQQ() {
        String url = "http://s.web2.qq.com/api/getvfwebqq?ptwebqq=" + loginInfo.getPtWebQQ() + "&clientid=53999199&psessionid=&t=0.1";
        String referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";

        String responseString = WebHelper.getGetString(loginInfo, url, referer);
        String vfWebQQ = "";
        if (JsonHelper.getRetcodeFromJsonString(responseString) == 0) {
            vfWebQQ = JsonHelper.getResultJsonObjectFromString(responseString).getString("vfwebqq");
        }

        if (!vfWebQQ.isEmpty()) {
            this.loginInfo.setVfWebQQ(vfWebQQ);
            return true;
        }

        return false;
    }

    private boolean getPtWebQQ() {
        CloseableHttpClient httpClient = null;
        String referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
        try {
            URI uri = new URIBuilder(loginInfo.getUrlToGetPtWebQQ()).build();
            List<Header> headerList = new ArrayList<>();
            headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
            httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
            HttpUriRequest httpUriRequest = RequestBuilder.get().setUri(uri).build();
            HttpClientContext context = loginInfo.getHttpClientContext();

            httpClient.execute(httpUriRequest, context);

            context = loginInfo.getHttpClientContext();
            String ptWebQQ = "";
            for (Cookie cookie : context.getCookieStore().getCookies()) {
                if (cookie.getName().equalsIgnoreCase("ptwebqq")) {
                    ptWebQQ = cookie.getValue();
                }
            }

            if (!ptWebQQ.isEmpty()) {
                this.loginInfo.setPtWebQQ(ptWebQQ);
                return true;
            }

            return false;
        } catch (URISyntaxException | IOException e) {
            return false;
        } finally {
            if (httpClient != null) {
                try {
                    httpClient.close();
                } catch (IOException ignored) {
                }
            }
        }
    }

    private void processQRSucceedRespond() {
        String[] succeedRespond = loginInfo.getQrScannedResult().split(",");
        String urlToGetPtWebQQ = succeedRespond[2];
        urlToGetPtWebQQ = urlToGetPtWebQQ.substring(0, urlToGetPtWebQQ.lastIndexOf("'")).substring(urlToGetPtWebQQ.indexOf("'") + 1);
        loginInfo.setUrlToGetPtWebQQ(urlToGetPtWebQQ);
    }


    private QRStatus getQRCodeStatus() {
        CloseableHttpClient httpClient = null;
        String referer = "https://ui.ptlogin2.qq.com/cgi-bin/login?daid=164&target=self&style=16&mibao_css=m_webqq&appid=501004106&enable_qlogin=0&no_verifyimg=1&s_url=http%3A%2F%2Fw.qq.com%2Fproxy.html&f_url=loginerroralert&strong_login=1&login_state=10&t=20131024001";

        try {
            HttpClientContext context = loginInfo.getHttpClientContext();
            String ptQrToken = "";
            for (Cookie cookie : context.getCookieStore().getCookies()) {
                if (cookie.getName().equalsIgnoreCase("qrsig")) {
                    ptQrToken = QQHash.BKNHash(cookie.getValue(), 0);
                }
            }

            String url = "https://ssl.ptlogin2.qq.com/ptqrlogin?ptqrtoken=" + ptQrToken + "&webqq_type=10&remember_uin=1&login2qq=1&aid=501004106&u1=http%3A%2F%2Fw.qq.com%2Fproxy.html%3Flogin2qq%3D1%26webqq_type%3D10&ptredirect=0&ptlang=2052&daid=164&from_ui=1&pttype=1&dumy=&fp=loginerroralert&action=0-0-157510&mibao_css=m_webqq&t=1&g=1&js_type=0&js_ver=10143&login_sig=&pt_randsalt=0";

            List<Header> headerList = new ArrayList<>();
            headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
            httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();

            URI uri = new URIBuilder(url).build();
            HttpUriRequest httpUriRequest = RequestBuilder.get().setUri(uri).build();
            HttpResponse response = httpClient.execute(httpUriRequest, context);

            if (response.getStatusLine().getStatusCode() != 200) {
                return QRStatus.Unknown;
            }

            BufferedReader reader = new BufferedReader(new InputStreamReader(response.getEntity().getContent(), "UTF-8"));
            String result = reader.readLine();

            if (result.contains("二维码未失效")) {
                return QRStatus.WaitForScan;
            } else if (result.contains("二维码认证中")) {
                return QRStatus.WaitForAuthorization;
            } else if (result.contains("二维码已失效")) {
                return QRStatus.Invalid;
            } else if (result.contains("登录成功")) {
                loginInfo.setQrScannedResult(result);
                return QRStatus.LoginSuccess;
            } else {
                return QRStatus.Unknown;
            }
        } catch (URISyntaxException | IOException e) {
            e.printStackTrace();
            return null;
        } finally {
            if (httpClient != null) {
                try {
                    httpClient.close();
                } catch (IOException ignored) {
                }
            }
        }
    }

    private byte[] getQRCode() {
        CloseableHttpClient httpClient = null;
        String url = "https://ssl.ptlogin2.qq.com/ptqrshow?appid=501004106&e=0&l=M&s=5&d=72&v=4&t=0.1";

        try {
            httpClient = HttpClients.createDefault();
            URI uri = new URIBuilder(url).build();
            HttpUriRequest httpUriRequest = RequestBuilder.get().setUri(uri).build();
            HttpClientContext context = loginInfo.getHttpClientContext();
            HttpResponse response = httpClient.execute(httpUriRequest, context);
            return WebHelper.ReadBinaryFromRespondWithSize(response.getEntity().getContent(), response.getEntity().getContentLength());
        } catch (URISyntaxException | IOException e) {
            e.printStackTrace();
            return null;
        } finally {
            if (httpClient != null) {
                try {
                    httpClient.close();
                } catch (IOException ignored) {
                }
            }
        }
    }

    private void removeCookie(String cookieName) {
        List<Cookie> newCookieList = new ArrayList<>();
        HttpClientContext context = loginInfo.getHttpClientContext();
        CookieStore store = context.getCookieStore();
        for (Cookie cookie : store.getCookies()) {
            if (!cookie.getName().equalsIgnoreCase(cookieName)) {
                newCookieList.add(cookie);
            }
        }

        store.clear();

        for (Cookie cookie : newCookieList) {
            store.addCookie(cookie);
        }

        context.setCookieStore(store);
        loginInfo.setHttpClientContext(context);
    }

    public boolean login(EventManager eventManager) {

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

    public QRLogin(LoginInfo loginInfo) {
        this.loginInfo = loginInfo;
    }

    private enum QRStatus {WaitForScan, WaitForAuthorization, Invalid, LoginSuccess, Unknown}
}