using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Tiles
{
    public class Ember : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
			Main.tileNoAttach[Type] = true;
			DustType = Mod.Find<ModDust>("EmberDust").Type;
            ItemDrop = ModContent.ItemType<Items.Placeable.Ember>();
            AddMapEntry(new Color(179, 79, 36));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}