using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    public sealed class TextPosition
    {
        /// <summary>
        /// Creates a default text position, with all values set to 1.
        /// </summary>
        public TextPosition()
            : this(index: 1, line: 1, lineIndex: 1, column: 1, lastCharacter: '\0')
        {
        }

        /// <summary>
        /// Creates a text position with specific initial settings.
        /// </summary>
        /// <param name="index">The one-based index of the full text character.</param>
        /// <param name="line">The one-based index of the full text line.</param>
        /// <param name="lineIndex">The one-based index of the character in the current line.</param>
        /// <param name="column">The one-based index of the column displayed in the current line.</param>
        /// <param name="followsCarriageReturn">The last character of the text position.</param>
        public TextPosition(int index = 1, int line = 1, int lineIndex = 1, int column = 1, char lastCharacter = '\0')
        {
            Index = index;
            Line = line;
            LineIndex = lineIndex;
            Column = column;
            LastCharacter = lastCharacter;
        }

        /// <summary>
        /// Gets or sets the one-based index of the full text character.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets of sets the one-based index of the full text line.
        /// </summary>
        public int Line { get; internal set; }

        /// <summary>
        /// Gets or sets the one-based index of the character in the current line.
        /// </summary>
        public int LineIndex { get; internal set; }

        /// <summary>
        /// Gets or sets the one-based index of the column displayed in the current line.
        /// While most characters advance the columns index by 1, some others, such as tabs, might use a different value.
        /// </summary>
        public int Column { get; internal set; }

        /// <summary>
        /// Gets the character the current position is directly following.
        /// At the very beginning it's a string terminator character ('\0').
        /// </summary>
        public char LastCharacter { get; internal set; }

        /// <summary>
        /// Creates a copy of the text position.
        /// </summary>
        /// <returns>The created copy.</returns>
        public TextPosition Clone()
        {
            return new TextPosition(Index, Line, LineIndex, Column, LastCharacter);
        }

        /// <summary>
        /// Indicates whether the text position corresponds to the very beginning.
        /// </summary>
        public bool IsBeginning
            => Index == 1 && Line == 1 && LineIndex == 1 && Column == 1 && LastCharacter == '\0';
    }
}
