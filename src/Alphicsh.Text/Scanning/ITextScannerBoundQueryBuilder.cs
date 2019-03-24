using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public interface ITextScannerBoundQueryBuilder : ITextScannerQueryBuilder
    {
        // determining the scope
        new ITextScannerBoundQueryBuilder Take(int length);
        new ITextScannerBoundQueryBuilder TakeRest();
        new ITextScannerBoundQueryBuilder TakeLine(LineReadingHandling lineReadingHandling);

        // limiting the character set
        new ITextScannerBoundQueryBuilder TakeChartable(Chartable chartable);
        new ITextScannerBoundQueryBuilder TakeWhile(Func<char, bool> predicate);
        new ITextScannerBoundQueryBuilder TakeString(IEnumerable<string> strings);

        // checking the subquery results
        new ITextScannerBoundQueryBuilder CheckAny(ResultCheckMode mode);
        new ITextScannerBoundQueryBuilder CheckString(IEnumerable<string> strings, ResultCheckMode mode);
        new ITextScannerBoundQueryBuilder CheckResult(Func<string, bool> predicate, ResultCheckMode mode);
        new ITextScannerBoundQueryBuilder CheckChartable(Chartable chartable, ResultCheckMode mode);

        // beginning another subquery
        new ITextScannerBoundQueryBuilder SkipThen();
        new ITextScannerBoundQueryBuilder AppendThen();

        // finishing
        string Peek();
        IList<string> PeekPieces();
        string Read();
        IList<string> ReadPieces();
    }
}
