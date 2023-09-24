using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheDepths.Tiles
{
	public class DepthsBanners : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[3] {
				16,
				16,
				16
			};

			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.DrawFlipHorizontal = false;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.newAlternate.DrawFlipHorizontal = false;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(Type);

			TileID.Sets.DisableSmartCursor[Type] = true;
			AddMapEntry(new Color(233, 207, 94), Language.GetText("MapObject.Banner"));
			AdjTiles = new int[] { TileID.Banners };
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			bool intoRenderTargets = true;
			bool flag = intoRenderTargets || Main.LightingEveryFrame;

			if (Main.tile[i, j].TileFrameX % 18 == 0 && Main.tile[i, j].TileFrameY % 54 == 0 && flag)
			{
				Main.instance.TilesRenderer.AddSpecialPoint(i, j, 5);
			}

			return false;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			if ((Framing.GetTileSafely(i, j - 1).HasTile && TileID.Sets.Platforms[Framing.GetTileSafely(i, j - 1).TileType]) ||
				(Framing.GetTileSafely(i, j - 2).HasTile && TileID.Sets.Platforms[Framing.GetTileSafely(i, j - 2).TileType]) ||
				(Framing.GetTileSafely(i, j - 3).HasTile && TileID.Sets.Platforms[Framing.GetTileSafely(i, j - 3).TileType]))
			{
				offsetY -= 8;
			}
		}

		public override void NearbyEffects(int i, int j, bool closer) {
			if (closer) {
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].TileFrameX / 18;
				string type;
				switch (style) {
					case 0:
						type = "AlbinoBat";
						break;
					case 1:
						type = "ShadowSlime";
						break;
					case 2:
						type = "Geomancer";
						break;
					case 3:
						type = "Shade";
						break;
					case 4:
						type = "SapphireSerpentHead";
						break;
					case 5:
						type = "GoldBat";
						break;
					case 6:
						type = "Ferroslime";
						break;
					case 7:
						type = "ShadowBat";
						break;
					case 8:
						type = "CrystalKing";
						break;
					case 9:
						type = "KingCoal";
						break;
					case 10:
						type = "Archroma";
						break;
						default:
					return;
				}
				Main.SceneMetrics.NPCBannerBuff[Mod.Find<ModNPC>(type).Type] = true;
				Main.SceneMetrics.hasBanner = true;
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 1) {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
}
