namespace bnj.utility_toolkit.Runtime
{
    /// <summary>
    /// String formatting helpers for common game UI patterns:
    /// percentages, fractions, large-number suffixes (K / M / B / T), countdown timers, and CamelCase splitting.
    /// </summary>
    public static class StringUtils
    {
        static readonly string[] highNumberSuffixes = new[] { "", "K", "M", "B", "T" };

        public static string GetFractionString(float value1, float value2) =>
            $"{GetDecimalString(value1.Ceil())} / {GetDecimalString(value2.Ceil())}";

        public static string GetPercentageString(this float value) =>
            $"{(value * 100).Round()}%";

        // dont use for values > 2M, can break bc of overflow (made double, not tested!)
        public static string GetDecimalString(this float value, int maxPrecision = 0)
        {
            // TODO: the applied precision is not working as intended, e.g. rounding to 2.10 where it should be 2.1!
            int appliedPrecision = 0;
            double roundedValue = ((double)value * 10.Pow(maxPrecision)).Round() / 10.Pow(maxPrecision);
            for (int i = 0; i < maxPrecision; i++)
            {
                if (roundedValue % .1f.Pow(i) < .5f * .1f.Pow(i + 1))
                    break;

                appliedPrecision = i + 1;
            }

            //DebugUtils.Log(value + " -> " + roundedValue);
            return roundedValue.ToString($"N{appliedPrecision}");
        }

        public static string GetHighDecimalString(this float value)
        {
            float displayedValue = value;
            int suffixId = 0;
            while (displayedValue > 1000)
            {
                suffixId += 1;
                displayedValue /= 1000;
            }
            return $"{GetDecimalString(displayedValue, 1)}{highNumberSuffixes[suffixId]}";
        }

        public static string GetTimerString(this float value)
        {
            int fullSeconds = value.Ceil();
            int minutes = fullSeconds / 60;
            int seconds = fullSeconds % 60;
            return $"{minutes}:{(seconds < 10 ? $"0{seconds}" : seconds)}";
        }

        public static string GetCeiled(this float value) => value.Ceil().ToString();
        public static bool IsValid(this string input) => !string.IsNullOrWhiteSpace(input);

        // Thanks to https://discussions.unity.com/t/take-a-string-and-put-spaces-between-words-in-that-string/943613/8
        public static string AddSpaces(this string input, bool preserveAcronyms = false) => System.Text.RegularExpressions.Regex.Replace(input, preserveAcronyms ? "(?<=[a-z])(?=[A-Z0-9])|(?<=[A-Z0-9])(?=[A-Z][a-z])" : "(?<=[a-z])(?=[A-Z])", " ");
    }
}