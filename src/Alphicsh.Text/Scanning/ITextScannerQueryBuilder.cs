using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public interface ITextScannerQueryBuilder
    {
        // determining the scope
        ITextScannerQueryBuilder Take(int length);
        ITextScannerQueryBuilder TakeRest();
        ITextScannerQueryBuilder TakeLine(LineReadingHandling lineReadingHandling);

        // limiting the character set
        ITextScannerQueryBuilder TakeChartable(Chartable chartable);
        ITextScannerQueryBuilder TakeWhile(Func<char, bool> predicate);

        // words handling
        ITextScannerQueryBuilder TakeString(IEnumerable<string> strings);

        // checking the subquery results
        ITextScannerQueryBuilder CheckAny(ResultCheckMode mode);
        ITextScannerQueryBuilder CheckString(IEnumerable<string> strings, ResultCheckMode mode);
        ITextScannerQueryBuilder CheckResult(Func<string, bool> predicate, ResultCheckMode mode);
        ITextScannerQueryBuilder CheckChartable(Chartable chartable, ResultCheckMode mode);

        // beginning another subquery
        ITextScannerQueryBuilder SkipThen();
        ITextScannerQueryBuilder AppendThen();

        // passing reusable query
        TextScannerQuery EndQuery();
    }
}
