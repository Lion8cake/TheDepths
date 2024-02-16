using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;
using static Terraria.WorldGen;

namespace TheDepths.Tiles.Trees
{
	public class OnyxGemtreeSapling : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.CommonSapling[Type] = true;
			TileID.Sets.ReplaceTileBreakUp[Type] = true;
			TileID.Sets.SlowlyDiesInWater[Type] = true;
			TileID.Sets.DrawFlipMode[Type] = 1;
			TileID.Sets.SwaysInWindBasic[Type] = true;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[2] { 16, 18 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorValidTiles = new int[16] { 1, 25, 117, 203, 182, 180, 179, 381, 183, 181, 534, 536, 539, 625, 627, ModContent.TileType<Tiles.Shalestone>() };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(128, 128, 128));
			DustType = ModContent.DustType<Dusts.GemOnyxDust>();
			AdjTiles = new int[1] { TileID.Saplings };
		}

		public override bool CanDrop(int i, int j)
		{
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if (j > Main.rockLayer)
			{
				Tile tile = Main.tile[i, j];
				if (tile.HasUnactuatedTile)
				{
					for (int k = 0; k < (Main.maxTilesX * Main.maxTilesY); k++)
					{
						if (WorldGen.genRand.Next(5) == 0)
						{
							AttemptToGrowOnyxFromSapling(i, j, underground: true);
						}
					}
				}
			}
		}

		public static bool AttemptToGrowOnyxFromSapling(int x, int y, bool underground)
		{
			if (Main.netMode == 1)
			{
				return false;
			}
			if (!InWorld(x, y, 2))
			{
				return false;
			}
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.HasTile)
			{
				return false;
			}
			if (!underground)
			{
				return false;
			}
			bool flag = DepthsModTree.GrowModdedTreeWithSettings(x, y, OnyxGemtree.GemTree_Onyx);
			if (flag && PlayerLOS(x, y))
			{
				GrowOnyxTreeFXCheck(x, y);
			}
			return flag;
		}

		public static void GrowOnyxTreeFXCheck(int x, int y)
		{
			int treeHeight = 1;
			for (int num = -1; num > -100; num--)
			{
				Tile tile = Main.tile[x, y + num];
				if (!tile.HasTile || !TileID.Sets.GetsCheckedForLeaves[tile.TileType])
				{
					break;
				}
				treeHeight++;
			}
			for (int i = 1; i < 5; i++)
			{
				Tile tile2 = Main.tile[x, y + i];
				if (tile2.HasTile && TileID.Sets.GetsCheckedForLeaves[tile2.TileType])
				{
					treeHeight++;
					continue;
				}
				break;
			}
			if (treeHeight > 0)
			{
				if (Main.netMode == 2)
				{
					NetMessage.SendData(112, -1, -1, null, 1, x, y, treeHeight, ModContent.GoreType<OnyxGemtreeLeaf>());
				}
				if (Main.netMode == 0)
				{
					WorldGen.TreeGrowFX(x, y, treeHeight, ModContent.GoreType<OnyxGemtreeLeaf>());
				}
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects)
		{
			if (i % 2 == 0)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
		}
	}
}