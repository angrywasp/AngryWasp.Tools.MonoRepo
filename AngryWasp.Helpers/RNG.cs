using System;

namespace AngryWasp.Helpers
{
    public class MersenneTwister
    {
        private const int N = 624;
        private const int M = 397;
        private const uint MATRIX_A = 0x9908b0df;
        private const uint UPPER_MASK = 0x80000000;
        private const uint LOWER_MASK = 0x7fffffff;

        private const uint TEMPERING_MASK_B = 0x9d2c5680;
        private const uint TEMPERING_MASK_C = 0xefc60000;

        private static uint TEMPERING_SHIFT_U(uint y) => (y >> 11);
        private static uint TEMPERING_SHIFT_S(uint y) => (y << 7);
        private static uint TEMPERING_SHIFT_T(uint y) => (y << 15);
        private static uint TEMPERING_SHIFT_L(uint y) => (y >> 18);

        private uint[] mt = new uint[N];

        private uint seed;
        private short mti;

        private static uint[] mag01 = { 0x0, MATRIX_A };

        public MersenneTwister(uint seed)
        {
            Seed = seed;
        }

        public uint Seed
        {
            set
            {
                seed = value;
                mt[0] = seed & 0xffffffffU;
                for (mti = 1; mti < N; mti++)
                    mt[mti] = (69069 * mt[mti - 1]) & 0xffffffffU;
            }

            get => seed;
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
            y ^= TEMPERING_SHIFT_U(y);
            y ^= TEMPERING_SHIFT_S(y) & TEMPERING_MASK_B;
            y ^= TEMPERING_SHIFT_T(y) & TEMPERING_MASK_C;
            y ^= TEMPERING_SHIFT_L(y);

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

        public double NextDouble() => (double)this.GenerateUInt() / uint.MaxValue;

        public ulong NextULong() => (ulong)(NextDouble() * 1000000000000.0d);
    }

    public class XoShiRo256
    {
        private ulong[] state = null;

        public XoShiRo256(ulong[] state)
        {
            if (state.Length != 4)
                throw new InvalidOperationException("State must contain 4 elements");

            this.state = state;
        }

        private ulong Rotl64(ulong x, int k) => (x << k) | (x >> (64 - k));

        public ulong NextULong()
        {
            ulong result = Rotl64(state[1] * 5, 7) * 9;
            ulong t = state[1] << 17;

            state[2] ^= state[0];
            state[3] ^= state[1];
            state[1] ^= state[2];
            state[0] ^= state[3];

            state[2] ^= t;
            state[3] = Rotl64(state[3], 45);

            return result;
        }
    }
}