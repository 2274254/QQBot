package com.binklac.qqbot.helper.http;

import org.apache.http.Header;
import org.apache.http.HttpHeaders;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpUriRequest;
import org.apache.http.client.methods.RequestBuilder;
import org.apache.http.client.protocol.HttpClientContext;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.cookie.Cookie;
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
import java.util.Arrays;
import java.util.List;

public class SimpleHttpSession {
    private HttpClientContext httpClientContext = HttpClientContext.create();

    private final static List<String> TextTypes
            = new ArrayList<>(Arrays.asList(
            "text/",
            "application/json",
            "application/xml",
            "application/javascript"
    ));

    private static byte[] ReadBinaryFromRespondWithSize(InputStream respondInputStream, long contentLength) throws IOException {
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

    private static String ReadTextFromRespond(InputStream respondInputStream, String charset) throws IOException {
        StringBuilder respondBuffer = new StringBuilder();
        BufferedReader htmlReader = new BufferedReader(new InputStreamReader(respondInputStream));
        String html;

        while ((html = htmlReader.readLine()) != null) {
            if (charset != null) {
                respondBuffer.append(new String(html.getBytes(), charset));
            } else {
                respondBuffer.append(html);
            }
        }

        htmlReader.close();
        return respondBuffer.toString();
    }

    private static Boolean CheckIsTextType(String respondContentType) {
        if (respondContentType == null) {
            return false;
        } else {
            respondContentType = respondContentType.toLowerCase();
            for (String textType : TextTypes) {
                if (respondContentType.contains(textType)) {
                    return true;
                }
            }
            return false;
        }
    }

    private static String GetCharsetOrNull(String respondContentType) {
        if (respondContentType == null) {
            return null;
        } else {
            int charsetIndex = respondContentType.indexOf("charset");
            if (charsetIndex != -1) {
                int semicolonIndex = respondContentType.indexOf(';', charsetIndex);
                if (semicolonIndex != -1) {
                    return respondContentType.substring(respondContentType.indexOf('=', charsetIndex) + 1, semicolonIndex);
                } else {
                    return respondContentType.substring(respondContentType.indexOf('=', charsetIndex) + 1, respondContentType.length());
                }
            } else {
                return null;
            }
        }
    }

    public Respond doHttpPost(String url, String referer, String origin, List<NameValuePair> parameter) {
        CloseableHttpClient httpClient = null;

        try {
            URI uri = new URIBuilder(url).build();

            if ((referer != null && !referer.isEmpty()) || (origin != null && !origin.isEmpty())) {
                List<Header> headerList = new ArrayList<>();
                if (referer != null && !referer.isEmpty()) {
                    headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
                }

                if (origin != null && !origin.isEmpty()) {
                    headerList.add(new BasicHeader("Origin", origin));
                }

                httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
            } else {
                httpClient = HttpClients.createDefault();
            }

            HttpUriRequest httpUriRequest = RequestBuilder.post().setUri(uri).setEntity(new UrlEncodedFormEntity(parameter, "utf-8")).build();

            HttpResponse response = httpClient.execute(httpUriRequest, httpClientContext);


            boolean isTextType = CheckIsTextType(response.getEntity().getContentType().getValue());

            Boolean isEmptyRespond = true;
            String textData = null;
            byte[] binaryData = null;
            int trueContentLength = 0;

            if (isTextType) {
                textData = ReadTextFromRespond(response.getEntity().getContent(), GetCharsetOrNull(response.getEntity().getContentType().getValue()));
                if (textData != null && !textData.isEmpty()) {
                    isEmptyRespond = false;
                    trueContentLength = textData.length();
                }
            } else {
                binaryData = ReadBinaryFromRespondWithSize(response.getEntity().getContent(), response.getEntity().getContentLength());
                if (binaryData.length != 0) {
                    isEmptyRespond = false;
                    trueContentLength = binaryData.length;
                }
            }

            return new Respond(response.getStatusLine().getStatusCode(), trueContentLength, isTextType, isEmptyRespond, response.getEntity().getContentType().getValue(), textData, binaryData);
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

    public Respond doHttpGet(String url, String referer, String origin) {
        CloseableHttpClient httpClient = null;

        try {
            URI uri = new URIBuilder(url).build();

            if ((referer != null && !referer.isEmpty()) || (origin != null && !origin.isEmpty())) {
                List<Header> headerList = new ArrayList<>();
                if (referer != null && !referer.isEmpty()) {
                    headerList.add(new BasicHeader(HttpHeaders.REFERER, referer));
                }

                if (origin != null && !origin.isEmpty()) {
                    headerList.add(new BasicHeader("Origin", origin));
                }

                httpClient = HttpClients.custom().setDefaultHeaders(headerList).build();
            } else {
                httpClient = HttpClients.createDefault();
            }

            HttpUriRequest httpUriRequest = RequestBuilder.get().setUri(url).build();

            HttpResponse response = httpClient.execute(httpUriRequest, httpClientContext);
            boolean isTextType;
            Header contentType = response.getEntity().getContentType();
            if (contentType == null) {
                isTextType = false;
            } else {
                isTextType = CheckIsTextType(contentType.getValue());
            }


            Boolean isEmptyRespond = true;
            String textData = null;
            byte[] binaryData = null;
            int trueContentLength = 0;

            if (isTextType) {
                textData = ReadTextFromRespond(response.getEntity().getContent(), GetCharsetOrNull(response.getEntity().getContentType().getValue()));
                if (textData != null && !textData.isEmpty()) {
                    isEmptyRespond = false;
                    trueContentLength = textData.length();
                }
            } else {
                binaryData = ReadBinaryFromRespondWithSize(response.getEntity().getContent(), response.getEntity().getContentLength());
                if (binaryData.length != 0) {
                    isEmptyRespond = false;
                    trueContentLength = binaryData.length;
                }
            }

            return new Respond(response.getStatusLine().getStatusCode(), trueContentLength, isTextType, isEmptyRespond, contentType == null ? null : contentType.getValue(), textData, binaryData);
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

    public String getCookieByName(String name) {
        for (Cookie cookie : httpClientContext.getCookieStore().getCookies()) {
            if (cookie.getName().equalsIgnoreCase(name)) {
                return cookie.getValue();
            }
        }

        return null;
    }

    public HttpClientContext getHttpClientContext() {
        return httpClientContext;
    }

    public void setHttpClientContext(HttpClientContext httpClientContext) {
        this.httpClientContext = httpClientContext;
    }
}
