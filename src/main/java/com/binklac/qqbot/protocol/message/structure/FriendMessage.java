package com.binklac.qqbot.protocol.message.structure;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;


public class FriendMessage {
    private long time;
    private String message;
    private Long sender;
    private Font font;

    public FriendMessage(JSONObject json) {
        this.time = json.getLongValue("time");
        this.sender = json.getLongValue("from_uin");
        JSONArray contents = json.getJSONArray("content");
        this.font = contents.getJSONArray(0).getObject(1, Font.class);

        int size = contents.size();
        StringBuilder contentBuilder = new StringBuilder();
        for (int i = 1; i < size; i++) {
            contentBuilder.append(contents.getString(i));
        }
        this.message = contentBuilder.toString();
    }

    public Long getSender() {
        return sender;
    }

    public Font getFont() {
        return font;
    }

    public String getMessage() {
        return message;
    }

    public long getTime() {
        return time;
    }
}
