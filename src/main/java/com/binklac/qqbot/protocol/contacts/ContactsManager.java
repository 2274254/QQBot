package com.binklac.qqbot.protocol.contacts;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.binklac.qqbot.eventmanager.EventManager;
import com.binklac.qqbot.helper.JsonHelper;
import com.binklac.qqbot.protocol.hash.QQHash;
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

    private void updateFriendList() {
        String url = "https://s.web2.qq.com/api/get_user_friends2";
        String referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";

        JSONObject r = new JSONObject();
        r.put("vfwebqq", loginInfo.getVfWebQQ());
        r.put("hash", QQHash.QHash(loginInfo.getUin(), loginInfo.getPtWebQQ()));

        List<NameValuePair> parameter = new ArrayList<>();
        parameter.add(new BasicNameValuePair("r", r.toJSONString()));

        String responseString = loginInfo.getHttpSession().doHttpPost(url, referer, null, parameter).getTextData();

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
