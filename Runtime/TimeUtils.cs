using UnityEngine;

namespace bnj.utility_toolkit.Runtime
{
    public static class TimeUtils
    {
        public static float DeltaTime => Time.deltaTime;
        public static float FixedTime => Time.fixedTime;

        public static float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }

        // TODO: time utility for pausing the game from multiple places?
    }
}
