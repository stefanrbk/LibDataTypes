using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Properties;
using System.Runtime.CompilerServices;

namespace System
{
    [Serializable]
    public readonly struct Half : IComparable, IFormattable, IConvertible, IComparable<Half>, IEquatable<Half>
    {
        #region [ Members ]

        // Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ushort _value;
        private static readonly uint[] _mantissaTable = GenerateMantissaTable();
        private static readonly uint[] _exponentTable = GenerateExponentTable();
        private static readonly ushort[] _offsetTable = GenerateOffsetTable();
        private static readonly ushort[] _baseTable = GenerateBaseTable();
        private static readonly sbyte[] _shiftTable = GenerateShiftTable();

        #endregion

        #region [ Constructors ]

        private Half(ushort value) =>
            this._value = value;

        #endregion

        #region [ Methods ]

        public int CompareTo(object obj) =>
            obj switch
            {
                null => 1,
                Half num => CompareTo(num),
                _ => throw new ArgumentException(ExceptionStrings.CompareToObjectArgument,
                                                        nameof(obj))
            };

        public int CompareTo(Half other) =>
            other switch
            {
                var o when
                    this < o ||
                    // There are multiple values which satisfy "NaN".
                    (IsNaN(this) && !IsNaN(o)) => -1,
                var o when
                    this > o ||
                    // There are multiple values which satisfy "NaN".
                    (!IsNaN(this) && IsNaN(o)) => 1,
                _ => 0
            };
        public override bool Equals(object obj) =>
            obj is Half num && Equals(num);

        public bool Equals(Half other) =>
            other._value == this._value ||
            // There are multiple values which satisfy "NaN".
            (IsNaN(other) && IsNaN(this));

        public override int GetHashCode() =>
            HashCode.Combine(this._value, 5);

        public TypeCode GetTypeCode() =>
            (TypeCode)255;

        public override string ToString() =>
            ((float)this).ToString(CultureInfo.CurrentCulture);

        public string ToString(string format) =>
            ((float)this).ToString(format, CultureInfo.CurrentCulture);

        public string ToString(IFormatProvider formatProvider) =>
            ((float)this).ToString(formatProvider);

        public string ToString(string format,
                               IFormatProvider formatProvider) =>
            ((float)this).ToString(format, formatProvider);

        public bool TryFormat(Span<char> destination,
                              out int charsWritten,
                              ReadOnlySpan<char> format = default,
                              IFormatProvider provider = default) =>
            ((float)this).TryFormat(destination, out charsWritten,
                                    format, provider);

        #region [ Explicit IConvertible Implementation ]

        bool IConvertible.ToBoolean(IFormatProvider provider) =>
            Convert.ToBoolean(this);

        byte IConvertible.ToByte(IFormatProvider provider) =>
            Convert.ToByte(this);

        char IConvertible.ToChar(IFormatProvider provider) =>
            throw new InvalidCastException();

        DateTime IConvertible.ToDateTime(IFormatProvider provider) =>
            throw new InvalidCastException();

        decimal IConvertible.ToDecimal(IFormatProvider provider) =>
            Convert.ToDecimal(this);

        double IConvertible.ToDouble(IFormatProvider provider) =>
            Convert.ToDouble(this);

        short IConvertible.ToInt16(IFormatProvider provider) =>
            Convert.ToInt16(this);

        int IConvertible.ToInt32(IFormatProvider provider) =>
            Convert.ToInt32(this);

        long IConvertible.ToInt64(IFormatProvider provider) =>
            Convert.ToInt64(this);

        sbyte IConvertible.ToSByte(IFormatProvider provider) =>
            Convert.ToSByte(this);

        float IConvertible.ToSingle(IFormatProvider provider) =>
            Convert.ToSingle(this);

        string IConvertible.ToString(IFormatProvider provider) =>
            Convert.ToString(this, CultureInfo.CurrentCulture);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(this, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) =>
            Convert.ToUInt16(this);

        uint IConvertible.ToUInt32(IFormatProvider provider) =>
            Convert.ToUInt32(this);

        ulong IConvertible.ToUInt64(IFormatProvider provider) =>
            Convert.ToUInt64(this);

        #endregion

        #endregion

        #region [ Operators ]

        #region [ Comparison Operators ]

        public static bool operator ==(Half left,
                                       Half right) =>
            left._value == right._value;

        public static bool operator !=(Half left,
                                       Half right) =>
            left._value != right._value;

        public static bool operator <(Half left,
                                       Half right) =>
            // There are multiple values which satisfy "NaN".
            // If either are "NaN", then return false.
            !IsNaN(left) && !IsNaN(right) &&
            left._value < right._value;

        public static bool operator >(Half left,
                                       Half right) =>
            // There are multiple values which satisfy "NaN".
            // If either are "NaN", then return false.
            !IsNaN(left) && !IsNaN(right) &&
            left._value > right._value;

        public static bool operator <=(Half left,
                                       Half right) =>
            // There are multiple values which satisfy "NaN".
            // If either are "NaN", then return false.
            !IsNaN(left) && !IsNaN(right) &&
            left._value <= right._value;

        public static bool operator >=(Half left,
                                       Half right) =>
            // There are multiple values which satisfy "NaN".
            // If either are "NaN", then return false.
            !IsNaN(left) && !IsNaN(right) &&
            left._value >= right._value;

        #endregion

        #region [ Type Conversion Operators ]

        #region [ Explicit Narrowing Conversions ]

        public static explicit operator byte(Half value) =>
            (byte)(float)value;

        public static explicit operator char(Half value) =>
            (char)(float)value;

        public static explicit operator decimal(Half value) =>
            (decimal)(float)value;

        public static explicit operator Half(float value) =>
            SingleToHalf(value);

        public static explicit operator Half(double value) =>
            SingleToHalf((float)value);

        public static explicit operator Half(decimal value) =>
            SingleToHalf((float)value);

        public static explicit operator int(Half value) =>
            (int)(float)value;

        public static explicit operator long(Half value) =>
            (long)(float)value;

        public static explicit operator sbyte(Half value) =>
            (sbyte)(float)value;

        public static explicit operator short(Half value) =>
            (short)(float)value;

        public static explicit operator uint(Half value) =>
            (uint)(float)value;

        public static explicit operator ulong(Half value) =>
            (ulong)(float)value;

        public static explicit operator ushort(Half value) =>
            (ushort)(float)value;

        #endregion

        #region [ Implicit Widening Conversions ]

        public static implicit operator double(Half value) =>
            HalfToSingle(value);

        public static implicit operator float(Half value) =>
            HalfToSingle(value);

        public static implicit operator Half(byte value) =>
            SingleToHalf(value);

        public static implicit operator Half(sbyte value) =>
            SingleToHalf(value);

        public static implicit operator Half(char value) =>
            SingleToHalf(value);

        public static implicit operator Half(short value) =>
            SingleToHalf(value);

        public static implicit operator Half(ushort value) =>
            SingleToHalf(value);

        public static implicit operator Half(int value) =>
            SingleToHalf(value);

        public static implicit operator Half(uint value) =>
            SingleToHalf(value);

        public static implicit operator Half(long value) =>
            SingleToHalf(value);

        public static implicit operator Half(ulong value) =>
            SingleToHalf(value);

        #endregion

        #endregion

        #region [ Arithmetic Operators ]

        [EditorBrowsable(EditorBrowsableState.Advanced), SpecialName]
#pragma warning disable IDE1006,CA1707 // Identifiers should not contain underscores or start lowercase
        public static double op_Exponent(Half left,
#pragma warning restore IDE1006,CA1707 // Identifiers should not contain underscores or start lowercase
                                       Half right) =>
            Math.Pow(left, right);

        public static Half operator -(Half half) =>
            Negate(half);

        public static Half operator -(Half left, Half right) =>
            (Half)(left - (float)right);

        public static Half operator --(Half half) =>
            (Half)(half - 1f);

        public static Half operator %(Half left,
                                      Half right) =>
            (Half)((float)left % right);

        public static Half operator *(Half left, Half right) =>
            (Half)(left * (float)right);

        public static Half operator /(Half left, Half right) =>
            (Half)(left / (float)right);

        public static Half operator +(Half half) =>
            half;

        public static Half operator +(Half left, Half right) =>
            (Half)(left + (float)right);

        public static Half operator ++(Half half) =>
            (Half)(half + 1f);

        #endregion

        #endregion

        #region [ Static ]

        #region [ Static Fields ]

        public static Half Epsilon =>
            new Half(0x0001);

        public static Half MaxValue =>
            new Half(0x7bff);

        public static Half MinValue =>
            new Half(0xfbff);

        public static Half NaN =>
            new Half(0xfe00);

        public static Half NegativeInfinity =>
            new Half(0xfc00);

        public static Half PositiveInfinity =>
            new Half(0x7c00);

        public static int SizeOf =>
            sizeof(ushort);

        #endregion

        #region [ Static Methods ]

        public static byte[] GetBytes(Half value) =>
            BitConverter.GetBytes(value._value);

        public static bool IsFinite(Half value) =>
            !IsInfinity(value) &&
            !IsNaN(value);

        public static bool IsInfinity(Half half) =>
            (half._value & 0x7fff) == 0x7c00;

        public static bool IsNaN(Half half) =>
            (half._value & 0x7fff) > 0x7c00;

        public static bool IsNegative(Half half) =>
            (half._value & 0x8000) != 0;

        public static bool IsNegativeInfinity(Half half) =>
            half._value == 0xfc00;

        public static bool IsNormal(Half half) =>
            !IsSubnormal(half) &&
            !IsInfinity(half) &&
            !IsNaN(half);

        public static bool IsPositiveInfinity(Half half) =>
            half._value == 0x7c00;

        public static bool IsSubnormal(Half half) =>
            (half._value & 0x7c00) == 0;

        public static Half ToHalf(byte[] value,
                                  int startIndex) =>
            new Half(BitConverter.ToUInt16(value, startIndex));

        public static Half ToHalf(ushort sign,
                                  short exponent,
                                  ushort mantissa) =>
            new Half((ushort)((sign << 15) | (ushort)((exponent + 15) << 10) | (mantissa >> 6)));

        public static Half Parse(string value) =>
            (Half)Single.Parse(value, CultureInfo.CurrentCulture);

        public static Half Parse(string value,
                                 NumberStyles style) =>
            (Half)Single.Parse(value, style, CultureInfo.CurrentCulture);

        public static Half Parse(string value,
                                 IFormatProvider formatProvider) =>
            (Half)Single.Parse(value, formatProvider);

        public static Half Parse(string value,
                                 NumberStyles style,
                                 IFormatProvider formatProvider) =>
            (Half)Single.Parse(value, style, formatProvider);

        public static Half Parse(ReadOnlySpan<char> value,
                                 NumberStyles style = NumberStyles.AllowDecimalPoint |
                                                      NumberStyles.AllowExponent |
                                                      NumberStyles.AllowLeadingSign |
                                                      NumberStyles.AllowLeadingWhite |
                                                      NumberStyles.AllowThousands |
                                                      NumberStyles.AllowTrailingWhite |
                                                      NumberStyles.Float |
                                                      NumberStyles.Integer,
                                 IFormatProvider formatProvider = default) =>
            (Half)Single.Parse(value, style, formatProvider);

        public static bool TryParse(string value,
                                    out Half result)
        {
            var ret = Single.TryParse(value, out var f);
            result = (Half)f;
            return ret;
        }

        public static bool TryParse(string value,
                                    NumberStyles style,
                                    IFormatProvider formatProvider,
                                    out Half result)
        {
            var ret = Single.TryParse(value, style, formatProvider, out var f);
            result = (Half)f;
            return ret;
        }

        public static bool TryParse(ReadOnlySpan<char> value,
                                    NumberStyles style,
                                    IFormatProvider formatProvider,
                                    out Half result)
        {
            var ret = Single.TryParse(value, style, formatProvider, out var f);
            result = (Half)f;
            return ret;
        }

        #endregion

        #endregion

        #region [ Helper Methods ]

        // Transforms the subnormal representation to a normalized one.
        private static uint ConvertMantissa(int i)
        {
            var m = (uint)(i << 13); // Zero pad mantissa bits
            var e = 0u; // Zero exponent

            // While not normalized
            while ((m & 0x00800000) == 0)
            {
                e -= 0x00800000; // Decrement exponent (1<<23)
                m <<= 1; // Shift mantissa
            }
            m &= unchecked((uint)~0x00800000); // Clear leading 1 bit
            e += 0x38800000; // Adjust bias ((127-14)<<23)
            return m | e; // Return combined number
        }

        private static uint[] GenerateMantissaTable()
        {
            var mantissaTable = new uint[2048];
            mantissaTable[0] = 0;
            for (var i = 1; i < 1024; i++)
                mantissaTable[i] = ConvertMantissa(i);

            for (var i = 1024; i < 2048; i++)
                mantissaTable[i] = (uint)(0x38000000 + ((i - 1024) << 13));

            return mantissaTable;
        }

        private static uint[] GenerateExponentTable()
        {
            var exponentTable = new uint[64];
            exponentTable[0] = 0;
            for (var i = 1; i < 31; i++)
                exponentTable[i] = (uint)(i << 23);

            exponentTable[31] = 0x47800000;
            exponentTable[32] = 0x80000000;
            for (var i = 33; i < 63; i++)
                exponentTable[i] = (uint)(0x80000000 + ((i - 32) << 23));

            exponentTable[63] = 0xc7800000;

            return exponentTable;
        }

        private static ushort[] GenerateOffsetTable()
        {
            var offsetTable = new ushort[64];
            offsetTable[0] = 0;
            for (var i = 1; i < 32; i++)
                offsetTable[i] = 1024;

            offsetTable[32] = 0;
            for (var i = 33; i < 64; i++)
                offsetTable[i] = 1024;

            return offsetTable;
        }

        private static ushort[] GenerateBaseTable()
        {
            var baseTable = new ushort[512];
            for (var i = 0; i < 256; ++i)
            {
                var e = (sbyte)(127 - i);
                if (e > 24)
                { // Very small numbers map to zero
                    baseTable[i | 0x000] = 0x0000;
                    baseTable[i | 0x100] = 0x8000;
                }
                else if (e > 14)
                { // Small numbers map to denorms
                    baseTable[i | 0x000] = (ushort)(0x0400 >> (18 + e));
                    baseTable[i | 0x100] = (ushort)((0x0400 >> (18 + e)) | 0x8000);
                }
                else if (e >= -15)
                { // Normal numbers just lose precision
                    baseTable[i | 0x000] = (ushort)((15 - e) << 10);
                    baseTable[i | 0x100] = (ushort)(((15 - e) << 10) | 0x8000);
                }
                else if (e > -128)
                { // Large numbers map to Infinity
                    baseTable[i | 0x000] = 0x7c00;
                    baseTable[i | 0x100] = 0xfc00;
                }
                else
                { // Infinity and NaN's stay Infinity and NaN's
                    baseTable[i | 0x000] = 0x7c00;
                    baseTable[i | 0x100] = 0xfc00;
                }
            }

            return baseTable;
        }

        private static sbyte[] GenerateShiftTable()
        {
            var shiftTable = new sbyte[512];
            for (var i = 0; i < 256; ++i)
            {
                var e = (sbyte)(127 - i);
                if (e > 24)
                { // Very small numbers map to zero
                    shiftTable[i | 0x000] = 24;
                    shiftTable[i | 0x100] = 24;
                }
                else if (e > 14)
                { // Small numbers map to denorms
                    shiftTable[i | 0x000] = (sbyte)(e - 1);
                    shiftTable[i | 0x100] = (sbyte)(e - 1);
                }
                else if (e >= -15)
                { // Normal numbers just lose precision
                    shiftTable[i | 0x000] = 13;
                    shiftTable[i | 0x100] = 13;
                }
                else if (e > -128)
                { // Large numbers map to Infinity
                    shiftTable[i | 0x000] = 24;
                    shiftTable[i | 0x100] = 24;
                }
                else
                { // Infinity and NaN's stay Infinity and NaN's
                    shiftTable[i | 0x000] = 13;
                    shiftTable[i | 0x100] = 13;
                }
            }

            return shiftTable;
        }

        private static unsafe float HalfToSingle(Half half)
        {
            var result = _mantissaTable[_offsetTable[half._value >> 10] + (half._value & 0x3ff)] + _exponentTable[half._value >> 10];
            return *(float*)&result;
        }

        private static unsafe Half SingleToHalf(float value)
        {
            var asUint = *(uint*)&value;

            var result = (ushort)(_baseTable[(asUint >> 23) & 0x1ff] + ((asUint & 0x007fffff) >> _shiftTable[asUint >> 23]));
            return new Half(result);
        }

        private static Half Negate(Half half) =>
            new Half((ushort)(half._value ^ 0x8000));

        internal static Half Abs(Half half) =>
            new Half((ushort)(half._value & 0x7fff));

        #endregion
    }
}
