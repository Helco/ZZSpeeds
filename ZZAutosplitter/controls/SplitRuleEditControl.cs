using System;
using System.Windows.Forms;

namespace ZZAutosplitter.controls
{
    public class SplitRuleEditControl : UserControl
    {
        public event Action OnRuleChanged;

        protected void InvokeRuleChanged() => OnRuleChanged?.Invoke();
    }
}
