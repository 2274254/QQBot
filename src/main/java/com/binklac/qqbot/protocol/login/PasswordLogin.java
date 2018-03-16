package com.binklac.qqbot.protocol.login;

import org.apache.http.client.CookieStore;
import org.apache.http.client.protocol.HttpClientContext;
import org.apache.http.impl.cookie.BasicClientCookie;
import org.openqa.selenium.*;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class PasswordLogin {
    private final static Logger logger = LoggerFactory.getLogger(PasswordLogin.class);
    private final LoginInfo loginInfo;
    private final WebDriver webDriver;
    private final WebDriverWait webDriverWait;
    private final JavascriptExecutor jsExecutor;

    public PasswordLogin(LoginInfo loginInfo, WebDriver webDriver) {
        this.loginInfo = loginInfo;
        this.webDriver = webDriver;
        this.webDriverWait = new WebDriverWait(webDriver, 10);
        this.jsExecutor = (JavascriptExecutor) webDriver;
    }

    private boolean loginToQZone(String number, String password) {
        try {
            logger.info("尝试载入页面<http://m.qzone.com>...");
            webDriver.get("http://m.qzone.com");
            webDriver.manage().window().maximize();
            webDriverWait.until(ExpectedConditions.elementToBeClickable(By.id("go")));
            webDriver.findElement(By.id("u")).clear();
            webDriver.findElement(By.id("u")).sendKeys(number);
            webDriver.findElement(By.id("p")).clear();
            webDriver.findElement(By.id("p")).sendKeys(password);
            webDriver.findElement(By.id("go")).click();
            webDriverWait.until(ExpectedConditions.elementToBeClickable(By.id("header")));
            logger.info("成功登录到QQ空间...");
            return true;
        } catch (Exception e) {
            logger.error("登录QQ空间失败!");
            return false;
        }
    }

    private boolean isScriptReady() {
        boolean isNeedRefresh = true;
        int refreshCount = 0;
        int getCount = 0;
        String StringVal = "";

        do {
            try {
                Thread.sleep(2000);
            } catch (InterruptedException ignored) {
            }

            if (isNeedRefresh) {
                try {
                    logger.info("尝试载入页面<http://web2.qq.com>...");
                    webDriver.get("http://web2.qq.com");
                    webDriverWait.until(ExpectedConditions.presenceOfElementLocated(By.className("container")));
                    webDriverWait.until(ExpectedConditions.javaScriptThrowsNoExceptions("console.log(mq);"));
                } catch (Exception ignored) {
                }
                refreshCount++;
                isNeedRefresh = false;
            } else {
                try {
                    StringVal = (String) jsExecutor.executeScript("return mq.vfwebqq");
                } catch (Exception ignored) {
                }

                if (getCount++ > 3) {
                    logger.warn("连续三次无法取回结果...刷新网页...");
                    getCount = 0;
                    isNeedRefresh = true;
                }
            }

            if (refreshCount > 5) {
                webDriver.close();
                logger.error("多次重试后无法正常执行脚本!");
                return false;
            }
        } while (StringVal.length() == 0);
        logger.info("脚本系统已经准备好...");
        return true;
    }


    private boolean getLoginToken() {
        webDriver.get("http://web2.qq.com");
        try {
            webDriver.switchTo().frame("ptlogin");
            webDriverWait.until(ExpectedConditions.elementToBeClickable(By.className("face")));
            webDriver.findElement(By.className("face")).click();
        } catch (NoSuchFrameException | NoSuchElementException | TimeoutException ignored) {
        }

        if (isScriptReady()) {
            HttpClientContext httpClientContext = this.loginInfo.getHttpClientContext();
            CookieStore cookieStore = httpClientContext.getCookieStore();
            for (Cookie  webDriverCookie : webDriver.manage().getCookies()) {
                BasicClientCookie cookie = new BasicClientCookie(webDriverCookie.getName(), webDriverCookie.getValue());
                cookie.setDomain(webDriverCookie.getDomain());
                cookie.setExpiryDate(webDriverCookie.getExpiry());
                cookie.setPath(webDriverCookie.getPath());
                cookie.setSecure(webDriverCookie.isSecure());
                cookieStore.addCookie(cookie);
            }
            httpClientContext.setCookieStore(cookieStore);
            loginInfo.setHttpClientContext(httpClientContext);
            loginInfo.setVfWebQQ((String) jsExecutor.executeScript("return mq.vfwebqq"));
            loginInfo.setPtWebQQ((String) jsExecutor.executeScript("return mq.ptwebqq"));
            loginInfo.setPSessionId((String) jsExecutor.executeScript("return mq.psessionid"));
            return true;
        }
        return false;
    }

    public boolean login(String number, String password) {
        try {
            return loginToQZone(number, password) && getLoginToken();
        } finally {
            webDriver.close();
        }
    }
}

