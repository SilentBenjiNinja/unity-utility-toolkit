namespace bnj.utility_toolkit.Runtime.Audio
{
    public static class AudioUtils
    {
        public const float MIN_PITCH = -3f;
        public const float MAX_PITCH = 3f;

        public static float AmplitudeToDecibels(float amplitude) =>
            amplitude.Clamp(.0001f, 1f).Log10() * 20;
    }
}
