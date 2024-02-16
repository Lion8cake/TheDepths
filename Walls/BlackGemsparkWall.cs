using Microsoft.Xna.Framework;
using TheDepths.Items.Placeable;
using TheDepths.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Walls
{
	public class BlackGemsparkWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;
			Main.wallLight[Type] = true;
			DustType = ModContent.DustType<GemOnyxDust>();
			AddMapEntry(new Color(22, 19, 28));
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = (float)(0.058f * 0.8);
			g = (float)(0.061f * 0.8);
			b = (float)(0.06f * 0.8);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}