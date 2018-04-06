package com.binklac.qqbot.protocol.message;

import com.alibaba.fastjson.JSONObject;
import com.binklac.qqbot.helper.TimeoutCallable;
import com.binklac.qqbot.protocol.login.LoginInfo;
import org.apache.http.Header;
import org.apache.http.HttpHeaders;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpUriRequest;
import org.apache.http.client.methods.RequestBuilder;
import org.apache.http.client.protocol.HttpClientContext;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.LinkedBlockingQueue;
import java.util.concurrent.SynchronousQueue;
import java.util.concurrent.ThreadPoolExecutor;
import java.util.concurrent.TimeUnit;

public class MessagePoller {
    private final static String url = "https://d1.web2.qq.com/channel/poll2";
    private final static String referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
    private SynchronousQueue<String> resultQueue = new SynchronousQueue<>();
    private ThreadPoolExecutor eventExecutorPool;
    private final List<Header> headerList = new ArrayList<>();
    private final HttpUriRequest httpUriRequest;
    private final HttpClientContext context;
    private boolean isStartPoll = false;

    public MessagePoller(LoginInfo loginInfo, int threadPollSize) throws URISyntaxException, UnsupportedEncodingException {
        eventExecutorPool = new ThreadPoolExecutor(threadPollSize, threadPollSize, 20, TimeUnit.SECONDS, new LinkedBlockingQueue<>());
        headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
        JSONObject r = new JSONObject();
        r.put("ptwebqq", loginInfo.getPtWebQQ());
        r.put("clientid", Long.parseLong(loginInfo.getClientId()));
        r.put("psessionid", loginInfo.getPSessionId());
        r.put("key", "");
        List<NameValuePair> parameter = new ArrayList<>();
        parameter.add(new BasicNameValuePair("r", r.toJSONString()));
        URI uri = new URIBuilder(url).build();
        httpUriRequest = RequestBuilder.post().setUri(uri).setEntity(new UrlEncodedFormEntity(parameter, "utf-8")).build();
        context = loginInfo.getHttpSession().getHttpClientContext();
    }

    private void pollMessage(CloseableHttpClient httpClient) throws Exception {
        String result = new TimeoutCallable<String>(() -> {
            HttpResponse response = httpClient.execute(httpUriRequest, context);
            BufferedReader reader = new BufferedReader(new InputStreamReader(response.getEntity().getContent(), "UTF-8"));
            return reader.readLine();
        }, 5000).call();
        if (result != null) {
            resultQueue.add(result);
        }
    }

    public void startPollMessage() {
        if (!isStartPoll) {
            this.isStartPoll = true;

            for (int i = 0; i < eventExecutorPool.getMaximumPoolSize(); i++) {
                eventExecutorPool.execute(() -> {
                    CloseableHttpClient httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
                    while (isStartPoll) {
                        try {
                            pollMessage(httpClient);
                        } catch (Exception e) {
                            if (httpClient != null) {
                                try {
                                    httpClient.close();
                                } catch (IOException ignored) {
                                }
                            }
                            httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
                        }
                    }
                });
            }
        }
    }

    public void stopPoll() {
        this.isStartPoll = false;
        new Timer().schedule(new TimerTask() {
            @Override
            public void run() {
                eventExecutorPool.shutdownNow();
            }
        }, 5000);
    }

    public String getMessage() {
        try {
            return resultQueue.take();
        } catch (InterruptedException e) {
            return null;
        }
    }
}
