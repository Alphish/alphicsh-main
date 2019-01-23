using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    public class TextPosition
    {
        /// <summary>
        /// Creates a default text position, with all values set to 1.
        /// </summary>
        public TextPosition()
            : this(character: 1, line: 1, lineCharacter: 1, column: 1)
        {
        }

        /// <summary>
        /// Creates a text position with specific initial settings.
        /// </summary>
        /// <param name="character">The one-based index of the full text character.</param>
        /// <param name="line">The one-based index of the full text line.</param>
        /// <param name="lineCharacter">The one-based index of the character in the current line.</param>
        /// <param name="column">The one-based index of the column displayed in the current line.</param>
        public TextPosition(long character = 1, long line = 1, long lineCharacter = 1, long column = 1)
        {
            Character = character;
            Line = line;
            LineCharacter = lineCharacter;
            Column = column;
        }

        /// <summary>
        /// Gets or sets the one-based index of the full text character.
        /// </summary>
        public long Character { get; set; }

        /// <summary>
        /// Gets of sets the one-based index of the full text line.
        /// </summary>
        public long Line { get; set; }

        /// <summary>
        /// Gets or sets the one-based index of the character in the current line.
        /// </summary>
        public long LineCharacter { get; set; }

        /// <summary>
        /// Gets or sets the one-based index of the column displayed in the current line.
        /// While most characters advance the columns index by 1, some others, such as tabs, might use a different value.
        /// </summary>
        public long Column { get; set; }
    }
}
