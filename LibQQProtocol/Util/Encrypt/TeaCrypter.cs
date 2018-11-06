using System;

namespace LibQQProtocol.Util {
    public class TeaCrypter {
        // Token: 0x04000334 RID: 820
        private Int64 _contextStart;

        // Token: 0x04000335 RID: 821
        private Int64 _crypt;

        // Token: 0x04000336 RID: 822
        private Int64 _preCrypt;

        // Token: 0x04000337 RID: 823
        private Boolean _header;

        // Token: 0x04000338 RID: 824
        private Byte[] _key = new Byte[16];

        // Token: 0x04000339 RID: 825
        private Byte[] _out;

        // Token: 0x0400033A RID: 826
        private Int64 _padding;

        // Token: 0x0400033B RID: 827
        private Byte[] _plain;

        // Token: 0x0400033C RID: 828
        private Int64 _pos;

        // Token: 0x0400033D RID: 829
        private Byte[] _prePlain;

        // Token: 0x06000629 RID: 1577 RVA: 0x000256F4 File Offset: 0x000238F4
        public Byte[] MD5(Byte[] data) {
            return System.Security.Cryptography.MD5.Create().ComputeHash(data);
        }

        // Token: 0x0600062A RID: 1578 RVA: 0x00025710 File Offset: 0x00023910
        private Byte[] CopyMemory(Byte[] arr, Int32 arrIndex, Int64 input) {
            if (arrIndex + 4 > arr.Length) {
                return arr;
            }

            arr[arrIndex + 3] = (Byte)((input & 4278190080u) >> 24);
            arr[arrIndex + 2] = (Byte)((input & 0xFF0000) >> 16);
            arr[arrIndex + 1] = (Byte)((input & 0xFF00) >> 8);
            arr[arrIndex] = (Byte)(input & 0xFF);
            arr[arrIndex] &= Byte.MaxValue;
            arr[arrIndex + 1] &= Byte.MaxValue;
            arr[arrIndex + 2] &= Byte.MaxValue;
            arr[arrIndex + 3] &= Byte.MaxValue;
            return arr;
        }

        private Int64 CopyMemory(Int64 Out, Byte[] arr, Int32 arrIndex) {
            if (arrIndex + 4 > arr.Length) {
                return Out;
            }

            Int64 num = arr[arrIndex + 3] << 24;
            Int64 num2 = arr[arrIndex + 2] << 16;
            Int64 num3 = arr[arrIndex + 1] << 8;
            Int64 num4 = arr[arrIndex];
            var num5 = num | num2 | num3 | num4;
            return num5 & UInt32.MaxValue;
        }

        private Int64 GetUnsignedInt(Byte[] arrayIn, Int32 offset, Int32 len) {
            var num = 0L;
            var num2 = len <= 8 ? offset + len : offset + 8;
            for (var i = offset; i < num2; i++) {
                num <<= 8;
                num |= (UInt16)(arrayIn[i] & 0xFF);
            }

            return (num & UInt32.MaxValue) | (num >> 32);
        }

        // Token: 0x0600062D RID: 1581 RVA: 0x000258A8 File Offset: 0x00023AA8
        private Int64 Rand() {
            return 272L;
        }

        // Token: 0x0600062E RID: 1582 RVA: 0x000258C0 File Offset: 0x00023AC0
        private Byte[] Decipher(Byte[] arrayIn, Byte[] arrayKey, Int64 offset = 0L) {
            var arr = new Byte[24];
            var array = new Byte[8];
            if (arrayIn.Length < 8) {
                return array;
            }

            if (arrayKey.Length < 16) {
                return array;
            }

            var num = 3816266640L;
            num &= UInt32.MaxValue;
            var num2 = 2654435769L;
            num2 &= UInt32.MaxValue;
            var num3 = this.GetUnsignedInt(arrayIn, (Int32)offset, 4);
            var num4 = this.GetUnsignedInt(arrayIn, (Int32)offset + 4, 4);
            var unsignedInt = this.GetUnsignedInt(arrayKey, 0, 4);
            var unsignedInt2 = this.GetUnsignedInt(arrayKey, 4, 4);
            var unsignedInt3 = this.GetUnsignedInt(arrayKey, 8, 4);
            var unsignedInt4 = this.GetUnsignedInt(arrayKey, 12, 4);
            for (var i = 1; i <= 16; i++) {
                num4 -= ((num3 << 4) + unsignedInt3) ^ (num3 + num) ^ ((num3 >> 5) + unsignedInt4);
                num4 &= UInt32.MaxValue;
                num3 -= ((num4 << 4) + unsignedInt) ^ (num4 + num) ^ ((num4 >> 5) + unsignedInt2);
                num3 &= UInt32.MaxValue;
                num -= num2;
                num &= UInt32.MaxValue;
            }

            arr = this.CopyMemory(arr, 0, num3);
            arr = this.CopyMemory(arr, 4, num4);
            array[0] = arr[3];
            array[1] = arr[2];
            array[2] = arr[1];
            array[3] = arr[0];
            array[4] = arr[7];
            array[5] = arr[6];
            array[6] = arr[5];
            array[7] = arr[4];
            return array;
        }

        // Token: 0x06000630 RID: 1584 RVA: 0x00025A54 File Offset: 0x00023C54
        private Byte[] Encipher(Byte[] arrayIn, Byte[] arrayKey, Int64 offset = 0L) {
            var array = new Byte[8];
            var arr = new Byte[24];
            if (arrayIn.Length < 8 || arrayKey.Length < 16) {
                return array;
            }

            var num = 0L;
            var num2 = 2654435769L;
            num2 &= UInt32.MaxValue;
            var num3 = this.GetUnsignedInt(arrayIn, (Int32)offset, 4);
            var num4 = this.GetUnsignedInt(arrayIn, (Int32)offset + 4, 4);
            var unsignedInt = this.GetUnsignedInt(arrayKey, 0, 4);
            var unsignedInt2 = this.GetUnsignedInt(arrayKey, 4, 4);
            var unsignedInt3 = this.GetUnsignedInt(arrayKey, 8, 4);
            var unsignedInt4 = this.GetUnsignedInt(arrayKey, 12, 4);
            for (var i = 1; i <= 16; i++) {
                num += num2;
                num &= UInt32.MaxValue;
                num3 += ((num4 << 4) + unsignedInt) ^ (num4 + num) ^ ((num4 >> 5) + unsignedInt2);
                num3 &= UInt32.MaxValue;
                num4 += ((num3 << 4) + unsignedInt3) ^ (num3 + num) ^ ((num3 >> 5) + unsignedInt4);
                num4 &= UInt32.MaxValue;
            }

            arr = this.CopyMemory(arr, 0, num3);
            arr = this.CopyMemory(arr, 4, num4);
            array[0] = arr[3];
            array[1] = arr[2];
            array[2] = arr[1];
            array[3] = arr[0];
            array[4] = arr[7];
            array[5] = arr[6];
            array[6] = arr[5];
            array[7] = arr[4];
            return array;
        }

        // Token: 0x06000632 RID: 1586 RVA: 0x00025BD4 File Offset: 0x00023DD4
        private void Encrypt8Bytes() {
            for (this._pos = 0L; this._pos < 8; this._pos++) {
                if (this._header) {
                    this._plain[this._pos] = (Byte)(this._plain[this._pos] ^ this._prePlain[this._pos]);
                } else {
                    this._plain[this._pos] = (Byte)(this._plain[this._pos] ^ this._out[this._preCrypt + this._pos]);
                }
            }

            var array = this.Encipher(this._plain, this._key);
            for (var i = 0; i <= 7; i++) {
                this._out[this._crypt + i] = array[i];
            }

            for (this._pos = 0L; this._pos <= 7; this._pos++) {
                this._out[this._crypt + this._pos] = (Byte)(this._out[this._crypt + this._pos] ^ this._prePlain[this._pos]);
            }

            this._plain.CopyTo(this._prePlain, 0);
            this._preCrypt = this._crypt;
            this._crypt += 8L;
            this._pos = 0L;
            this._header = false;
        }

        // Token: 0x06000633 RID: 1587 RVA: 0x00025D88 File Offset: 0x00023F88
        private Boolean Decrypt8Bytes(Byte[] arrayIn, Int64 offset = 0L) {
            for (this._pos = 0L; this._pos <= 7; this._pos++) {
                if (this._contextStart + this._pos > arrayIn.Length - 1) {
                    return true;
                }

                this._prePlain[this._pos] = (Byte)(this._prePlain[this._pos] ^ arrayIn[offset + this._crypt + this._pos]);
            }

            try {
                this._prePlain = this.Decipher(this._prePlain, this._key);
            } catch {
                return false;
            }

            var num = this._prePlain.Length - 1;
            this._contextStart += 8L;
            this._crypt += 8L;
            this._pos = 0L;
            return true;
        }

        // Token: 0x06000635 RID: 1589 RVA: 0x00025EB0 File Offset: 0x000240B0
        public Byte[] Encrypt(Byte[] arrayIn, Byte[] arrayKey, Int64 offset) {
            this._plain = new Byte[8];
            this._prePlain = new Byte[8];
            this._pos = 1L;
            this._padding = 0L;
            this._crypt = this._preCrypt = 0L;
            this._key = arrayKey;
            this._header = true;
            this._pos = 2L;
            this._pos = (arrayIn.Length + 10) % 8;
            if (this._pos != 0) {
                this._pos = 8 - this._pos;
            }

            this._out = new Byte[arrayIn.Length + this._pos + 10];
            this._plain[0] = (Byte)((this.Rand() & 0xF8) | this._pos);
            for (var i = 1; i <= this._pos; i++) {
                this._plain[i] = (Byte)(this.Rand() & 0xFF);
            }

            this._pos++;
            this._padding = 1L;
            while (this._padding < 3) {
                if (this._pos < 8) {
                    this._plain[this._pos] = (Byte)(this.Rand() & 0xFF);
                    this._padding++;
                    this._pos++;
                } else if (this._pos == 8) {
                    this.Encrypt8Bytes();
                }
            }

            var num = (Int32)offset;
            Int64 num2 = arrayIn.Length;
            while (num2 > 0) {
                if (this._pos < 8) {
                    this._plain[this._pos] = arrayIn[num];
                    num++;
                    this._pos++;
                    num2--;
                } else if (this._pos == 8) {
                    this.Encrypt8Bytes();
                }
            }

            this._padding = 1L;
            while (this._padding < 9) {
                if (this._pos < 8) {
                    this._plain[this._pos] = 0;
                    this._pos++;
                    this._padding++;
                } else if (this._pos == 8) {
                    this.Encrypt8Bytes();
                }
            }

            return this._out;
        }

        // Token: 0x06000636 RID: 1590 RVA: 0x000261C0 File Offset: 0x000243C0
        public Byte[] Encrypt(Byte[] arrayIn, Byte[] arrayKey) {
            Byte[] array = null;
            var num = 0;
            while (array == null && num < 2) {
                try {
                    array = this.Encrypt(arrayIn, arrayKey, 0L);
                } catch {
                }

                num++;
            }

            return array;
        }

        // Token: 0x06000637 RID: 1591 RVA: 0x00026210 File Offset: 0x00024410
        public Byte[] Decrypt(Byte[] inData, Byte[] key) {
            var result = new Byte[0];
            try {
                result = this.Decrypt(inData, key, 0L);
            } catch {
            }

            return result;
        }

        // Token: 0x06000638 RID: 1592 RVA: 0x00026250 File Offset: 0x00024450
        public Byte[] Decrypt(Byte[] arrayIn, Byte[] arrayKey, Int64 offset) {
            var result = new Byte[0];
            if (arrayIn.Length < 16 || arrayIn.Length % 8 != 0) {
                return result;
            }

            var array = new Byte[offset + 8];
            arrayKey.CopyTo(this._key, 0);
            this._crypt = this._preCrypt = 0L;
            this._prePlain = this.Decipher(arrayIn, arrayKey, offset);
            this._pos = this._prePlain[0] & 7;
            var num = arrayIn.Length - this._pos - 10;
            if (num <= 0) {
                return result;
            }

            this._out = new Byte[num];
            this._preCrypt = 0L;
            this._crypt = 8L;
            this._contextStart = 8L;
            this._pos++;
            this._padding = 1L;
            while (this._padding < 3) {
                if (this._pos < 8) {
                    this._pos++;
                    this._padding++;
                } else if (this._pos == 8) {
                    for (var i = 0; i < array.Length; i++) {
                        array[i] = arrayIn[i];
                    }

                    if (!this.Decrypt8Bytes(arrayIn, offset)) {
                        return result;
                    }
                }
            }

            var num2 = 0L;
            while (num != 0) {
                if (this._pos < 8) {
                    this._out[num2] = (Byte)(array[offset + this._preCrypt + this._pos] ^ this._prePlain[this._pos]);
                    num2++;
                    num--;
                    this._pos++;
                } else if (this._pos == 8) {
                    array = arrayIn;
                    this._preCrypt = this._crypt - 8;
                    if (!this.Decrypt8Bytes(arrayIn, offset)) {
                        return result;
                    }
                }
            }

            for (this._padding = 1L; this._padding <= 7; this._padding++) {
                if (this._pos < 8) {
                    if ((array[offset + this._preCrypt + this._pos] ^ this._prePlain[this._pos]) != 0) {
                        return result;
                    }

                    this._pos++;
                } else if (this._pos == 8) {
                    for (var i = 0; i < array.Length; i++) {
                        array[i] = arrayIn[i];
                    }

                    this._preCrypt = this._crypt;
                    if (!this.Decrypt8Bytes(arrayIn, offset)) {
                        return result;
                    }
                }
            }

            return this._out;
        }
    }
}
