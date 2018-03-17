package com.binklac.qqbot.protocol.message.structure;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;

public class DiscussMessage {
    private String message;
    private long discuss;
    private long time;
    private long sender;
    private Font font;

    public DiscussMessage(JSONObject json) {
        JSONArray contents = json.getJSONArray("content");
        this.font = contents.getJSONArray(0).getObject(1, Font.class);

        final int size = contents.size();
        final StringBuilder contentBuilder = new StringBuilder();
        for (int i = 1; i < size; i++) {
            contentBuilder.append(contents.getString(i));
        }
        this.message = contentBuilder.toString();

        this.time = json.getLongValue("time");
        this.discuss = json.getLongValue("did");
        this.sender = json.getLongValue("send_uin");
    }

    public long getSender() {
        return sender;
    }

    public Font getFont() {
        return font;
    }

    public long getTime() {
        return time;
    }

    public long getDiscuss() {
        return discuss;
    }

    public String getMessage() {
        return message;
    }
}
