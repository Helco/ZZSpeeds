using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ZZAutosplitter.DatabaseScenes;

namespace ZZAutosplitter
{
    partial class Database
    {
        private static Dictionary<SceneId, string> LoadScenes()
        {
            var xmlSerializer = new XmlSerializer(typeof(scenes));
            using var stringReader = new StringReader(Properties.Resources.scenes);
            var scenes = (scenes)xmlSerializer.Deserialize(stringReader);

            var results = new Dictionary<SceneId, string>();
            foreach (var scene in scenes.scene)
            {
                results.Add(new SceneId(scene.id), scene.name);
            }
            return results;
        }
    }
}

namespace ZZAutosplitter.DatabaseScenes
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class scenes
    {

        private scenesScene[] sceneField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("scene")]
        public scenesScene[] scene
        {
            get
            {
                return this.sceneField;
            }
            set
            {
                this.sceneField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class scenesScene
    {

        private ushort idField;

        private string nameField;

        /// <remarks/>
        public ushort id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
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
    }
}
