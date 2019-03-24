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
        /// <param name="lineBreakConvention">The used line break convention.</param>
        public static ITextPositionHandler Create(
            TextPosition textPosition = null,
            int tabColumnCount = 4,
            LineBreakConvention lineBreakConvention = default
            )
        {
            if (!Enum.IsDefined(typeof(LineBreakConvention), lineBreakConvention))
                throw new ArgumentException($"The value {lineBreakConvention} is not recognized for {nameof(LineBreakConvention)} enumeration.", nameof(lineBreakConvention));

            return new TextPositionHandler(textPosition, tabColumnCount, lineBreakConvention);
        }

        // ----------
        // Public API
        // ----------

        public LineBreakConvention LineBreakConvention => _lineBreakConvention;

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
        private readonly LineBreakConvention _lineBreakConvention;

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
            LineBreakConvention lineBreakConvention
            )
        {
            _textPosition = textPosition ?? new TextPosition();
            _tabColumnCount = tabColumnCount;
            _lineBreakConvention = lineBreakConvention;
        }

        // Methods

        private int DoAdvanceByCharacter(char character)
        {
            _textPosition.Index++;

            // if the character is a NL matching CR, don't change anything else
            var isNewLineAfterCarriageReturn = character == '\n' && _textPosition.LastCharacter == '\r';
            _textPosition.LastCharacter = character;
            if (isNewLineAfterCarriageReturn && _lineBreakConvention == LineBreakConvention.BothWithCrlf)
                return 0;

            _textPosition.LineIndex++;

            // handle next line characters
            var isNextLine = (
                (character == '\r' && _lineBreakConvention != LineBreakConvention.LineFeedOnly) ||
                (character == '\n' && _lineBreakConvention != LineBreakConvention.CarriageReturnOnly)
                );

            if (isNextLine)
                BeginLine();

            // handle other characters, including tab
            var isTab = (character == '\t');
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
