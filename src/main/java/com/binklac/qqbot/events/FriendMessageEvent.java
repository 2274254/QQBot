package com.binklac.qqbot.events;

import com.binklac.qqbot.QQBotEvent;

public class FriendMessageEvent extends QQBotEvent {
    private final String message;
    private final String senderName;
    private final Long sender;
    private final Long time;

    public FriendMessageEvent(String message, String senderName, Long sender, Long time) {
        this.message = message;
        this.senderName = senderName;
        this.sender = sender;
        this.time = time;
        this.setEventCanCancel(true);
    }

    public String getMessage() {
        return message;
    }

    public Long getSender() {
        return sender;
    }

    public Long getTime() {
        return time;
    }
}