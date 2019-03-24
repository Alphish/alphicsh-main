using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    public enum LineBreakConvention
    {
        /// <summary>
        /// Default method, which corresponds to <see cref="BothWithCrlf"/> method.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Treats LINE FEED character, CARRIAGE RETURN character and CRLF sequence as a single line break.
        /// </summary>
        BothWithCrlf = 0,
        /// <summary>
        /// Treats LINE FEED character and CARRIAGE RETURN character as a single line break and CRLF sequence as two separate line breaks.
        /// </summary>
        BothWithoutCrlf = 1,
        /// <summary>
        /// Treats only the LINE FEED character as a line break.
        /// </summary>
        LineFeedOnly = 2,
        /// <summary>
        /// Treats only the CARRIAGE RETURN character as a line break.
        /// </summary>
        CarriageReturnOnly = 3,
    }
}
