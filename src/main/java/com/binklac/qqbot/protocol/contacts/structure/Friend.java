package com.binklac.qqbot.protocol.contacts.structure;

public class Friend {
    private final long userId;
    private final String nickname;

    public Friend(long userId, String nickname) {
        this.userId = userId;
        this.nickname = nickname;
    }
}
