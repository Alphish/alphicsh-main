using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public enum LineReadingHandling
    {
        /// <summary>
        /// The default handling, which corresponds to <see cref="ReturnBeforeAdvanceAfter"/> handling.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Doesn't include the newline characters in the result, advances past the newline characters.
        /// </summary>
        ReturnBeforeAdvanceAfter = 0,
        /// <summary>
        /// Doesn't include the newline characters in the result, advances before the newline characters.
        /// </summary>
        ReturnAndAdvanceBefore = 1,
        /// <summary>
        /// Includes the newline characters in the result, advances after the newline characters.
        /// </summary>
        ReturnAndAdvanceAfter = 2,
    }
}
