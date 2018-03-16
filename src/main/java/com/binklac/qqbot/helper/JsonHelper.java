package com.binklac.qqbot.helper;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONObject;

public class JsonHelper {
    public static Integer getRetcodeFromJsonString(String string) {
        return JSON.parseObject(string).getInteger("retcode");
    }

    public static Integer getIntegerFromJsonString(String string, String key) {
        return JSON.parseObject(string).getInteger(key);
    }

    public static String getStringFromJsonString(String string, String key) {
        return JSON.parseObject(string).getString(key);
    }

    public static JSONObject getResultJsonObjectFromString(String string) {
        return JSON.parseObject(string).getJSONObject("result");
    }

    public static JSONObject getJsonObjectFromString(String string) {
        return JSON.parseObject(string);
    }

}