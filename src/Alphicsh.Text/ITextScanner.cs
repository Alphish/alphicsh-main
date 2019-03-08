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

        char Peek();
        string PeekBlock(int count);
        string PeekWhile(Func<char, bool> predicate);
        string PeekCharset(bool[] charset);

        char Read();
        string ReadBlock(int count);
        string ReadWhile(Func<char, bool> predicate);
        string ReadCharset(bool[] charset);

        void SavePosition(string savepointName);
        void LoadPosition(string savepointName);
        void ForgetPosition(string savepointName);
    }
}
