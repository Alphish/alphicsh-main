using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public sealed class TextScannerBoundQueryBuilder : TextScannerQueryBuilder, ITextScannerBoundQueryBuilder
    {
        public static ITextScannerBoundQueryBuilder BeginBoundQuery(ITextScanner boundScanner)
        {
            return new TextScannerBoundQueryBuilder(boundScanner);
        }

        private ITextScanner _boundScanner;

        private TextScannerBoundQueryBuilder(ITextScanner boundScanner)
            : base()
        {
            _boundScanner = boundScanner;
        }

        // shadowing with new functions

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.Take(int length)
            => Take(length) as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.TakeRest()
            => TakeRest() as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.TakeLine(LineReadingHandling lineReadingHandling)
            => TakeLine(lineReadingHandling) as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.TakeWhile(Func<char, bool> predicate)
            => TakeWhile(predicate) as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.TakeChartable(Chartable chartable)
            => TakeChartable(chartable) as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.TakeString(IEnumerable<string> strings)
            => TakeString(strings) as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.CheckAny(ResultCheckMode mode)
            => CheckAny(mode) as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.CheckResult(Func<string, bool> predicate, ResultCheckMode mode)
            => CheckResult(predicate, mode) as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.CheckString(IEnumerable<string> strings, ResultCheckMode mode)
            => CheckString(strings, mode) as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.CheckChartable(Chartable chartable, ResultCheckMode mode)
            => CheckChartable(chartable, mode) as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.AppendThen()
            => AppendThen() as ITextScannerBoundQueryBuilder;

        ITextScannerBoundQueryBuilder ITextScannerBoundQueryBuilder.SkipThen()
            => SkipThen() as ITextScannerBoundQueryBuilder;

        // applying the query to the text scanner

        public string Peek()
        {
            return _boundScanner.PeekQuery(EndQuery());
        }

        public IList<string> PeekPieces()
        {
            return _boundScanner.PeekQueryPieces(EndQuery());
        }

        public string Read()
        {
            return _boundScanner.ReadQuery(EndQuery());
        }

        public IList<string> ReadPieces()
        {
            return _boundScanner.ReadQueryPieces(EndQuery());
        }
    }
}
