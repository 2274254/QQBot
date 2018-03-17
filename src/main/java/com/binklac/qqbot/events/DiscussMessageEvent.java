package com.binklac.qqbot.events;

import com.binklac.qqbot.QQBotEvent;

public class DiscussMessageEvent extends QQBotEvent {
    private final String message;
    private final Long discuss;
    private final Long sender;
    private final Long time;

    public DiscussMessageEvent(String message, Long discuss, Long sender, Long time) {
        this.message = message;
        this.discuss = discuss;
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

    public Long getDiscuss() {
        return discuss;
    }
}