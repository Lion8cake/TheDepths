using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using TheDepths.Dusts;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Tiles
{
    public class PetrifiedWood : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            ItemDrop = ModContent.ItemType<Items.Placeable.PetrifiedWood>();
            AddMapEntry(new Color(55, 55, 45));
			DustType = ModContent.DustType<PetrifiedWoodDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}