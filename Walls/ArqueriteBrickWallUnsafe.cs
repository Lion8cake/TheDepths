using Microsoft.Xna.Framework;
using TheDepths.Items.Placeable;
using TheDepths.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Walls
{
	public class ArqueriteBrickWallUnsafe : ModWall
	{
		public override void SetStaticDefaults() {
			DustType = ModContent.DustType<ArqueriteDust>();
			AddMapEntry(new Color(38, 45, 55));
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}