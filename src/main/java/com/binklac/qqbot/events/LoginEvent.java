package com.binklac.qqbot.events;

import com.binklac.qqbot.QQBotEvent;

public class LoginEvent extends QQBotEvent {
    private final String QQNumber;

    public LoginEvent(String qqNumber) {
        this.setEventCanCancel(false);
        QQNumber = qqNumber;
    }

    public String getQQNumber() {
        return QQNumber;
    }
}