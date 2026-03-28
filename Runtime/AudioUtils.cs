namespace bnj.utility_toolkit.Runtime.Audio
{
    /// <summary>Audio math helpers.</summary>
    public static class AudioUtils
    {
        /// <summary>Minimum valid pitch value for an <c>AudioSource</c>.</summary>
        public const float MIN_PITCH = -3f;
        /// <summary>Maximum valid pitch value for an <c>AudioSource</c>.</summary>
        public const float MAX_PITCH = 3f;

        /// <summary>
        /// Converts a linear amplitude (0–1) to decibels.
        /// Clamps input to [0.0001, 1] before conversion to avoid <c>-Infinity</c>.
        /// </summary>
        public static float AmplitudeToDecibels(float amplitude) =>
            amplitude.Clamp(.0001f, 1f).Log10() * 20;
    }
}
