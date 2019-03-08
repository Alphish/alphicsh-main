using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphicsh.Text
{
    public partial class TextScanner
    {
        private Dictionary<string, TextPosition> _savepoints = new Dictionary<string, TextPosition>();
        private int? _savepointsEarliestIndex = null;

        // Positions store handling

        private void DoSavePosition(string savepointName)
        {
            var position = _positionHandler.TextPosition.Clone();
            _savepoints[savepointName] = position;
            if (_savepointsEarliestIndex == null || position.Index < _savepointsEarliestIndex)
                _savepointsEarliestIndex = position.Index;
        }

        private void DoLoadPosition(string savepointName)
        {
            _positionHandler.TextPosition = _savepoints[savepointName].Clone();
            _readStringPosition = (_positionHandler.TextPosition.Index - 1) - _readStringOffset;
            UpdateCurrentCharacter();
        }

        private void DoForgetPosition(string savepointName)
        {
            var shouldEarliestIndexBeRecalculated = _savepointsEarliestIndex == _savepoints[savepointName].Index;
            _savepoints.Remove(savepointName);
            if (shouldEarliestIndexBeRecalculated)
                _savepointsEarliestIndex = _savepoints.Any() ? _savepoints.Values.Min(sp => sp.Index) : (int?)null;
        }
    }
}
