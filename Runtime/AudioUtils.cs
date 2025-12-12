namespace bnj.utility_toolkit.Runtime
{
    public static class AudioUtils
    {
        public const float MIN_VOL = .0001f;
        public const float MAX_VOL = 1f;

        public const float MIN_PITCH = -3f;
        public const float MAX_PITCH = 3f;

        public static float AmplitudeToDecibels(float amplitude) =>
            amplitude.Clamp(MIN_VOL, MAX_VOL).Log10() * 20;
    }
}
