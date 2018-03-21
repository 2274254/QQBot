package com.binklac.qqbot;

import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.events.*;
import com.binklac.qqbot.protocol.login.LoginInfo;
import com.binklac.qqbot.protocol.login.LoginManager;
import com.binklac.qqbot.protocol.message.MessageManager;
import org.apache.http.client.protocol.HttpClientContext;
import org.apache.http.impl.client.BasicCookieStore;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.UnsupportedEncodingException;
import java.net.URISyntaxException;

public class QQBot {
    private final static Logger logger = LoggerFactory.getLogger(QQBot.class);
    private final EventManager eventManager = new EventManager(this);
    private final LoginInfo loginInfo = new LoginInfo();
    private final MessageManager messageManager;
    private void registerInnerEvent() {
        eventManager.registerEvent(GetQRCodeEvent.class, 1);
        eventManager.registerEvent(LoginEvent.class, 2);
        eventManager.registerEvent(FriendMessageEvent.class, 3);
        eventManager.registerEvent(GroupMessageEvent.class, 4);
        eventManager.registerEvent(DiscussMessageEvent.class, 5);
    }

    private void setDefaultEventHandler() {
        eventManager.registerEventHandler(new QQBotDefaultEventHandler());
    }

    private void initHttpClientContext() {
        HttpClientContext context = loginInfo.getHttpClientContext();
        context.setCookieStore(new BasicCookieStore());
        loginInfo.setHttpClientContext(context);
    }

    public QQBot(QQBotConfig config) {
        registerInnerEvent();
        setDefaultEventHandler();
        initHttpClientContext();

        if (!(config.isUsePasswordLogin() ? LoginManager.passwordLogin(loginInfo, config) : LoginManager.qrLogin(loginInfo, eventManager))) {
            logger.error("登陆失败!");
            throw new RuntimeException("登录失败!");
        } else {
            logger.info("登陆成功!登录帐号为 [" + loginInfo.getUin() + "]");
            eventManager.dispatchAsyncEvent(new LoginEvent(loginInfo.getUin()));
        }

        try {
            messageManager = new MessageManager(loginInfo, eventManager, config.getThreadPoolSize());
        } catch (UnsupportedEncodingException | URISyntaxException e) {
            logger.error("初始化失败!");
            throw new RuntimeException("初始化失败!");
        }

        messageManager.beginMessagePoll();
    }

    public static void main(String[] args) {
        QQBotConfig config = new QQBotConfig();
        config.setUsePasswordLogin(false);
        config.setThreadPoolSize(40);
        QQBot bot = new QQBot(config);
    }
}
