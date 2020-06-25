using System.Globalization;
using System.IO.MemoryMappedFiles;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using static System.Half;

namespace System.Tests
{
    [TestFixture(TestOf = typeof(Half))]
    public static class HalfParityWithSingleTests
    {
        #region [ Setup ]

        public static readonly IFormatProvider Culture = CultureInfo.CurrentCulture;
        public const NumberStyles Number = NumberStyles.Number;
        public static readonly (Half, float)[] ValueTargets =
        {
            (72, 72),
            (PositiveInfinity, PositiveInfinity),
            (NegativeInfinity, NegativeInfinity),
            (NaN, NaN),
            (-42, -42),
            (Epsilon, Single.Epsilon),
            (0, 0),
            (ToHalf(new byte[] {0x00, 0x80}, 0), 
                BitConverter.ToSingle(new byte[] {0x00, 0x00, 0x00, 0x80 }, 0))
        };
        public static readonly Half[] ToStringTargets =
        {
            MathH.PI,
            10000,
            (Half)(-5.5)
        };
        public static readonly string[] FormatTargets =
        {
            "C",
            "e",
            "n",
            "##;(##);[  ##  ]"
        };
        public static readonly IFormatProvider[] ProviderTargets =
        {
            new CultureInfo("en-US"),
            new CultureInfo("fr-FR"),
            new CultureInfo("en-GB"),
            new CultureInfo("ja-JP")
        };
        public static readonly (Half, Half)[] CompareToHalfTargets =
        {
            (1, 2),
            (666, 555),
            (7, 7),
            (PositiveInfinity, PositiveInfinity),
            (NegativeInfinity, NegativeInfinity),
            (0,0),
            (0, (Half)(-0f)),
            (NaN, NaN),
            (NaN, 100),
            (100, NaN)
        };
        public static readonly (Half, object, object)[] CompareToObjectTargets =
        {
            (1, (Half)2f, 2f),
            (666, (Half)555f, 555f),
            (7, (Half)7f, 7f)
        };
        public static readonly (string, bool)[] TryParseTargets =
        {
            ("z", false),
            ("42", true),
            ((-0.5f).ToString(CultureInfo.CurrentCulture), true)
        };
        public static readonly (Half, Half)[] ArithmaticTargets =
        {
            (0, NaN),
            (5, 7),
            (11, (Half)0.5),
            (-3, 17),
            (PositiveInfinity, 2),
            (-8, NegativeInfinity)
        };

        #endregion

        [Test]
        public static void compare_to(
            [ValueSource("CompareToHalfTargets")] (Half left, Half right) target)
        {
            var actual = target.left.CompareTo(target.right);
            var expected = ((float)target.left).CompareTo(target.right);
            (var testLeft, var testRight) = (target.left.ToString(Culture), target.right.ToString(Culture));

            TestContext.WriteLine($@"
(Half){testLeft}.CompareTo((Half){testRight}) = {actual}
(Single){testLeft}.CompareTo((Single){testRight}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void compare_to(
            [ValueSource("CompareToObjectTargets")]
            (Half left, object halfRight, object singleRight) target)
        {
            var actual = target.left.CompareTo(target.halfRight);
            var expected = ((float)target.left).CompareTo(target.singleRight);
            (var testLeft, var testRight) = (target.left.ToString(Culture), target.halfRight.ToString());

            TestContext.WriteLine($@"
(Half){testLeft}.CompareTo((Object){testRight}) = {actual}
(Single){testLeft}.CompareTo((Object){testRight}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void equals(
            [Range(0, 2)] int option)
        {
            (var actual, var expected, var test) = option switch
            {
                0 => (MinValue.Equals(MinValue),
                      ((float)MinValue).Equals(MinValue),
                      $"{MinValue}"),
                1 => (((Half)12345).Equals(12345),
                      ((float)12345).Equals(12345),
                      $"12345"),
                _ => (new Half().Equals(new float()),
                      new float().Equals(new double()),
                      $"new ")
            };
            var halfTest = $"{test}{(option != 0 && option != 1 ? "Half()" : "")}";
            var singleTest = $"{test}{(option != 0 && option != 1 ? "Single()" : "")}";
            var doubleTest = $"{test}{(option != 0 && option != 1 ? "Double()" : "")}";

            TestContext.WriteLine($@"
(Half){halfTest}.Equals({singleTest}) = {actual}
(Single){singleTest}.Equals({doubleTest}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void is_finite(
            [ValueSource("ValueTargets")] (Half half, float single) target)
        {
            var actual = IsFinite(target.half);
            var expected = Single.IsFinite(target.single);

            TestContext.WriteLine($@"
Half.IsFinite({target.half}) = {actual}
Single.IsFinite({target.single}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void is_infinity(
            [ValueSource("ValueTargets")] (Half half, float single) target)
        {
            var actual = IsInfinity(target.half);
            var expected = Single.IsInfinity(target.single);

            TestContext.WriteLine($@"
Half.IsInfinity({target.half}) = {actual}
Single.IsInfinity({target.single}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void is_nan(
            [ValueSource("ValueTargets")] (Half half, float single) target)
        {
            var actual = IsNaN(target.half);
            var expected = Single.IsNaN(target.single);

            TestContext.WriteLine($@"
Half.IsNaN({target.half}) = {actual}
Single.IsNaN({target.single}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void is_negative(
            [ValueSource("ValueTargets")] (Half half, float single) target)
        {
            var actual = IsNegative(target.half);
            var expected = Single.IsNegative(target.single);

            TestContext.WriteLine($@"
Half.IsNegative({target.half}) = {actual}
Single.IsNegative({target.single}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void is_negative_infinity(
            [ValueSource("ValueTargets")] (Half half, float single) target)
        {
            var actual = IsNegativeInfinity(target.half);
            var expected = Single.IsNegativeInfinity(target.single);

            TestContext.WriteLine($@"
Half.IsFinite({target.half}) = {actual}
Single.IsFinite({target.single}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void is_normal(
            [ValueSource("ValueTargets")] (Half half, float single) target)
        {
            var actual = IsNormal(target.half);
            var expected = Single.IsNormal(target.single);

            TestContext.WriteLine($@"
Half.IsNormal({target.half}) = {actual}
Single.IsNormal({target.single}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void is_positive_infinity(
            [ValueSource("ValueTargets")] (Half half, float single) target)
        {
            var actual = IsPositiveInfinity(target.half);
            var expected = Single.IsPositiveInfinity(target.single);

            TestContext.WriteLine($@"
Half.IsPositiveInfinity({target.half}) = {actual}
Single.IsPositiveInfinity({target.single}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void is_subnormal(
            [ValueSource("ValueTargets")] (Half half, float single) target)
        {
            var actual = IsSubnormal(target.half);
            var expected = Single.IsSubnormal(target.single);

            TestContext.WriteLine($@"
Half.IsSubnormal({target.half}) = {actual}
Single.IsSubnormal({target.single}) = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void parse(
            [ValueSource("parse_targets")] string target)
        {
            var actual = Parse(target);
            var expected = Single.Parse(target);

            TestContext.WriteLine($@"
Half.Parse(""{target}"") = {actual}
Single.Parse(""{target}"") = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));

            actual = Parse(target, NumberStyles.Number);
            expected = Single.Parse(target, NumberStyles.Number);

            TestContext.WriteLine($@"
Half.Parse(""{target}"", {NumberStyles.Number}) = {actual}
Single.Parse(""{target}"", {NumberStyles.Number}) = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));

            actual = Parse(new ReadOnlySpan<char>(target.ToArray()));
            expected = Single.Parse(new ReadOnlySpan<char>(target.ToArray()));

            TestContext.WriteLine($@"
Half.Parse((ReadOnlySpan<Char>)""{target}"") = {actual}
Single.Parse((ReadOnlySpan<Char>)""{target}"") = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        [Test]
        public static void parse(
            [ValueSource("parse_with_provider_targets")] (string str, IFormatProvider fp) target)
        {
            var actual = Parse(target.str, target.fp);
            var expected = Single.Parse(target.str, target.fp);

            TestContext.WriteLine($@"
Half.Parse(""{target.str}"", {target.fp}) = {actual}
Single.Parse(""{target.str}"", {target.fp}) = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));

            actual = Parse(target.str, NumberStyles.Number, target.fp);
            expected = Single.Parse(target.str, NumberStyles.Number, target.fp);

            TestContext.WriteLine($@"
Half.Parse(""{target.str}"", {NumberStyles.Number}, {target.fp}) = {actual}
Single.Parse(""{target.str}"", {NumberStyles.Number}, {target.fp}) = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        [Test]
        public static void to_string(
            [ValueSource("ToStringTargets")] Half target)
        {
            var actual = target.ToString();
            var expected = ((float)target).ToString();

            TestContext.WriteLine($@"
(Half){target}.ToString() == ""{actual}""
(Single){target}.ToString() == ""{expected}""");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, Pairwise]
        public static void to_string(
            [ValueSource("ToStringTargets")] Half target,
            [ValueSource("FormatTargets")] string format)
        {
            var actual = target.ToString(format);
            var expected = ((float)target).ToString(format);

            TestContext.WriteLine($@"
(Half){target}.ToString({format}) = ""{actual}""
(Single){target}.ToString({format}) = ""{expected}""");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void to_string(
            [ValueSource("ToStringTargets")] Half target,
            [ValueSource("ProviderTargets")] IFormatProvider provider)
        {
            var actual = target.ToString(provider);
            var expected = ((float)target).ToString(provider);

            TestContext.WriteLine($@"
(Half){target}.ToString({provider}) = ""{actual}""
(Single){target}.ToString({provider}) = ""{expected}""");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void to_string(
            [ValueSource("ToStringTargets")] Half target,
            [ValueSource("FormatTargets")] string format,
            [ValueSource("ProviderTargets")] IFormatProvider provider)
        {
            var actual = target.ToString(format, provider);
            var expected = ((float)target).ToString(format, provider);

            TestContext.WriteLine($@"
(Half){target}.ToString(""{format}"", {provider}) = ""{actual}""
(Single){target}.ToString(""{format}"", {provider}) = ""{expected}""");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void try_format(
            [ValueSource("ToStringTargets")] Half target,
            [ValueSource("FormatTargets")] string format,
            [ValueSource("ProviderTargets")] IFormatProvider provider)
        {
            var hResult = new char[20].AsSpan();
            var sResult = new char[20].AsSpan();

            var actual = target.TryFormat(hResult, out var hWritten, format, provider);
            var expected = ((float)target).TryFormat(sResult, out var sWritten, format, provider);

            TestContext.WriteLine($@"
(Half){target}.TryFormat(hResult, out hWritten, {format}, {provider}) = {actual}
(Single){target}.TryFormat(sResult, out sWritten, {format}, {provider}) = {expected}

hWritten = {hWritten}
sWritten = {sWritten}

hResult = ""{hResult.Trim('\0').ToString()}""
sResult = ""{sResult.Trim('\0').ToString()}""");

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(hWritten, Is.EqualTo(sWritten));
            Assert.That(hResult.SequenceEqual(sResult));
        }

        [Test]
        public static void try_parse(
            [ValueSource("TryParseTargets")] (string value, bool result) target)
        {
            var actual = TryParse(target.value, out var hResult);
            var expected = Single.TryParse(target.value, out var sResult);

            TestContext.WriteLine($@"
Half.TryParse(""{target.value}"", out halfResult) = {actual}
Single.TryParse(""{target.value}"", out singleResult) = {expected}

halfResult = {hResult}
singleResult = {sResult}");

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(hResult, Is.EqualTo((Half)sResult));

            actual = TryParse(target.value, Number, Culture, out hResult);
            expected = Single.TryParse(target.value, Number, Culture, out sResult);

            TestContext.WriteLine($@"
Half.TryParse(""{target.value}"", {Number}, {Culture}, out halfResult) = {actual}
Single.TryParse(""{target.value}"", {Number}, {Culture}, out singleResult) == {expected}

halfResult = {hResult}
singleResult == {sResult}");

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(hResult, Is.EqualTo((Half)sResult));

            actual = TryParse(target.value.AsSpan(), Number, Culture, out hResult);
            expected = Single.TryParse(target.value.AsSpan(), Number, Culture, out sResult);

            TestContext.WriteLine($@"
Half.TryParse((ReadOnlySpan<Char>)""{target}"", {Number}, {Culture}, out hResult) = {actual}
Single.TryParse((ReadOnlySpan<Char>)""{target}"", {Number}, {Culture}, out sResult) = {expected}

hResult = {hResult}
sResult = {sResult}");

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(hResult, Is.EqualTo((Half)sResult));
        }

        [Test]
        public static void to_boolean(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            IConvertibleVars<bool>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try {
                hResult = hCon.ToBoolean(Culture);
            } catch {
                hCanConvert = false;
            }
            try {
                sResult = sCon.ToBoolean(Culture);
            } catch {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToBoolean", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_byte(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<byte>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToByte(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToByte(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToByte", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_char(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<char>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToChar(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToChar(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToChar", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_date_time(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<DateTime>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToDateTime(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToDateTime(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToDateTime", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_decimal(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<decimal>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToDecimal(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToDecimal(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToDecimal", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_double(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<double>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToDouble(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToDouble(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToDouble", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_int16(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<short>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToInt16(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToInt16(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToInt16", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_int32(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<int>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToInt32(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToInt32(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToInt32", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_int64(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<long>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToInt64(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToInt64(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToInt64", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_sbyte(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<sbyte>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToSByte(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToSByte(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToSByte", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_single(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<float>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToSingle(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToSingle(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToSingle", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_string(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<string>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToString(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToString(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToString", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_type(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<object>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToType(typeof(double), Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToType(typeof(double), Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            TestContext.WriteLine($@"
(Half){target.half}.ToType({nameof(Double)}, {Culture}) = {(hCanConvert ? hResult.ToString() : "<undefined>")}
(Single){target.half}.ToType({nameof(Double)}, {Culture}) = {(sCanConvert ? sResult.ToString() : "<undefined>")}");

            Assert.That(hCanConvert, Is.EqualTo(sCanConvert));
            Assert.That(hResult, Is.EqualTo(sResult));

            hCanConvert = true;
            sCanConvert = true;

            try
            {
                hResult = hCon.ToType(typeof(long), Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToType(typeof(long), Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            TestContext.WriteLine($@"
(Half){target.half}.ToType({nameof(Int64)}, {Culture}) = {(hCanConvert ? hResult.ToString() : "<undefined>")}
(Single){target.half}.ToType({nameof(Int64)}, {Culture}) = {(sCanConvert ? sResult.ToString() : "<undefined>")}");

            Assert.That(hCanConvert, Is.EqualTo(sCanConvert));
            Assert.That(hResult, Is.EqualTo(sResult));

            hCanConvert = true;
            sCanConvert = true;

            try
            {
                hResult = hCon.ToType(typeof(DateTime), Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToType(typeof(DateTime), Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            TestContext.WriteLine($@"
(Half){target.half}.ToType({nameof(DateTime)}, {Culture}) = {(hCanConvert ? hResult.ToString() : "<undefined>")}
(Single){target.half}.ToType({nameof(DateTime)}, {Culture}) = {(sCanConvert ? sResult.ToString() : "<undefined>")}");

            Assert.That(hCanConvert, Is.EqualTo(sCanConvert));
            Assert.That(hResult, Is.EqualTo(sResult));
        }

        [Test]
        public static void to_uint16(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<ushort>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToUInt16(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToUInt16(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToUInt16", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_uint32(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<uint>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToUInt32(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToUInt32(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToUInt32", hResult, sResult, hCanConvert, sCanConvert);
        }

        [Test]
        public static void to_uint64(
            [ValueSource("ValueTargets")] (Half half, float) target)
        {
            IConvertibleVars<ulong>(target, out var hCon, out var sCon, out var hResult, out var sResult);

            var hCanConvert = true;
            var sCanConvert = true;

            try
            {
                hResult = hCon.ToUInt64(Culture);
            }
            catch
            {
                hCanConvert = false;
            }
            try
            {
                sResult = sCon.ToUInt64(Culture);
            }
            catch
            {
                sCanConvert = false;
            }

            IConvertibleResults(target.half, "ToUInt64", hResult, sResult, hCanConvert, sCanConvert);
#pragma warning restore CA1031 // Do not catch general exception types
        }

        [Test]
        public static void op_equality(
            [ValueSource("CompareToHalfTargets")] (Half left, Half right) half)
        {
            var (left, right) = ((float)half.left, (float)half.right);

            var actual = half.left == half.right;
            var expected = left == right;

            TestContext.WriteLine($@"
(Half){half.left} == {half.right} = {actual}
(Single){left} == {right} = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void op_inequality(
            [ValueSource("CompareToHalfTargets")] (Half left, Half right) half)
        {
            var (left, right) = ((float)half.left, (float)half.right);

            var actual = half.left != half.right;
            var expected = left != right;

            TestContext.WriteLine($@"
(Half){half.left} == {half.right} = {actual}
(Single){left} == {right} = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void op_greater_than(
            [ValueSource("CompareToHalfTargets")] (Half left, Half right) half)
        {
            var (left, right) = ((float)half.left, (float)half.right);

            var actual = half.left > half.right;
            var expected = left > right;

            TestContext.WriteLine($@"
(Half){half.left} == {half.right} = {actual}
(Single){left} == {right} = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void op_greater_than_or_equal(
            [ValueSource("CompareToHalfTargets")] (Half left, Half right) half)
        {
            var (left, right) = ((float)half.left, (float)half.right);

            var actual = half.left >= half.right;
            var expected = left >= right;

            TestContext.WriteLine($@"
(Half){half.left} == {half.right} = {actual}
(Single){left} == {right} = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void op_less_than(
            [ValueSource("CompareToHalfTargets")] (Half left, Half right) half)
        {
            var (left, right) = ((float)half.left, (float)half.right);

            var actual = half.left < half.right;
            var expected = left < right;

            TestContext.WriteLine($@"
(Half){half.left} == {half.right} = {actual}
(Single){left} == {right} = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void op_less_than_or_equal(
            [ValueSource("CompareToHalfTargets")] (Half left, Half right) half)
        {
            var (left, right) = ((float)half.left, (float)half.right);

            var actual = half.left <= half.right;
            var expected = left <= right;

            TestContext.WriteLine($@"
(Half){half.left} == {half.right} = {actual}
(Single){left} == {right} = {expected}");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public static void op_unary_plus(
            [ValueSource("ArithmaticTargets")] (Half left, Half _) target)
        {
            var value = (float)target.left;

            var actual = +target.left;
            var expected = +value;

            TestContext.WriteLine($@"
(Half)+{target.left} = {actual}
(Single)+{value} = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        [Test]
        public static void op_unary_minus(
            [ValueSource("ArithmaticTargets")] (Half left, Half _) target)
        {
            var value = (float)target.left;

            var actual = -target.left;
            var expected = -value;

            TestContext.WriteLine($@"
(Half)-{target.left} = {actual}
(Single)-{value} = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        [Test]
        public static void op_increment(
            [ValueSource("ArithmaticTargets")] (Half left, Half _) target)
        {
            var value = (float)target.left;

            var actual = ++target.left;
            var expected = ++value;

            TestContext.WriteLine($@"
(Half)++{target.left} = {actual}
(Single)++{value} = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        [Test]
        public static void op_decrement(
            [ValueSource("ArithmaticTargets")] (Half left, Half _) target)
        {
            var value = (float)target.left;

            var actual = --target.left;
            var expected = --value;

            TestContext.WriteLine($@"
(Half)--{target.left} = {actual}
(Single)--{value} = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        [Test]
        public static void op_addition(
            [ValueSource("ArithmaticTargets")] (Half left, Half right) target)
        {
            (var left, var right) = ((float)target.left, (float)target.right);

            var actual = target.left + target.right;
            var expected = left + right;

            TestContext.WriteLine($@"
(Half){target.left} + {target.right} = {actual}
(Single){left} + {right} = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        [Test]
        public static void op_subtraction(
            [ValueSource("ArithmaticTargets")] (Half left, Half right) target)
        {
            (var left, var right) = ((float)target.left, (float)target.right);

            var actual = target.left - target.right;
            var expected = left - right;

            TestContext.WriteLine($@"
(Half){target.left} - {target.right} = {actual}
(Single){left} - {right} = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        [Test]
        public static void op_multiplication(
            [ValueSource("ArithmaticTargets")] (Half left, Half right) target)
        {
            (var left, var right) = ((float)target.left, (float)target.right);

            var actual = target.left * target.right;
            var expected = left * right;

            TestContext.WriteLine($@"
(Half){target.left} * {target.right} = {actual}
(Single){left} * {right} = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        [Test]
        public static void op_division(
            [ValueSource("ArithmaticTargets")] (Half left, Half right) target)
        {
            (var left, var right) = ((float)target.left, (float)target.right);

            var actual = target.left / target.right;
            var expected = left / right;

            TestContext.WriteLine($@"
(Half){target.left} / {target.right} = {actual}
(Single){left} / {right} = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        [Test]
        public static void op_mod(
            [ValueSource("ArithmaticTargets")] (Half left, Half right) target)
        {
            (var left, var right) = ((float)target.left, (float)target.right);

            var actual = target.left % target.right;
            var expected = left % right;

            TestContext.WriteLine($@"
(Half){target.left} % {target.right} = {actual}
(Single){left} % {right} = {expected}");

            Assert.That(actual, Is.EqualTo((Half)expected));
        }

        public static string[] parse_targets() =>
            ToStringTargets
                .Select(a => a.ToString())
                .ToArray();
        public static (string, IFormatProvider)[] parse_with_provider_targets() =>
            ToStringTargets
                .SelectMany(a => ProviderTargets
                                     .Select(b => a.ToString(b)))
                .Zip(ToStringTargets
                         .SelectMany(_ => ProviderTargets))
                .ToArray();

        private static void IConvertibleVars<T>(in (Half half, float) target,
                                                out IConvertible half,
                                                out IConvertible single,
                                                out T halfResult,
                                                out T singleResult) =>
            (half, single, halfResult, singleResult) =
            (target.half, (float)target.half, default(T), default(T));

        private static void IConvertibleResults<T>(Half value, 
                                                   string method,
                                                   T hResult, 
                                                   T sResult, 
                                                   bool hCanConvert, 
                                                   bool sCanConvert)
        {
            TestContext.WriteLine($@"
(Half){value}.{method}({Culture}) = {(hCanConvert ? hResult.ToString() : "<undefined>")}
(Single){value}.{method}({Culture}) = {(sCanConvert ? sResult.ToString() : "<undefined>")}");

            Assert.That(hCanConvert, Is.EqualTo(sCanConvert));
            Assert.That(hResult, Is.EqualTo(sResult));
        }
    }
}
