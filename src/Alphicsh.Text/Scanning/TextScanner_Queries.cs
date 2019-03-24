using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public partial class TextScanner
    {
        private readonly Func<char, bool> _newlinePredicate;
        private readonly Func<char, bool> _noNewlinePredicate;

        public string PeekQuery(TextScannerQuery query)
        {
            var queryPieces = PeekQueryPieces(query);
            return queryPieces != null ? string.Join("", queryPieces) : null;
        }

        public IList<string> PeekQueryPieces(TextScannerQuery query)
        {
            return ProcessQuery(query, false);
        }

        public string ReadQuery(TextScannerQuery query)
        {
            var queryPieces = ReadQueryPieces(query);
            return string.Join("", queryPieces);
        }

        public IList<string> ReadQueryPieces(TextScannerQuery query)
        {
            return ProcessQuery(query, true);
        }

        private IList<string> ProcessQuery(TextScannerQuery query, bool advance)
        {
            int initialIndex = _currentIndex;
            var result = new List<string>();
            foreach (var entry in query.Entries)
            {
                var item = ProcessQueryEntry(entry);
                if (item == null)
                {
                    switch (entry.ResultCheckMode)
                    {
                        case ResultCheckMode.Try:
                            continue;
                        case ResultCheckMode.Assume:
                            _currentIndex = initialIndex;
                            return null;
                        case ResultCheckMode.Expect:
                            throw new Exception("Unexpected query result.");
                    }
                }

                if (!entry.IsSkipped)
                    result.Add(item);
            }

            // advancement, if applicable
            if (advance)
            {
                for (var i = initialIndex; i < _currentIndex; i++)
                {
                    _positionHandler.AdvanceByCharacter(_sourceString[i]);
                }
            }
            else
            {
                _currentIndex = initialIndex;
            }
            UpdateCurrentCharacter();
            return result;
        }

        private string ProcessQueryEntry(TextScannerQueryEntry entry)
        {
            var maxLength = Math.Min(_sourceString.Length - _currentIndex, entry.Length != 0 ? entry.Length : int.MaxValue);
            string result = null;
            var startPosition = _currentIndex;
            if (entry.TakeStrings != null)
            {
                foreach (var str in entry.TakeStrings)
                {
                    if (str.Length > maxLength)
                        continue;

                    if (_sourceString.Substring(_currentIndex, str.Length) == str)
                    {
                        result = str;
                        _currentIndex += str.Length;
                        break;
                    }
                }
            }
            else
            {
                var endPosition = Math.Min(startPosition + maxLength, _sourceString.Length);
                for (var i = startPosition; i < endPosition; i++)
                {
                    char checkedCharacter = _sourceString[i];
                    if (entry.TakePredicate != null && entry.TakePredicate(checkedCharacter))
                    {
                        endPosition = i;
                        break;
                    }

                    if (entry.LineReadingHandling.HasValue && _newlinePredicate(checkedCharacter))
                    {
                        endPosition = i;
                        break;
                    }
                }

                _currentIndex = endPosition;
                if (entry.LineReadingHandling.HasValue && entry.LineReadingHandling != LineReadingHandling.ReturnAndAdvanceBefore && _newlinePredicate(_sourceString[_currentIndex]))
                {
                    _currentIndex++;
                    if (_positionHandler.LineBreakConvention == LineBreakConvention.BothWithCrlf && _sourceString[_currentIndex - 1] == '\r' && _sourceString[_currentIndex] == '\n')
                        _currentIndex++;

                    if (entry.LineReadingHandling == LineReadingHandling.ReturnAndAdvanceAfter)
                        endPosition = _currentIndex;
                }
                result = _sourceString.Substring(startPosition, endPosition - startPosition);
            }

            if (result == null || !entry.ResultCheckPredicate(result))
            {
                _currentIndex = startPosition;
                return null;
            }

            return result;
        }

    }
}
