package com.binklac.qqbot.events;

import com.binklac.qqbot.QQBotEvent;

public class LoginEvent extends QQBotEvent {
    private final Long QQNumber;

    public LoginEvent(Long qqNumber) {
        this.setEventCanCancel(false);
        QQNumber = qqNumber;
    }

    public Long getQQNumber() {
        return QQNumber;
    }
}