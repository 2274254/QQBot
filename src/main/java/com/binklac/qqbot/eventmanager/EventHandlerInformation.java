package com.binklac.qqbot.eventmanager;

import java.lang.reflect.Method;

public class EventHandlerInformation implements Comparable<EventHandlerInformation> {
    private final Integer identity;
    private final EventPriority priority;
    private final Boolean isSlowEvent;
    private final Long eventTimeOut;
    private final Method handlerMethod;
    private final Object handlerClassObject;

    public EventHandlerInformation(Integer identity, EventPriority priority, Boolean isSlowEvent, Long eventTimeOut, Method handlerMethod, Object handlerClassObject) {
        this.identity = identity;
        this.priority = priority;
        this.isSlowEvent = isSlowEvent;
        this.eventTimeOut = eventTimeOut;
        this.handlerMethod = handlerMethod;
        this.handlerClassObject = handlerClassObject;
    }

    public Integer getIdentity() {
        return identity;
    }

    public EventPriority getPriority() {
        return priority;
    }

    public Boolean getSlowEvent() {
        return isSlowEvent;
    }

    public Long getEventTimeOut() {
        return eventTimeOut;
    }

    public Method getHandlerMethod() {
        return handlerMethod;
    }

    public Object getHandlerClassObject() {
        return handlerClassObject;
    }

    @Override
    public int compareTo(EventHandlerInformation eventInformation) {
        return eventInformation.getPriority().compareTo(this.getPriority());
    }
}