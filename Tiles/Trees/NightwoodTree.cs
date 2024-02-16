using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using TheDepths.Dusts;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;
using System;
using Terraria.GameContent;
using Terraria.Utilities;
using Terraria.DataStructures;
using System.Reflection;
using Terraria.GameContent.Skies.CreditsRoll;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terraria.GameContent.Tile_Entities;
using static Terraria.WorldGen;
using Terraria.Enums;
using System.Threading;
using static Terraria.GameContent.TilePaintSystemV2;
using ReLogic.Content;
using TheDepths.Items;
using Terraria.GameContent.Liquid;
using static Terraria.GameContent.Drawing.TileDrawing;

namespace TheDepths.Tiles.Trees
{
    public class NightwoodTree : DepthsModTree
    {
		public override string TexturePath => "TheDepths/Tiles/Trees/NightwoodTree";

		public static GrowTreeSettings Tree_Nightmare = new GrowTreeSettings
		{
			GroundTest = NightmareTreeGroundTest,
			WallTest = DefaultTreeWallTest,
			TreeHeightMax = 12,
			TreeHeightMin = 7,
			TreeTileType = (ushort)ModContent.TileType<NightwoodTree>(),
			TreeTopPaddingNeeded = 4,
			SaplingTileType = (ushort)ModContent.TileType<NightSapling>()
		};

		public static TreePaintingSettings TreeNightmare = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		public override void SetStaticDefaults()
        {
            Main.tileAxe[Type] = true;
            Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.IsATreeTrunk[Type] = true;
            TileID.Sets.IsShakeable[Type] = true;
            TileID.Sets.GetsDestroyedForMeteors[Type] = true;
            TileID.Sets.GetsCheckedForLeaves[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(42, 43, 51), name);
			DustType = ModContent.DustType<NightWoodDust>();
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			width = 20;
			height = 20;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile tile = Main.tile[i, j];
			if (i > 5 && j > 5 && i < Main.maxTilesX - 5 && j < Main.maxTilesY - 5 && Main.tile[i, j] != null)
			{
				if (tile.HasTile)
				{
					if (Main.tileFrameImportant[Type])
					{
						CheckTreeWithSettings(i, j, new CheckTreeSettings
						{
							IsGroundValid = NightmareTreeGroundTest
						});
					}
				}
			} 
			return false;
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			int dropItem = ItemID.None;
			int dropItemStack = 1;
			int secondaryItem = ItemID.None;
			Tile tileCache = Main.tile[i, j];
			bool bonusWood = false;
			KillTile_GetTreeDrops(i, j, tileCache, ref bonusWood, ref dropItem, ref secondaryItem);
			if (bonusWood)
			{
				dropItemStack++;
			}
			yield return new Item(dropItem, dropItemStack);
			yield return new Item(secondaryItem);
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Tile tile = Main.tile[i, j];
			if (fail)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient && TileID.Sets.IsShakeable[tile.TileType])
				{
					ShakeTree(i, j);
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
			DrawTrees(i, j);
			spriteBatch.End();
			spriteBatch.Begin(); //No params as PostDraw doesn't use spritebatch with params
			TheDepthsModSystem.DrawGlowmask(i, j);
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = (float)0.43 / 2;
			g = (float)0.28 / 2;
			b = (float)0.83 / 2;
		}

		private static void ShakeTree(int i, int j)
		{
			FieldInfo numTreeShakesReflect = typeof(WorldGen).GetField("numTreeShakes", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
			int numTreeShakes = (int)numTreeShakesReflect.GetValue(null);
			int maxTreeShakes = (int)typeof(WorldGen).GetField("maxTreeShakes", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
			int[] treeShakeX = (int[])typeof(WorldGen).GetField("treeShakeX", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
			int[] treeShakeY = (int[])typeof(WorldGen).GetField("treeShakeY", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
			if (numTreeShakes == maxTreeShakes)
			{
				return;
			}
			GetTreeBottom(i, j, out var x, out var y);
			for (int k = 0; k < numTreeShakes; k++)
			{
				if (treeShakeX[k] == x && treeShakeY[k] == y)
				{
					return;
				}
			}
			treeShakeX[numTreeShakes] = x;
			treeShakeY[numTreeShakes] = y;
			numTreeShakesReflect.SetValue(null, ++numTreeShakes);
			y--;
			while (y > 10 && Main.tile[x, y].HasTile && TileID.Sets.IsShakeable[Main.tile[x, y].TileType])
			{
				y--;
			}
			y++;
			if (!IsTileALeafyTreeTop(x, y) || Collision.SolidTiles(x - 2, x + 2, y - 2, y + 2))
			{
				return;
			}

			if (Main.getGoodWorld && genRand.Next(17) == 0)
			{
				Projectile.NewProjectile(new EntitySource_ShakeTree(x, y), x * 16, y * 16, (float)Main.rand.Next(-100, 101) * 0.002f, 0f, ProjectileID.Bomb, 0, 0f, Main.myPlayer, 16f, 16f);
			}
			else if (genRand.Next(35) == 0 && Main.halloween)
			{
				Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.RottenEgg, genRand.Next(1, 3));
			}
			else if (genRand.Next(12) == 0)
			{
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ModContent.ItemType<Items.Placeable.NightWood>(), genRand.Next(1, 4));
			}
			else if (genRand.Next(20) == 0)
			{
				int type = ItemID.CopperCoin;
				int num2 = genRand.Next(50, 100);
				if (genRand.Next(30) == 0)
				{
					type = ItemID.GoldCoin;
					num2 = 1;
					if (genRand.Next(5) == 0)
					{
						num2++;
					}
					if (genRand.Next(10) == 0)
					{
						num2++;
					}
				}
				else if (genRand.Next(10) == 0)
				{
					type = ItemID.SilverCoin;
					num2 = genRand.Next(1, 21);
					if (genRand.Next(3) == 0)
					{
						num2 += genRand.Next(1, 21);
					}
					if (genRand.Next(4) == 0)
					{
						num2 += genRand.Next(1, 21);
					}
				}
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, type, num2);
			}
			else if (genRand.Next(20) == 0 && y > Main.maxTilesY - 250)
			{
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, (WorldGen.genRand.NextBool(3) ? ModContent.NPCType<NPCs.AlbinoRat>() : (WorldGen.genRand.NextBool(2) ? ModContent.NPCType<NPCs.EnchantedNightmareWorm>() : ModContent.NPCType<NPCs.QuartzCrawler>())));
			}
			else if (Main.remixWorld && genRand.Next(20) == 0 && y > Main.maxTilesY - 250)
			{
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ItemID.Rope, genRand.Next(20, 41));
			}
			else if (genRand.Next(12) == 0)
			{
				int secondaryItemStack = ((genRand.Next(2) != 0) ? ModContent.ItemType<BlackOlive>() : ModContent.ItemType<Ciamito>());
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, secondaryItemStack);
			}

			if (Main.netMode == 2)
			{
				NetMessage.SendData(112, -1, -1, null, 1, x, y, 1f, ModContent.GoreType<PetrifiedTreeLeaf>());
			}
			if (Main.netMode == 0)
			{
				TreeGrowFX(x, y, 1, ModContent.GoreType<PetrifiedTreeLeaf>(), hitTree: true);
			}
		}

		private static void EmitPetrifiedLeaves(int tilePosX, int tilePosY, int grassPosX, int grassPosY)
		{
			bool _isActiveAndNotPaused = (bool)typeof(TileDrawing).GetField("_isActiveAndNotPaused", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			int _leafFrequency = (int)typeof(TileDrawing).GetField("_leafFrequency", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			UnifiedRandom _rand = (UnifiedRandom)typeof(TileDrawing).GetField("_rand", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			if (!_isActiveAndNotPaused)
			{
				return;
			}
			Tile tile = Main.tile[tilePosX, tilePosY];
			if (tile.LiquidAmount > 0)
			{
				return;
			}
			int num = 0;
			bool flag = (byte)num != 0;
			int num2 = _leafFrequency;
			bool flag2 = tilePosX - grassPosX != 0;
			if (flag)
			{
				num2 /= 2;
			}
			if (!WorldGen.DoesWindBlowAtThisHeight(tilePosY))
			{
				num2 = 10000;
			}
			if (flag2)
			{
				num2 *= 3;
			}
			if (_rand.Next(num2) != 0)
			{
				return;
			}
			int num3 = 2;
			Vector2 vector = new((float)(tilePosX * 16 + 8), (float)(tilePosY * 16 + 8));
			if (flag2)
			{
				int num4 = tilePosX - grassPosX;
				vector.X += num4 * 12;
				int num5 = 0;
				if (tile.TileFrameY == 220)
				{
					num5 = 1;
				}
				else if (tile.TileFrameY == 242)
				{
					num5 = 2;
				}
				if (tile.TileFrameX == 66)
				{
					switch (num5)
					{
						case 0:
							vector += new Vector2(0f, -6f);
							break;
						case 1:
							vector += new Vector2(0f, -6f);
							break;
						case 2:
							vector += new Vector2(0f, 8f);
							break;
					}
				}
				else
				{
					switch (num5)
					{
						case 0:
							vector += new Vector2(0f, 4f);
							break;
						case 1:
							vector += new Vector2(2f, -6f);
							break;
						case 2:
							vector += new Vector2(6f, -6f);
							break;
					}
				}
			}
			else
			{
				vector += new Vector2(-16f, -16f);
				if (flag)
				{
					vector.Y -= Main.rand.Next(0, 28) * 4;
				}
			}
			if (!WorldGen.SolidTile(vector.ToTileCoordinates()))
			{
				Gore.NewGoreDirect(new EntitySource_Misc(""), vector, Utils.RandomVector2(Main.rand, -num3, num3), ModContent.GoreType<PetrifiedTreeLeaf>(), 0.7f + Main.rand.NextFloat() * 0.6f).Frame.CurrentColumn = Main.tile[tilePosX, tilePosY].TileColor;
			}
		}

		public static bool NightmareTreeGroundTest(int tileType)
		{
			if (tileType < 0)
			{
				return false;
			}
			if (tileType == ModContent.TileType<NightmareGrass>())
			{
				return true;
			}
			return false;
		}

		public static bool GetNightmareTreeFoliageData(int i, int j, int xoffset, ref int treeFrame, out int floorY, out int topTextureFrameWidth, out int topTextureFrameHeight)
		{
			int num = i + xoffset;
			topTextureFrameWidth = 80;
			topTextureFrameHeight = 80;
			floorY = j;
			for (int k = 0; k < 100; k++)
			{
				floorY = j + k;
				Tile tile2 = Main.tile[num, floorY];
				if (tile2 == null)
				{
					return false;
				}
			}
			return true;
		}

		private void DrawTrees(int k, int l)
		{
			double _treeWindCounter = (double)typeof(TileDrawing).GetField("_treeWindCounter", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			float num15 = 0.08f;
			float num16 = 0.06f;
			int PositioningFix = 192; //Fix to the positioning to the Branches and Tops being 192 pixels to the top and left
			int x = k;
			int y = l;
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.HasTile)
			{
				return;
			}
			short frameX = tile.TileFrameX;
			short frameY = tile.TileFrameY;
			bool flag = tile.WallType > 0;
			if (frameY >= 198 && frameX >= 22)
			{
				int treeFrame = WorldGen.GetTreeFrame(tile);
				switch (frameX)
				{
					case 22:
						{
							int num5 = 0;
							int grassPosX = x + num5;
							if (!GetNightmareTreeFoliageData(x, y, num5, ref treeFrame, out int floorY3, out int topTextureFrameWidth3, out int topTextureFrameHeight3))
							{
								return;
							}
							//EmitPetrifiedLeaves(x, y, grassPosX, floorY3);
							byte tileColor3 = tile.TileColor;
							Texture2D treeTopTexture = GetTreeTopTexture(Type, 0, tileColor3);
							Vector2 vector = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8 + PositioningFix), (float)(y * 16 - (int)unscaledPosition.Y + 16 + PositioningFix)) + zero;
							float num7 = 0f;
							if (!flag)
							{
								num7 = Main.instance.TilesRenderer.GetWindCycle(x, y, _treeWindCounter);
							}
							vector.X += num7 * 2f;
							vector.Y += Math.Abs(num7) * 2f;
							Color color6 = Lighting.GetColor(x, y);
							if (tile.IsTileFullbright)
							{
								color6 = Color.White;
							}
							Main.spriteBatch.Draw(treeTopTexture, vector, (Rectangle?)new Rectangle(treeFrame * (topTextureFrameWidth3 + 2), 0, topTextureFrameWidth3, topTextureFrameHeight3), color6, num7 * num15, new Vector2((float)(topTextureFrameWidth3 / 2), (float)topTextureFrameHeight3), 1f, (SpriteEffects)0, 0f);
							//Glowmask
							Texture2D treeTopTexture2 = (Texture2D)ModContent.Request<Texture2D>(TexturePath + "_Tops" + "_Glow");
							Main.spriteBatch.Draw(treeTopTexture2, vector, (Rectangle?)new Rectangle(treeFrame * (topTextureFrameWidth3 + 2), 0, topTextureFrameWidth3, topTextureFrameHeight3), Color.White, num7 * num15, new Vector2((float)(topTextureFrameWidth3 / 2), (float)topTextureFrameHeight3), 1f, (SpriteEffects)0, 0f);
							break;
						}
					case 44:
						{
							int num21 = x;
							int num2 = 1;
							if (!GetNightmareTreeFoliageData(x, y, num2, ref treeFrame, out int floorY2, out _, out _))
							{
								return;
							}
							//EmitPetrifiedLeaves(x, y, num21 + num2, floorY2);
							byte tileColor2 = tile.TileColor;
							Texture2D treeBranchTexture2 = GetTreeBranchTexture(Type, 0, tileColor2);
							Vector2 position2 = new Vector2((float)(x * 16) + PositioningFix, (float)(y * 16) + PositioningFix) - unscaledPosition.Floor() + zero + new Vector2(16f, 12f);
							float num4 = 0f;
							if (!flag)
							{
								num4 = Main.instance.TilesRenderer.GetWindCycle(x, y, _treeWindCounter);
							}
							if (num4 > 0f)
							{
								position2.X += num4;
							}
							position2.X += Math.Abs(num4) * 2f;
							Color color4 = Lighting.GetColor(x, y);
							if (tile.IsTileFullbright)
							{
								color4 = Color.White;
							}
							Main.spriteBatch.Draw(treeBranchTexture2, position2, (Rectangle?)new Rectangle(0, treeFrame * 42, 40, 40), color4, num4 * num16, new Vector2(40f, 24f), 1f, (SpriteEffects)0, 0f);
							Texture2D treeBranchTexture3 = (Texture2D)ModContent.Request<Texture2D>(TexturePath + "_Branches" + "_Glow");
							Main.spriteBatch.Draw(treeBranchTexture3, position2, (Rectangle?)new Rectangle(0, treeFrame * 42, 40, 40), Color.White, num4 * num16, new Vector2(40f, 24f), 1f, (SpriteEffects)0, 0f);
							break;
						}
					case 66:
						{
							int num17 = x;
							int num18 = -1;
							if (!GetNightmareTreeFoliageData(x, y, num18, ref treeFrame, out int floorY, out _, out _))
							{
								return;
							}
							//EmitPetrifiedLeaves(x, y, num17 + num18, floorY);
							byte tileColor = tile.TileColor;
							Texture2D treeBranchTexture = GetTreeBranchTexture(Type, 0, tileColor);
							Vector2 position = new Vector2((float)(x * 16) + PositioningFix, (float)(y * 16) + PositioningFix) - unscaledPosition.Floor() + zero + new Vector2(0f, 18f);
							float num20 = 0f;
							if (!flag)
							{
								num20 = Main.instance.TilesRenderer.GetWindCycle(x, y, _treeWindCounter);
							}
							if (num20 < 0f)
							{
								position.X += num20;
							}
							position.X -= Math.Abs(num20) * 2f;
							Color color2 = Lighting.GetColor(x, y);
							if (tile.IsTileFullbright)
							{
								color2 = Color.White;
							}
							Main.spriteBatch.Draw(treeBranchTexture, position, (Rectangle?)new Rectangle(42, treeFrame * 42, 40, 40), color2, num20 * num16, new Vector2(0f, 30f), 1f, (SpriteEffects)0, 0f);
							Texture2D treeBranchTexture1 = (Texture2D)ModContent.Request<Texture2D>(TexturePath + "_Branches" + "_Glow");
							Main.spriteBatch.Draw(treeBranchTexture1, position, (Rectangle?)new Rectangle(42, treeFrame * 42, 40, 40), Color.White, num20 * num16, new Vector2(0f, 30f), 1f, (SpriteEffects)0, 0f);
							break;
						}
				}
			}
		}
	}
}