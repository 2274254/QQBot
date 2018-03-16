package com.binklac.qqbot.eventmanager;

import com.binklac.qqbot.QQBotEvent;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import java.util.concurrent.*;

public class EventManager {
    private ConcurrentHashMap<Class, Integer> eventMap = new ConcurrentHashMap<>();
    private CopyOnWriteArrayList<EventHandlerInformation> eventHandlerList = new CopyOnWriteArrayList<>();
    private ThreadPoolExecutor eventExecutorPool = new ThreadPoolExecutor(Runtime.getRuntime().availableProcessors() / 2, Runtime.getRuntime().availableProcessors(), 20, TimeUnit.SECONDS, new LinkedBlockingQueue<>());

    private List<EventHandlerInformation> searchEventHandlerForDispatch(Integer eventIdentity) {
        List<EventHandlerInformation> resultList = Collections.synchronizedList(new ArrayList<>());

        if (eventIdentity != null) {
            for (EventHandlerInformation handler : eventHandlerList) {
                if (handler.getIdentity().equals(eventIdentity)) {
                    resultList.add(handler);
                }
            }
        }
        return resultList;
    }

    private List<EventHandlerInformation> searchEventHandlerInHandlerClass(Class eventHandlerClass) {
        List<EventHandlerInformation> resultList = Collections.synchronizedList(new ArrayList<>());

        if (eventHandlerClass != null) {
            for (EventHandlerInformation handler : eventHandlerList) {
                if (handler.getHandlerClassObject().getClass().equals(eventHandlerClass)) {
                    resultList.add(handler);
                }
            }
        }
        return resultList;
    }

    private List<EventHandlerInformation> searchEventHandlerInHandlerClass(Class eventHandlerClass, Integer eventIdentity) {
        List<EventHandlerInformation> resultList = Collections.synchronizedList(new ArrayList<>());
        List<EventHandlerInformation> eventHandlers = searchEventHandlerInHandlerClass(eventHandlerClass);

        if (!eventHandlers.isEmpty()) {
            for (EventHandlerInformation handler : eventHandlers) {
                if (handler.getIdentity().equals(eventIdentity)) {
                    resultList.add(handler);
                }
            }
        }
        return resultList;
    }

    private List<EventHandlerInformation> searchEventHandlerInHandlerClass(Class eventHandlerClass, Integer eventIdentity, EventPriority priority) {
        List<EventHandlerInformation> resultList = Collections.synchronizedList(new ArrayList<>());
        List<EventHandlerInformation> eventHandlers = searchEventHandlerInHandlerClass(eventHandlerClass, eventIdentity);

        if (!eventHandlers.isEmpty()) {
            for (EventHandlerInformation handler : eventHandlers) {
                if (handler.getPriority().compareTo(priority) == 0) {
                    resultList.add(handler);
                }
            }
        }
        return resultList;
    }

    public void registerEvent(Class eventClass, Integer type) {
        eventMap.putIfAbsent(eventClass, type);
    }

    public void registerEventHandler(Object eventHandlerObject) {
        Class eventHandlerClass = eventHandlerObject.getClass();
        Method[] eventHandlerClassMethodsList = eventHandlerClass.getDeclaredMethods();
        for (Method eventHandlerClassMethod : eventHandlerClassMethodsList) {
            EventHandler eventHandlerMethodAnnotation = eventHandlerClassMethod.getAnnotation(EventHandler.class);
            Class<?>[] eventHandlerMethodParameterTypes = eventHandlerClassMethod.getParameterTypes();

            if (eventHandlerMethodAnnotation != null && eventHandlerMethodParameterTypes.length == 1) {
                for (Map.Entry<Class, Integer> event : eventMap.entrySet()) {
                    if (event.getKey().equals(eventHandlerMethodParameterTypes[0])) {
                        if (searchEventHandlerInHandlerClass(eventHandlerClass, event.getValue(), eventHandlerMethodAnnotation.Priority()).isEmpty()) {
                            eventHandlerList.add(new EventHandlerInformation(event.getValue(), eventHandlerMethodAnnotation.Priority(), eventHandlerMethodAnnotation.isSlowEvent(), eventHandlerMethodAnnotation.eventTimeOut(), eventHandlerClassMethod, eventHandlerObject));
                        } else {
                            throw new RuntimeException("Event handler already exist in same class!");
                        }
                    }
                }
            }
        }
    }

    public synchronized void dispatchAsyncEvent(QQBotEvent event) {
        eventExecutorPool.execute(() -> {
            List<EventHandlerInformation> eventHandlers = searchEventHandlerForDispatch(eventMap.get(event.getClass()));
            Collections.sort(eventHandlers);
            for (EventHandlerInformation handler : eventHandlers) {
                try {
                    if (!event.isCancelled()) {
                        //TODO: 添加SlowEvent
                        handler.getHandlerMethod().invoke(handler.getHandlerClassObject(), event);
                    }
                } catch (IllegalAccessException | InvocationTargetException e) {
                    e.printStackTrace();
                }
            }
        });
    }
}
