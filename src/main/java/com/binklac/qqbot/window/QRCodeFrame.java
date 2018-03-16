package com.binklac.qqbot.window;

import java.awt.*;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.io.IOException;

public class QRCodeFrame extends Frame {
    public QRCodeFrame(byte[] qrcodeBuffer) throws IOException {
        this.addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent windowEvent) {
                windowEvent.getWindow().dispose();
            }
        });
        this.add(new QRCodePanel(qrcodeBuffer));
        this.setLocationRelativeTo(null);
        this.pack();
        this.setVisible(true);
    }

    public static QRCodeFrame showQRCode(byte[] qrcodeBuffer) throws IOException {
        return new QRCodeFrame(qrcodeBuffer);
    }
}