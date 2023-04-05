using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
	public class QuicksilverDropletSource : ModTile
	{
		public override string Texture => "TheDepths/Projectiles/CrystalBall";

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(85, 96, 102), name);
			DustType = 0;
		}

		public override void RandomUpdate(int i, int j)
		{
			Gore.NewGore(new EntitySource_Misc(""), new Vector2((i - Main.tile[i, j].TileFrameX / 18) * 16, j - Main.tile[i, j].TileFrameY / 18 * 16), Vector2.Zero, ModContent.Find<ModGore>("TheDepths/QuicksilverDroplet").Type, 1f);
		}
	}
}