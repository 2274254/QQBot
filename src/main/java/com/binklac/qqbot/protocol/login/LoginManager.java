package com.binklac.qqbot.protocol.login;

import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.helper.http.SimpleHttpSession;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class LoginManager {
    private final static Logger logger = LoggerFactory.getLogger(LoginManager.class);

    public static boolean qrLogin(LoginInfo loginInfo, EventManager eventManager) {
        logger.info("尝试使用二维码登录...");
        Web2Login login = new Web2Login(loginInfo, eventManager);
        //QunLogin login2 = new QunLogin(loginInfo, eventManager);
        return login.login(); //&& login2.login();
    }

    public static boolean isSuccessLogin(LoginInfo loginInfo) {
        String url = "http://d1.web2.qq.com/channel/get_online_buddies2?vfwebqq=" + loginInfo.getVfWebQQ() + "&clientid=" + loginInfo.getClientId() + "&psessionid=" + loginInfo.getPSessionId() + "&t=" + Math.random();
        String referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";

        SimpleHttpSession session = loginInfo.getHttpSession();

        return session.doHttpGet(url, referer, null).getRespondCode() == 200;
    }
}
