using UnityEngine;

namespace bnj.utility_toolkit.Runtime
{
    /// <summary>Thin wrappers around <see cref="Time"/> for convenient access and time-scale control.</summary>
    public static class TimeUtils
    {
        /// <summary>The time in seconds it took to complete the last frame (<see cref="Time.deltaTime"/>).</summary>
        public static float DeltaTime => Time.deltaTime;
        /// <summary>The time at the beginning of the last fixed-rate update (<see cref="Time.fixedTime"/>).</summary>
        public static float FixedTime => Time.fixedTime;

        /// <summary>Gets or sets <see cref="Time.timeScale"/>.</summary>
        public static float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }

        // TODO: time utility for pausing the game from multiple places?
    }
}
