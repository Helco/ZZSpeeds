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
    [Flags]
    public enum IconSets
    {
        None = (1 << 0),
        Items = (1 << 1),
        Spells = (1 << 2),
        Fairies = (1 << 3),
        Faces = (1 << 4)
    }

    public partial class IconSelectionBox : Form
    {
        private readonly Database database;
        private readonly IconId?[] iconIds;

        public IconId? SelectedIcon
        {
            get => listIcons.SelectedIndices.Count > 0
                ? iconIds[listIcons.SelectedIndices[0]]
                : null;
            set
            {
                var index = Array.IndexOf(iconIds, value);
                listIcons.SelectedIndices.Clear();
                if (index >= 0)
                    listIcons.SelectedIndices.Add(index);
            }
        }
        
        public IconSelectionBox(Database database, IconSets iconSets)
        {
            this.database = database;
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            var iconIds = Enumerable.Empty<IconId?>();
            if (iconSets.HasFlag(IconSets.None))
                iconIds = new IconId?[] { null };
            if (iconSets.HasFlag(IconSets.Items))
                iconIds = iconIds.Concat(CardIconsFor(CardType.Item));
            if (iconSets.HasFlag(IconSets.Spells))
                iconIds = iconIds.Concat(CardIconsFor(CardType.Spell));
            if (iconSets.HasFlag(IconSets.Fairies))
                iconIds = iconIds.Concat(CardIconsFor(CardType.Fairy));
            if (iconSets.HasFlag(IconSets.Faces))
                iconIds = iconIds.Concat(database.FaceIcons.Keys.Select(k => new IconId?(k)));
            this.iconIds = iconIds.ToArray();

            var iconSize = new Size(listIcons.TileSize.Width - 4, listIcons.TileSize.Height - 4);
            var imgList = listIcons.LargeImageList = new ImageList();
            imgList.ImageSize = iconSize;
            imgList.Images.AddRange(this.iconIds
                .Select(id => id == null
                    ? new Bitmap(iconSize.Width, iconSize.Height)
                    : database.GetIconFor(id.Value))
                .Select(img => img.Size == iconSize
                    ? img
                    : new Bitmap(img, iconSize))
                .ToArray());
            listIcons.Items.AddRange(Enumerable
                .Range(0, this.iconIds.Length)
                .Select(i => new ListViewItem("", i))
                .ToArray());
        }

        private IEnumerable<IconId?> CardIconsFor(CardType type) => database.Cards.Keys
            .Where(c => c.type == type)
            .Select(c => new IconId?(new IconId(isCardId: true, $"0x{c.AsNumber:X}")));

        private void listIcons_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listIcons.SelectedIndices.Count > 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
