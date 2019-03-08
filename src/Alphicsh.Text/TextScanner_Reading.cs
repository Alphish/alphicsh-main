using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    public partial class TextScanner
    {
        private const int BUFFER_SIZE = 10240;

        private bool _isEndOfReaderReached;

        private StringBuilder _readString = new StringBuilder();
        private int _readStringOffset = 0;
        private int _readStringPosition = 0;

        // Peek methods

        private string DoPeekBlock(int count)
        {
            var countRead = TryBufferTillOffset(_readStringPosition + count - 1) ? count : _readString.Length - _readStringPosition;
            return _readString.ToString(_readStringPosition, countRead);
        }

        private string DoPeekWhile(Func<char, bool> predicate)
        {
            var endOffset = _readStringPosition;
            while (true)
            {
                if (!TryBufferTillOffset(endOffset))
                {
                    endOffset = _readString.Length;
                    break;
                }

                while (endOffset < _readString.Length)
                {
                    if (predicate(_readString[endOffset]))
                        endOffset++;
                    else
                        break;
                }

                if (endOffset < _readString.Length)
                    break;
            }
            return _readString.ToString(_readStringPosition, endOffset - _readStringPosition);
        }

        private string DoPeekCharset(bool[] charset)
        {
            var endOffset = _readStringPosition;
            while (true)
            {
                if (!TryBufferTillOffset(endOffset))
                {
                    endOffset = _readString.Length;
                    break;
                }

                while (endOffset < _readString.Length)
                {
                    var character = _readString[endOffset];
                    if (character < charset.Length && charset[_readString[endOffset]])
                        endOffset++;
                    else
                        break;
                }

                if (endOffset < _readString.Length)
                    break;
            }
            return _readString.ToString(_readStringPosition, endOffset - _readStringPosition);
        }

        // Read methods

        private char DoRead()
        {
            if (IsEndOfText)
                return _currentCharacter;

            var result = _currentCharacter;
            _positionHandler.AdvanceByCharacter(_currentCharacter);
            _readStringPosition++;
            UpdateCurrentCharacter();
            return result;
        }

        private string DoReadBlock(int count) => AdvanceScannerByString(DoPeekBlock(count));

        private string DoReadWhile(Func<char, bool> predicate) => AdvanceScannerByString(DoPeekWhile(predicate));

        private string DoReadCharset(bool[] charset) => AdvanceScannerByString(DoPeekCharset(charset));

        // Utility methods

        private void UpdateCurrentCharacter()
        {
            _currentCharacter = TryBufferTillOffset(_readStringPosition) ? _readString[_readStringPosition] : '\0';
        }

        private bool TryBufferTillOffset(int targetOffset)
        {
            while (targetOffset >= _readString.Length)
            {
                if (_isEndOfReaderReached)
                    return false;

                BufferNextBatch();
            }

            return true;
        }

        private void BufferNextBatch()
        {
            var targetPosition = (_savepointsEarliestIndex ?? _positionHandler.TextPosition.Index) - 1;
            if (_readStringOffset < targetPosition)
            {
                var currentDiscardedCount = targetPosition - _readStringOffset;
                _readString.Remove(0, currentDiscardedCount);
                _readStringOffset += targetPosition;
                _readStringPosition -= currentDiscardedCount;
            }

            var buffer = new char[BUFFER_SIZE];
            var readCharactersCount = _textReader.Read(buffer, 0, BUFFER_SIZE);
            _readString.Append(buffer, 0, readCharactersCount);
            _isEndOfReaderReached = readCharactersCount == 0;
        }

        private string AdvanceScannerByString(string readString)
        {
            foreach (char c in readString)
                _positionHandler.AdvanceByCharacter(c);

            UpdateCurrentCharacter();
            return readString;
        }
    }
}
