using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ZZAutosplitter.DatabaseNPCPresets;

namespace ZZAutosplitter
{
    partial class Database
    {
        private static SplitRuleDefeating[] LoadNPCPresets()
        {
            var xmlSerializer = new XmlSerializer(typeof(npcs));
            using var stringReader = new StringReader(Properties.Resources.npcs);
            var results = (npcs)xmlSerializer.Deserialize(stringReader);

            return results.npc.Select(row => new SplitRuleDefeating()
            {
                UID = Convert.ToUInt32(row.uid, 16),
                Name = row.name,
                Icon = new IconId(isCardId: false, row.icon)
            }).ToArray();
        }
    }
}

namespace ZZAutosplitter.DatabaseNPCPresets
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class npcs
    {

        private npcsNpc[] npcField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("npc")]
        public npcsNpc[] npc
        {
            get
            {
                return this.npcField;
            }
            set
            {
                this.npcField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class npcsNpc
    {

        private string uidField;

        private string nameField;

        private string iconField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string uid
        {
            get
            {
                return this.uidField;
            }
            set
            {
                this.uidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string icon
        {
            get
            {
                return this.iconField;
            }
            set
            {
                this.iconField = value;
            }
        }
    }


}
