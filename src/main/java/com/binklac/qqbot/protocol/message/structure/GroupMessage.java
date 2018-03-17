package com.binklac.qqbot.protocol.message.structure;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;

public class GroupMessage {
    private String message;
    private long group;
    private long sender;
    private long time;
    private Font font;

    public GroupMessage(JSONObject json) {
        JSONArray contents = json.getJSONArray("content");
        this.font = contents.getJSONArray(0).getObject(1, Font.class);

        final int size = contents.size();
        final StringBuilder contentBuilder = new StringBuilder();
        for (int i = 1; i < size; i++) {
            contentBuilder.append(contents.getString(i));
        }

        this.message = contentBuilder.toString();
        this.time = json.getLongValue("time");
        this.group = json.getLongValue("group_code");
        this.sender = json.getLongValue("send_uin");
    }

    public Font getFont() {
        return font;
    }

    public long getTime() {
        return time;
    }

    public long getSender() {
        return sender;
    }

    public long getGroup() {
        return group;
    }

    public String getMessage() {
        return message;
    }
}
