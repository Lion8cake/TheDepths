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
    public class ShadowBrick : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            AddMapEntry(new Color(26, 24, 37));
			DustType = ModContent.DustType<ShadowDust>();
            HitSound = SoundID.Tink;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}