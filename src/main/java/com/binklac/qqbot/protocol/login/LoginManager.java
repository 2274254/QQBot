package com.binklac.qqbot.protocol.login;

import com.binklac.qqbot.QQBotConfig;
import com.binklac.qqbot.eventmanager.EventManager;
import org.apache.http.Header;
import org.apache.http.HttpHeaders;
import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpUriRequest;
import org.apache.http.client.methods.RequestBuilder;
import org.apache.http.client.protocol.HttpClientContext;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.message.BasicHeader;
import org.openqa.selenium.WebDriver;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.List;

public class LoginManager {
    private final static Logger logger = LoggerFactory.getLogger(LoginManager.class);

    public static boolean passwordLogin(LoginInfo loginInfo, QQBotConfig config) {
        try {
            logger.info("尝试使用帐号密码登录...");
            System.setProperty(config.getWebDriverName(), config.getWebDriverPath());
            WebDriver webDriver = (WebDriver) Class.forName(config.getWebDriverClass()).newInstance();
            PasswordLogin login = new PasswordLogin(loginInfo, webDriver);
            return login.login(config.getUin(), config.getPassword());
        } catch (ClassNotFoundException | IllegalAccessException | InstantiationException e) {
            return false;
        }
    }

    public static boolean qrLogin(LoginInfo loginInfo, EventManager eventManager) {
        logger.info("尝试使用二维码登录...");
        QRLogin login = new QRLogin(loginInfo);
        return login.login(eventManager);
    }

    public static boolean isSuccessLogin(LoginInfo loginInfo) {
        String url = "http://d1.web2.qq.com/channel/get_online_buddies2?vfwebqq=" + loginInfo.getVfWebQQ() + "&clientid=" + loginInfo.getClientId() + "&psessionid=" + loginInfo.getPSessionId() + "&t=" + Math.random();
        String referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
        CloseableHttpClient httpClient = null;
        try {
            HttpClientContext context = loginInfo.getHttpClientContext();

            List<Header> headerList = new ArrayList<>();
            headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
            httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();

            URI uri = new URIBuilder(url).build();
            HttpUriRequest httpUriRequest = RequestBuilder.get().setUri(uri).build();
            HttpResponse response = httpClient.execute(httpUriRequest, context);

            return response.getStatusLine().getStatusCode() == 200;
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
}
