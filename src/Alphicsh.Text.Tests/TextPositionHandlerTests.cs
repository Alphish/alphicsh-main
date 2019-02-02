using System;

using Xunit;

using System.Collections.Generic;

namespace Alphicsh.Text.Tests
{
    public class TextPositionHandlerTests
    {
        // ------------------------
        // static.Create(...) tests
        // ------------------------

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(4, 3, 1, 1)]
        [InlineData(2, 1, 2, 5)]
        public void Create_SetsRightPosition(int initialCharacter, int initialLine, int initialLineCharacter, int initialColumn)
        {
            var position = new TextPosition(initialCharacter, initialLine, initialLineCharacter, initialColumn);
            var handler = TextPositionHandler.Create(textPosition: position);

            AssertPosition(handler.TextPosition, initialCharacter, initialLine, initialLineCharacter, initialColumn);
        }

        [Fact]
        public void Create_UsesRightDefaults()
        {
            var handler = TextPositionHandler.Create();
            AssertPosition(handler.TextPosition, 1, 1, 1, 1);

            AdvanceAndAssert(handler, '\t', 2, 1, 2, 5);
            AdvanceAndAssert(handler, '\t', 3, 1, 3, 9);
            AdvanceAndAssert(handler, '\r', 4, 2, 1, 1);
            AdvanceAndAssert(handler, '\n', 5, 2, 1, 1);
        }

        // --------------------------------------
        // instance.AdvanceByCharacter(...) tests
        // --------------------------------------

        [Fact]
        public void AdvanceByCharacter_AdvancesByOneCharacterWhenPrintable()
        {
            var handler = TextPositionHandler.Create();

            for (var i = 0; i < 95; i++)
            {
                AdvanceAndAssert(handler, (char)(32 + i), 2 + i, 1, 2 + i, 2 + i);
            }
        }

        [Theory]
        [InlineData(4,   1, 1, 1, 1,   2, 1, 2, 5)]
        [InlineData(4,   2, 1, 2, 2,   3, 1, 3, 5)]
        [InlineData(4,   3, 2, 3, 5,   4, 2, 4, 9)]
        [InlineData(4,   4, 2, 4, 7,   5, 2, 5, 9)]
        [InlineData(6,   1, 1, 1, 1,   2, 1, 2, 7)]
        [InlineData(6,   2, 1, 2, 2,   3, 1, 3, 7)]
        [InlineData(6,   3, 2, 3, 5,   4, 2, 4, 7)]
        [InlineData(6,   4, 2, 4, 7,   5, 2, 5, 13)]
        public void AdvanceByCharacter_UsesRightTabColumnCount(
            int tabColumnCount,
            int initialCharacter, int initialLine, int initialLineCharacter, int initialColumn,
            int nextCharacter, int nextLine, int nextLineCharacter, int nextColumn
            )
        {
            var position = new TextPosition(initialCharacter, initialLine, initialLineCharacter, initialColumn);
            var handler = TextPositionHandler.Create(textPosition: position, tabColumnCount: tabColumnCount);

            AdvanceAndAssert(handler, '\t', nextCharacter, nextLine, nextLineCharacter, nextColumn);
        }

        [Fact]
        public void AdvanceByCharacter_JoinsCrlfWhenJoinCrlfSet()
        {
            var handler = TextPositionHandler.Create(joinCrlf: true);

            AdvanceAndAssert(handler, '\r', 2, 2, 1, 1);
            AdvanceAndAssert(handler, '\r', 3, 3, 1, 1);
            AdvanceAndAssert(handler, '\n', 4, 3, 1, 1);
            AdvanceAndAssert(handler, '\n', 5, 4, 1, 1);
        }

        [Fact]
        public void AdvanceByCharacter_SeparatesCrlfWhenJoinCrlfNotSet()
        {
            var handler = TextPositionHandler.Create(joinCrlf: false);

            AdvanceAndAssert(handler, '\r', 2, 2, 1, 1);
            AdvanceAndAssert(handler, '\r', 3, 3, 1, 1);
            AdvanceAndAssert(handler, '\n', 4, 4, 1, 1);
            AdvanceAndAssert(handler, '\n', 5, 5, 1, 1);
        }

        // ----------------------
        // Helper private methods
        // ----------------------

        private void AdvanceAndAssert(ITextPositionHandler handler, char character, int index, int line, int lineIndex, int column)
        {
            handler.AdvanceByCharacter(character);
            AssertPosition(handler.TextPosition, index, line, lineIndex, column);
        }

        private void AssertPosition(TextPosition position, int index, int line, int lineIndex, int column)
        {
            Assert.Equal(index, position.Index);
            Assert.Equal(line, position.Line);
            Assert.Equal(lineIndex, position.LineIndex);
            Assert.Equal(column, position.Column);
        }
    }
}
