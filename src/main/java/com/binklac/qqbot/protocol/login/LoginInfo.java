package com.binklac.qqbot.protocol.login;

import com.binklac.qqbot.helper.http.SimpleHttpSession;

public class LoginInfo {
    private final String clientId = "53999199";
    private SimpleHttpSession httpSession = new SimpleHttpSession();
    private String ptWebQQ = null;
    private String vfWebQQ = null;
    private Long uin = null;
    private String pSessionId = null;
    private String web2QrScannedResult = null;
    private String web2UrlToGetPtWebQQ = null;

    public String getpSessionId() {
        return pSessionId;
    }

    public void setpSessionId(String pSessionId) {
        this.pSessionId = pSessionId;
    }

    public String getWeb2QrScannedResult() {
        return web2QrScannedResult;
    }

    public void setWeb2QrScannedResult(String web2QrScannedResult) {
        this.web2QrScannedResult = web2QrScannedResult;
    }

    public String getWeb2UrlToGetPtWebQQ() {
        return web2UrlToGetPtWebQQ;
    }

    public void setWeb2UrlToGetPtWebQQ(String web2UrlToGetPtWebQQ) {
        this.web2UrlToGetPtWebQQ = web2UrlToGetPtWebQQ;
    }

    public String getQunQrScannedResult() {
        return qunQrScannedResult;
    }

    public void setQunQrScannedResult(String qunQrScannedResult) {
        this.qunQrScannedResult = qunQrScannedResult;
    }

    public String getQunUrlToGetPtWebQQ() {
        return qunUrlToGetPtWebQQ;
    }

    public void setQunUrlToGetPtWebQQ(String qunUrlToGetPtWebQQ) {
        this.qunUrlToGetPtWebQQ = qunUrlToGetPtWebQQ;
    }

    private String qunQrScannedResult = null;
    private String qunUrlToGetPtWebQQ = null;

    public String getClientId() {
        return clientId;
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

    public String getPSessionId() {
        return pSessionId;
    }

    public void setPSessionId(String pSessionId) {
        this.pSessionId = pSessionId;
    }

    public Long getUin() {
        return uin;
    }

    public void setUin(Long uin) {
        this.uin = uin;
    }

    public SimpleHttpSession getHttpSession() {
        return httpSession;
    }

    public void setHttpSession(SimpleHttpSession httpSession) {
        this.httpSession = httpSession;
    }
}
