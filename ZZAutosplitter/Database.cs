using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZAutosplitter
{
    public enum CardType
    {
        Item = 0,
        Spell,
        Fairy
    }

    public enum ElementType
    {
        Nature = 1,
        Air,
        Water,
        Light,
        Energy,
        Mental,
        Stone,
        Ice,
        Fire,
        Dark,
        Chaos,
        Metal
    }

    public enum VideoId
    {
        Intro = 0,
        Book1,
        Book2,
        Book3,
        Book4,
        Book5,
        Outro
    }

    public struct CardId
    {
        public CardType type;
        public int index;

        public readonly uint AsNumber => 0xff | ((uint)type << 8) | ((uint)index << 16);

        public CardId(CardType type, int index)
        {
            this.type = type;
            this.index = index;
        }

        public CardId(uint number)
        {
            uint typeNumber = number >> 8 & 0xff;
            if (typeNumber > 2)
                throw new ArgumentOutOfRangeException("Invalid type number");
            type = (CardType)typeNumber;
            index = (int)(number >> 16);
        }

        public static bool operator ==(CardId a, CardId b) => a.Equals(b);
        public static bool operator !=(CardId a, CardId b) => !a.Equals(b);
        public override bool Equals(object obj)
        {
            return obj is CardId id &&
                   type == id.type &&
                   index == id.index;
        }

        public override int GetHashCode()
        {
            int hashCode = -842337720;
            hashCode = hashCode * -1521134295 + type.GetHashCode();
            hashCode = hashCode * -1521134295 + index.GetHashCode();
            return hashCode;
        }
    }

    public struct SceneId
    {
        // maybe scene type at some point?
        public int index;

        public SceneId(int index)
        {
            this.index = index;
        }
    }

    public struct IconId
    {
        public bool isCardId;
        public string filename;

        public IconId(bool isCardId, string filename)
        {
            this.isCardId = isCardId;
            this.filename = filename;
        }
    }

    public partial class Database
    {
        private readonly IReadOnlyDictionary<CardId, Image> cardIcons;

        public IReadOnlyDictionary<CardId, string> Cards { get; }
        public IReadOnlyDictionary<SceneId, string> Scenes { get; }
        public IReadOnlyCollection<ElementType> ElementTypes { get; }
        public IReadOnlyCollection<VideoId> Videos { get; }
        public IReadOnlyDictionary<IconId, Image> FaceIcons { get; }

        public Database()
        {
            Cards = LoadCards();
            Scenes = LoadScenes();
            ElementTypes = Enum.GetValues(typeof(ElementType)).Cast<ElementType>().ToArray();
            Videos = Enum.GetValues(typeof(VideoId)).Cast<VideoId>().ToArray();

            cardIcons = LoadCardIcons();
            FaceIcons = LoadFaceIcons();
        }

        public string GetNameFor(CardId cardId) => Cards.TryGetValue(cardId, out var name) ? name : "<unknown>";
        public string GetNameFor(SceneId sceneId) => Scenes.TryGetValue(sceneId, out var name) ? name : "<unknown>";
        public string GetNameFor(ElementType type) => type.ToString();
        public string GetNameFor(VideoId videoId) => videoId.ToString();

        public Image GetIconFor(IconId iconId) => iconId.isCardId
            ? GetIconFor(new CardId(uint.Parse(iconId.filename)))
            : FaceIcons.TryGetValue(iconId, out var image) ? image
            : SystemIcons.Question.ToBitmap();

        public Image GetIconFor(CardId cardId) => cardIcons.TryGetValue(cardId, out var image)
            ? image
            : SystemIcons.Question.ToBitmap();

        public string GetFileNameFor(VideoId videoId) => videoId switch
        {
            VideoId.Intro => "_v000",
            VideoId.Book1 => "_v001",
            VideoId.Book2 => "_v002",
            VideoId.Book4 => "_v003",
            VideoId.Book5 => "_v004",
            VideoId.Book3 => "_v005",
            VideoId.Outro => "_v006",
            _ => "____"
        };
    }
}
