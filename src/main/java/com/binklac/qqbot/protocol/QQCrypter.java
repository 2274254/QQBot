package com.binklac.qqbot.protocol;

import java.util.Random;

public class QQCrypter {
    private static void code(byte[] In, int inOffset, int inPos, byte[] Out, int outOffset, int outPos, byte[] key) {
        if (outPos > 0) {
            for (int i = 0; i < 8; i++) {
                In[outOffset + outPos + i] = (byte) (In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8]);
            }
        }
        int[] formattedKey = FormatKey(key);
        int y = ConvertByteArrayToUInt(In, outOffset + outPos);
        int z = ConvertByteArrayToUInt(In, outOffset + outPos + 4);
        int sum = 0;
        int delta = 0x9e3779b9;
        int n = 16;

        while (n-- > 0) {
            sum += delta;
            y += ((z << 4) + formattedKey[0]) ^ (z + sum) ^ ((z >> 5) + formattedKey[1]);
            z += ((y << 4) + formattedKey[2]) ^ (y + sum) ^ ((y >> 5) + formattedKey[3]);
        }
        System.arraycopy(ConvertUIntToByteArray(y), 0, Out, outOffset + outPos, 4);
        System.arraycopy(ConvertUIntToByteArray(z), 0, Out, outOffset + outPos + 4, 4);
        if (inPos > 0) {
            for (int i = 0; i < 8; i++) {
                Out[outOffset + outPos + i] = (byte) (Out[outOffset + outPos + i] ^ In[inOffset + inPos + i - 8]);
            }
        }
    }

    private static void decode(byte[] In, int inOffset, int inPos, byte[] Out, int outOffset, int outPos, byte[] key) {
        if (outPos > 0) {
            for (int i = 0; i < 8; i++) {
                Out[outOffset + outPos + i] = (byte) (In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8]);
            }
        } else {
            System.arraycopy(In, inOffset, Out, outOffset, 8);
        }
        int[] formattedKey = FormatKey(key);
        int y = ConvertByteArrayToUInt(Out, outOffset + outPos);
        int z = ConvertByteArrayToUInt(Out, outOffset + outPos + 4);
        int sum = 0xE3779B90;
        int delta = 0x9e3779b9;
        int n = 16;

        while (n-- > 0) {
            z -= ((y << 4) + formattedKey[2]) ^ (y + sum) ^ ((y >> 5) + formattedKey[3]);
            y -= ((z << 4) + formattedKey[0]) ^ (z + sum) ^ ((z >> 5) + formattedKey[1]);
            sum -= delta;
        }
        System.arraycopy(ConvertUIntToByteArray(y), 0, Out, outOffset + outPos, 4);
        System.arraycopy(ConvertUIntToByteArray(z), 0, Out, outOffset + outPos + 4, 4);
    }

    public static byte[] Decrypt(byte[] In, int offset, int len, byte[] key) {
        if ((len % 8 != 0) || (len < 16)) {
            return null;
        }
        byte[] Out = new byte[len];
        for (int i = 0; i < len; i += 8) {
            decode(In, offset, i, Out, 0, i, key);
        }
        for (int i = 8; i < len; i++) {
            Out[i] = (byte) (Out[i] ^ In[offset + i - 8]);
        }
        int pos = Out[0] & 0x07;
        len = len - pos - 10;
        byte[] res = new byte[len];
        System.arraycopy(Out, pos + 3, res, 0, len);
        return res;
    }

    public static byte[] Encrypt(byte[] In, int offset, int len, byte[] key) {
        // 计算头部填充字节数
        int pos = (len + 10) % 8;
        if (pos != 0) {
            pos = 8 - pos;
        }
        byte[] plain = new byte[len + pos + 10];
        Random Rnd = new Random();
        plain[0] = (byte) ((Rnd.nextInt(255) & 0xF8) | pos);
        for (int i = 1; i < pos + 3; i++) {
            plain[i] = (byte) (Rnd.nextInt(255) & 0xFF);
        }
        System.arraycopy(In, 0, plain, pos + 3, len);
        for (int i = pos + 3 + len; i < plain.length; i++) {
            plain[i] = 0x0;
        }
        // 定义输出流
        byte[] outer = new byte[len + pos + 10];
        for (int i = 0; i < outer.length; i += 8) {
            code(plain, 0, i, outer, 0, i, key);
        }
        return outer;
    }

    private static int[] FormatKey(byte[] key) {
        if (key.length == 0) {
            throw new RuntimeException("Key must be between 1 and 16 characters in length");
        }
        byte[] refineKey = new byte[16];
        if (key.length < 16) {
            System.arraycopy(key, 0, refineKey, 0, key.length);
            for (int k = key.length; k < 16; k++) {
                refineKey[k] = 0x20;
            }
        } else {
            System.arraycopy(key, 0, refineKey, 0, 16);
        }

        int[] formattedKey = new int[4];
        int j = 0;
        for (int i = 0; i < refineKey.length; i += 4) {
            formattedKey[j++] = ConvertByteArrayToUInt(refineKey, i);
        }
        return formattedKey;
    }

    private static byte[] ConvertUIntToByteArray(int v) {
        byte[] result = new byte[4];
        result[0] = (byte) ((v >> 24) & 0xFF);
        result[1] = (byte) ((v >> 16) & 0xFF);
        result[2] = (byte) ((v >> 8) & 0xFF);
        result[3] = (byte) ((v >> 0) & 0xFF);
        return result;
    }

    private static int ConvertByteArrayToUInt(byte[] v, int offset) {
        if (offset + 4 > v.length) {
            return 0;
        }
        int output;
        output = (v[offset] << 24);
        output |= (v[offset + 1] << 16);
        output |= (v[offset + 2] << 8);
        output |= (v[offset + 3]);
        return output;
    }
}
