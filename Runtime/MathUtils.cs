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
        /// <summary>Raises <paramref name="baseValue"/> to the power of <paramref name="exp"/>.</summary>
        public static float Pow(this float baseValue, float exp) => Mathf.Pow(baseValue, exp);
        /// <inheritdoc cref="Pow(float,float)"/>
        public static int Pow(this int baseValue, int exp) => (int)Pow((float)baseValue, exp);

        /// <summary>Returns the base-10 logarithm of <paramref name="value"/>.</summary>
        public static float Log10(this float value) => Mathf.Log10(value);
        /// <summary>Returns the square root of <paramref name="value"/>.</summary>
        public static float Sqrt(this float value) => Mathf.Sqrt(value);
        /// <summary>Returns the sine of <paramref name="value"/> in radians.</summary>
        public static float Sin(this float value) => Mathf.Sin(value);

        /// <summary>Returns the smaller of two values.</summary>
        public static int Min(int value1, int value2) => value1 > value2 ? value2 : value1;
        /// <summary>Returns the larger of two values.</summary>
        public static int Max(int value1, int value2) => value1 < value2 ? value2 : value1;

        /// <inheritdoc cref="Min(int,int)"/>
        public static float Min(float value1, float value2) => value1 > value2 ? value2 : value1;
        /// <inheritdoc cref="Max(int,int)"/>
        public static float Max(float value1, float value2) => value1 < value2 ? value2 : value1;

        /// <summary>Clamps <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/>.</summary>
        public static int Clamp(this int value, int min, int max) => Min(Max(min, value), max);
        /// <inheritdoc cref="Clamp(int,int,int)"/>
        public static float Clamp(this float value, float min, float max) => Min(Max(min, value), max);
        /// <summary>Clamps <paramref name="value"/> between 0 and 1.</summary>
        public static float Clamp01(this float value) => Clamp(value, 0, 1);

        /// <summary>Returns the absolute value of <paramref name="value"/>.</summary>
        public static float Abs(this float value) => value < 0 ? value * -1 : value;

        /// <summary>Returns the largest integer less than or equal to <paramref name="value"/>.</summary>
        public static int Floor(this float value) => (int)Math.Floor(value);
        /// <summary>Returns the smallest integer greater than or equal to <paramref name="value"/>.</summary>
        public static int Ceil(this float value) => (int)Math.Ceiling(value);
        /// <summary>Rounds <paramref name="value"/> to the nearest integer, with midpoints rounding away from zero.</summary>
        public static int Round(this float value) => (int)Math.Round(value, MidpointRounding.AwayFromZero);
        /// <summary>Rounds <paramref name="value"/> to <paramref name="decimalPlaces"/> decimal places, with midpoints rounding away from zero.</summary>
        public static float Round(this float value, int decimalPlaces) => (float)Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);

        #region Vectors, Colors, Quaternions

        /// <summary>Linearly interpolates between <paramref name="value1"/> and <paramref name="value2"/> by <paramref name="t"/>. Does not clamp <paramref name="t"/>.</summary>
        public static float Lerp(float value1, float value2, float t) => value1 + (value2 - value1) * t;
        /// <inheritdoc cref="Lerp(float,float,float)"/>
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float t) => value1 + (value2 - value1) * t;
        /// <inheritdoc cref="Lerp(float,float,float)"/>
        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float t) => value1 + (value2 - value1) * t;
        /// <inheritdoc cref="Lerp(float,float,float)"/>
        public static Color Lerp(Color value1, Color value2, float t) => value1 + (value2 - value1) * t;

        /// <summary>Returns a <see cref="Vector2"/> with the x and y components of <paramref name="vec3"/>.</summary>
        public static Vector2 ToVector2(this Vector3 vec3) => new(vec3.x, vec3.y);
        /// <summary>Returns a copy of <paramref name="col"/> with its alpha channel set to <paramref name="a"/>.</summary>
        public static Color WithAlpha(this Color col, float a) => new(col.r, col.g, col.b, a.Clamp01());

        /// <summary>Returns a Z-axis Euler rotation (in degrees) whose forward direction matches <paramref name="lookVector"/>.</summary>
        public static Vector3 Get2DEulerRotationForLookVector(Vector3 lookVector) =>
            new(0, 0, Vector3.SignedAngle(Vector3.up, lookVector.normalized, Vector3.forward));

        /// <summary>Returns the 2D unit look vector for a given <paramref name="angle"/> in degrees, measured from world up.</summary>
        public static Vector3 Get2DLookVectorFromAngle(float angle)
        {
            var angleRad = angle * Mathf.Deg2Rad;
            return new(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        /// <summary>Returns a <see cref="Quaternion"/> that rotates a 2D object at <paramref name="from"/> to face <paramref name="to"/>.</summary>
        public static Quaternion Get2DLookAtRotation(Vector3 from, Vector3 to) =>
            Quaternion.Euler(Get2DEulerRotationForLookVector((to - from).normalized));

        /// <summary>Returns <see langword="true"/> if the distance between <paramref name="a"/> and <paramref name="b"/> is less than <paramref name="distance"/>. Uses squared magnitude to avoid a square root.</summary>
        public static bool IsDistanceBetweenPointsCloserThan(Vector3 a, Vector3 b, float distance) =>
            (a - b).sqrMagnitude < distance * distance;

        #endregion

        #region Fast functions

        /// <summary>
        /// Fast square root using the Quake III inverse sqrt trick. Approximately 4× faster than <see cref="Sqrt"/>, max error ~0.175%.
        /// Not suitable where precision is critical.
        /// </summary>
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

        /// <summary>Bhaskara sine approximation. Max error ~0.17%, valid for the full float range. Not suitable where precision is critical.</summary>
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

        /// <summary>Fast base-2 logarithm via IEEE 754 bit manipulation. Suitable for magnitude estimates only.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log2Fast(this float value)
        {
            int bits = BitConverter.SingleToInt32Bits(value);
            return (bits >> 23) - 127 + (float)(bits & 0x7FFFFF) / (1 << 23);
        }

        /// <summary>Fast base-10 logarithm derived from <see cref="Log2Fast"/>. Suitable for magnitude estimates only.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log10Fast(this float value) => value.Log2Fast() * 0.30103f;

        #endregion
    }
}