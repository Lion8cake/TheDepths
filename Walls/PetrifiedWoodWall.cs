using Microsoft.Xna.Framework;
using TheDepths.Items.Placeable;
using TheDepths.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Walls
{
	public class PetrifiedWoodWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<PetrifiedWoodDust>();
			AddMapEntry(new Color(27, 27, 22));
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}