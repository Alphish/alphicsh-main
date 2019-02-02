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
            int tabColumnCount = 4,
            bool joinCrlf = true
            )
        {
            return new TextPositionHandler(textPosition, tabColumnCount, joinCrlf);
        }

        // ----------
        // Public API
        // ----------

        public TextPosition TextPosition
        {
            get => _textPosition;
            set => _textPosition = value;
        }

        public int AdvanceByCharacter(char character) => DoAdvanceByCharacter(character);

        // -------------
        // Configuration
        // -------------

        private readonly int _tabColumnCount;
        private readonly bool _joinCrlf;

        // --------------
        // Internal state
        // --------------

        private TextPosition _textPosition;

        // --------------
        // Internal logic
        // --------------

        // Constructor

        private TextPositionHandler(
            TextPosition textPosition,
            int tabColumnCount,
            bool joinCrlf
            )
        {
            _textPosition = textPosition ?? new TextPosition();
            _tabColumnCount = tabColumnCount;
            _joinCrlf = joinCrlf;
        }

        // Methods

        private int DoAdvanceByCharacter(char character)
        {
            _textPosition.Index++;

            // if the character is a NL matching CR, don't change anything else
            var isNewLineAfterCarriageReturn = character == '\n' && _textPosition.LastCharacter == '\r';
            _textPosition.LastCharacter = character;
            if (isNewLineAfterCarriageReturn && _joinCrlf)
                return 0;

            _textPosition.LineIndex++;

            var isNextLine = (character == '\r' || character == '\n');
            var isTab = (character == '\t');

            if (isNextLine)
                BeginLine();

            int addedColumnCount = isNextLine ? 0 : isTab ? GetNextTabCount() : /* is regular character */ 1;
            _textPosition.Column += addedColumnCount;
            return addedColumnCount;
        }

        private void BeginLine()
        {
            _textPosition.Line++;
            _textPosition.LineIndex = 1; 
            _textPosition.Column = 1;
        }

        private int GetNextTabCount()
        {
            return _tabColumnCount - (_textPosition.Column - 1) % _tabColumnCount;
        }
    }
}
