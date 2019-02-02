using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    public interface ITextScanner : IDisposable
    {
        char CurrentCharacter { get; }

        bool IsEndOfText { get; }

        TextPosition Position { get; }

        char Read();

        char Buffer();

        void Backtrack();

        string Flush();
    }
}
