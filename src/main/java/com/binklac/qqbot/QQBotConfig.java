package com.binklac.qqbot;

public class QQBotConfig {
    private boolean usePasswordLogin = false;
    private String webDriverClass = "org.openqa.selenium.chrome.ChromeDriver";
    private String webDriverName = "webdriver.chrome.driver";
    private String webDriverPath = "";
    private String uin = "";
    private String password = "";

    public boolean isUsePasswordLogin() {
        return usePasswordLogin;
    }

    public void setUsePasswordLogin(boolean usePasswordLogin) {
        this.usePasswordLogin = usePasswordLogin;
    }

    public String getUin() {
        return uin;
    }

    public void setUin(String uin) {
        this.uin = uin;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getWebDriverName() {
        return webDriverName;
    }

    public void setWebDriverName(String webDriverName) {
        this.webDriverName = webDriverName;
    }

    public String getWebDriverPath() {
        return webDriverPath;
    }

    public void setWebDriverPath(String webDriverPath) {
        this.webDriverPath = webDriverPath;
    }

    public String getWebDriverClass() {
        return webDriverClass;
    }

    public void setWebDriverClass(String webDriverClass) {
        this.webDriverClass = webDriverClass;
    }
}
