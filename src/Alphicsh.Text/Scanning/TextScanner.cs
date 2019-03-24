using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public sealed partial class TextScanner : ITextScanner
    {
        // --------
        // Creation
        // --------

        public static ITextScanner CreateFromString(string str, ITextPositionHandler positionHandler = null)
        {
            return new TextScanner(str, positionHandler ?? TextPositionHandler.Create());
        }
        
        public static ITextScanner CreateFromTextReader(TextReader reader, ITextPositionHandler positionHandler = null)
        {
            return new TextScanner(reader.ReadToEnd(), positionHandler ?? TextPositionHandler.Create());
        }

        // ----------
        // Public API
        // ----------

        public char CurrentCharacter => _currentCharacter;
        public int CurrentIndex => _currentIndex;
        public int Length => _sourceString.Length;
        public bool IsEndOfText => _currentIndex == _sourceString.Length;
        public TextPosition Position => _positionHandler.TextPosition;

        public char Peek() => _currentCharacter;
        public char Read()
        {
            var c = _currentCharacter;
            if (!IsEndOfText)
            {
                _currentIndex++;
                _positionHandler.AdvanceByCharacter(c);
                UpdateCurrentCharacter();
            }
            return c;
        }

        public ITextScannerBoundQueryBuilder BeginQuery() => TextScannerBoundQueryBuilder.BeginBoundQuery(this);

        // --------------------
        // Basic internal state
        // --------------------

        private string _sourceString;
        private int _currentIndex;
        private char _currentCharacter;
        private ITextPositionHandler _positionHandler;

        // -----------
        // Constructor
        // -----------

        private TextScanner(string sourceString, ITextPositionHandler positionHandler)
        {
            _sourceString = sourceString;
            _positionHandler = positionHandler;

            var lineBreakConvention = positionHandler.LineBreakConvention;
            switch (lineBreakConvention)
            {
                case LineBreakConvention.CarriageReturnOnly:
                    _newlinePredicate = c => c == '\r';
                    _noNewlinePredicate = c => c != '\r';
                    break;
                case LineBreakConvention.LineFeedOnly:
                    _newlinePredicate = c => c == '\n';
                    _noNewlinePredicate = c => c != '\n';
                    break;
                default:
                    _newlinePredicate = c => c == '\r' || c == '\n';
                    _noNewlinePredicate = c => c != '\r' && c != '\n';
                    break;
            }

            _currentCharacter = _sourceString[_currentIndex];
            UpdateCurrentCharacter();
        }

        private void UpdateCurrentCharacter()
        {
            _currentCharacter = _currentIndex < _sourceString.Length ? _sourceString[_currentIndex] : '\0';
        }

        // --------------
        // Internal logic
        // --------------

        // the Query handling implementations are in TextScanner_Queries section
    }
}
