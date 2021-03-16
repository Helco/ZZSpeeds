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
        Psy,
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

        public CardId(string number) : this(ExtractHexNumber(number)) { }
        private static uint ExtractHexNumber(string str)
        {
            if (str.StartsWith("0x"))
                str = str.Substring(2);
            return Convert.ToUInt32(str, 16);
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

        public override bool Equals(object obj)
        {
            return obj is SceneId id &&
                   index == id.index;
        }

        public override int GetHashCode()
        {
            return -1982729373 + index.GetHashCode();
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

        public override bool Equals(object obj)
        {
            return obj is IconId id &&
                   isCardId == id.isCardId &&
                   filename == id.filename;
        }

        public override int GetHashCode()
        {
            int hashCode = 144670618;
            hashCode = hashCode * -1521134295 + isCardId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(filename);
            return hashCode;
        }
    }

    public partial class Database
    {
        private readonly IReadOnlyDictionary<CardId, Image> cardIcons;
        private readonly IReadOnlyDictionary<SceneId, IconId> sceneIcons;

        public IReadOnlyDictionary<CardId, string> Cards { get; }
        public IReadOnlyDictionary<SceneId, string> SceneNames { get; }
        public IReadOnlyCollection<ElementType> ElementTypes { get; }
        public IReadOnlyCollection<VideoId> Videos { get; }
        public IReadOnlyDictionary<IconId, Image> FaceIcons { get; }

        public Database()
        {
            Cards = LoadCards();
            SceneNames = LoadScenes(out sceneIcons);
            ElementTypes = Enum.GetValues(typeof(ElementType)).Cast<ElementType>().ToArray();
            Videos = Enum.GetValues(typeof(VideoId)).Cast<VideoId>().ToArray();

            cardIcons = LoadCardIcons();
            FaceIcons = LoadFaceIcons();
        }

        public string GetNameFor(CardId cardId) => Cards.TryGetValue(cardId, out var name) ? name : "<unknown>";
        public string GetNameFor(SceneId sceneId) => SceneNames.TryGetValue(sceneId, out var name) ? name : "<unknown>";
        public string GetNameFor(ElementType type) => type.ToString();
        public string GetNameFor(VideoId videoId) => videoId.ToString();

        public Image GetIconFor(IconId iconId) => iconId.isCardId
            ? GetIconFor(new CardId(iconId.filename))
            : FaceIcons.TryGetValue(iconId, out var image) ? image
            : SystemIcons.Question.ToBitmap();

        public Image GetIconFor(CardId cardId) => cardIcons.TryGetValue(cardId, out var image)
            ? image
            : SystemIcons.Question.ToBitmap();

        public Image GetIconFor(SceneId sceneId) => sceneIcons.TryGetValue(sceneId, out var iconId)
            ? GetIconFor(iconId)
            : SystemIcons.Question.ToBitmap();

        public Image GetIconFor(ElementType elementType) => elementType switch
        {
            ElementType.Nature  => GetIconFor(new CardId(CardType.Fairy, 0)),   // Silia
            ElementType.Air     => GetIconFor(new CardId(CardType.Fairy, 13)),  // Luria
            ElementType.Water   => GetIconFor(new CardId(CardType.Fairy, 6)),   // Tadana
            ElementType.Light   => GetIconFor(new CardId(CardType.Fairy, 74)),  // Suane
            ElementType.Energy  => GetIconFor(new CardId(CardType.Fairy, 43)),  // Darbue
            ElementType.Psy     => GetIconFor(new CardId(CardType.Fairy, 47)),  // Mentaur
            ElementType.Stone   => GetIconFor(new CardId(CardType.Fairy, 3)),   // Grem
            ElementType.Ice     => GetIconFor(new CardId(CardType.Fairy, 35)),  // Greez
            ElementType.Fire    => GetIconFor(new CardId(CardType.Fairy, 31)),  // Pixe
            ElementType.Dark    => GetIconFor(new CardId(CardType.Fairy, 71)),  // Dredanox
            ElementType.Chaos   => GetIconFor(new CardId(CardType.Fairy, 15)),  // Rasrow
            ElementType.Metal   => GetIconFor(new CardId(CardType.Fairy, 65)),  // Minari
            _ => SystemIcons.Question.ToBitmap()
        };

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
