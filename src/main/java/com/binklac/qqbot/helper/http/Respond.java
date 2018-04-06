package com.binklac.qqbot.helper.http;


public class Respond {
    private final int RespondCode;
    private final int RespondSize;
    private final Boolean IsTextType;
    private final Boolean IsEmpty;
    private final String ContextType;
    private final String TextData;
    private final byte[] BinaryData;

    public Respond(int respondCode, int respondSize, Boolean isTextType, Boolean isEmpty, String contextType, String textData, byte[] binaryData) {
        RespondCode = respondCode;
        RespondSize = respondSize;
        IsTextType = isTextType;
        IsEmpty = isEmpty;
        ContextType = contextType;
        TextData = textData;
        BinaryData = binaryData;
    }

    public int getRespondCode() {
        return RespondCode;
    }

    public int getRespondSize() {
        return RespondSize;
    }

    public Boolean getTextType() {
        return IsTextType;
    }

    public Boolean getEmpty() {
        return IsEmpty;
    }

    public String getContextType() {
        return ContextType;
    }

    public String getTextData() {
        return TextData;
    }

    public byte[] getBinaryData() {
        return BinaryData;
    }
}

