using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZAutosplitter.triggers
{
    class SplitOnGettingFairiesOfClass : SplitRuleTrigger<SplitRuleGettingFairiesOfClass>
    {
        public SplitOnGettingFairiesOfClass(GameState state, SplitRuleGettingFairiesOfClass rule) : base(state, rule)
        {}

        protected override bool ShouldSplit() => State.Inventory.Items
            .Where(item => item.cardId.type == CardType.Fairy)
            .Select(item => State.Database.FairyElements[item.cardId])
            .Count(type => type == Rule.Type) >= Rule.Amount;
    }
}

 
