using AngryWasp.Helpers;
using Nethereum.Util;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AngryWasp.Cryptography
{
    public class EthAddressJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(EthAddress);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            EthAddress hk = ((string)reader.Value).TrimHexPrefix().FromByteHex();
            return hk;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            EthAddress hk = (EthAddress)value;
            writer.WriteValue(hk.ToString());
        }
    }

    [JsonConverter(typeof(EthAddressJsonConverter))]
    public struct EthAddress : IReadOnlyList<byte>, IEquatable<EthAddress>, IEquatable<byte[]>, IComparable<EthAddress>
    {
        private static readonly AddressUtil addressUtil = new AddressUtil();

        private readonly byte[] value;

        public byte this[int index]
        {
            get
            {
                if (this.value != null)
                    return this.value[index];

                return default(byte);
            }
        }

        public int Count => 20;

        public static readonly EthAddress Empty = new byte[20];

        public static bool IsNullOrEmpty(EthAddress address)
        {
            if (address.value == null)
                return true;

            if (address.value.Length != 20)
                return true;

            if (address.value.SequenceEqual(Empty))
                return true;

            return false;
        }

        public EthAddress(byte[] bytes)
        {
            if (bytes == null || bytes.Length != 20)
                value = new byte[20];
            else
                value = bytes;
        }

        public bool Equals(EthAddress other) => this.SequenceEqual(other);
        public bool Equals(byte[] other) => this.SequenceEqual(other);
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is EthAddress && this.Equals((EthAddress)obj);
        }

        public IEnumerator<byte> GetEnumerator()
        {
            if (this.value != null)
                return ((IList<byte>)this.value).GetEnumerator();

            return Enumerable.Repeat(default(byte), 20).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public override int GetHashCode()
        {
            if (this.value == null)
                return 0;

            int offset = 0;
            return
                this.value.ToInt(ref offset) ^
                this.value.ToInt(ref offset) ^
                this.value.ToInt(ref offset) ^
                this.value.ToInt(ref offset) ^
                this.value.ToInt(ref offset);
        }

        public static bool TryParse(string input, out EthAddress output)
        {
            output = EthAddress.Empty;
            if (input == null)
                return false;

            if (!input.StartsWith("0x") || input.Length != 42)
                return false;

            try
            {
                output = new EthAddress(input.TrimHexPrefix().FromByteHex());
                return true;
            }
            catch { return false; }
        }

        public static bool operator ==(EthAddress left, EthAddress right) => left.Equals(right);

        public static bool operator !=(EthAddress left, EthAddress right) => !left.Equals(right);

        public static bool operator ==(byte[] left, EthAddress right) => right.Equals(left);

        public static bool operator !=(byte[] left, EthAddress right) => !right.Equals(left);

        public static bool operator ==(EthAddress left, byte[] right) => left.Equals(right);

        public static bool operator !=(EthAddress left, byte[] right) => !left.Equals(right);

        public static implicit operator EthAddress(byte[] value) => new EthAddress(value);

        public static implicit operator byte[](EthAddress value) => value.ToByte();

        public static implicit operator List<byte>(EthAddress value) => value.ToList();

        public static implicit operator EthAddress(List<byte> value) => new EthAddress(value.ToArray());

        public static implicit operator EthAddress(string hex) => new EthAddress(hex.TrimHexPrefix().FromByteHex());
        public static implicit operator string(EthAddress value) => value.ToString();

        public override string ToString() => addressUtil.ConvertToChecksumAddress(this.value.ToHex());

        public byte[] ToByte() => value;

        public string ToLower() => value.ToPrefixedHex();

        public int CompareTo(EthAddress other)
        {
            var a = new BigInteger(this.value, true);
            var b = new BigInteger(other.value, true);

            if (a < b)
                return -1;

            if (b < a)
                return 1;

            return 0;
        }
    }
}