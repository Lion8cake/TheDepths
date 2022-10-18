using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TheDepths.Tiles
{
    public class SilverfallBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = false;
            HitSound = SoundID.Dig;
            ItemDrop = ItemType<Items.Placeable.SilverfallBlock>();
            AddMapEntry(new Color(232, 233, 234), (LocalizedText)null);
            AnimationFrameHeight = 90;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileNoAttach[Type] = false;
            Main.tileFrameImportant[Type] = false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float BASE = 1 / 255;
            r = BASE * 185;
            g = BASE * 197;
            b = BASE * 200;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[327];
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
    }
}
