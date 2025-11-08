using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using TheDepths.Dusts;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Tiles
{
    public class NightWood : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            Main.tileMerge[Type][ModContent.TileType<ShaleBlock>()] = true;
            AddMapEntry(new Color(42, 43, 51));
			DustType = ModContent.DustType<NightWoodDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}