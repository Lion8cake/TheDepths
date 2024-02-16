using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using TheDepths.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
	public class GlitterBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = false;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 60;
			DustType = ModContent.DustType<GlitterDust>();
			AddMapEntry(new Color(205, 181, 82));
		}
	}
}