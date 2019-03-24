using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public enum ResultCheckMode
    {
        /// <summary>
        /// If the result does not match the conditions, skip it.
        /// </summary>
        Try = 0,
        /// <summary>
        /// If the result does not match the conditions, interrupt the whole query and return null.
        /// </summary>
        Assume = 1,
        /// <summary>
        /// If the result does not match the conditions, report a failure.
        /// </summary>
        Expect = 2,
    }
}
