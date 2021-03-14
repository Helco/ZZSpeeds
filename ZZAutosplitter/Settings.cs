using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ZZAutosplitter
{
    public class Settings
    {
        private static XmlSerializer CreateSerializer() => new XmlSerializer(typeof(Settings));
        public static Settings FromXmlNode(XmlNode xmlNode) =>
            (Settings)CreateSerializer().Deserialize(new XmlNodeReader(xmlNode));

        public XmlNode ToXmlNode(XmlDocument document)
        {
            var tmpDocument = new XmlDocument();
            var navigator = tmpDocument.CreateNavigator();
            var writer = navigator.AppendChild();
            writer.WriteStartDocument();
            CreateSerializer().Serialize(writer, this);
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
            return tmpDocument.DocumentElement;
        }

        [DefaultValue(true)]
        public bool EnableAutoSplits { get; set; } = true;
        
        [DefaultValue(true)]
        public bool EnableAutoStart { get; set; } = true;
        
        [DefaultValue(true)]
        public bool EnableLoadTimeRemoval { get; set; } = true;

        public List<SplitRule> SplitRules { get; set; } = new List<SplitRule>();
    }

    [XmlInclude(typeof(SplitRuleGettingCards))]
    [XmlInclude(typeof(SplitRuleGettingFairiesOfClass))]
    [XmlInclude(typeof(SplitRuleGettingTotalFairies))]
    [XmlInclude(typeof(SplitRuleReaching))]
    [XmlInclude(typeof(SplitRuleDefeating))]
    [XmlInclude(typeof(SplitRuleWatching))]
    public abstract class SplitRule
    {
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        public virtual SplitRule Clone() => (SplitRule)MemberwiseClone();
        public abstract string GetDescription(Database db);
        public abstract Image GetIcon(Database db);
    }

    public class SplitRuleGettingCards : SplitRule
    {
        public CardId Card { get; set; } = new CardId(CardType.Fairy, 0);
        public int Amount { get; set; } = 1;

        public override string GetDescription(Database db) => Amount > 1
            ? $"Getting at least {Amount} x {db.GetNameFor(Card)}"
            : $"Getting a {db.GetNameFor(Card)}";

        public override Image GetIcon(Database db) => db.GetIconFor(Card);
    }

    public class SplitRuleGettingFairiesOfClass : SplitRule
    {
        public ElementType Type { get; set; } = ElementType.Nature;
        public int Amount { get; set; } = 1;

        public override string GetDescription(Database db) => Amount > 1
            ? $"Getting at least {Amount} fairies of type {db.GetNameFor(Type)}"
            : $"Getting some fairy of type {db.GetNameFor(Type)}";

        public override Image GetIcon(Database db) => db.GetIconFor(Type);
    }

    public class SplitRuleGettingTotalFairies : SplitRule
    {
        public int Amount { get; set; } = 1;

        public override string GetDescription(Database db) => Amount > 1
            ? $"Getting at least {Amount} fairies"
            : $"Getting some fairy";

        public override Image GetIcon(Database db) =>
            db.GetIconFor(new CardId(CardType.Item, 59)); // Fairy bag
    }

    public class SplitRuleReaching : SplitRule
    {
        public SceneId Scene { get; set; } = new SceneId(2400);
        public IconId? OverrideIcon { get; set; } = null;

        public override string GetDescription(Database db) =>
            $"Reaching \"{db.GetNameFor(Scene)}\" ({Scene.index})";

        public override Image GetIcon(Database db) => OverrideIcon.HasValue
            ? db.GetIconFor(OverrideIcon.Value)
            : db.GetIconFor(Scene);
    }

    public class SplitRuleDefeating : SplitRule
    {
        public uint UID { get; set; } = 0;
        public string Name { get; set; } = "<no-name>";
        public IconId? Icon { get; set; } = null;

        public override string GetDescription(Database db) =>
            $"Defeating {Name} ({UID.ToString("X8")})";

        public override Image GetIcon(Database db) => Icon.HasValue
            ? db.GetIconFor(Icon.Value)
            : db.GetIconFor(new IconId(false, "CHR01"));
    }

    public class SplitRuleWatching : SplitRule
    {
        public VideoId Video { get; set; } = VideoId.Outro;

        public override string GetDescription(Database db) =>
            $"Watching {db.GetNameFor(Video)}";

        public override Image GetIcon(Database db) =>
            db.GetIconFor(new CardId(CardType.Item, 59)); // Fairy book
    }

}
