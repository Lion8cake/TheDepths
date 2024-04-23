using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Dusts;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace TheDepths.Tiles
{
    public class Ember: ModTile //This now dud tile is a placeholder for the Depth's map background color
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileNoAttach[Type] = true;
            DustType = ModContent.DustType<EmberDust>();
            AddMapEntry(new Color(5, 5, 7));
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.Ember>());
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            int type = -1;
            if (tileBelow.HasTile && !tileBelow.BottomSlope)
            {
                type = tileBelow.TileType;
            }
            if (type == Type)
            {
                return true;
            }
            WorldGen.KillTile(i, j);
            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override bool CanPlace(int i, int j)
        {
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            Tile tileAbove = Framing.GetTileSafely(i, j - 1);
            int type = -1;
            return !(!tileBelow.HasTile || tileBelow.IsActuated || tileBelow.IsHalfBlock || tileBelow.BottomSlope || tileBelow.TopSlope || tileAbove.HasTile || !Main.tileSolid[type] || !Main.tileSolidTop[type]);
        }
    }
}