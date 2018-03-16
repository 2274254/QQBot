package com.binklac.qqbot.protocol.login;

import com.alibaba.fastjson.JSONObject;
import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.events.GetQRCodeEvent;
import com.binklac.qqbot.helper.JsonHelper;
import com.binklac.qqbot.helper.TimeoutCallable;
import com.binklac.qqbot.protocol.hash.QQHash;
import org.apache.http.Header;
import org.apache.http.HttpHeaders;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.entity.UrlEncodedFormEntity;
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
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URI;
import java.net.URISyntaxException;
import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public class QRLogin {
    private final static Logger logger = LoggerFactory.getLogger(QRLogin.class);
    private final LoginInfo loginInfo;
    private String qrScannedResult = "";
    private String urlToGetPtWebQQ = "";

    private static byte[] ReadBinaryFromRespondWithSize(InputStream respondInputStream, long contentLength) throws IOException {
        ByteBuffer respondByteBuffer = ByteBuffer.allocate((int) contentLength);
        int availableSize;
        while ((availableSize = respondInputStream.available()) != 0) {
            byte[] tempBuffer = new byte[availableSize];
            int realReadSize = respondInputStream.read(tempBuffer);
            if (realReadSize > 0) {
                respondByteBuffer.put(tempBuffer, 0, realReadSize);
            }
        }
        return respondByteBuffer.array();
    }

    private boolean getUinAndPsessionid() {
        CloseableHttpClient httpClient = null;
        String url = "http://d1.web2.qq.com/channel/login2";
        String referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";

        JSONObject r = new JSONObject();
        r.put("ptwebqq", loginInfo.getPtWebQQ());
        r.put("clientid", Long.parseLong(loginInfo.getClientId()));
        r.put("psessionid", "");
        r.put("status", "online");

        List<NameValuePair> parameter = new ArrayList<>();
        parameter.add(new BasicNameValuePair("r", r.toJSONString()));

        try {
            URI uri = new URIBuilder(url).build();

            List<Header> headerList = new ArrayList<>();
            headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
            httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
            HttpUriRequest httpUriRequest = RequestBuilder.post().setUri(uri).setEntity(new UrlEncodedFormEntity(parameter, "utf-8")).build();
            HttpClientContext context = loginInfo.getHttpClientContext();

            HttpResponse response = httpClient.execute(httpUriRequest, context);

            BufferedReader reader = new BufferedReader(new InputStreamReader(response.getEntity().getContent(), "UTF-8"));
            String responseString = reader.readLine();

            if (JsonHelper.getRetcodeFromJsonString(responseString) == 0) {
                JSONObject result = JsonHelper.getResultJsonObjectFromString(responseString);
                loginInfo.setPSessionId(result.getString("psessionid"));
                loginInfo.setUin(result.getString("uin"));
                return true;
            }
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

        return false;
    }

    private boolean getVfWebQQ() {
        CloseableHttpClient httpClient = null;
        String url = "http://s.web2.qq.com/api/getvfwebqq?ptwebqq=" + loginInfo.getPtWebQQ() + "&clientid=53999199&psessionid=&t=0.1";
        String referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
        try {
            URI uri = new URIBuilder(url).build();
            List<Header> headerList = new ArrayList<>();
            headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
            httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
            HttpUriRequest httpUriRequest = RequestBuilder.get().setUri(uri).build();
            HttpClientContext context = loginInfo.getHttpClientContext();

            HttpResponse response = httpClient.execute(httpUriRequest, context);

            String responseString = new String(ReadBinaryFromRespondWithSize(response.getEntity().getContent(), response.getEntity().getContentLength()));

            String vfWebQQ = "";
            if (JsonHelper.getRetcodeFromJsonString(responseString) == 0) {
                vfWebQQ = JsonHelper.getResultJsonObjectFromString(responseString).getString("vfwebqq");
            }

            if (!vfWebQQ.isEmpty()) {
                this.loginInfo.setVfWebQQ(vfWebQQ);
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

    private boolean getPtWebQQ() {
        CloseableHttpClient httpClient = null;
        String referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
        try {
            URI uri = new URIBuilder(urlToGetPtWebQQ).build();
            List<Header> headerList = new ArrayList<>();
            headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
            httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
            HttpUriRequest httpUriRequest = RequestBuilder.get().setUri(uri).build();
            HttpClientContext context = loginInfo.getHttpClientContext();

            HttpResponse response = httpClient.execute(httpUriRequest, context);

            context = loginInfo.getHttpClientContext();
            String ptWebQQ = "";
            for (Cookie cookie : context.getCookieStore().getCookies()) {
                if (cookie.getName().equalsIgnoreCase("ptwebqq")) {
                    ptWebQQ = QQHash.BKNHash(cookie.getValue(), 0);
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
        String[] succeedRespond = qrScannedResult.split(",");
        String tempUrlToGetPtWebQQ = succeedRespond[2];
        urlToGetPtWebQQ = tempUrlToGetPtWebQQ.substring(0, tempUrlToGetPtWebQQ.lastIndexOf("'")).substring(tempUrlToGetPtWebQQ.indexOf("'") + 1);
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
                qrScannedResult = result;
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
            return ReadBinaryFromRespondWithSize(response.getEntity().getContent(), response.getEntity().getContentLength());
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

    public boolean login(EventManager eventManager) {
        byte[] qrCodeBuffer = getQRCode();
        if (qrCodeBuffer == null) {
            return false;
        }

        if (getQRCodeStatus() != QRStatus.WaitForScan) {
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
            if (!qrCodeStatusChecker.call()) {
                return false;
            }
        } catch (Exception e) {
            return false;
        }

        processQRSucceedRespond();

        if (!getPtWebQQ()) {
            return false;
        }

        if (!getVfWebQQ()) {
            return false;
        }

        return getUinAndPsessionid();
    }

    public QRLogin(LoginInfo loginInfo) {
        this.loginInfo = loginInfo;
    }

    private enum QRStatus {WaitForScan, WaitForAuthorization, Invalid, LoginSuccess, Unknown}
}