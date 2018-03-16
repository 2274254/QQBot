package com.binklac.qqbot;

public abstract class QQBotEvent {
    private boolean isCanCancel = false;
    private boolean isCancelled = false;

    protected void setEventCanCancel(boolean IsCanCancel) {
        this.isCanCancel = IsCanCancel;
    }

    public boolean cancelEvent() {
        if (isCanCancel) {
            isCancelled = true;
            return true;
        } else {
            return false;
        }
    }

    public boolean isCancelled() {
        return isCancelled;
    }
}