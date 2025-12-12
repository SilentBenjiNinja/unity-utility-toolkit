using UnityEngine;

namespace bnj.utility_toolkit.Runtime
{
    public static class RandomUtils
    {
        const int RANDOM_SEED = 69420666;

        static System.Random _randomizer = new();

        public static int RandomInt => _randomizer.Next();
        public static int RandomIntBetween(int minInclusive, int maxExclusive) =>
            _randomizer.Next(minInclusive, maxExclusive);

        public static float RandomFloat => (float)_randomizer.NextDouble();
        public static float RandomFloatBetween(float min, float max) =>
            min + (max - min) * RandomFloat;

        public static bool Chance(float chance) => chance > RandomFloat;

        public static Vector2 RandomPointInsideCircle(Vector2 origin, float maxRadius) =>
            origin + Random.insideUnitCircle * maxRadius;

        public static Vector3 RandomPointOnSphere(Vector3 origin, float maxRadius) =>
            origin + Random.onUnitSphere * maxRadius;

        public static float Perlin(float x, float frequency = 1) => Mathf.PerlinNoise1D(x * frequency);

        public static Color RandomColor =>
            new(RandomFloat, RandomFloat, RandomFloat);
    }
}