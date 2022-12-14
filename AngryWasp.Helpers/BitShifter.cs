using System.Collections.Generic;

namespace AngryWasp.Helpers
{
    public static class BitShifter
    {
        #region ToByte

        public static byte[] ToByte(this bool value)
        {
            return new byte[] { value ? (byte)1 : (byte)0 };
        }

        public static byte[] ToByte(this short value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8)
            };
        }

        public static byte[] ToByte(this ushort value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8)
            };
        }

        public static byte[] ToByte(this int value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24)
            };
        }

        public static byte[] ToByte(this uint value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24)
            };
        }

        public static byte[] ToByte(this long value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24),
                (byte)(value >> 32),
                (byte)(value >> 40),
                (byte)(value >> 48),
                (byte)(value >> 56)
            };
        }

        public static byte[] ToByte(this ulong value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24),
                (byte)(value >> 32),
                (byte)(value >> 40),
                (byte)(value >> 48),
                (byte)(value >> 56)
            };
        }

        public static unsafe byte[] ToByte(this float value)
        {
            uint num = *(uint*)(&value);
            return new byte[]
            {
                (byte)num,
                (byte)(num >> 8),
                (byte)(num >> 16),
                (byte)(num >> 24)
            };
        }

        public static unsafe byte[] ToByte(this double value)
        {
            ulong num = *(ulong*)(&value);
            return new byte[]
            {
                (byte)num,
                (byte)(num >> 8),
                (byte)(num >> 16),
                (byte)(num >> 24),
                (byte)(num >> 32),
                (byte)(num >> 40),
                (byte)(num >> 48),
                (byte)(num >> 56),
            };
        }

        #endregion

        #region FromByte

        public static bool ToBoolean(this byte[] value)
        {
            return value[0] == 1;
        }

        public static bool ToBoolean(this List<byte> value)
        {
            return value[0] == 1;
        }

        public static bool ToBoolean(this byte[] value, ref int start)
        {
            return value[start++] == 1;
        }

        public static bool ToBoolean(this List<byte> value, ref int start)
        {
            return value[start++] == 1;
        }

        public static short ToShort(this byte[] value)
        {
            return (short)(value[0] | value[1] << 8);
        }

        public static short ToShort(this List<byte> value)
        {
            return (short)(value[0] | value[1] << 8);
        }

        public static short ToShort(this byte[] value, ref int start)
        {
            return (short)(value[start++] | value[start++] << 8);
        }

        public static short ToShort(this List<byte> value, ref int start)
        {
            return (short)(value[start++] | value[start++] << 8);
        }

        public static ushort ToUShort(this byte[] value)
        {
            return (ushort)(value[0] | value[1] << 8);
        }

        public static ushort ToUShort(this List<byte> value)
        {
            return (ushort)(value[0] | value[1] << 8);
        }

        public static ushort ToUShort(this byte[] value, ref int start)
        {
            return (ushort)(value[start++] | value[start++] << 8);
        }

        public static ushort ToUShort(this List<byte> value, ref int start)
        {
            return (ushort)(value[start++] | value[start++] << 8);
        }

        public static int ToInt(this byte[] value)
        {
            return value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24;
        }

        public static int ToInt(this List<byte> value)
        {
            return value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24;
        }

        public static int ToInt(this byte[] value, ref int start)
        {
            return value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24;
        }

        public static int ToInt(this List<byte> value, ref int start)
        {
            return value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24;
        }

        public static uint ToUInt(this byte[] value)
        {
            return (uint)(value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24);
        }

        public static uint ToUInt(this List<byte> value)
        {
            return (uint)(value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24);
        }

        public static uint ToUInt(this byte[] value, ref int start)
        {
            return (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
        }

        public static uint ToUInt(this List<byte> value, ref int start)
        {
            return (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
        }

        public static long ToLong(this byte[] value)
        {
            uint num = (uint)(value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24);
            uint num2 = (uint)(value[4] | value[5] << 8 | value[6] << 16 | value[7] << 24);
            return (long)((ulong)num2 << 32 | num);
        }

        public static long ToLong(this List<byte> value)
        {
            uint num = (uint)(value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24);
            uint num2 = (uint)(value[4] | value[5] << 8 | value[6] << 16 | value[7] << 24);
            return (long)((ulong)num2 << 32 | num);
        }

        public static long ToLong(this byte[] value, ref int start)
        {
            uint num = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            uint num2 = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            return (long)((ulong)num2 << 32 | num);
        }

        public static long ToLong(this List<byte> value, ref int start)
        {
            uint num = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            uint num2 = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            return (long)((ulong)num2 << 32 | num);
        }

        public static ulong ToULong(this byte[] value)
        {
            uint num = (uint)(value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24);
            uint num2 = (uint)(value[4] | value[5] << 8 | value[6] << 16 | value[7] << 24);
            return (ulong)num2 << 32 | num;
        }

        public static ulong ToULong(this List<byte> value)
        {
            uint num = (uint)(value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24);
            uint num2 = (uint)(value[4] | value[5] << 8 | value[6] << 16 | value[7] << 24);
            return (ulong)num2 << 32 | num;
        }

        public static ulong ToULong(this byte[] value, ref int start)
        {
            uint num = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            uint num2 = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            return (ulong)num2 << 32 | num;
        }

        public static ulong ToULong(this List<byte> value, ref int start)
        {
            uint num = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            uint num2 = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            return (ulong)num2 << 32 | num;
        }

        public static unsafe float ToFloat(this byte[] value)
        {
            uint num = (uint)(value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24);
            return *(float*)(&num);
        }

        public static unsafe float ToFloat(this List<byte> value)
        {
            uint num = (uint)(value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24);
            return *(float*)(&num);
        }

        public static unsafe float ToFloat(this byte[] value, ref int start)
        {
            uint num = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            return *(float*)(&num);
        }

        public static unsafe float ToFloat(this List<byte> value, ref int start)
        {
            uint num = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            return *(float*)(&num);
        }

        public static unsafe double ToDouble(this byte[] value)
        {
            uint num = (uint)(value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24);
            uint num2 = (uint)(value[4] | value[5] << 8 | value[6] << 16 | value[7] << 24);
            ulong num3 = (ulong)num2 << 32 | num;
            return *(double*)(&num3);
        }

        public static unsafe double ToDouble(this List<byte> value)
        {
            uint num = (uint)(value[0] | value[1] << 8 | value[2] << 16 | value[3] << 24);
            uint num2 = (uint)(value[4] | value[5] << 8 | value[6] << 16 | value[7] << 24);
            ulong num3 = (ulong)num2 << 32 | num;
            return *(double*)(&num3);
        }

        public static unsafe double ToDouble(this byte[] value, ref int start)
        {
            uint num = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            uint num2 = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            ulong num3 = (ulong)num2 << 32 | num;
            return *(double*)(&num3);
        }

        public static unsafe double ToDouble(this List<byte> value, ref int start)
        {
            uint num = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            uint num2 = (uint)(value[start++] | value[start++] << 8 | value[start++] << 16 | value[start++] << 24);
            ulong num3 = (ulong)num2 << 32 | num;
            return *(double*)(&num3);
        }

        #endregion

        #region Bit manipulation

        public static bool GetBit(this byte b, int index)
        {
            int bitIndex = index % 8;
            byte mask = (byte)(1 << bitIndex);

            return (b & mask) != 0;
        }

        public static bool[] GetBits(this byte b)
        {
            bool[] ret = new bool[8];

            for (int i = 0; i < 8; i++)
                ret[i] = GetBit(b, i);

            return ret;
        }

        public static bool[] GetBits(this byte[] b)
        {
            bool[] ret = new bool[b.Length * 8];

            int x = 0;
            for (int y = 0; y < b.Length; y++)
                for (int z = 0; z < 8; z++)
                    ret[x++] = GetBit(b[y], z);

            return ret;
        }

        public static void SetBit(ref byte b, int index, bool value)
        {
            int bitIndex = index % 8;
            byte mask = (byte)(1 << bitIndex);

            b = (byte)(value ? (b | mask) : (b & ~mask));
        }

        public static void SetBits(ref byte b, bool[] bits)
        {
            for (int i = 0; i < bits.Length; i++)
                SetBit(ref b, i, bits[i]);
        }

        public static byte ToggleBit(this byte b, int index)
        {
            int bitIndex = index % 8;
            byte mask = (byte)(1 << bitIndex);

            return b ^= mask;
        }

        #endregion
    }
}