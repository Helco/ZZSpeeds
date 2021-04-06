using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZAutosplitter.triggers
{
    class SplitOnGettingCards : SplitRuleTrigger<SplitRuleGettingCards>
    {
        public SplitOnGettingCards(GameState state, SplitRuleGettingCards rule) : base(state, rule)
        {}

        protected override bool ShouldSplit() => State.Inventory.Items
            .Where(item => item.cardId == Rule.Card)
            .Sum(item => item.amount) >= Rule.Amount;
    }
}

 
