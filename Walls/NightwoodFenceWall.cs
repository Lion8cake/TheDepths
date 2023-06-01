using Microsoft.Xna.Framework;
using TheDepths.Items.Placeable;
using TheDepths.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Walls
{
	public class NightwoodFenceWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<NightWoodDust>();
			AddMapEntry(new Color(23, 25, 28));
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}