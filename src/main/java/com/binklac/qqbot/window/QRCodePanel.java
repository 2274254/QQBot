package com.binklac.qqbot.window;

import javax.imageio.ImageIO;
import javax.imageio.stream.ImageInputStream;
import javax.imageio.stream.MemoryCacheImageInputStream;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.IOException;

public class QRCodePanel extends Panel {
    private final BufferedImage qrcodeImage;

    public QRCodePanel(byte[] qrcodeBuffer) throws IOException {
        qrcodeImage = coverByteDataToImage(qrcodeBuffer);
        setPreferredSize(new Dimension(qrcodeImage.getWidth() * 2, qrcodeImage.getHeight() * 2));
    }

    private static BufferedImage coverByteDataToImage(byte[] qrcodeBuffer) throws IOException {
        ImageInputStream imageInputstream = new MemoryCacheImageInputStream(new ByteArrayInputStream(qrcodeBuffer));
        return ImageIO.read(imageInputstream);
    }

    @Override
    public void paint(Graphics graphics) {
        graphics.drawImage(qrcodeImage, qrcodeImage.getWidth() / 2, qrcodeImage.getHeight() / 2, null);
    }
}