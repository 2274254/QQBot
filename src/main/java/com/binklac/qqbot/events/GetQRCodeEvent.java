package com.binklac.qqbot.events;

import com.binklac.qqbot.QQBotEvent;

public class GetQRCodeEvent extends QQBotEvent {
    private final byte[] qrcodeData;

    public GetQRCodeEvent(byte[] qrcode) {
        this.setEventCanCancel(true);
        this.qrcodeData = qrcode;
    }

    public byte[] getQrcode() {
        return qrcodeData;
    }
}
