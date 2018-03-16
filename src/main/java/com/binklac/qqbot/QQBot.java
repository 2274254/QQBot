package com.binklac.qqbot;

import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.events.GetQRCodeEvent;
import com.binklac.qqbot.events.LoginEvent;
import com.binklac.qqbot.protocol.login.LoginInfo;
import com.binklac.qqbot.protocol.login.PasswordLogin;
import com.binklac.qqbot.protocol.login.QRLogin;
import org.apache.http.client.protocol.HttpClientContext;
import org.apache.http.impl.client.BasicCookieStore;
import org.openqa.selenium.WebDriver;

public class QQBot {
    private final EventManager eventManager = new EventManager();
    private final LoginInfo loginInfo = new LoginInfo();

    private void initHttpClientContext() {
        HttpClientContext context = loginInfo.getHttpClientContext();
        context.setCookieStore(new BasicCookieStore());
        loginInfo.setHttpClientContext(context);
    }

    private boolean passwordLogin(QQBotConfig config) {
        try {
            System.setProperty(config.getWebDriverName(), config.getWebDriverPath());
            WebDriver webDriver = (WebDriver) Class.forName(config.getWebDriverClass()).newInstance();
            PasswordLogin login = new PasswordLogin(loginInfo, webDriver);
            return login.login(config.getUin(), config.getPassword());
        } catch (ClassNotFoundException | IllegalAccessException | InstantiationException e) {
            return false;
        }
    }

    private boolean qrCodeLogin(QQBotConfig config) {
        QRLogin login = new QRLogin(loginInfo);
        return login.login(eventManager);
    }

    public QQBot(QQBotConfig config) {
        eventManager.registerEvent(GetQRCodeEvent.class, 1);
        eventManager.registerEvent(LoginEvent.class, 2);

        eventManager.registerEventHandler(new QQBotDefaultEventHandler());

        initHttpClientContext();


        if (!(config.isUsePasswordLogin() ? passwordLogin(config) : qrCodeLogin(config))) {
            throw new RuntimeException("登录失败!");
        } else {
            eventManager.dispatchAsyncEvent(new LoginEvent(loginInfo.getUin()));

        }
    }

    public static void main(String[] args) {
        QQBotConfig config = new QQBotConfig();
        config.setUsePasswordLogin(false);

        QQBot bot = new QQBot(config);


    }
}
