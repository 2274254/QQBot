package com.binklac.qqbot.protocol.message.structure;

public class Font {
    private int[] style;
    private String color;
    private String name;
    private int size;

    public int[] getStyle() {
        return style;
    }

    public void setStyle(int[] style) {
        this.style = style;
    }

    public String getColor() {
        return color;
    }

    public void setColor(String color) {
        this.color = color;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getSize() {
        return size;
    }

    public void setSize(int size) {
        this.size = size;
    }
}
