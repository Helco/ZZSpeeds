using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZAutosplitter.triggers
{
    class SplitOnGettingTotalFairies : SplitRuleTrigger<SplitRuleGettingTotalFairies>
    {
        public SplitOnGettingTotalFairies(GameState state, SplitRuleGettingTotalFairies rule) : base(state, rule)
        {}

        protected override bool ShouldSplit() => State.Inventory.Items
            .Where(item => item.cardId.type == CardType.Fairy)
            .Count() >= Rule.Amount;
    }
}

 
