using System;
using System.Collections.Generic;
using System.Text;

namespace Alphicsh.Text
{
    public partial class TextScanner
    {
        private class BufferState
        {
            private StringBuilder _bufferBuilder = new StringBuilder();
            private int _position = 0;

            public TextPosition StartPosition { get; private set; }

            public void Buffer(char c)
            {
                _bufferBuilder.Append(c);
                _position++;
            }

            public bool TryAdvance(out char c)
            {
                if (_position == _bufferBuilder.Length)
                {
                    c = '\0';
                    return false;
                }
                else
                {
                    c = _bufferBuilder[_position++];
                    return true;
                }
            }

            public void Backtrack()
            {
                _position = 0;
            }

            public string Flush(TextPosition newStartPosition)
            {
                if (_position == 0)
                    return string.Empty;

                StartPosition = newStartPosition;

                var result = _bufferBuilder.ToString(0, _position);
                _bufferBuilder.Remove(0, _position);
                _position = 0;
                return result;
            }
        }
    }
}
