using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Walls
{
	public class BlackStainedGlass : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(96, 94, 98));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}