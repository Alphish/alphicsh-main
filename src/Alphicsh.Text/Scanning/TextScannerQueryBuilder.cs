using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphicsh.Text.Scanning
{
    public class TextScannerQueryBuilder : ITextScannerQueryBuilder
    {
        public static ITextScannerQueryBuilder BeginQuery()
        {
            return new TextScannerQueryBuilder();
        }

        private List<TextScannerQueryEntry> _entries;

        private bool _isNewEntry;
        private int _entryLength;
        private LineReadingHandling? _entryLineReadingHandling;

        private Func<char, bool> _entryTakePredicate;
        private IEnumerable<string> _entryTakeStrings;

        private Func<string, bool> _entryResultCheckPredicate;
        private ResultCheckMode? _entryResultCheckMode;

        private protected TextScannerQueryBuilder()
        {
            _entries = new List<TextScannerQueryEntry>();
            BeginAnotherEntry();
        }

        public ITextScannerQueryBuilder Take(int length)
        {
            EnsureEntryLengthIsNotOverwritten(nameof(Take));
            _entryLength = length;
            _isNewEntry = false;
            return this;
        }

        public ITextScannerQueryBuilder TakeRest()
        {
            EnsureEntryLengthIsNotOverwritten(nameof(TakeRest));
            _entryLength = int.MaxValue;
            _isNewEntry = false;
            return this;
        }

        public ITextScannerQueryBuilder TakeLine(LineReadingHandling lineReadingHandling)
        {
            EnsureEntryConfigurationIsNotOverwritten(_entryLineReadingHandling, nameof(TakeLine));
            _entryLineReadingHandling = lineReadingHandling;
            _isNewEntry = false;
            return this;
        }

        public ITextScannerQueryBuilder TakeWhile(Func<char, bool> predicate)
        {
            EnsureEntryConfigurationIsNotOverwritten(_entryTakePredicate, nameof(TakeWhile));
            _entryTakePredicate = predicate;
            _isNewEntry = false;
            return this;
        }

        public ITextScannerQueryBuilder TakeChartable(Chartable chartable)
        {
            EnsureEntryConfigurationIsNotOverwritten(_entryTakePredicate, nameof(TakeChartable));
            _entryTakePredicate = chartable.Matches;
            _isNewEntry = false;
            return this;
        }

        public ITextScannerQueryBuilder TakeString(IEnumerable<string> strings)
        {
            EnsureEntryConfigurationIsNotOverwritten(_entryTakeStrings, nameof(TakeString));
            _entryTakeStrings = strings;
            _isNewEntry = false;
            return this;
        }

        public ITextScannerQueryBuilder CheckAny(ResultCheckMode mode)
        {
            return CheckResult(s => s != string.Empty, mode);
        }

        public ITextScannerQueryBuilder CheckString(IEnumerable<string> strings, ResultCheckMode mode)
        {
            var set = new HashSet<string>(strings);
            return CheckResult(set.Contains, mode);
        }

        public ITextScannerQueryBuilder CheckResult(Func<string, bool> predicate, ResultCheckMode mode)
        {
            EnsureEntryConfigurationIsNotOverwritten(_entryResultCheckPredicate, nameof(CheckResult));
            _entryResultCheckPredicate = predicate;
            _entryResultCheckMode = mode;
            _isNewEntry = false;
            return this;
        }

        public ITextScannerQueryBuilder CheckChartable(Chartable chartable, ResultCheckMode mode)
        {
            return CheckResult(str => str.ToCharArray().All(c => chartable[c]), mode);
        }

        public ITextScannerQueryBuilder SkipThen()
        {
            WrapEntry(true);
            BeginAnotherEntry();
            return this;
        }

        public ITextScannerQueryBuilder AppendThen()
        {
            WrapEntry(false);
            BeginAnotherEntry();
            return this;
        }

        public TextScannerQuery EndQuery()
        {
            if (!_isNewEntry)
                WrapEntry(false);

            return new TextScannerQuery(_entries);
        }

        private void WrapEntry(bool isSkipped)
        {
            EnsureEntryHasConfiguration();    

            _entries.Add(new TextScannerQueryEntry(
                _entryLength, _entryLineReadingHandling,
                _entryTakePredicate, _entryTakeStrings,
                _entryResultCheckPredicate, _entryResultCheckMode,
                isSkipped
                ));
        }

        private void BeginAnotherEntry()
        {
            _isNewEntry = true;
            _entryLength = default;
            _entryLineReadingHandling = default;
            _entryTakePredicate = default;
            _entryTakeStrings = default;
            _entryResultCheckPredicate = default;
            _entryResultCheckMode = default;
        }

        // Guards

        private void EnsureEntryLengthIsNotOverwritten(string currentMethod)
        {
            if (_entryLength == default)
                return;

            string appliedMethod = _entryLength < int.MaxValue ? nameof(Take) : nameof(TakeRest);
            if (appliedMethod == currentMethod)
                throw new InvalidOperationException($"The method '{appliedMethod}' has been already applied to this query entry.");
            else
                throw new InvalidOperationException($"The method '{currentMethod}' would overwrite the current query entry configuration set with '{appliedMethod}'.");
        }

        private void EnsureEntryConfigurationIsNotOverwritten<TConfiguration>(TConfiguration configuration, string methodName)
        {
            if (configuration != default)
                throw new InvalidOperationException($"The method '{methodName}' has been already applied to this query entry.");
        }

        private void EnsureEntryHasConfiguration()
        {
            if (_isNewEntry)
                throw new InvalidOperationException("The current query entry has no configuration applied.");
        }
    }
}
