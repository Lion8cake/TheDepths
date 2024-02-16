using Microsoft.Xna.Framework;
using TheDepths.Items.Placeable;
using TheDepths.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Walls
{
	public class ShaleWallUnsafe : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = false;
			DustType = ModContent.DustType<ShaleDust>();
			AddMapEntry(new Color(14, 17, 16));
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}