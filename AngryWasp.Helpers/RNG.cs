using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace AngryWasp.Helpers
{
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