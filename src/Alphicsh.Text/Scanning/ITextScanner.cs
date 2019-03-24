using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public interface ITextScanner
    {
        char CurrentCharacter { get; }
        int CurrentIndex { get; }
        int Length { get; }
        bool IsEndOfText { get; }
        TextPosition Position { get; }

        char Peek();
        char Read();

        ITextScannerBoundQueryBuilder BeginQuery();

        string PeekQuery(TextScannerQuery query);
        IList<string> PeekQueryPieces(TextScannerQuery query);
        string ReadQuery(TextScannerQuery query);
        IList<string> ReadQueryPieces(TextScannerQuery query);
    }
}
