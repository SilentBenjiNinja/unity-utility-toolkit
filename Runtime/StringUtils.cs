namespace bnj.utility_toolkit.Runtime
{
    /// <summary>
    /// String formatting helpers for common game UI patterns:
    /// percentages, fractions, large-number suffixes (K / M / B / T), countdown timers, and CamelCase splitting.
    /// </summary>
    public static class StringUtils
    {
        static readonly string[] highNumberSuffixes = new[] { "", "K", "M", "B", "T" };

        /// <summary>Returns a formatted fraction string, e.g. <c>"3 / 5"</c>. Both values are ceiling-rounded.</summary>
        public static string GetFractionString(float value1, float value2) =>
            $"{value1.Ceil()} / {value2.Ceil()}";

        /// <summary>Returns <paramref name="value"/> as a whole-number percentage string, e.g. <c>0.75</c> → <c>"75%"</c>.</summary>
        public static string GetPercentageString(this float value) =>
            $"{(value * 100).Round()}%";

        /// <summary>Returns <paramref name="value"/> as a decimal string rounded to <paramref name="maxPrecision"/> places.</summary>
        public static string GetDecimalString(this float value, int maxPrecision = 0) =>
            maxPrecision == 0 ? value.Round().ToString() :
            value.Round(maxPrecision).ToString($"0.{new string('#', maxPrecision)}");

        /// <summary>Returns <paramref name="value"/> abbreviated with a K / M / B / T suffix, e.g. <c>12500</c> → <c>"12.5K"</c>.</summary>
        public static string GetHighDecimalString(this float value)
        {
            float displayedValue = value;
            int suffixId = 0;
            while (displayedValue > 999)
            {
                suffixId += 1;
                displayedValue /= 1000;
            }
            return $"{GetDecimalString(displayedValue, 1)}{highNumberSuffixes[suffixId]}";
        }

        /// <summary>Returns <paramref name="value"/> (in seconds) as a <c>m:ss</c> timer string, e.g. <c>90</c> → <c>"1:30"</c>.</summary>
        public static string GetTimerString(this float value)
        {
            int fullSeconds = value.Ceil();
            return $"{fullSeconds / 60}:{fullSeconds % 60:D2}";
        }

        /// <summary>Returns <paramref name="value"/> ceiling-rounded as a string.</summary>
        public static string GetCeiled(this float value) => value.Ceil().ToString();
        /// <summary>Returns <see langword="true"/> if <paramref name="input"/> is not null, empty, or whitespace.</summary>
        public static bool IsValid(this string input) => !string.IsNullOrWhiteSpace(input);

        /// <summary>Inserts spaces before capital letters in <paramref name="input"/>. Set <paramref name="preserveAcronyms"/> to keep consecutive capitals together.</summary>
        // Thanks to https://discussions.unity.com/t/take-a-string-and-put-spaces-between-words-in-that-string/943613/8
        public static string AddSpaces(this string input, bool preserveAcronyms = false) => System.Text.RegularExpressions.Regex.Replace(input, preserveAcronyms ? "(?<=[a-z])(?=[A-Z0-9])|(?<=[A-Z0-9])(?=[A-Z][a-z])" : "(?<=[a-z])(?=[A-Z])", " ");
    }
}
