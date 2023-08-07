using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Walls
{
	public class SilverfallWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            ItemDrop = ModContent.ItemType<Items.Placeable.SilverfallWall>();
            AddMapEntry(new Color(96, 94, 98));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void AnimateWall(ref byte frame, ref byte frameCounter)
        {
			frame = Main.wallFrame[137];
        }
    }
}