using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using TheDepths.Dusts;
using TheDepths.Buffs;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Tiles
{
    public class QuartzPillar : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            AddMapEntry(new Color(138, 158, 168));
			DustType = ModContent.DustType<QuartzCrystals>();
            HitSound = SoundID.Tink;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}