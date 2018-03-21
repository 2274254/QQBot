package com.binklac.qqbot.protocol.message;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.events.DiscussMessageEvent;
import com.binklac.qqbot.events.FriendMessageEvent;
import com.binklac.qqbot.events.GroupMessageEvent;
import com.binklac.qqbot.helper.JsonHelper;
import com.binklac.qqbot.protocol.contacts.ContactsManager;
import com.binklac.qqbot.protocol.login.LoginInfo;
import com.binklac.qqbot.protocol.login.LoginManager;
import com.binklac.qqbot.protocol.message.structure.DiscussMessage;
import com.binklac.qqbot.protocol.message.structure.FriendMessage;
import com.binklac.qqbot.protocol.message.structure.GroupMessage;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.UnsupportedEncodingException;
import java.net.URISyntaxException;

public class MessageManager {
    private final static Logger logger = LoggerFactory.getLogger(MessageManager.class);
    private final EventManager eventManager;
    private final ContactsManager contactsManager;
    private final LoginInfo loginInfo;
    private Thread dispatcherThread = null;
    private MessagePoller poller;
    private boolean stopPoll = true;

    public MessageManager(LoginInfo loginInfo, EventManager eventManager, ContactsManager contactsManager, int threadPoolSize) throws UnsupportedEncodingException, URISyntaxException {
        this.eventManager = eventManager;
        this.contactsManager = contactsManager;
        this.poller = new MessagePoller(loginInfo, threadPoolSize);
        this.loginInfo = loginInfo;
    }

    public void beginMessagePoll() {
        if (stopPoll) {
            stopPoll = false;
            dispatcherThread = new Thread(() -> {
                while (!stopPoll) {
                    if (!synchronizationDispatchMessage()) {
                        if (!LoginManager.isSuccessLogin(loginInfo)) {
                            logger.warn("Cookie 已经失效了?");
                            poller.stopPoll();
                            stopPoll = true;
                        }
                    }
                }
            });
            poller.startPollMessage();
            dispatcherThread.start();
        }
    }

    public void stopMessagePoll() {
        if (!stopPoll) {
            stopPoll = true;
            poller.stopPoll();
            dispatcherThread.interrupt();
        }
    }

    private boolean synchronizationDispatchMessage() {
        String responseString = poller.getMessage();

        if (responseString != null) {
            JSONArray resultArray = JsonHelper.getResultJsonArrayFromString(responseString);
            if (resultArray != null) {
                for (int i = 0; i < resultArray.size(); i++) {
                    JSONObject message = resultArray.getJSONObject(i);
                    String type = message.getString("poll_type");

                    if (type.equalsIgnoreCase("message")) {
                        FriendMessage _message = new FriendMessage(message.getJSONObject("value"));
                        if (!_message.getMessage().isEmpty()) {
                            String friendName = contactsManager.getFriendName(_message.getSender());
                            logger.info("收到来自好友 [" + friendName + "] 的消息 -> " + _message.getMessage());
                            eventManager.dispatchAsyncEvent(new FriendMessageEvent(_message.getMessage(), friendName, _message.getSender(), _message.getTime()));
                        }
                    } else if (type.equalsIgnoreCase("group_message")) {
                        GroupMessage _message = new GroupMessage(message.getJSONObject("value"));
                        if (!_message.getMessage().isEmpty()) {
                            logger.info("收到来自群 [" + _message.getGroup() + "] 的用户 [" + _message.getSender() + "] 的消息 -> " + _message.getMessage());
                            eventManager.dispatchAsyncEvent(new GroupMessageEvent(_message.getMessage(), _message.getGroup(), _message.getSender(), _message.getTime()));
                        }
                    } else if (type.equalsIgnoreCase("discu_message")) {
                        DiscussMessage _message = new DiscussMessage(message.getJSONObject("value"));
                        if (!_message.getMessage().isEmpty()) {
                            logger.info("收到来自讨论组 [" + _message.getDiscuss() + "] 的用户 [" + _message.getSender() + "] 的消息 -> " + _message.getMessage());
                            eventManager.dispatchAsyncEvent(new DiscussMessageEvent(_message.getMessage(), _message.getDiscuss(), _message.getSender(), _message.getTime()));
                        }
                    }
                }
                return true;
            } else {
                logger.warn(responseString);
                return false;
            }
        } else {
            return false;
        }
    }
}
