using System;
using System.Collections.Generic;
using System.Text;

using M = System.Math;

namespace System
{
    public static class MathH
    {
        #region [ Fields ]

        public static Half E =>
            (Half)M.E;

        public static Half PI =>
            (Half)M.PI;

        #endregion

        #region [ Methods ]

        public static Half Abs(Half value) =>
            Half.Abs(value);

        public static Half Acos(Half x) =>
            (Half)M.Acos(x);

        public static Half Acosh(Half x) =>
            (Half)M.Acosh(x);

        public static Half Asin(Half x) =>
            (Half)M.Asin(x);

        public static Half Asinh(Half x) =>
            (Half)M.Asinh(x);

        public static Half Atan(Half x) =>
            (Half)M.Atan(x);

        public static Half Atan2(Half x,
                                 Half y) =>
            (Half)M.Atan2(y, x);

        public static Half Atanh(Half x) =>
            (Half)M.Atanh(x);

        public static Half Cbrt(Half x) =>
            (Half)M.Cbrt(x);

        public static Half Ceiling(Half x) =>
            (Half)M.Ceiling(x);

        public static Half Cos(Half x) =>
            (Half)M.Cos(x);

        public static Half Cosh(Half x) =>
            (Half)M.Cosh(x);

        public static Half Exp(Half x) =>
            (Half)M.Exp(x);

        public static Half Floor(Half x) =>
            (Half)M.Floor(x);

        public static Half IEEERemainder(Half x,
                                         Half y) =>
            (Half)M.IEEERemainder(x, y);

        public static Half Log(Half x) =>
            (Half)M.Log(x);

        public static Half Log(Half x,
                               Half y) =>
            (Half)M.Log(x, y);

        public static Half Log10(Half x) =>
            (Half)M.Log10(x);

        public static Half Max(Half x,
                               Half y) =>
            (Half)M.Max(x, y);

        public static Half Min(Half x,
                               Half y) =>
            (Half)M.Min(x, y);

        public static Half Pow(Half x,
                               Half y) =>
            (Half)M.Pow(x, y);

        public static Half Round(Half x,
                                 MidpointRounding mode) =>
            (Half)M.Round(x, mode);

        public static Half Round(Half x,
                                 int digits,
                                 MidpointRounding mode) =>
            (Half)M.Round(x, digits, mode);

        public static Half Round(Half x) =>
            (Half)M.Round(x);

        public static Half Round(Half x,
                                 int digits) =>
            (Half)M.Round(x, digits);

        public static int Sign(Half x) =>
            x switch
            {
                var h when Half.IsNaN(h) => throw new ArithmeticException(),
                var h when h > 0 => 1,
                var h when h < 0 => -1,
                _ => 0
            };

        public static Half Sin(Half x) =>
            (Half)M.Sin(x);

        public static Half Sinh(Half x) =>
            (Half)M.Sinh(x);

        public static Half Sqrt(Half x) =>
            (Half)M.Sqrt(x);

        public static Half Tan(Half x) =>
            (Half)M.Tan(x);

        public static Half Tanh(Half x) =>
            (Half)M.Tanh(x);

        public static Half Truncate(Half x) =>
            (Half)M.Truncate(x);

        #endregion
    }
}
