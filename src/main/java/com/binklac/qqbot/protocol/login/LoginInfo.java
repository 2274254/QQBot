package com.binklac.qqbot.protocol.login;

import org.apache.http.client.protocol.HttpClientContext;

public class LoginInfo {
    private final String clientId = "53999199";
    private HttpClientContext httpClientContext = HttpClientContext.create();
    private String ptWebQQ = null;
    private String vfWebQQ = null;
    private Long uin = null;
    private String pSessionId = null;
    private String qrScannedResult = null;
    private String urlToGetPtWebQQ = null;

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

    public String getPSessionId() {
        return pSessionId;
    }

    public void setPSessionId(String pSessionId) {
        this.pSessionId = pSessionId;
    }

    public String getQrScannedResult() {
        return qrScannedResult;
    }

    public void setQrScannedResult(String qrScannedResult) {
        this.qrScannedResult = qrScannedResult;
    }

    public String getUrlToGetPtWebQQ() {
        return urlToGetPtWebQQ;
    }

    public void setUrlToGetPtWebQQ(String urlToGetPtWebQQ) {
        this.urlToGetPtWebQQ = urlToGetPtWebQQ;
    }

    public Long getUin() {
        return uin;
    }

    public void setUin(Long uin) {
        this.uin = uin;
    }
}
