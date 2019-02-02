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
        // --------------------------
        // Simple expectations checks
        // --------------------------

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
        public void Read_Works(string str)
        {
            TestSimpleExpectation(
                baseString: str, readAfterCheck: false,
                check: (baseString, i, scanner) => (i < baseString.Length ? baseString[i] : '\0') == scanner.Read()
                );
        }

        [Theory]
        [MemberData(nameof(SimpleExpectationParameters))]
        public void Buffer_Works(string str)
        {
            TestSimpleExpectation(
                baseString: str, readAfterCheck: false,
                check: (baseString, i, scanner) => (i < baseString.Length ? baseString[i] : '\0') == scanner.Buffer()
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
            var scanner = TextScanner.CreateFromString(baseString);
            for (var i = 0; i < 20; i++)
            {
                Assert.True(check(baseString, i, scanner));
                if (readAfterCheck)
                    scanner.Read();
            }
        }

        // -------------------------
        // Buffer manipulation tests
        // -------------------------

        [Theory]
        [InlineData("Hello, world!", "Hello", ", world", "!", "")]
        [InlineData("Lorem ipsum dolor sit amet", "Lo", "rem ip", "sum d", "olor sit a", "met")]
        public void Flush_Works(string baseString, params string[] flushResults)
        {
            var scanner = TextScanner.CreateFromString(baseString);

            foreach (var result in flushResults)
            {
                for (var i = 0; i < result.Length; i++)
                    scanner.Buffer();
                Assert.Equal(result, scanner.Flush());
            }
        }
    }
}
