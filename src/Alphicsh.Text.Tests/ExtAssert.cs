using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Alphicsh.Text
{
    public static class ExtAssert
    {
        public static void AssertPosition(TextPosition position, int index, int line, int lineIndex, int column)
        {
            Assert.Equal(index, position.Index);
            Assert.Equal(line, position.Line);
            Assert.Equal(lineIndex, position.LineIndex);
            Assert.Equal(column, position.Column);
        }
    }
}
