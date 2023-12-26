using Microsoft.Xna.Framework;
using TheDepths.Items.Placeable;
using TheDepths.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Walls
{
	public class NaturalQuicksilverWall4 : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = false;
			DustType = ModContent.DustType<ArqueriteDust>();
			AddMapEntry(new Color(18, 19, 22));
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}