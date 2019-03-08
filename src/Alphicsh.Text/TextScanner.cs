using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    // yes, it is a partial class
    // yes, with all the files combined, this class would be pretty big
    // however, the class still has a single primary concern - reading text from a sequence and being awesome in that area
    public sealed partial class TextScanner : ITextScanner, IDisposable
    {
        // --------
        // Creation
        // --------

        public static ITextScanner CreateFromString(string str, ITextPositionHandler positionHandler = null)
        {
            return CreateFromTextReader(new StringReader(str), positionHandler);
        }
        
        public static ITextScanner CreateFromStream(Stream stream, ITextPositionHandler positionHandler = null)
        {
            return CreateFromTextReader(new StreamReader(stream), positionHandler);
        }

        public static ITextScanner CreateFromTextReader(TextReader reader, ITextPositionHandler positionHandler = null)
        {
            return new TextScanner(reader, positionHandler ?? TextPositionHandler.Create());
        }

        // ----------
        // Public API
        // ----------

        public char CurrentCharacter => _currentCharacter;
        public bool IsEndOfText => _currentCharacter == '\0';
        public TextPosition Position => _positionHandler.TextPosition;

        public char Peek() => _currentCharacter;    // Peek and CurrentCharacter are pretty much equivalent
        public string PeekBlock(int count) => DoPeekBlock(count);
        public string PeekWhile(Func<char, bool> predicate) => DoPeekWhile(predicate);
        public string PeekCharset(bool[] charset) => DoPeekCharset(charset);

        public char Read() => DoRead();
        public string ReadBlock(int count) => DoReadBlock(count);
        public string ReadWhile(Func<char, bool> predicate) => DoReadWhile(predicate);
        public string ReadCharset(bool[] charset) => DoReadCharset(charset);

        public void SavePosition(string savepointName) => DoSavePosition(savepointName);
        public void LoadPosition(string savepointName) => DoLoadPosition(savepointName);
        public void ForgetPosition(string savepointName) => DoForgetPosition(savepointName);

        // --------------------
        // Basic internal state
        // --------------------

        private TextReader _textReader;
        private ITextPositionHandler _positionHandler;

        private char _currentCharacter;

        // -----------
        // Constructor
        // -----------

        private TextScanner(TextReader textReader, ITextPositionHandler positionHandler)
        {
            _textReader = textReader;
            _positionHandler = positionHandler;
            UpdateCurrentCharacter();
        }

        // --------------
        // Internal logic
        // --------------

        // the DoPeek*/DoRead* implementations are in TextScanner_Reading section
        // the Save/Load/ForgetPosition implementations are in TextScanner_Positions section

        // --------------
        // Waste disposal
        // --------------

        private bool _isDisposed = false;

        void Dispose(bool isDisposing)
        {
            if (_isDisposed)
                return;

            if (isDisposing)
                _textReader.Dispose();

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
