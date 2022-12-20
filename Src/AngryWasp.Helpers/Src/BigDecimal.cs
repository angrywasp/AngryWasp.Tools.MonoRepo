using System;
using System.Globalization;
using System.Numerics;

namespace AngryWasp.Helpers
{
    public struct BigDecimal : IComparable, IComparable<BigDecimal>
    {
        public const int Precision = 50;

        private BigInteger mantissa;
        private int exponent;

        public BigInteger Mantissa => mantissa;
        public int Decimals => -exponent;

        public static BigDecimal Zero(int decimals = 18) => Create(0, decimals);

        public static BigDecimal Create(BigInteger mantissa, int decimals)
        {
            var b = new BigDecimal
            {
                mantissa = mantissa,
                exponent = -decimals
            };

            return b;
        }

        public static BigDecimal Create(int mantissa, int decimals)
        {
            var b = new BigDecimal
            {
                mantissa = mantissa * BigInteger.Pow(new BigInteger(10), decimals),
                exponent = -decimals
            };

            return b;
        }

        public BigDecimal ShiftDecimals(int newDecimals)
        {
            var powExponent = (-exponent) - newDecimals;

            if (powExponent == 0)
                return this;

            var b = new BigDecimal();

            if (powExponent < 0)
                b.mantissa = mantissa * BigInteger.Pow(new BigInteger(10), -powExponent);
            else
                b.mantissa = mantissa / BigInteger.Pow(new BigInteger(10), powExponent);

            b.exponent = -newDecimals;

            return b;
        }

        public int CompareTo(object obj)
        {
            if (obj is null || obj is not BigDecimal)
                throw new ArgumentException("Object is not of type BigDecimal");
            return CompareTo((BigDecimal)obj);
        }

        public int CompareTo(BigDecimal other)
        {
            return this < other ? -1 : (this > other ? 1 : 0);
        }

        public BigDecimal RoundAwayFromZero(int significantDigits)
        {
            if (significantDigits < 0 || significantDigits > 2_000_000_000)
                throw new ArgumentOutOfRangeException(paramName: nameof(significantDigits));

            if (exponent >= -significantDigits) return this;

            bool negative = this.Mantissa < 0;
            var shortened = negative ? -this : this;

            while (shortened.exponent < -significantDigits)
            {
                shortened.mantissa = BigInteger.DivRem(shortened.Mantissa, 10, out var rem);
                shortened.mantissa += rem >= 5 ? +1 : 0;
                shortened.exponent++;
            }

            return negative ? -shortened : shortened;
        }

        public override string ToString()
        {
            var s = Mantissa.ToString();
            if (exponent != 0)
            {
                var decimalPos = s.Length + exponent;
                if (decimalPos < s.Length)
                    if (decimalPos >= 0)
                        s = s.Insert(decimalPos, decimalPos == 0 ? "0." : ".");
                    else
                        s = "0." + s.PadLeft(decimalPos * -1 + s.Length, '0');
                else
                    s = s.PadRight(decimalPos, '0');
            }
            return s;
        }

        public bool Equals(BigDecimal other)
        {
            var first = this;
            var second = other;
            return second.Mantissa.Equals(first.Mantissa) && second.exponent == first.exponent;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            return obj is BigDecimal && Equals((BigDecimal)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Mantissa.GetHashCode() * 397) ^ exponent;
            }
        }

        #region Conversions

        public static explicit operator double(BigDecimal value) => double.Parse(value.ToString(), CultureInfo.InvariantCulture);

        public static explicit operator float(BigDecimal value) => float.Parse(value.ToString(), CultureInfo.InvariantCulture);

        public static explicit operator decimal(BigDecimal value) => decimal.Parse(value.ToString(), CultureInfo.InvariantCulture);

        public static explicit operator int(BigDecimal value) => Convert.ToInt32((decimal)value);

        public static explicit operator uint(BigDecimal value) => Convert.ToUInt32((decimal)value);

        #endregion

        #region Operators

        public static BigDecimal operator +(BigDecimal value) => value;

        public static BigDecimal operator -(BigDecimal value)
        {
            value.mantissa *= -1;
            return value;
        }

        public static BigDecimal operator ++(BigDecimal value) => value + Create(1, -value.exponent);

        public static BigDecimal operator --(BigDecimal value) => value - Create(1, -value.exponent);

        public static BigDecimal operator +(BigDecimal left, BigDecimal right)
        {
            CheckExponentMatch(left, right);
            var x = left.Mantissa + right.Mantissa;
            return BigDecimal.Create(x, -left.exponent);
        }

        public static BigDecimal operator -(BigDecimal left, BigDecimal right)
        {
            CheckExponentMatch(left, right);
            var x = left.Mantissa - right.Mantissa;
            return BigDecimal.Create(x, -left.exponent);
        }

        public static BigDecimal operator *(BigDecimal left, BigDecimal right)
        {
            CheckExponentMatch(left, right);
            var x = left.Mantissa * right.Mantissa;
            x /= BigInteger.Pow(new BigInteger(10), -left.exponent);
            return BigDecimal.Create(x, -left.exponent);
        }

        public static BigDecimal operator /(BigDecimal left, BigDecimal right)
        {
            CheckExponentMatch(left, right);
            var x = (left.Mantissa * BigInteger.Pow(new BigInteger(10), -left.exponent)) / (right.Mantissa);
            return BigDecimal.Create(x, -left.exponent);
        }

        public static bool operator ==(BigDecimal left, BigDecimal right)
        {
            CheckExponentMatch(left, right);
            return left.Mantissa == right.Mantissa;
        }

        public static bool operator !=(BigDecimal left, BigDecimal right)
        {
            CheckExponentMatch(left, right);
            return left.Mantissa != right.Mantissa;
        }

        public static bool operator <(BigDecimal left, BigDecimal right)
        {
            CheckExponentMatch(left, right);
            return left.Mantissa < right.Mantissa;
        }

        public static bool operator >(BigDecimal left, BigDecimal right)
        {
            CheckExponentMatch(left, right);
            return left.Mantissa > right.Mantissa;
        }

        public static bool operator <=(BigDecimal left, BigDecimal right)
        {
            CheckExponentMatch(left, right);
            return left.Mantissa <= right.Mantissa;
        }

        public static bool operator >=(BigDecimal left, BigDecimal right)
        {
            CheckExponentMatch(left, right);
            return left.Mantissa >= right.Mantissa;
        }

        private static void CheckExponentMatch(BigDecimal a, BigDecimal b)
        {
            if (a.exponent != b.exponent)
                throw new Exception("BigDecimal exponent mismatch");
        }

        #endregion

        public static bool TryParse(string val, int decimals, out BigDecimal result)
        {
            result = default;

            char? separator = null;
            if (val.Contains('.'))
                separator = '.';
            else if (val.Contains(','))
                separator = ',';

            BigInteger a = 0;

            if (separator.HasValue)
            {
                if (val.StartsWith(separator.Value))
                    val = "0" + val;

                string[] i = val.Split(separator.Value, StringSplitOptions.RemoveEmptyEntries);

                a = BigInteger.Parse(i[0].PadRight(i[0].Length + decimals, '0'));

                if (i.Length > 1)
                {
                    if (i[1].Length > decimals)
                        i[1] = i[1][..decimals];
                    a += BigInteger.Parse(i[1].PadRight(decimals, '0'));
                }
            }
            else
                a = BigInteger.Parse(val.PadRight(val.Length + decimals, '0'));

            result = Create(a, decimals);
            return true;
        }

        public static BigDecimal Parse(string val, int decimals = 18)
        {
            if (!TryParse(val, decimals, out BigDecimal d))
                throw new FormatException($"Failed to parse {val} as a BigDecimal");

            return d;
        }
    }
}