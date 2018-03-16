package com.binklac.qqbot.protocol.login;

import org.apache.http.client.protocol.HttpClientContext;

public class LoginInfo {
    private final String clientId = "53999199";
    private HttpClientContext httpClientContext = HttpClientContext.create();
    private String ptWebQQ = null;
    private String vfWebQQ = null;
    private String uin = null;
    private String pSessionId = null;

    public String getClientId() {
        return clientId;
    }

    public HttpClientContext getHttpClientContext() {
        return httpClientContext;
    }

    public void setHttpClientContext(HttpClientContext httpClientContext) {
        this.httpClientContext = httpClientContext;
    }

    public String getPtWebQQ() {
        return ptWebQQ;
    }

    public void setPtWebQQ(String ptWebQQ) {
        this.ptWebQQ = ptWebQQ;
    }

    public String getVfWebQQ() {
        return vfWebQQ;
    }

    public void setVfWebQQ(String vfWebQQ) {
        this.vfWebQQ = vfWebQQ;
    }

    public String getUin() {
        return uin;
    }

    public void setUin(String uin) {
        this.uin = uin;
    }

    public String getPSessionId() {
        return pSessionId;
    }

    public void setPSessionId(String pSessionId) {
        this.pSessionId = pSessionId;
    }
}
