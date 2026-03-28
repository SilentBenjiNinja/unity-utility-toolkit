using UnityEngine;

namespace bnj.utility_toolkit.Runtime
{
    // TODO: float, vector3, OnCircle, InsideSphere
    /// <summary>
    /// Random value helpers backed by <see cref="System.Random"/>.
    /// Covers integers, floats, probability checks, spatial distributions (circle, sphere),
    /// Perlin noise sampling, and random colours.
    /// </summary>
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

        public static Vector2 RandomInsideCircle(Vector2 origin, float maxRadius) =>
            origin + Random.insideUnitCircle * maxRadius;

        public static Vector3 RandomOnCircle(Vector2 origin, float radius) =>
            origin + Random.onUnitCircle * radius;

        public static Vector3 RandomInsideSphere(Vector3 origin, float maxRadius) =>
            origin + Random.insideUnitSphere * maxRadius;

        public static Vector3 RandomOnSphere(Vector3 origin, float radius) =>
            origin + Random.onUnitSphere * radius;

        public static float Perlin(float x, float frequency = 1) => Mathf.PerlinNoise1D(x * frequency);

        public static Color RandomColor =>
            new(RandomFloat, RandomFloat, RandomFloat);
    }
}