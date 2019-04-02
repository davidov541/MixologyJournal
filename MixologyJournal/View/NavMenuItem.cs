using System;
using Windows.UI.Xaml.Controls;

namespace MixologyJournal.View
{
    public class NavMenuItem
    {
        private Action _executeAction;

        private Symbol _symbol;
        public Symbol Symbol
        {
            get
            {
                return _symbol;
            }
        }

        private String _label;
        public String Label
        {
            get
            {
                return _label;
            }
        }

        public NavMenuItem(String label, Symbol symbol, Action executeAction)
        {
            _label = label;
            _symbol = symbol;
            _executeAction = executeAction;
        }

        public void Execute()
        {
            _executeAction?.Invoke();
        }
    }
}
