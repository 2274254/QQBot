package com.binklac.common;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public abstract class Singleton {
    static final Map<Class, Object> instances = new ConcurrentHashMap<>();

    Singleton(Object singletonObject) {
       if(singletonObject instanceof Singleton){
            instances.put(singletonObject.getClass(), singletonObject);
        }
    }

    static Object getInstance(Class clazz) {
        return instances.get(clazz);
    }
}
