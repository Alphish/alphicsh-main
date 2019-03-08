using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Alphicsh.Text.Tests
{
    public class TextScannerTests
    {
        // ------------------------------------------
        // Simple properties and Peek()/Read() checks
        // ------------------------------------------

        // Tests

        [Theory]
        [MemberData(nameof(SimpleExpectationParameters))]
        public void CurrentCharacter_Works(string str)
        {
            TestSimpleExpectation(
                baseString: str, readAfterCheck: true,
                check: (baseString, i, scanner) => (i < baseString.Length ? baseString[i] : '\0') == scanner.CurrentCharacter
                );
        }

        [Theory]
        [MemberData(nameof(SimpleExpectationParameters))]
        public void IsEndOfText_Works(string str)
        {
            TestSimpleExpectation(
                baseString: str, readAfterCheck: true,
                check: (baseString, i, scanner) => i >= baseString.Length == scanner.IsEndOfText
                );
        }

        [Theory]
        [MemberData(nameof(SimpleExpectationParameters))]
        public void Position_Works(string str)
        {
            TestSimpleExpectation(
                baseString: str, readAfterCheck: true,
                check: (baseString, i, scanner) => Math.Min(i, baseString.Length) + 1 == scanner.Position.Index
                );
        }

        [Theory]
        [MemberData(nameof(SimpleExpectationParameters))]
        public void Peek_Works(string str)
        {
            TestSimpleExpectation(
                baseString: str, readAfterCheck: true,
                check: (baseString, i, scanner) => (i < baseString.Length ? baseString[i] : '\0') == scanner.Peek()
                );
        }

        [Theory]
        [MemberData(nameof(SimpleExpectationParameters))]
        public void Read_Works(string str)
        {
            TestSimpleExpectation(
                baseString: str, readAfterCheck: false,
                check: (baseString, i, scanner) => (i < baseString.Length ? baseString[i] : '\0') == scanner.Read()
                );
        }

        // Helper methods

        public static object[][] SimpleExpectationParameters => new object[][]
        {
            new object[] { "Hello, world!" },
            new object[] { "Lorem" }
        };

        private delegate bool SimpleExpectationCheck(string baseString, int i, ITextScanner scanner);

        private void TestSimpleExpectation(string baseString, bool readAfterCheck, SimpleExpectationCheck check)
        {
            using (var scanner = TextScanner.CreateFromString(baseString))
            {
                for (var i = 0; i < 20; i++)
                {
                    Assert.True(check(baseString, i, scanner));
                    if (readAfterCheck)
                        scanner.Read();
                }
            }
        }

        // -------------------------
        // Block Peek*/Read* methods
        // -------------------------

    }
}
