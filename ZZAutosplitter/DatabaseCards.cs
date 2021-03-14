using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ZZAutosplitter.DatabaseCards;

namespace ZZAutosplitter
{
    partial class Database
    {
        private static Dictionary<CardId, string> LoadCards()
        {
            var xmlSerializer = new XmlSerializer(typeof(results));
            using var stringReader = new StringReader(Properties.Resources.cards);
            var results = (results)xmlSerializer.Deserialize(stringReader);

            var cards = new Dictionary<CardId, string>();
            foreach (var row in results.rows)
            {
                var cardId = new CardId((CardType)int.Parse(row[0].Value), int.Parse(row[1].Value));
                cards.Add(cardId, row[2].Value);
            }
            return cards;
        }
    }
}

namespace ZZAutosplitter.DatabaseCards
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class results
    {
        private resultsRowValue[][] rowsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("row", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("value", IsNullable = false, NestingLevel = 1)]
        public resultsRowValue[][] rows
        {
            get
            {
                return this.rowsField;
            }
            set
            {
                this.rowsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class resultsRowValue
    {

        private byte columnField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte column
        {
            get
            {
                return this.columnField;
            }
            set
            {
                this.columnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
