package com.binklac.qqbot.events;

import com.binklac.qqbot.QQBotEvent;

public class GroupMessageEvent extends QQBotEvent {
    private final String message;
    private final Long group;
    private final Long sender;
    private final Long time;

    public GroupMessageEvent(String message, Long group, Long sender, Long time) {
        this.message = message;
        this.group = group;
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

    public Long getGroup() {
        return group;
    }
}