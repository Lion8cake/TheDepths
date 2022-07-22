using Microsoft.Xna.Framework;
using TheDepths.Items.Placeable;
using TheDepths.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Walls
{
	public class QuartzBrickWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<QuartzCrystals>();
			ItemDrop = ModContent.ItemType<Items.Placeable.QuartzBrickWall>();
			AddMapEntry(new Color(54, 68, 73));
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}