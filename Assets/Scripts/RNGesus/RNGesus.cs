using System;

namespace RNGesus
{
    public static class RNGesus
    {
        private static Random _random = new Random();

        // Allow setting a custom seed for reproducibility
        public static void SetSeed(int seed)
        {
            _random = new Random(seed);
        }

        // Allow swapping out the random implementation (for testing, etc.)
        public static void SetRandomSource(Random random)
        {
            _random = random ?? new Random();
        }

        public static float GetRandom()
        {
            return (float)_random.NextDouble();
        }

        public static int GetRangeInt(int min = 0, int max = 21)
        {
            return _random.Next(min, max);
        }

        public static float GetRangeFloat(float min = 0, float max = 1)
        {
            return (float)(_random.NextDouble() * (max - min) + min);
        }

        public static double GetDouble()
        {
            return _random.NextDouble();
        }
    }
}