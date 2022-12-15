using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace AngryWasp.Helpers
{
    /*public class MersenneTwister
    {
        private const int N = 624;
        private const int M = 397;
        private const uint UPPER_MASK = 0x80000000;
        private const uint LOWER_MASK = 0x7fffffff;

        private uint[] mt = new uint[N];

        private short mti;

        private static uint[] mag01 = { 0x0, 0x9908b0df };

        public MersenneTwister()
        {
            var r = new byte[4];
            RandomNumberGenerator.Create().GetNonZeroBytes(r);
            var seed = BitShifter.ToUInt(r);

            mt[0] = seed & 0xffffffffU;
            for (mti = 1; mti < N; mti++)
                mt[mti] = (69069 * mt[mti - 1]) & 0xffffffffU;
        }

        public uint GenerateUInt()
        {
            uint y;

            if (mti >= N)
            {
                short kk;

                for (kk = 0; kk < N - M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1];
                }

                for (; kk < N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1];
                }

                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1];

                mti = 0;
            }

            y = mt[mti++];
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680;
            y ^= (y << 15) & 0xefc60000;
            y ^= (y >> 18);

            return y;
        }

        public uint NextUInt() => this.GenerateUInt();

        public uint NextUInt(uint maxValue) => (uint)(this.GenerateUInt() / ((double)uint.MaxValue / maxValue));

        public uint NextUInt(uint minValue, uint maxValue) => (uint)(this.GenerateUInt() / ((double)uint.MaxValue / (maxValue - minValue)) + minValue);

        public int Next() => (int)(this.GenerateUInt() / 2);

        public int Next(int maxValue) => (int)(this.GenerateUInt() / (uint.MaxValue / maxValue));

        public int Next(int minValue, int maxValue) => (int)(this.GenerateUInt() / ((double)uint.MaxValue / (maxValue - minValue)) + minValue);

        public byte[] NextBytes(uint length)
        {
            byte[] b = new byte[length];
            NextBytes(b);
            return b;
        }

        public void NextBytes(byte[] buffer)
        {
            for (int idx = 0; idx < buffer.Length; idx++)
                buffer[idx] = (byte)(this.GenerateUInt() / (uint.MaxValue / byte.MaxValue));
        }

        public float NextFloat() => (float)this.GenerateUInt() / uint.MaxValue;

        public double NextDouble() => (double)this.GenerateUInt() / uint.MaxValue;

        public ulong NextULong() => (ulong)(NextDouble() * 1000000000000.0d);
    }*/

    public abstract class XoShiRo128
    {
        protected uint[] state = null;

        public XoShiRo128()
        {
            var r = new byte[16];
            var start = 0;

            RandomNumberGenerator.Create().GetNonZeroBytes(r);

            state = new uint[] {
                BitShifter.ToUInt(r, ref start),
                BitShifter.ToUInt(r, ref start),
                BitShifter.ToUInt(r, ref start),
                BitShifter.ToUInt(r, ref start)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected uint Rotl(uint x, int k) => (x << k) | (x >> (32 - k));

        public abstract uint Next();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void Scramble()
        {
            uint t = state[1] << 9;

            state[2] ^= state[0];
            state[3] ^= state[1];
            state[1] ^= state[2];
            state[0] ^= state[3];

            state[2] ^= t;
            state[3] = Rotl(state[3], 11);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Next(int minValue, int maxValue) => (int)(this.Next() / ((float)uint.MaxValue / (maxValue - minValue)) + minValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float NextFloat() => (float)this.Next() / uint.MaxValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NextBytes(byte[] buffer)
        {
            for (int idx = 0; idx < buffer.Length; idx++)
                buffer[idx] = (byte)(this.Next() / (uint.MaxValue / byte.MaxValue));
        }
    }

    public class XoShiRo128Plus : XoShiRo128
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override uint Next()
        {
            uint result = state[0] + state[3];
            Scramble();
            return result;
        }
    }

    public class XoShiRo128PlusPlus : XoShiRo128
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override uint Next()
        {
            uint result = Rotl(state[0] + state[3], 7) + state[0];
            Scramble();
            return result;
        }
    }

    public class XoShiRo128StarStar : XoShiRo128
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override uint Next()
        {
            uint result = Rotl(state[1] * 5, 7) * 9;
            Scramble();
            return result;
        }
    }

    public abstract class XoShiRo256
    {
        protected ulong[] state = null;

        public XoShiRo256()
        {
            var r = new byte[32];
            var start = 0;

            RandomNumberGenerator.Create().GetNonZeroBytes(r);

            state = new ulong[] {
                BitShifter.ToULong(r, ref start),
                BitShifter.ToULong(r, ref start),
                BitShifter.ToULong(r, ref start),
                BitShifter.ToULong(r, ref start)
            };
        }

        protected ulong Rotl(ulong x, int k) => (x << k) | (x >> (64 - k));

        public abstract ulong Next();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Scramble()
        {
            ulong t = state[1] << 17;

            state[2] ^= state[0];
            state[3] ^= state[1];
            state[1] ^= state[2];
            state[0] ^= state[3];

            state[2] ^= t;
            state[3] = Rotl(state[3], 45);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Next(long minValue, long maxValue) => (long)(this.Next() / ((double)ulong.MaxValue / (maxValue - minValue)) + minValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double NextDouble() => (double)this.Next() / ulong.MaxValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NextBytes(byte[] buffer)
        {
            for (int idx = 0; idx < buffer.Length; idx++)
                buffer[idx] = (byte)(this.Next() / (ulong.MaxValue / byte.MaxValue));
        }
    }

    public class XoShiRo256PlusPlus : XoShiRo256
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override ulong Next()
        {
            ulong result = Rotl(state[0] + state[3], 23) + state[0];
            Scramble();
            return result;
        }
    }

    public class XoShiRo256StarStar : XoShiRo256
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override ulong Next()
        {
            ulong result = Rotl(state[1] * 5, 7) * 9;
            Scramble();
            return result;
        }
    }
}