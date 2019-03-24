using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphicsh.Text
{
    public sealed class Chartable
    {
        // -------------
        // Table lengths
        // -------------

        /// <summary>
        /// The length of an ASCII chartable.
        /// </summary>
        public const int AsciiChartableLength = 128;

        /// <summary>
        /// The length of a chartable with each valid <see cref="System.Char"/> value specified.
        /// Note that it's not suitable for handling surrogate pairs.
        /// </summary>
        public const int CompleteChartableLength = char.MaxValue + 1;

        // ----------------
        // General creation
        // ----------------

        /// <summary>
        /// Creates a chartable based on provided explicit matches.
        /// </summary>
        /// <param name="explicitMatches">The char-indexed array of boolean values indicating whether a given character is matched or not.</param>
        /// <param name="isOutOfRangeMatched">Whether out of range characters should be treated as matching or not.</param>
        /// <returns></returns>
        public static Chartable CreateFromMatches(bool[] explicitMatches, bool isOutOfRangeMatched)
        {
            EnsureChartableLengthIsInRange(explicitMatches.Length, nameof(explicitMatches));
            return new Chartable(explicitMatches, isOutOfRangeMatched);
        }

        /// <summary>
        /// Creates a chartable of characters meeting a condition specified by the given predicate.
        /// Only characters within the given chartable length (i.e. from U+0 to U+[length-1]) are taken into account.
        /// </summary>
        /// <param name="predicate">The predicate corresponding to the condition.</param>
        /// <param name="length">The length of the created chartable.</param>
        /// <param name="isOutOfRangeMatched">Whether out of range characters should be treated as matching or not.</param>
        /// <returns>The chartable with all characters within range meeting the condition matched.</returns>
        public static Chartable CreateFromPredicate(Func<char, bool> predicate, int length, bool isOutOfRangeMatched = false)
        {
            EnsureChartableLengthIsInRange(length, nameof(length));
            var explicitMatches = Enumerable.Range(0, length).Select(i => predicate((char)i)).ToArray();
            return new Chartable(explicitMatches, isOutOfRangeMatched);
        }

        /// <summary>
        /// Creates a chartable of ASCII characters meeting a condition specified by the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate corresponding to the condition.</param>
        /// <param name="isOutOfRangeMatched">Whether out of range characters should be treated as matching or not.</param>
        /// <returns>The chartable with all ASCII characters meeting the condition matched.</returns>
        public static Chartable CreateFromPredicate(Func<char, bool> predicate, bool isOutOfRangeMatched = false)
            => CreateFromPredicate(predicate, AsciiChartableLength);

        /// <summary>
        /// Creates a chartable corresponding to a specific character set within the given chartable length (i.e. from U+0 to U+[length-1]).
        /// </summary>
        /// <param name="charset">The character set to cover by the chartable.</param>
        /// <param name="length">The length of the created chartable.</param>
        /// <param name="isOutOfRangeMatched">Whether out of range characters should be treated as matching or not.</param>
        /// <returns>The chartable with all given characters matched.</returns>
        public static Chartable CreateFromCharset(IEnumerable<char> charset, int length, bool isOutOfRangeMatched = false)
        {
            EnsureChartableLengthIsInRange(length, nameof(length));
            EnsureCharsetFitsWithinLength(charset, length);

            var explicitMatches = new bool[length];
            foreach (char c in charset)
                explicitMatches[c] = true;

            return new Chartable(explicitMatches, isOutOfRangeMatched);
        }

        /// <summary>
        /// Creates a chartable corresponding to a specific set of ASCII characters.
        /// </summary>
        /// <param name="charset">The character set to cover by the chartable.</param>
        /// <param name="isOutOfRangeMatched">Whether out of range characters should be treated as matching or not.</param>
        /// <returns>The chartable with all given ASCII characters matched.</returns>
        public static Chartable CreateFromCharset(IEnumerable<char> charset, bool isOutOfRangeMatched = false)
            => CreateFromCharset(charset, AsciiChartableLength);

        /// <summary>
        /// Creates a chartable corresponding to a specific character set within the given chartable length (i.e. from U+0 to U+[length-1]).
        /// </summary>
        /// <param name="charset">The character set to cover by the chartable.</param>
        /// <param name="length">The length of the created chartable.</param>
        /// <param name="isOutOfRangeMatched">Whether out of range characters should be treated as matching or not.</param>
        /// <returns>The chartable with all given characters matched.</returns>
        public static Chartable CreateFromCharset(string charset, int length, bool isOutOfRangeMatched = false)
            => CreateFromCharset(charset.ToCharArray(), length);

        /// <summary>
        /// Creates a chartable corresponding to a specific set of ASCII characters.
        /// </summary>
        /// <param name="charset">The character set to cover by the chartable.</param>
        /// <param name="isOutOfRangeMatched">Whether out of range characters should be treated as matching or not.</param>
        /// <returns>The chartable with all given ASCII characters matched.</returns>
        public static Chartable CreateFromCharset(string charset, bool isOutOfRangeMatched = false)
            => CreateFromCharset(charset.ToCharArray());

        // ------------------------
        // Creation from chartables
        // ------------------------

        /// <summary>
        /// Creates an exact opposite of the given chartable.
        /// </summary>
        /// <param name="chartableToNegate">The chartable used to create the negated chartable.</param>
        /// <returns>The negated chartable.</returns>
        public static Chartable CreateNegated(Chartable chartableToNegate)
        {
            return new Chartable(chartableToNegate._explicitMatches.Select(value => !value).ToArray(), !chartableToNegate._isOutOfRangeMatched);
        }

        /// <summary>
        /// Creates a chartable with all individual chartables' characters matched.
        /// </summary>
        /// <param name="chartables">The chartables to combine.</param>
        /// <returns>The combined chartable.</returns>
        public static Chartable CreateCombined(IEnumerable<Chartable> chartables)
        {
            var isOutOfRangeMatched = chartables.Any(chartable => chartable._isOutOfRangeMatched);
            var length = isOutOfRangeMatched ? chartables.Min(chartable => chartable._explicitMatches.Length) : chartables.Max(chartable => chartable._explicitMatches.Length);
            var explicitMatches = Enumerable.Range(0, length)
                .Select(character => chartables.Any(chartable => chartable[(char)character]))
                .ToArray();
            return new Chartable(explicitMatches, isOutOfRangeMatched);
        }

        /// <summary>
        /// Creates a chartable with all individual chartables' characters matched.
        /// </summary>
        /// <param name="chartables">The chartables to combine.</param>
        /// <returns>The combined chartable.</returns>
        public static Chartable CreateCombined(params Chartable[] chartables)
            => CreateCombined(chartables as IEnumerable<Chartable>);

        // ------
        // Guards
        // ------

        private static void EnsureChartableLengthIsInRange(int length, string paramName)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(paramName, "The chartable length should be non-negative.");

            if (length > CompleteChartableLength)
                throw new ArgumentOutOfRangeException(paramName, $"The chartable length cannot exceed {CompleteChartableLength}.");
        }

        private static void EnsureCharsetFitsWithinLength(IEnumerable<char> characters, int length)
        {
            foreach (char character in characters)
            {
                if (character >= length)
                    throw new ArgumentOutOfRangeException(nameof(characters), $"The character '{character}' (U+{((int)character).ToString("X4")}) is beyond the given chartable length.");
            }
        }

        // ----------
        // Data setup
        // ----------

        private bool[] _explicitMatches;
        private bool _isOutOfRangeMatched;

        private Chartable(bool[] explicitMatches, bool isOutOfRangeMatched)
        {
            _explicitMatches = explicitMatches;
            _isOutOfRangeMatched = isOutOfRangeMatched;
        }

        // ------------
        // Data methods
        // ------------

        public bool Matches(char character)
        {
            return this[character];
        }

        public bool this[char character]
        {
            get => character < _explicitMatches.Length ? _explicitMatches[character] : _isOutOfRangeMatched;
        }
    }
}
