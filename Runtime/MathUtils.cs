using UnityEngine;

namespace bnj.utility_toolkit.Runtime
{
    // TODO: more stuff for vectors, matrix, quaternion, fixed point numbers?
    // cost-effective sine, log, etc
    public static class MathUtils
    {
        public static float Log10(this float value) => Mathf.Log10(value);
        public static float Sqrt(this float value) => Mathf.Sqrt(value);

        public static int Clamp(this int value, int min, int max) => Min(Max(min, value), max);
        public static float Clamp(this float value, float min, float max) => Min(Max(min, value), max);
        public static float Clamp01(this float value) => Clamp(value, 0, 1);

        public static float Abs(this float value) => value < 0 ? value * -1 : value;

        public static int Floor(this float value) => (int)value;
        public static int Ceil(this float value) => (int)(value % 1 == 0 ? value : value + 1);
        public static int Round(this float value) => (int)(value % 1 < .5f ? value : value + 1);
        public static int Round(this double value) => (int)(value % 1 < .5f ? value : value + 1);

        public static Vector2 ToVector2(this Vector3 vec3) => new(vec3.x, vec3.y);

        public static Color WithAlpha(this Color col, float a) => new(col.r, col.g, col.b, a.Clamp01());

        public static float Pow(this float baseValue, float exp) => Mathf.Pow(baseValue, exp);
        public static int Pow(this int baseValue, int exp) => (int)Pow((float)baseValue, exp);
        public static float Sin(float value) => Mathf.Sin(value);

        public static int Min(int value1, int value2) => value1 > value2 ? value2 : value1;
        public static int Max(int value1, int value2) => value1 < value2 ? value2 : value1;

        public static float Min(float value1, float value2) => value1 > value2 ? value2 : value1;
        public static float Max(float value1, float value2) => value1 < value2 ? value2 : value1;

        public static float Lerp(float value1, float value2, float t) => value1 + (value2 - value1) * t;
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float t) => value1 + (value2 - value1) * t;
        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float t) => value1 + (value2 - value1) * t;
        public static Color Lerp(Color value1, Color value2, float t) => value1 + (value2 - value1) * t;

        public static Vector3 LocalToWorldPosition(Vector3 localPosition, Transform transform) =>
            transform.position + (Vector3)(transform.localToWorldMatrix * localPosition);

        public static Vector3 Get2DEulerRotationForLookVector(Vector3 lookVector) =>
            new(0, 0, Vector3.SignedAngle(Vector3.up, lookVector.normalized, Vector3.forward));

        public static Vector3 Get2DLookVectorFromAngle(float angle)
        {
            var angleRad = angle * Mathf.PI / 180;
            return new(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static Quaternion Get2DLookAtRotation(Vector3 from, Vector3 to) =>
            Quaternion.Euler(Get2DEulerRotationForLookVector((to - from).normalized));

        public static bool IsDistanceBetweenPointsCloserThan(Vector3 a, Vector3 b, float distance) =>
            (a - b).sqrMagnitude < distance * distance;
    }
}