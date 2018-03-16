package com.binklac.qqbot.protocol.hash;

public class QQHash {
    public static String QHash(String QQNumberString, String K) {
        long QQNumber = Long.parseLong(QQNumberString);
        int[] N = new int[]{0, 0, 0, 0};
        for (int T = 0; T < K.length(); T++) {
            N[T % 4] ^= (int) K.charAt(T);
        }
        String U = "ECOK";
        long[] V = new long[]{0, 0, 0, 0};
        V[0] = ((QQNumber >> 24) & 255) ^ (int) U.charAt(0);
        V[1] = ((QQNumber >> 16) & 255) ^ (int) U.charAt(1);
        V[2] = ((QQNumber >> 8) & 255) ^ (int) U.charAt(2);
        V[3] = ((QQNumber) & 255) ^ (int) U.charAt(3);
        long[] U1 = new long[]{0, 0, 0, 0, 0, 0, 0, 0};
        for (int T = 0; T < 8; T++) {
            U1[T] = T % 2 == 0 ? N[T >> 1] : V[T >> 1];
        }
        String N1 = "0123456789ABCDEF";
        StringBuilder V1 = new StringBuilder();
        for (long aU1 : U1) {
            V1.append(N1.charAt((int) ((aU1 >> 4) & 15)));
            V1.append(N1.charAt((int) ((aU1) & 15)));
        }
        return V1.toString();
    }

    public static String BKNHash(String sKey) {
        return BKNHash(sKey, 5381);
    }

    public static String BKNHash(String sKey, int IntiNumber) {
        int Hash = IntiNumber;
        for (int T = 0; T < sKey.length(); T++) {
            Hash += (Hash << 5) + (int) sKey.charAt(T);
        }
        Hash = Hash & 2147483647;
        return String.valueOf(Hash);
    }
}
