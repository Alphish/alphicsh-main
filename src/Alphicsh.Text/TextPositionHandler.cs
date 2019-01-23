using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    public sealed class TextPositionHandler : ITextPositionHandler
    {
        // --------
        // Creation
        // --------

        /// <summary>
        /// Creates a new text position handler for a specified text position tracker, or creates a new one if none is passed.
        /// Also, additional settings can be specified.
        /// </summary>
        /// <param name="textPosition">The text position to handle.</param>
        /// <param name="tabColumnCount">The maximum number of columns added for a tab character.</param>
        /// <param name="joinCrlf">Whether a CRLF sequence should advance the position by a single line (join) or two (separate).</param>
        public static ITextPositionHandler Create(
            TextPosition textPosition = null,
            long tabColumnCount = 4,
            bool joinCrlf = true
            )
        {
            return new TextPositionHandler(textPosition, tabColumnCount, joinCrlf);
        }

        // ----------
        // Public API
        // ----------

        public TextPosition TextPosition => _textPosition;

        public long AdvanceByCharacter(char character) => DoAdvanceByCharacter(character);

        // -------------
        // Configuration
        // -------------

        private readonly TextPosition _textPosition;
        private readonly long _tabColumnCount;
        private readonly bool _joinCrlf;

        // --------------
        // Internal state
        // --------------

        private bool _hasUnmatchedCarriageReturn;

        // --------------
        // Internal logic
        // --------------

        // Constructor

        private TextPositionHandler(
            TextPosition textPosition,
            long tabColumnCount,
            bool joinCrlf
            )
        {
            _textPosition = textPosition ?? new TextPosition();
            _tabColumnCount = tabColumnCount;
            _joinCrlf = joinCrlf;
        }

        // Methods

        private long DoAdvanceByCharacter(char character)
        {
            _textPosition.Character++;

            // if the character is a NL matching CR, don't change anything else
            var matchCarriageReturn = _hasUnmatchedCarriageReturn && character == '\n';
            _hasUnmatchedCarriageReturn = false;
            if (matchCarriageReturn)
                return 0;

            _textPosition.LineCharacter++;

            long columnCount = 0;

            if (character == '\r')
                columnCount = HandleCarriageReturn();
            else if (character == '\n')
                columnCount = HandleNewLine();
            else if (character == '\t')
                columnCount = HandleTab();
            else
                columnCount = HandleRegularCharacter();

            _textPosition.Column += columnCount;

            return columnCount;
        }

        private long HandleCarriageReturn()
        {
            BeginLine();
            _hasUnmatchedCarriageReturn = _joinCrlf;
            return 0;
        }

        private long HandleNewLine()
        {
            BeginLine();
            return 0;
        }

        private long HandleTab()
        {
            return _tabColumnCount - (_textPosition.Column - 1) % _tabColumnCount;
        }

        private long HandleRegularCharacter()
        {
            return 1;
        }

        private void BeginLine()
        {
            _textPosition.Line++;
            _textPosition.LineCharacter = 1; 
            _textPosition.Column = 1;
        }
    }
}
