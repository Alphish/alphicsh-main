using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public class TextScannerQuery
    {
        public TextScannerQuery(IEnumerable<TextScannerQueryEntry> entries)
        {
            Entries = entries.ToList();
        }

        public IEnumerable<TextScannerQueryEntry> Entries { get; }
    }
}
