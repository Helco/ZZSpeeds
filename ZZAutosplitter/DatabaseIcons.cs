using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ZZAutosplitter.Properties.Resources;

namespace ZZAutosplitter
{
    partial class Database
    {
        private static IReadOnlyDictionary<CardId, Image> LoadCardIcons()
        {
            IEnumerable<KeyValuePair<CardId, Image>> TileSheet(CardType type, Bitmap bitmap)
            {
                // first line is a width marker line, but icons will always be square
                int size = bitmap.Height - 1;
                int count = bitmap.Width / size;
                return Enumerable
                    .Range(0, count)
                    .Select(i => new KeyValuePair<CardId, Image>(
                        new CardId(type, i),
                        bitmap.Clone(new Rectangle(i * size, 1, size, size), bitmap.PixelFormat)));
            }

            return TileSheet(CardType.Item, ITM000T)
                .Concat(TileSheet(CardType.Spell, SPL000T))
                .Concat(TileSheet(CardType.Fairy, WIZ000T))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        private static IReadOnlyDictionary<IconId, Image> LoadFaceIcons() => new Dictionary<string, Image>()
        {
            { nameof(CHR01), CHR01 },
            { nameof(D000S00M), D000S00M },
            { nameof(D001S01M), D001S01M },
            { nameof(D002S02M), D002S02M },
            { nameof(D003S03M), D003S03M },
            { nameof(D004S00M), D004S00M },
            { nameof(D005S01M), D005S01M },
            { nameof(D006S02M), D006S02M },
            { nameof(D007S03M), D007S03M },
            { nameof(D008S00M), D008S00M },
            { nameof(D009S09M), D009S09M },
            { nameof(G000S00M), G000S00M },
            { nameof(G001S01M), G001S01M },
            { nameof(G002S02M), G002S02M },
            { nameof(G003S03M), G003S03M },
            { nameof(G004S03M), G004S03M },
            { nameof(G005S02M), G005S02M },
            { nameof(G006S00M), G006S00M },
            { nameof(G007S01M), G007S01M },
            { nameof(G200S20M), G200S20M },
            { nameof(G201S21M), G201S21M },
            { nameof(G202S22M), G202S22M },
            { nameof(G203S23M), G203S23M },
            { nameof(G204S20M), G204S20M },
            { nameof(G205S21M), G205S21M },
            { nameof(G206S22M), G206S22M },
            { nameof(G207S23M), G207S23M },
            { nameof(G208S21M), G208S21M },
            { nameof(G300S30M), G300S30M },
            { nameof(G301S31M), G301S31M },
            { nameof(G302S32M), G302S32M },
            { nameof(I002SI02), I002SI02 },
            { nameof(I003SI03), I003SI03 },
            { nameof(U000S00M), U000S00M },
            { nameof(U001S01M), U001S01M },
            { nameof(U002S02M), U002S02M },
            { nameof(U003S03M), U003S03M },
            { nameof(V000S00M), V000S00M },
            { nameof(V001S01M), V001S01M },
            { nameof(W000S00M), W000S00M },
            { nameof(W001S01M), W001S01M },
            { nameof(W002S02M), W002S02M },
            { nameof(W003S03M), W003S03M },
            { nameof(X000S00M), X000S00M },
            { nameof(pixie), pixie },
        }.ToDictionary(kv => new IconId(false, kv.Key), kv => kv.Value);
    }
}
