using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheDepths.Tiles
{
	public class DepthsVanityBanners : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);
			DustType = -1;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(13, 88, 130), name);
		}

		/*public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			int style = frameX / 18;
			string item;
			switch (style) {
				case 0:
					item = "AlbinoBatBanner";
					break;
				case 1:
					item = "ShadowSlimeBanner";
					break;
				case 2:
					item = "GeomancerBanner";
					break;
				case 3:
					item = "ShadeBanner";
					break;
				case 4:
					item = "SapphireSerpentBanner";
					break;
				case 5:
					item = "GoldBatBanner";
					break;
				case 6:
					item = "FerroslimeBanner";
					break;
				case 7:
					item = "ShadowBatBanner";
					break;
				case 8:
					item = "CrystalKingBanner";
					break;
				case 9:
					item = "KingCoalBanner";
					break;
				case 10:
					item = "AchromaBanner";
					break;
				default:
					return;
					
			}
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, Mod.Find<ModItem>(item).Type);
		}*/

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 1) {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
}
