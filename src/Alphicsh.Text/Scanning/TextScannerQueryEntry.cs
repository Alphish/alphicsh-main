using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public class TextScannerQueryEntry
    {
        public TextScannerQueryEntry(
            int length, LineReadingHandling? lineReadingHandling,
            Func<char, bool> takePredicate, IEnumerable<string> takeStrings,
            Func<string, bool> resultCheckPredicate, ResultCheckMode? resultCheckMode,
            bool isSkipped
            )
        {
            Length = length;
            LineReadingHandling = lineReadingHandling;

            TakePredicate = takePredicate;
            TakeStrings = takeStrings;

            ResultCheckPredicate = resultCheckPredicate;
            ResultCheckMode = resultCheckMode;

            IsSkipped = isSkipped;
        }

        public int Length { get; }
        public LineReadingHandling? LineReadingHandling { get; }
        public Func<char, bool> TakePredicate { get; }
        public IEnumerable<string> TakeStrings { get; }

        public Func<string, bool> ResultCheckPredicate { get; }
        public ResultCheckMode? ResultCheckMode { get; }

        public bool IsSkipped { get; }
    }
}
