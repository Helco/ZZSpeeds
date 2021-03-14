using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZZAutosplitter.controls
{
    public partial class GettingCardsEditControl : SplitRuleEditControl
    {
        private readonly Database database;
        private readonly SplitRuleGettingCards rule;
        private readonly bool isInitialised;

        public GettingCardsEditControl(SplitRuleGettingCards rule, Database database)
        {
            this.rule = rule;
            this.database = database;
            InitializeComponent();
            comboCardType.DataSource = Enum.GetValues(typeof(CardType));
            comboCardIndex.DisplayMember = nameof(KeyValuePair<CardId, string>.Value);
            SetCardIndexDataSource();

            numericAmount.Value = rule.Amount;
            comboCardType.SelectedItem = rule.Card.type;
            comboCardIndex.SelectedItem = rule.Card;
            UpdateIcon();
            isInitialised = true;
        }

        private void comboCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Card = new CardId((CardType)comboCardType.SelectedItem, 0);
            SetCardIndexDataSource();
            UpdateIcon();
            InvokeRuleChanged();
        }

        private void comboCardIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Card = ((KeyValuePair<CardId, string>)comboCardIndex.SelectedItem).Key;
            UpdateIcon();
            InvokeRuleChanged();
        }

        private void numericAmount_ValueChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Amount = (int)numericAmount.Value;
            InvokeRuleChanged();
        }

        private void SetCardIndexDataSource() => comboCardIndex.DataSource = database.Cards
            .Where(c => c.Key.type == (CardType)comboCardType.SelectedItem)
            .ToArray();

        private void UpdateIcon() => pictureBox.Image = rule.GetIcon(database);
    }
}
