using Microsoft.Xna.Framework;
using TheDepths.Items.Placeable;
using TheDepths.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Walls
{
	public class QuartzBrickWallUnsafe : ModWall
	{
		public override void SetStaticDefaults() {
			DustType = ModContent.DustType<QuartzCrystals>();
			AddMapEntry(new Color(54, 68, 73));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.QuartzBrickWall>());
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}