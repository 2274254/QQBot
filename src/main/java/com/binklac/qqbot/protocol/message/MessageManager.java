package com.binklac.qqbot.protocol.message;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.events.DiscussMessageEvent;
import com.binklac.qqbot.events.FriendMessageEvent;
import com.binklac.qqbot.events.GroupMessageEvent;
import com.binklac.qqbot.helper.JsonHelper;
import com.binklac.qqbot.helper.WebHelper;
import com.binklac.qqbot.protocol.login.LoginInfo;
import com.binklac.qqbot.protocol.message.structure.DiscussMessage;
import com.binklac.qqbot.protocol.message.structure.Font;
import com.binklac.qqbot.protocol.message.structure.FriendMessage;
import com.binklac.qqbot.protocol.message.structure.GroupMessage;
import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.List;

public class MessageManager {
    private final static Logger logger = LoggerFactory.getLogger(MessageManager.class);
    private final EventManager eventManager;
    private final LoginInfo loginInfo;

    public MessageManager(LoginInfo loginInfo, EventManager eventManager) {
        this.eventManager = eventManager;
        this.loginInfo = loginInfo;

        while (true) {
            try {
                Thread.sleep(500);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            pollMessage();
        }
    }

    private static Font defaultFont() {
        Font font = new Font();
        font.setColor("000000");
        font.setStyle(new int[]{0, 0, 0});
        font.setName("宋体");
        font.setSize(10);
        return font;
    }

    private void pollMessage() {
        String url = "https://d1.web2.qq.com/channel/poll2";
        String referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";

        JSONObject r = new JSONObject();
        r.put("ptwebqq", loginInfo.getPtWebQQ());
        r.put("clientid", Long.parseLong(loginInfo.getClientId()));
        r.put("psessionid", loginInfo.getPSessionId());
        r.put("key", "");

        List<NameValuePair> parameter = new ArrayList<>();
        parameter.add(new BasicNameValuePair("r", r.toJSONString()));

        String responseString = WebHelper.getPostString(loginInfo, url, referer, parameter);

        JSONArray resultArray = JsonHelper.getResultJsonArrayFromString(responseString);

        for (int i = 0; resultArray != null && i < resultArray.size(); i++) {
            JSONObject message = resultArray.getJSONObject(i);
            String type = message.getString("poll_type");

            if (type.equalsIgnoreCase("message")) {
                FriendMessage _message = new FriendMessage(message.getJSONObject("value"));
                logger.info("收到来自好友 [" + _message.getSender() + "] 的消息 -> " + _message.getMessage());
                eventManager.dispatchAsyncEvent(new FriendMessageEvent(_message.getMessage(), _message.getSender(), _message.getTime()));
            } else if (type.equalsIgnoreCase("group_message")) {
                GroupMessage _message = new GroupMessage(message.getJSONObject("value"));
                logger.info("收到来自群 [" + _message.getGroup() + "] 的用户 [" + _message.getSender() + "] 的消息 -> " + _message.getMessage());
                eventManager.dispatchAsyncEvent(new GroupMessageEvent(_message.getMessage(), _message.getGroup(), _message.getSender(), _message.getTime()));
            } else if (type.equalsIgnoreCase("discu_message")) {
                DiscussMessage _message = new DiscussMessage(message.getJSONObject("value"));
                logger.info("收到来自讨论组 [" + _message.getDiscuss() + "] 的用户 [" + _message.getSender() + "] 的消息 -> " + _message.getMessage());
                eventManager.dispatchAsyncEvent(new DiscussMessageEvent(_message.getMessage(), _message.getDiscuss(), _message.getSender(), _message.getTime()));
            }
        }
    }
}
