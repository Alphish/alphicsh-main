using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    public sealed partial class TextScanner : ITextScanner
    {
        // --------
        // Creation
        // --------

        public static TextScanner CreateFromString(string str, ITextPositionHandler positionHandler = null)
        {
            return CreateFromTextReader(new StringReader(str), positionHandler);
        }
        
        public static TextScanner CreateFromStream(Stream stream, ITextPositionHandler positionHandler = null)
        {
            return CreateFromTextReader(new StreamReader(stream), positionHandler);
        }

        public static TextScanner CreateFromTextReader(TextReader reader, ITextPositionHandler positionHandler = null)
        {
            return new TextScanner(reader, positionHandler ?? TextPositionHandler.Create());
        }

        // ----------
        // Public API
        // ----------

        public char CurrentCharacter => _currentCharacter;

        public bool IsEndOfText => _currentCharacter == '\0';

        public TextPosition Position => _positionHandler.TextPosition;

        public char Read() => DoRead();

        public char Buffer() => DoBuffer();

        public void Backtrack() => DoBacktrack();

        public string Flush() => DoFlush();

        // --------------
        // Internal state
        // --------------

        private TextReader _textReader;
        private char _currentCharacter;
        private ITextPositionHandler _positionHandler;
        private BufferState _bufferState;

        // --------------
        // Internal logic
        // --------------

        // Constructor

        private TextScanner(TextReader textReader, ITextPositionHandler positionHandler)
        {
            _textReader = textReader;
            _positionHandler = positionHandler;

            _currentCharacter = '\0';
            _bufferState = new BufferState();

            ReadCurrentCharacterAndAdvance();
        }

        // Methods

        private char DoRead()
        {
            var result = DoBuffer();
            DoFlush();
            return result;
        }

        private char DoBuffer()
        {
            if (IsEndOfText)
                return _currentCharacter;

            var result = ReadCurrentCharacterAndAdvance();
            _bufferState.Buffer(result);
            _positionHandler.AdvanceByCharacter(result);
            return result;
        }

        private void DoBacktrack()
        {
            _bufferState.Backtrack();
            _positionHandler.TextPosition = _bufferState.StartPosition;
            ReadCurrentCharacterAndAdvance();
        }

        private string DoFlush()
        {
            return _bufferState.Flush(_positionHandler.TextPosition);
        }

        private char ReadCurrentCharacterAndAdvance()
        {
            var result = _currentCharacter;
            if (_bufferState.TryAdvance(out char c))
            {
                _currentCharacter = c;
            }
            else
            {
                int readCharacter = _textReader.Read();
                _currentCharacter = readCharacter >= 0 ? (char)readCharacter : '\0';
            }
            return result;
        }

        // --------------
        // Waste disposal
        // --------------

        private bool _isDisposeHandler = false;

        void Dispose(bool isDisposing)
        {
            if (_isDisposeHandler)
                return;

            if (isDisposing)
                _textReader.Dispose();

            _isDisposeHandler = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
