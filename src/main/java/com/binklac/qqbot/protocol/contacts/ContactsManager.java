package com.binklac.qqbot.protocol.contacts;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.helper.JsonHelper;
import com.binklac.qqbot.helper.WebHelper;
import com.binklac.qqbot.protocol.login.LoginInfo;
import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ContactsManager {
    private final static Logger logger = LoggerFactory.getLogger(ContactsManager.class);
    private final EventManager eventManager;
    private final LoginInfo loginInfo;
    private Map<Long, String> friendNameMap = new HashMap<>();

    public ContactsManager(EventManager eventManager, LoginInfo loginInfo) {
        this.eventManager = eventManager;
        this.loginInfo = loginInfo;
    }

    private static String hash(long x, String K) {
        int[] N = new int[4];
        for (int T = 0; T < K.length(); T++) {
            N[T % 4] ^= K.charAt(T);
        }
        String[] U = {"EC", "OK"};
        long[] V = new long[4];
        V[0] = x >> 24 & 255 ^ U[0].charAt(0);
        V[1] = x >> 16 & 255 ^ U[0].charAt(1);
        V[2] = x >> 8 & 255 ^ U[1].charAt(0);
        V[3] = x & 255 ^ U[1].charAt(1);

        long[] U1 = new long[8];

        for (int T = 0; T < 8; T++) {
            U1[T] = T % 2 == 0 ? N[T >> 1] : V[T >> 1];
        }

        String[] N1 = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F"};
        String V1 = "";
        for (long aU1 : U1) {
            V1 += N1[(int) ((aU1 >> 4) & 15)];
            V1 += N1[(int) (aU1 & 15)];
        }
        return V1;
    }

    private void updateFriendList() {
        String url = "https://s.web2.qq.com/api/get_user_friends2";
        String referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";

        JSONObject r = new JSONObject();
        r.put("vfwebqq", loginInfo.getVfWebQQ());
        r.put("hash", hash(loginInfo.getUin(), loginInfo.getPtWebQQ()));

        List<NameValuePair> parameter = new ArrayList<>();
        parameter.add(new BasicNameValuePair("r", r.toJSONString()));

        String responseString = WebHelper.getPostString(loginInfo, url, referer, parameter);

        Map<Long, String> _friendNameMap = new HashMap<>();
        JSONArray info = JsonHelper.getResultJsonObjectFromString(responseString).getJSONArray("info");
        for (int i = 0; info != null && i < info.size(); i++) {
            JSONObject item = info.getJSONObject(i);
            _friendNameMap.put(item.getLongValue("uin"), item.getString("nick"));
        }

        //TODO: 清除无效好友
        friendNameMap = _friendNameMap;
    }

    public String getFriendName(Long uid) {
        String name = friendNameMap.get(uid);
        if (name == null) {
            updateFriendList();
            name = friendNameMap.get(uid);
            if (name != null) {
                //TODO: 添加增加好友事件
            }
        }
        return name;
    }
}
