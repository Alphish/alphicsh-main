using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Alphicsh.Text.Scanning
{
    // Query methods checks
    public partial class TextScannerTests
    {
        [Fact]
        public void Query_Works()
        {
            var tagQuery = TextScannerQueryBuilder.BeginQuery()
                .TakeString(new string[] { ":" }).CheckAny(ResultCheckMode.Expect).SkipThen()
                .Take(2).CheckChartable(Chartable.CreateFromCharset(Charsets.AsciiDigits), ResultCheckMode.Expect).AppendThen()
                .Take(1).CheckChartable(Chartable.CreateFromCharset(Charsets.AsciiLetters), ResultCheckMode.Try).AppendThen()
                .TakeString(new string[] { ":" }).CheckAny(ResultCheckMode.Expect).SkipThen()
                .EndQuery();

            var scanner = TextScanner.CreateFromString(":20C::21::17a::64z:");
            Assert.Equal("20C", scanner.ReadQuery(tagQuery));
            Assert.Equal("21", scanner.ReadQuery(tagQuery));
            Assert.Equal("17a", scanner.ReadQuery(tagQuery));
            Assert.Equal("64z", scanner.ReadQuery(tagQuery));
        }
    }
}
