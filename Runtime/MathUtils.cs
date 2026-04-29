using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace bnj.utility_toolkit.Runtime
{
    // TODO: more stuff for vectors, matrix, quaternion, fixed point numbers?
    /// <summary>
    /// Lightweight math helpers and extension methods for <see langword="float"/>, <see langword="int"/>,
    /// <see cref="Vector2"/>, <see cref="Vector3"/>, <see cref="Color"/>, and <see cref="Transform"/>.
    /// Adds 2D rotation/look-vector utilities and avoids allocations for common operations.
    /// </summary>
    public static class MathUtils
    {
        public static float Pow(this float baseValue, float exp) => Mathf.Pow(baseValue, exp);
        public static int Pow(this int baseValue, int exp) => (int)Pow((float)baseValue, exp);

        public static float Log10(this float value) => Mathf.Log10(value);
        public static float Sqrt(this float value) => Mathf.Sqrt(value);
        public static float Sin(this float value) => Mathf.Sin(value);

        public static int Min(int value1, int value2) => value1 > value2 ? value2 : value1;
        public static int Max(int value1, int value2) => value1 < value2 ? value2 : value1;

        public static float Min(float value1, float value2) => value1 > value2 ? value2 : value1;
        public static float Max(float value1, float value2) => value1 < value2 ? value2 : value1;

        public static int Clamp(this int value, int min, int max) => Min(Max(min, value), max);
        public static float Clamp(this float value, float min, float max) => Min(Max(min, value), max);
        public static float Clamp01(this float value) => Clamp(value, 0, 1);

        public static float Abs(this float value) => value < 0 ? value * -1 : value;

        public static int Floor(this float value) => (int)Math.Floor(value);
        public static int Ceil(this float value) => (int)Math.Ceiling(value);
        public static int Round(this float value) => (int)Math.Round(value, MidpointRounding.AwayFromZero);
        public static float Round(this float value, int decimalPlaces) => (float)Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);

        #region Vectors, Colors, Quaternions

        public static float Lerp(float value1, float value2, float t) => value1 + (value2 - value1) * t;
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float t) => value1 + (value2 - value1) * t;
        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float t) => value1 + (value2 - value1) * t;
        public static Color Lerp(Color value1, Color value2, float t) => value1 + (value2 - value1) * t;

        public static Vector2 ToVector2(this Vector3 vec3) => new(vec3.x, vec3.y);
        public static Color WithAlpha(this Color col, float a) => new(col.r, col.g, col.b, a.Clamp01());

        public static Vector3 Get2DEulerRotationForLookVector(Vector3 lookVector) =>
            new(0, 0, Vector3.SignedAngle(Vector3.up, lookVector.normalized, Vector3.forward));

        public static Vector3 Get2DLookVectorFromAngle(float angle)
        {
            var angleRad = angle * Mathf.Deg2Rad;
            return new(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static Quaternion Get2DLookAtRotation(Vector3 from, Vector3 to) =>
            Quaternion.Euler(Get2DEulerRotationForLookVector((to - from).normalized));

        public static bool IsDistanceBetweenPointsCloserThan(Vector3 a, Vector3 b, float distance) =>
            (a - b).sqrMagnitude < distance * distance;

        #endregion

        #region Fast functions

        // --- Fast approximations (use where precision is not critical) ---

        // Quake III fast inverse sqrt — ~4x faster than Mathf.Sqrt, max error ~0.175%
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrtFast(this float value)
        {
            float half = 0.5f * value;
            int i = BitConverter.SingleToInt32Bits(value);
            i = 0x5f3759df - (i >> 1);
            float y = BitConverter.Int32BitsToSingle(i);
            y *= 1.5f - half * y * y;  // one Newton-Raphson iteration
            return value * y;           // convert from inverse sqrt to sqrt
        }

        // Bhaskara sine approximation — max error ~0.17%, valid for full float range
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SinFast(this float x)
        {
            x %= 2f * Mathf.PI;
            if (x < 0) x += 2f * Mathf.PI;
            bool mirror = x > Mathf.PI;
            if (mirror) x -= Mathf.PI;
            float x2 = x * (Mathf.PI - x);
            float result = 4f * x2 / (40.528473f - x2);
            return mirror ? -result : result;
        }

        // Fast log2 via IEEE 754 bit layout — rough magnitude estimate
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log2Fast(this float value)
        {
            int bits = BitConverter.SingleToInt32Bits(value);
            return (bits >> 23) - 127 + (float)(bits & 0x7FFFFF) / (1 << 23);
        }

        // log10(x) = log2(x) / log2(10)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log10Fast(this float value) => value.Log2Fast() * 0.30103f;

        #endregion
    }
}