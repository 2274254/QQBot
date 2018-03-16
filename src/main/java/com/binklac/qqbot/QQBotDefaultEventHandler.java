package com.binklac.qqbot;

import com.binklac.qqbot.eventmanager.EventHandler;
import com.binklac.qqbot.eventmanager.EventPriority;
import com.binklac.qqbot.events.GetQRCodeEvent;
import com.binklac.qqbot.events.LoginEvent;
import com.binklac.qqbot.window.QRCodeFrame;

import java.awt.*;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;

public class QQBotDefaultEventHandler {
    @EventHandler(Priority = EventPriority.HIGHEST)
    public void onBotLoginSuccess(LoginEvent event) {
        System.out.println("QQ [" + event.getQQNumber() + "] 登录成功!");
    }

    @EventHandler(Priority = EventPriority.HIGHEST)
    public void onQRLoginShowOnScreen(GetQRCodeEvent event) {
        try {
            QRCodeFrame.showQRCode(event.getQrcode());
            event.cancelEvent();
        } catch (Exception ignored) {
        }
    }

    @EventHandler(Priority = EventPriority.LOWEST)
    public void onQRLoginWriteToFile(GetQRCodeEvent Event) throws IOException {
        File qrcodePngFile = File.createTempFile("qqbot_qrlogin_", ".png");
        FileOutputStream pngWriter = new FileOutputStream(qrcodePngFile);
        pngWriter.write(Event.getQrcode());
        pngWriter.flush();
        pngWriter.close();

        try {
            Desktop.getDesktop().open(qrcodePngFile);
        } catch (Exception e) {
            System.out.println("file is saved on:" + qrcodePngFile.getCanonicalPath());
        }

        Event.cancelEvent();
    }
}
