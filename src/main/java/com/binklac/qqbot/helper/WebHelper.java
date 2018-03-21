package com.binklac.qqbot.helper;

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

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URI;
import java.net.URISyntaxException;
import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public class WebHelper {

    public static byte[] ReadBinaryFromRespondWithSize(InputStream respondInputStream, long contentLength) throws IOException {
        ByteBuffer respondByteBuffer = ByteBuffer.allocate((int) contentLength);
        int availableSize;
        while ((availableSize = respondInputStream.available()) != 0) {
            byte[] tempBuffer = new byte[availableSize];
            int realReadSize = respondInputStream.read(tempBuffer);
            if (realReadSize > 0) {
                respondByteBuffer.put(tempBuffer, 0, realReadSize);
            }
        }
        return respondByteBuffer.array();
    }

    public static String getGetString(LoginInfo loginInfo, String url, String referer) {
        CloseableHttpClient httpClient = null;
        try {
            URI uri = new URIBuilder(url).build();
            List<Header> headerList = new ArrayList<>();
            headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
            httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
            HttpUriRequest httpUriRequest = RequestBuilder.get().setUri(uri).build();
            HttpClientContext context = loginInfo.getHttpClientContext();
            HttpResponse response = httpClient.execute(httpUriRequest, context);
            //return new String(ReadBinaryFromRespondWithSize(response.getEntity().getContent(), response.getEntity().getContentLength()));
            BufferedReader reader = new BufferedReader(new InputStreamReader(response.getEntity().getContent(), "UTF-8"));
            return reader.readLine();

        } catch (URISyntaxException | IOException e) {
            return null;
        } finally {
            if (httpClient != null) {
                try {
                    httpClient.close();
                } catch (IOException ignored) {
                }
            }
        }
    }

    public static String getGetString(LoginInfo loginInfo, String url) {
        CloseableHttpClient httpClient = null;

        try {
            URI uri = new URIBuilder(url).build();
            httpClient = HttpClients.createDefault();
            HttpUriRequest httpUriRequest = RequestBuilder.get().setUri(uri).build();
            HttpClientContext context = loginInfo.getHttpClientContext();
            HttpResponse response = httpClient.execute(httpUriRequest, context);
            return new String(ReadBinaryFromRespondWithSize(response.getEntity().getContent(), response.getEntity().getContentLength()));
        } catch (URISyntaxException | IOException e) {
            return null;
        } finally {
            if (httpClient != null) {
                try {
                    httpClient.close();
                } catch (IOException ignored) {
                }
            }
        }
    }

    public static String getPostString(LoginInfo loginInfo, String url, List<NameValuePair> parameter) {
        CloseableHttpClient httpClient = null;

        try {
            URI uri = new URIBuilder(url).build();
            httpClient = HttpClients.createDefault();
            HttpUriRequest httpUriRequest = RequestBuilder.post().setUri(uri).setEntity(new UrlEncodedFormEntity(parameter, "utf-8")).build();
            HttpClientContext context = loginInfo.getHttpClientContext();
            HttpResponse response = httpClient.execute(httpUriRequest, context);
            BufferedReader reader = new BufferedReader(new InputStreamReader(response.getEntity().getContent(), "UTF-8"));
            return reader.readLine();
        } catch (URISyntaxException | IOException e) {
            return null;
        } finally {
            if (httpClient != null) {
                try {
                    httpClient.close();
                } catch (IOException ignored) {
                }
            }
        }
    }

    public static String getPostString(LoginInfo loginInfo, String url, String referer, List<NameValuePair> parameter) {
        CloseableHttpClient httpClient = null;

        try {
            URI uri = new URIBuilder(url).build();
            List<Header> headerList = new ArrayList<>();
            headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
            httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
            HttpUriRequest httpUriRequest = RequestBuilder.post().setUri(uri).setEntity(new UrlEncodedFormEntity(parameter, "utf-8")).build();
            HttpClientContext context = loginInfo.getHttpClientContext();
            HttpResponse response = httpClient.execute(httpUriRequest, context);
            BufferedReader reader = new BufferedReader(new InputStreamReader(response.getEntity().getContent(), "UTF-8"));
            return reader.readLine();
        } catch (URISyntaxException | IOException e) {
            return null;
        } finally {
            if (httpClient != null) {
                try {
                    httpClient.close();
                } catch (IOException ignored) {
                }
            }
        }
    }

    public static String getPostString(LoginInfo loginInfo, String url, String referer, String origin, List<NameValuePair> parameter) {
        CloseableHttpClient httpClient = null;

        try {
            URI uri = new URIBuilder(url).build();
            List<Header> headerList = new ArrayList<>();
            headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
            headerList.add(new BasicHeader("Origin", origin));
            httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
            HttpUriRequest httpUriRequest = RequestBuilder.post().setUri(uri).setEntity(new UrlEncodedFormEntity(parameter, "utf-8")).build();
            HttpClientContext context = loginInfo.getHttpClientContext();
            HttpResponse response = httpClient.execute(httpUriRequest, context);
            BufferedReader reader = new BufferedReader(new InputStreamReader(response.getEntity().getContent(), "UTF-8"));
            return reader.readLine();
        } catch (URISyntaxException | IOException e) {
            return null;
        } finally {
            if (httpClient != null) {
                try {
                    httpClient.close();
                } catch (IOException ignored) {
                }
            }
        }
    }
}
