using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    public static class Charsets
    {
        /// <summary>
        /// Gets all ASCII digits.
        /// </summary>
        public const string AsciiDigits = "0123456789";
        /// <summary>
        /// Gets all ASCII uppercase letters.
        /// </summary>
        public const string AsciiUppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// Gets all ASCII lowercase letters.
        /// </summary>
        public const string AsciiLowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// Gets all ASCII letters.
        /// </summary>
        public const string AsciiLetters = AsciiUppercaseLetters + AsciiLowercaseLetters;
        /// <summary>
        /// Gets all ASCII alphanumeric characters.
        /// </summary>
        public const string AsciiAlphanumeric = AsciiDigits + AsciiLetters;

        /// <summary>
        /// Gets all ASCII whitespace characters (tab, line feed, vertical tab, form feed, carriage return, space).
        /// </summary>
        public const string AsciiWhitespace = "\t\n\v\f\r ";
        /// <summary>
        /// Gets all insignificant whitespace characters specified for JSON format (tab, line feed, carriage return, space).
        /// </summary>
        public const string JsonWhitespace = "\t\n\r ";

        /// <summary>
        /// Gets all ASCII printable characters.
        /// </summary>
        public const string AsciiPrintable = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";


    }
}
