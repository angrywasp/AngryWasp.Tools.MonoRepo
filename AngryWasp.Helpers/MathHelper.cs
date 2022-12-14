using System;

namespace AngryWasp.Helpers
{
    public static class MathHelper
    {
        public const float Pi = 3.141592653589793239f;
        public const float TwoPi = 6.283185307179586477f;
        public const float PiOver2 = 1.570796326794896619f;
        public const float PiOver4 = 0.785398163397448310f;
        public const float ToRad = 0.0174532925199432958f;
        public const float ToDeg = 57.2957795130823208768f;

        public static float Min(float a, float b)
        {
            return a < b ? a : b;
        }

        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }

        public static float Cos(float a)
        {
            return (float)Math.Cos(a);
        }

        public static float Sin(float a)
        {
            return (float)Math.Sin(a);
        }

        public static float Tan(float a)
        {
            return (float)Math.Tan(a);
        }

        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        public static int FloorToInt(float f)
        {
            return (int)Math.Floor(f);
        }

        public static float Floor(float f)
        {
            return (float)Math.Floor(f);
        }

        public static float Average(params float[] numbers)
        {
            float total = 0;

            if (numbers.Length == 0)
                return float.NaN;

            foreach (float f in numbers)
                total += f;

            return total / numbers.Length;
        }

        /// <summary>
        /// interpolates a set of values between min and max of pointCount length
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="pointCount"></param>
        /// <returns></returns>
        public static float[] GetInterpolationPoints(float min, float max, int pointCount)
        {
            float avg = Average(max - min);
            float div = avg / pointCount;

            float[] result = new float[pointCount];

            for (int i = 0; i < pointCount; i++)
                result[i] = min + (div * i);

            return result;
        }

        /// <summary>Linearly interpolates between two values.</summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        public static float[] GetRelativeInterpolationPoints(float min, float max, int pointCount)
        {
            float avg = Average(max - min);
            float div = avg / pointCount;

            float[] result = new float[pointCount];

            for (int i = 0; i < pointCount; i++)
                result[i] = div;

            return result;
        }

        #region Clamp

        public static void Clamp(ref int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void Clamp(ref float value, float min, float max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;

            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;

            return value;
        }

        #endregion

        /// <summary>
        /// Calculates the weight of a given value between the min and max specified
        /// </summary>
        /// <param name="fValue"></param>
        /// <param name="fMin"></param>
        /// <param name="fMax"></param>
        /// <returns></returns>
        public static float ComputeWeight(float fValue, float fMin, float fMax)
        {
            float fWeight = 0.0f;

            if (fValue >= fMin && fValue <= fMax)
            {
                float fSpan = fMax - fMin;
                fWeight = fValue - fMin;

                // convert to a -1 to 1 range between min and max
                fWeight /= fSpan;
                fWeight -= 0.5f;
                fWeight *= 2.0f;

                // square result for non-linear falloff
                fWeight *= fWeight;

                // invert result
                fWeight = 1.0f - fWeight;
            }

            return fWeight;
        }

        /// <summary>
        /// converts a float value in degrees to it's radian equivalent
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        /*public static float DegreesToRadians(float deg)
        {
            return deg * ToRad;
        }

        public static float RadiansToDegrees(float rad)
        {
            return rad * ToDeg;
        }*/

        /// <summary>
        /// calculates whether a number is above the threshold value
        /// </summary>
        /// <param name="value">the value to test</param>
        /// <param name="thresh">the threshold to test it against</param>
        /// <returns>0 if value is less that theshold, otherwise value</returns>
        public static float Threshold(float value, float thresh)
        {
            return value < thresh ? 0 : value;
        }

        /// <summary>
        /// Class for generating random data types
        /// </summary>
        public static class Random
        {
            private static MersenneTwister r = new MersenneTwister(GenerateRandomSeed());

            public static uint GenerateRandomSeed()
            {
                uint a = (uint)(Math.Abs(Guid.NewGuid().GetHashCode()));
                uint b = (uint)(Math.Abs(Guid.NewGuid().GetHashCode()));

                return a + b;
            }

            public static void Reset()
            {
                r = new MersenneTwister(GenerateRandomSeed());
            }

            /// <summary>
            /// Generates a random float value between min and max values
            /// </summary>
            /// <param name="min">the minimum value for the generated number</param>
            /// <param name="max">the maximum value for the generated number</param>
            /// <returns>the generated float</returns>
            public static float NextFloat(float min, float max)
            {
                return (float)((max - min) * r.NextDouble() + min);
            }


            /// <summary>
            /// Generates a random double value in the range 0.0 - 1.0
            /// </summary>
            /// <param name="min">the minimum value for the generated number</param>
            /// <param name="max">the maximum value for the generated number</param>
            /// <returns>the generated float</returns>
            public static double NextDouble()
            {
                return r.NextDouble();
            }

            /// <summary>
            /// Generates a random double value between min and max values
            /// </summary>
            /// <param name="min">the minimum value for the generated number</param>
            /// <param name="max">the maximum value for the generated number</param>
            /// <returns>the generated float</returns>
            public static double NextDouble(double min, double max)
            {
                return (max - min) * r.NextDouble() + min;
            }

            /// <summary>
            /// generate a random byte in the range 0 - 255
            /// </summary>
            /// <returns>the generated byte</returns>
            public static byte NextByte()
            {
                return (byte)NextInt(0, 255);
            }

            /// <summary>
            /// generate a random sequence of n bytes
            /// </summary>
            /// <returns>the generated byte array</returns>
            public static byte[] NextBytes(uint count)
            {
                return r.NextBytes(count);
            }

            public static int NextInt(int min, int max)
            {
                return r.Next(min, max);
            }
        }
    }
}

