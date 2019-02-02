using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    public interface ITextPositionHandler
    {
        /// <summary>
        /// The underlying text position.
        /// </summary>
        TextPosition TextPosition { get; set; }

        /// <summary>
        /// Advance the text position by a specific character, using the handler's rules.
        /// </summary>
        /// <param name="character">The character to advance the position by.</param>
        /// <returns>The number of columns the character occupies (0 for newlines).</returns>
        int AdvanceByCharacter(char character);
    }
}
