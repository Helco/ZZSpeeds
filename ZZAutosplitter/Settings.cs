using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public abstract class SplitRule
    {
        public abstract string GetDescription(Database db);
    }

    public class SplitRuleGettingCards : SplitRule
    {
        public CardId Card { get; set; }
        public int Amount { get; set; }

        public override string GetDescription(Database db) => Amount > 1
            ? $"Getting at least {Amount} x {db.GetNameFor(Card)}"
            : $"Getting a {db.GetNameFor(Card)}";
    }

    public class SplitRuleGettingFairiesOfClass : SplitRule
    {
        public ElementType Type { get; set; }
        public int Amount { get; set; }

        public override string GetDescription(Database db) => Amount > 1
            ? $"Getting at least {Amount} fairies of type {db.GetNameFor(Type)}"
            : $"Getting some fairy of type {db.GetNameFor(Type)}";
    }

    public class SplitRuleGettingTotalFairies : SplitRule
    {
        public int Amount { get; set; }

        public override string GetDescription(Database db) => Amount > 1
            ? $"Getting at least {Amount} fairies"
            : $"Getting some fairy";
    }

    public class SplitRuleReaching : SplitRule
    {
        public SceneId Scene { get; set; }

        public override string GetDescription(Database db) =>
            $"Reaching \"{db.GetNameFor(Scene)}\" ({Scene.index})";
    }

    public class SplitRuleDefeating : SplitRule
    {
        public uint UID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public override string GetDescription(Database db) =>
            $"Defeating {Name} ({UID.ToString("X8")})";
    }

    public class SplitRuleWatching : SplitRule
    {
        public VideoId Video { get; set; }

        public override string GetDescription(Database db) =>
            $"Watching {db.GetNameFor(Video)}";
    }

}
