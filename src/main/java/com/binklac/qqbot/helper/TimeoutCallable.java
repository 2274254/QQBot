package com.binklac.qqbot.helper;

import java.util.concurrent.*;

public class TimeoutCallable<V> implements Callable<V> {
    private final Callable<V> callable;
    private final long timeout;

    public TimeoutCallable(Callable<V> callable, long timeout) {
        this.timeout = timeout;
        this.callable = callable;
    }

    @Override
    public V call() throws Exception {
        ExecutorService executor = Executors.newSingleThreadExecutor();
        Future<V> future = executor.submit(callable);
        V result = future.get(timeout, TimeUnit.MILLISECONDS);
        executor.shutdownNow();
        return result;
    }

}