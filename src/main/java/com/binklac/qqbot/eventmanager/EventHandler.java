package com.binklac.qqbot.eventmanager;

import java.lang.annotation.*;

@Retention(RetentionPolicy.RUNTIME)
@Target({ElementType.METHOD})
@Documented
public @interface EventHandler {
    EventPriority Priority() default EventPriority.NORMAL;

    boolean isSlowEvent() default false;

    long eventTimeOut() default 0;
}
