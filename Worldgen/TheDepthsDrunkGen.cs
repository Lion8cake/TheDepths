using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using TheDepths.Tiles;
using TheDepths.Walls;
using static Terraria.ModLoader.ModContent;
using TheDepths.Tiles.Furniture;

namespace TheDepths.Worldgen
{
    public class TheDepthsDrunkGen : ModSystem 
	{
		#region DrunkGenLeft

		#region RemixShinanigansLeft
		public static void RemixIslandStuffLeft(GenerationProgress progress, GameConfiguration configuration)
		{
			if (WorldGen.remixWorldGen)
			{
				for (int k = Main.maxTilesX / 2; k < Main.maxTilesX; k++)
				{
					for (int l = Main.maxTilesY - 300; l < Main.maxTilesY; l++)
					{
						if (Main.tile[k, l].TileType == ModContent.TileType<ShaleBlock>())
						{
							WorldGen.KillTile(k, l, false, false, false);
							Main.tile[k, l].TileType = (ushort)TileID.Ash;
							Tile tile = Main.tile[k, l];
							tile.HasTile = true;
						}
						else if (Main.tile[k, l].TileType == ModContent.TileType<NightmareGrass>())
						{
							WorldGen.KillTile(k, l, false, false, false);
							Main.tile[k, l].TileType = (ushort)TileID.AshGrass;
							Tile tile = Main.tile[k, l];
							tile.HasTile = true;
						}
					}
				}

				int num854 = (int)((double)Main.maxTilesX * 0.38);
				int num855 = (int)((double)Main.maxTilesX * 0.62);
				int num856 = num854;
				int num857 = Main.maxTilesY - 1;
				int num858 = Main.maxTilesY - 135;
				int num859 = Main.maxTilesY - 160;
				for (int num864 = num854; num864 < num855 + 15; num864++)
				{
					for (int num865 = Main.maxTilesY - 200; num865 < num858 + 20; num865++)
					{
						if (Main.tile[num864, num865].TileType == 633 && Main.tile[num864, num865].HasTile == true && !Main.tile[num864, num865 - 1].HasTile == true && WorldGen.genRand.Next(3) == 0)
						{
							WorldGen.TryGrowingTreeByType(634, num864, num865);
						}
					}
				}
			}
		}
		#endregion

		#region DrunkDropletReplacerLeft
		public static void DrippingQuicksilverTileCleanupLeft(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Placing Dripping Quicksilver";
			for (int k = 0; k < Main.maxTilesX / 2; k++)
			{
				for (int l = 0; l < Main.maxTilesY; l++)
				{
					if (Main.tile[k, l].TileType == TileID.LavaDrip)
					{
						WorldGen.KillTile(k, l);
						Main.tile[k, l].TileType = (ushort)ModContent.TileType<QuicksilverDropletSource>();
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
					}
					if (Main.tile[k, l].WallType == WallID.ObsidianBackUnsafe)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall1>();
					}
					if (Main.tile[k, l].WallType == WallID.LavaUnsafe1 || Main.tile[k, l].WallType == WallID.LavaUnsafe4)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall2>();
					}
					if (Main.tile[k, l].WallType == WallID.LavaUnsafe2)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall3>();
					}
					if (Main.tile[k, l].WallType == WallID.LavaUnsafe3)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall4>();
					}
					if (Main.tile[k, l].TileType == TileID.GeyserTrap)
					{
						WorldGen.KillTile(k, l);
						WorldGen.Place2x1(k, l, (ushort)ModContent.TileType<WaterGeyser>(), 0);
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
					}
				}
			}
		}
		#endregion

		#region DrunkMossLeft
		public static void MossGenLeft(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Growing Sparkly Moss";
			for (int k = 0; k < Main.maxTilesX / 2; k++)
			{
				for (int l = 0; l < Main.maxTilesY; l++)
				{
					if (Main.tile[k, l].TileType == TileID.LavaMoss)
					{
						WorldGen.KillTile(k, l);
						Main.tile[k, l].TileType = (ushort)ModContent.TileType<MercuryMoss>();
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
					}
				}
			}
		}
		#endregion

		#region DrunkPotRemovalLeft
		public static void KILLTHEPOTSLeft(GenerationProgress progress, GameConfiguration configuration)
		{
			//int potsBroken = 0;
			for (int X = 0; X < Main.maxTilesX / 2; X++)
			{
				for (int Y = Main.maxTilesY - 200; Y < Main.maxTilesY; Y++)
				{
					if (Main.tile[X, Y].TileType == TileID.Pots)
					{
						WorldGen.KillTile(X, Y);
						//potsBroken++;
					}
				}
			}
			//ModContent.GetInstance<TheDepths>().Logger.Debug("The Depths, Pots borken counter: " + potsBroken); //debugging
		}
		#endregion

        #region DrunkPotsLeft
        public static void PotsLeft(GenerationProgress progress, GameConfiguration configuration)
		{
			double num444 = (double)((Main.maxTilesX / 2) * Main.maxTilesY) * 0.0008;
			if (Main.starGame)
			{
				num444 *= Main.starGameMath(0.2);
			}
			for (int num445 = 0; (double)num445 < num444; num445++)
			{
				double num446 = (double)num445 / num444;
				progress.Set(num446);
				bool flag25 = false;
				int num447 = 0;
				while (!flag25)
				{
					int num448 = WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, Main.maxTilesY - 10);
					if (num446 > 0.93)
					{
						num448 = Main.maxTilesY - 150;
					}
					else if (num446 > 0.75)
					{
						num448 = (int)GenVars.worldSurfaceLow;
					}
					int num449 = WorldGen.genRand.Next(20, (Main.maxTilesX / 2) - 20);
					bool flag26 = false;
					for (int num450 = num448; num450 < Main.maxTilesY - 20; num450++)
					{
						if (!flag26)
						{
							if (Main.tile[num449, num450].HasTile && Main.tileSolid[Main.tile[num449, num450].TileType] && !(Main.tile[num449, num450 - 1].LiquidType == LiquidID.Lava) && !(Main.tile[num449, num450 - 1].LiquidType == LiquidID.Shimmer))
							{
								flag26 = true;
							}
						}
						else if (!((double)num450 < Main.worldSurface) || Main.tile[num449, num450].WallType != 0)
						{
							int style = WorldGen.genRand.Next(0, 4);
							int type = 28;
							if (num450 > Main.UnderworldLayer)
							{
								style = 0;
								type = ModContent.TileType<DepthsPot>();
							}
							if (!WorldGen.oceanDepths(num449, num450) && !(Main.tile[num449, num450].LiquidType == LiquidID.Shimmer) && WorldGen.PlacePot(num449, num450, (ushort)type, style))
							{
								flag25 = true;
								break;
							}
							num447++;
							if (num447 >= 10000)
							{
								flag25 = true;
								break;
							}
						}
					}
				}
			}
		}
#endregion

        #region DrunkGemforgeLeft
        public static void GemforgeLeft(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Placing gemforges";
			for (int index1 = 0; index1 < (Main.maxTilesX / 200) / 2; ++index1)
            {
                float num2 = index1 / (Main.maxTilesX / 200);
                progress.Set(num2);
                bool flag = false;
                int num3 = 0;
                while (!flag)
                {
                    int i = WorldGen.genRand.Next(1, Main.maxTilesX / 2);
                    int index2 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 30);
                    try
                    {
                        if (Main.tile[i, index2].WallType != WallType<ArqueriteBrickWallUnsafe>())
                        {
                            if (Main.tile[i, index2].WallType != WallType<QuartzBrickWallUnsafe>())
                                continue;
                        }
                        while (!Main.tile[i, index2].HasTile && index2 < Main.maxTilesY - 20)
                            ++index2;
                        int j = index2 - 1;
                        WorldGen.PlaceTile(i, j, TileType<Gemforge>());
                        if (Main.tile[i, j].TileType == TileType<Gemforge>())
                        {
                            flag = true;
                        }
                        else
                        {
                            ++num3;
                            if (num3 >= 10000)
                                flag = true;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
#endregion

        #region DrunkLeft
        public static void DepthsLeft(GenerationProgress progress, GameConfiguration configuration)
		{
			{
				progress.Message = "Creating depths";
				progress.Set(0.0f);
				for (int X999 = 0; X999 < Main.maxTilesX / 2; X999++)
				{
					for (int Y999 = Main.maxTilesY - 300; Y999 < Main.maxTilesY; Y999++)
					{
						if (Main.tile[X999, Y999].TileType == TileID.ObsidianBrick || Main.tile[X999, Y999].TileType == TileID.HellstoneBrick || Main.tile[X999, Y999].TileType == TileID.Beds || Main.tile[X999, Y999].TileType == TileID.Bathtubs || Main.tile[X999, Y999].TileType == TileID.Tables || Main.tile[X999, Y999].TileType == TileID.WorkBenches || Main.tile[X999, Y999].TileType == TileID.Chairs || Main.tile[X999, Y999].TileType == TileID.Platforms || Main.tile[X999, Y999].TileType == TileID.Candelabras || Main.tile[X999, Y999].TileType == TileID.GrandfatherClocks || Main.tile[X999, Y999].TileType == TileID.Pianos || Main.tile[X999, Y999].TileType == TileID.Bookcases || Main.tile[X999, Y999].TileType == TileID.Hellforge || Main.tile[X999, Y999].TileType == TileID.Chandeliers || Main.tile[X999, Y999].TileType == TileID.Torches || Main.tile[X999, Y999].TileType == 89 || Main.tile[X999, Y999].TileType == TileID.Dressers || Main.tile[X999, Y999].TileType == TileID.Candles || Main.tile[X999, Y999].TileType == TileID.Statues || Main.tile[X999, Y999].TileType == TileID.ClosedDoor || Main.tile[X999, Y999].TileType == TileID.OpenDoor || Main.tile[X999, Y999].TileType == TileID.TreeAsh || Main.tile[X999, Y999].TileType == TileID.AshVines || Main.tile[X999, Y999].TileType == TileID.HangingLanterns || Main.tile[X999, Y999].TileType == TileID.Lamps || Main.tile[X999, Y999].TileType == TileID.Banners || Main.tile[X999, Y999].TileType == TileID.Painting2X3 || Main.tile[X999, Y999].TileType == TileID.Painting3X2 || Main.tile[X999, Y999].TileType == TileID.Painting3X3 || Main.tile[X999, Y999].TileType == TileID.Painting4X3 || Main.tile[X999, Y999].TileType == TileID.Painting6X4)
						{
							Tile tile = Main.tile[X999, Y999];
							tile.HasTile = false;
						}
						if (Main.tile[X999, Y999].WallType == WallID.ObsidianBrickUnsafe || Main.tile[X999, Y999].WallType == WallID.HellstoneBrickUnsafe)
						{
							Tile tile = Main.tile[X999, Y999];
							tile.WallType = 0;
						}
						else if (Main.tile[X999, Y999].TileType == TileID.Ash)
						{
							//WorldGen.KillTile(X999, Y999, false, false, false);
							Main.tile[X999, Y999].TileType = (ushort)ModContent.TileType<ShaleBlock>();
							Tile tile = Main.tile[X999, Y999];
							tile.HasTile = true;
						}
						else if (Main.tile[X999, Y999].TileType == TileID.AshGrass)
						{
							//WorldGen.KillTile(X999, Y999, false, false, false);
							Main.tile[X999, Y999].TileType = (ushort)ModContent.TileType<NightmareGrass>();
							Tile tile = Main.tile[X999, Y999];
							tile.HasTile = true;
						}
					}
				}
				int num838 = Main.maxTilesY - WorldGen.genRand.Next(150, 190);
				for (int num839 = 0; num839 < Main.maxTilesX / 2; num839++)
				{
					num838 += WorldGen.genRand.Next(-3, 4);
					if (num838 < Main.maxTilesY - 190)
					{
						num838 = Main.maxTilesY - 190;
					}
					if (num838 > Main.maxTilesY - 160)
					{
						num838 = Main.maxTilesY - 160;
					}
					for (int num840 = num838 - 20 - WorldGen.genRand.Next(3); num840 < Main.maxTilesY; num840++)
					{
						if (num840 >= num838)
						{
							Tile tile = Main.tile[num839, num840];
							tile.HasTile = false;
							tile.WallType = 0;
							tile.LiquidType = 1;
							Main.tile[num839, num840].LiquidAmount = 0;
						}
						else
						{
							Main.tile[num839, num840].TileType = (ushort)ModContent.TileType<ShaleBlock>();
							Main.tile[num839, num840].WallType = 0;
						}
					}
				}
				int num841 = Main.maxTilesY - WorldGen.genRand.Next(40, 70);
				for (int num842 = 10; num842 < Main.maxTilesX / 2 - 10; num842++)
				{
					num841 += WorldGen.genRand.Next(-10, 11);
					if (num841 > Main.maxTilesY - 60)
					{
						num841 = Main.maxTilesY - 60;
					}
					if (num841 < Main.maxTilesY - 100)
					{
						num841 = Main.maxTilesY - 120;
					}
					for (int num843 = num841; num843 < Main.maxTilesY - 10; num843++)
					{
						if (!Main.tile[num842, num843].HasTile)
						{
							Tile tile = Main.tile[num842, num843];
							tile.LiquidType = LiquidID.Lava;
							Main.tile[num842, num843].LiquidAmount = byte.MaxValue;
						}
					}
				}
				for (int num844 = 0; num844 < Main.maxTilesX / 2; num844++)
				{
					if (WorldGen.genRand.Next(50) == 0)
					{
						int num845 = Main.maxTilesY - 65;
						while (!Main.tile[num844, num845].HasTile && num845 > Main.maxTilesY - 135)
						{
							num845--;
						}
						WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX / 2), num845 + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(15, 20), 1000, ModContent.TileType<ShaleBlock>(), addTile: true, 0.0, WorldGen.genRand.Next(1, 3), noYChange: true);
					}
				}
				Liquid.QuickWater(-2);
				for (int num846 = 0; num846 < Main.maxTilesX / 2; num846++)
				{
					double num847 = (double)num846 / (double)(Main.maxTilesX - 1);
					progress.Set(num847 / 2.0 + 0.5);
					if (WorldGen.genRand.Next(13) == 0)
					{
						int num848 = Main.maxTilesY - 65;
						while ((Main.tile[num846, num848].LiquidAmount > 0 || Main.tile[num846, num848].HasTile) && num848 > Main.maxTilesY - 140)
						{
							num848--;
						}
						if ((!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen) || WorldGen.genRand.Next(3) == 0 || !((double)num846 > (double)Main.maxTilesX * 0.4) || !((double)num846 < (double)Main.maxTilesX * 0.6))
						{
							WorldGen.TileRunner(num846, num848 - WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(5, 30), 1000, ModContent.TileType<ShaleBlock>(), addTile: true, 0.0, WorldGen.genRand.Next(1, 3), noYChange: true);
						}
						double num849 = WorldGen.genRand.Next(1, 3);
						if (WorldGen.genRand.Next(3) == 0)
						{
							num849 *= 0.5;
						}
						if ((!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen) || WorldGen.genRand.Next(3) == 0 || !((double)num846 > (double)Main.maxTilesX * 0.4) || !((double)num846 < (double)Main.maxTilesX * 0.6))
						{
							if (WorldGen.genRand.Next(2) == 0)
							{
								WorldGen.TileRunner(num846, num848 - WorldGen.genRand.Next(2, 5), (int)((double)WorldGen.genRand.Next(5, 15) * num849), (int)((double)WorldGen.genRand.Next(10, 15) * num849), ModContent.TileType<ShaleBlock>(), addTile: true, 1.0, 0.3);
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								num849 = WorldGen.genRand.Next(1, 3);
								WorldGen.TileRunner(num846, num848 - WorldGen.genRand.Next(2, 5), (int)((double)WorldGen.genRand.Next(5, 15) * num849), (int)((double)WorldGen.genRand.Next(10, 15) * num849), ModContent.TileType<ShaleBlock>(), addTile: true, -1.0, 0.3);
							}
						}
						WorldGen.TileRunner(num846 + WorldGen.genRand.Next(-10, 10), num848 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(5, 10), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
						if (WorldGen.genRand.Next(3) == 0)
						{
							WorldGen.TileRunner(num846 + WorldGen.genRand.Next(-10, 10), num848 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(10, 30), WorldGen.genRand.Next(10, 20), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
						}
						if (WorldGen.genRand.Next(5) == 0)
						{
							WorldGen.TileRunner(num846 + WorldGen.genRand.Next(-15, 15), num848 + WorldGen.genRand.Next(-15, 10), WorldGen.genRand.Next(15, 30), WorldGen.genRand.Next(5, 20), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
						}
					}
				}
				for (int num850 = 0; num850 < Main.maxTilesX / 2; num850++)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(20, Main.maxTilesX - 20), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 7), -2);
				}
				if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
				{
					for (int num851 = 0; num851 < Main.maxTilesX / 2 * 2; num851++)
					{
						WorldGen.TileRunner(WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.35), (int)((double)Main.maxTilesX * 0.65)), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(5, 20), WorldGen.genRand.Next(5, 10), -2);
					}
				}
				for (int num852 = 0; num852 < Main.maxTilesX / 2; num852++)
				{
					if (!Main.tile[num852, Main.maxTilesY - 145].HasTile)
					{
						Tile tile = Main.tile[num852, Main.maxTilesY - 145];
						Main.tile[num852, Main.maxTilesY - 145].LiquidAmount = byte.MaxValue;
						tile.LiquidType = LiquidID.Lava;
					}
					if (!Main.tile[num852, Main.maxTilesY - 144].HasTile)
					{
						Tile tile = Main.tile[num852, Main.maxTilesY - 144];
						Main.tile[num852, Main.maxTilesY - 144].LiquidAmount = byte.MaxValue;
						tile.LiquidType = 1;
					}
				}
				for (int num853 = 0; num853 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008) / 2; num853++)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX / 2), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), TileType<ArqueriteOre>());
				}
				for (int index = 0; index < (int)(Main.maxTilesX * Main.maxTilesY * 0.0008) / 2; ++index)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX / 2), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(5, 9), TileType<Shalestone>(), false, 0.0f, 0.0f, false, true);
				}
				if (!WorldGen.remixWorldGen)
				{
					Gems();
				}
				AddQuartzApartmentsLeft();
				}

				void Gems()
				{
					for (int type = 63; type <= 68; ++type)
					{
						int tile = 0;
						float num1 = 0.0f;
						if (type == 67)
						{
							num1 = Main.maxTilesX * 0.5f;
							tile = TileType<ShalestoneAmethyst>();
						}
						else if (type == 66)
						{
							num1 = Main.maxTilesX * 0.45f;
							tile = TileType<ShalestoneTopaz>();
						}
						else if (type == 63)
						{
							num1 = Main.maxTilesX * 0.3f;
							tile = TileType<ShalestoneSapphire>();
						}
						else if (type == 65)
						{
							num1 = Main.maxTilesX * 0.25f;
							tile = TileType<ShalestoneEmerald>();
						}
						else if (type == 64)
						{
							num1 = Main.maxTilesX * 0.1f;
							tile = TileType<ShalestoneRuby>();
						}
						else if (type == 68)
						{
							num1 = Main.maxTilesX * 0.05f;
							tile = TileType<ShalestoneDiamond>();
						}
						float num2 = num1 * 0.2f;
						for (int index = 0; index < num2; ++index)
						{
							int i = WorldGen.genRand.Next(0, Main.maxTilesX / 2);
							int j;
							for (j = WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY); Main.tile[i, j].TileType != 1; j = WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY))
								i = WorldGen.genRand.Next(0, Main.maxTilesX / 2);
							WorldGen.TileRunner(i, j, WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(3, 7), tile, false, 0.0f, 0.0f, false, true);
						}
					}
					for (int index1 = 0; index1 < 2; ++index1)
					{
						int num1 = 1;
						int num2 = 5;
						int num3 = Main.maxTilesX - 5;
						if (index1 == 1)
						{
							num1 = -1;
							num2 = Main.maxTilesX - 5;
							num3 = 5;
						}
						int index2 = num2;
						while (index2 != num3)
						{
							for (int index3 = 10; index3 < Main.maxTilesY - 10; ++index3)
							{
								if (Main.tile[index2, index3].HasTile && Main.tile[index2, index3 + 1].HasTile && Main.tileSand[Main.tile[index2, index3].TileType] && Main.tileSand[Main.tile[index2, index3 + 1].TileType])
								{
									ushort type = Main.tile[index2, index3].TileType;
									int index4 = index2 + num1;
									int index5 = index3 + 1;
									if (!Main.tile[index4, index3].HasTile && !Main.tile[index4, index3 + 1].HasTile)
									{
										while (!Main.tile[index4, index5].HasTile)
											++index5;
										int index6 = index5 - 1;
										Main.tile[index4, index6].TileType = type;
									}
								}
							}
							index2 += num1;
						}
					}
				}
			}
		#endregion

		#region RemixIslandLeft
		public static void RemixIslandLeft(GenerationProgress progress, GameConfiguration configuration)
		{
			if (WorldGen.remixWorldGen)
			{
				int num854 = (int)((double)Main.maxTilesX * 0.38);
				int num855 = (int)((double)Main.maxTilesX * 0.62);
				int num856 = num854;
				int num857 = Main.maxTilesY - 1;
				int num858 = Main.maxTilesY - 135;
				int num859 = Main.maxTilesY - 160;
				bool flag55 = false;
				for (; num857 < Main.maxTilesY - 1 || num856 < num855; num856++)
				{
					if (!flag55)
					{
						num857 -= WorldGen.genRand.Next(1, 4);
						if (num857 < num858)
						{
							flag55 = true;
						}
					}
					else if (num856 >= num855)
					{
						num857 += WorldGen.genRand.Next(1, 4);
						if (num857 > Main.maxTilesY - 1)
						{
							num857 = Main.maxTilesY - 1;
						}
					}
					else
					{
						if ((num856 <= Main.maxTilesX / 2 - 5 || num856 >= Main.maxTilesX / 2 + 5) && WorldGen.genRand.Next(4) == 0)
						{
							if (WorldGen.genRand.Next(3) == 0)
							{
								num857 += WorldGen.genRand.Next(-1, 2);
							}
							else if (WorldGen.genRand.Next(6) == 0)
							{
								num857 += WorldGen.genRand.Next(-2, 3);
							}
							else if (WorldGen.genRand.Next(8) == 0)
							{
								num857 += WorldGen.genRand.Next(-4, 5);
							}
						}
						if (num857 < num859)
						{
							num857 = num859;
						}
						if (num857 > num858)
						{
							num857 = num858;
						}
					}
					for (int num860 = num857; num860 > num857 - 20; num860--)
					{
						Main.tile[num856, num860].LiquidAmount = 0;
					}
					for (int num861 = num857; num861 < Main.maxTilesY; num861++)
					{
						Tile tile = Main.tile[num856, num861];
						tile.HasTile = true;
						Main.tile[num856, num861].TileType = (ushort)ModContent.TileType<ShaleBlock>();
					}
				}
			}
		}
		#endregion

		#region NightmareGroveLeft
		public static void NightmareGroveLeft(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Growing bioluminecent plants in very dark areas";
			//Proper Grove Gen
			if (WorldGen.remixWorldGen)
			{
				int num854 = (int)((double)Main.maxTilesX * 0.38);
				int num855 = (int)((double)(Main.maxTilesX / 2) * 0.62);
				int num856 = num854;
				int num857 = Main.maxTilesY - 1;
				int num858 = Main.maxTilesY - 135;
				int num859 = Main.maxTilesY - 160;
				bool flag55 = false;
				Liquid.QuickWater(-2);
				for (int num862 = num854; num862 < num855 + 15; num862++)
				{
					for (int num863 = Main.maxTilesY - 300; num863 < num858 + 20; num863++)
					{
						Main.tile[num862, num863].LiquidAmount = 0;
						if ((Main.tile[num862, num863].TileType == ModContent.TileType<ShaleBlock>() || Main.tile[num862, num863].TileType == ModContent.TileType<Shalestone>()) && Main.tile[num862, num863].HasTile && (!Main.tile[num862 - 1, num863 - 1].HasTile || !Main.tile[num862, num863 - 1].HasTile || !Main.tile[num862 + 1, num863 - 1].HasTile || !Main.tile[num862 - 1, num863].HasTile || !Main.tile[num862 + 1, num863].HasTile || !Main.tile[num862 - 1, num863 + 1].HasTile || !Main.tile[num862, num863 + 1].HasTile || !Main.tile[num862 + 1, num863 + 1].HasTile))
						{
							Main.tile[num862, num863].TileType = (ushort)ModContent.TileType<NightmareGrass>();
						}
					}
				}
				for (int num864 = num854; num864 < num855 + 15; num864++)
				{
					for (int num865 = Main.maxTilesY - 200; num865 < num858 + 20; num865++)
					{
						if (Main.tile[num864, num865].TileType == ModContent.TileType<NightmareGrass>() && Main.tile[num864, num865].HasTile && !Main.tile[num864, num865 - 1].HasTile && WorldGen.genRand.Next(3) == 0)
						{
							WorldGen.GrowTree(num864, num865);
						}
					}
				}
			}
			else if (!WorldGen.drunkWorldGen)
			{
				for (int num866 = 25; num866 < (Main.maxTilesX / 2) - 25; num866++)
				{
					if ((double)num866 < (double)(Main.maxTilesX / 2) * 0.17 || (double)num866 > (double)(Main.maxTilesX / 2) * 0.83)
					{
						for (int num867 = Main.maxTilesY - 300; num867 < Main.maxTilesY - 100 + WorldGen.genRand.Next(-1, 2); num867++)
						{
							if ((Main.tile[num866, num867].TileType == ModContent.TileType<ShaleBlock>() || Main.tile[num866, num867].TileType == ModContent.TileType<Shalestone>()) && Main.tile[num866, num867].HasTile && (!Main.tile[num866 - 1, num867 - 1].HasTile || !Main.tile[num866, num867 - 1].HasTile || !Main.tile[num866 + 1, num867 - 1].HasTile || !Main.tile[num866 - 1, num867].HasTile || !Main.tile[num866 + 1, num867].HasTile || !Main.tile[num866 - 1, num867 + 1].HasTile || !Main.tile[num866, num867 + 1].HasTile || !Main.tile[num866 + 1, num867 + 1].HasTile))
							{
								Main.tile[num866, num867].TileType = (ushort)ModContent.TileType<NightmareGrass>();
							}
						}
					}
				}
				for (int num868 = 25; num868 < (Main.maxTilesX / 2) - 25; num868++)
				{
					if ((double)num868 < (double)(Main.maxTilesX / 2) * 0.17 || (double)num868 > (double)(Main.maxTilesX / 2) * 0.83)
					{
						for (int num869 = Main.maxTilesY - 200; num869 < Main.maxTilesY - 50; num869++)
						{
							if (Main.tile[num868, num869].TileType == ModContent.TileType<NightmareGrass>() && Main.tile[num868, num869].HasTile && !Main.tile[num868, num869 - 1].HasTile && WorldGen.genRand.Next(3) == 0)
							{
								WorldGen.GrowTree(num868, num869);
							}
						}
					}
				}
			}
			else
			{
				for (int num866 = 25; num866 < (Main.maxTilesX / 2) - 25; num866++)
				{
					for (int num867 = Main.maxTilesY - 300; num867 < Main.maxTilesY - 100 + WorldGen.genRand.Next(-1, 2); num867++)
					{
						if ((Main.tile[num866, num867].TileType == ModContent.TileType<ShaleBlock>() || Main.tile[num866, num867].TileType == ModContent.TileType<Shalestone>()) && Main.tile[num866, num867].HasTile && (!Main.tile[num866 - 1, num867 - 1].HasTile || !Main.tile[num866, num867 - 1].HasTile || !Main.tile[num866 + 1, num867 - 1].HasTile || !Main.tile[num866 - 1, num867].HasTile || !Main.tile[num866 + 1, num867].HasTile || !Main.tile[num866 - 1, num867 + 1].HasTile || !Main.tile[num866, num867 + 1].HasTile || !Main.tile[num866 + 1, num867 + 1].HasTile))
						{
							Main.tile[num866, num867].TileType = (ushort)ModContent.TileType<NightmareGrass>();
						}
					}
				}
				for (int num868 = 25; num868 < (Main.maxTilesX / 2) - 25; num868++)
				{
					for (int num869 = Main.maxTilesY - 200; num869 < Main.maxTilesY - 50; num869++)
					{
						if (Main.tile[num868, num869].TileType == ModContent.TileType<NightmareGrass>() && Main.tile[num868, num869].HasTile && !Main.tile[num868, num869 - 1].HasTile && WorldGen.genRand.Next(3) == 0)
						{
							WorldGen.GrowTree(num868, num869);
						}
					}
				}
			}

			//Vines
			for (int num233 = 5; num233 < (Main.maxTilesX / 2) - 5; num233++)
			{
				int num234 = 0;
				for (int num253 = 0; num253 < Main.maxTilesY; num253++)
				{
					if (num234 > 0 && !Main.tile[num233, num253].HasTile)
					{
						Tile tile = Main.tile[num233, num253];
						tile.HasTile = true;
						Main.tile[num233, num253].TileType = (ushort)ModContent.TileType<NightmareVines>();
						num234--;
					}
					else
					{
						num234 = 0;
					}
					if (Main.tile[num233, num253].HasTile && !Main.tile[num233, num253].BottomSlope && Main.tile[num233, num253].TileType == ModContent.TileType<NightmareGrass>() && TheDepthsWorldGen.GrowMoreVines(num233, num253) && WorldGen.genRand.Next(5) < 3)
					{
						num234 = WorldGen.genRand.Next(1, 10);
					}
				}
			}

			//Foliage
			for (int num263 = 0; num263 < (Main.maxTilesX / 2); num263++)
			{
				progress.Set((double)num263 / (double)(Main.maxTilesX / 2));
				for (int num264 = 1; num264 < Main.maxTilesY; num264++)
				{
					if (Main.tile[num263, num264].TileType == ModContent.TileType<NightmareGrass>() && Main.tile[num263, num264].HasUnactuatedTile && !Main.tile[num263, num264 - 1].HasTile)
					{
						WorldGen.PlaceTile(num263, num264 - 1, ModContent.TileType<NightmareGrass_Foliage>(), mute: true);
					}
				}
			}
		}
		#endregion

		#region DrunkHousesLeft
		public static void AddQuartzApartmentsLeft()
		{
			int num = (int)((double)(Main.maxTilesX) * 0.25);
			for (int i = 100; i < (Main.maxTilesX / 2) - 100; i++)
			{
				if (((WorldGen.drunkWorldGen || WorldGen.remixWorldGen) && i > num && i < (Main.maxTilesX) - num) || (!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen && (i < num || i > (Main.maxTilesX) - num)))
				{
					continue;
				}
				int num2 = Main.maxTilesY - 40;
				while (Main.tile[i, num2].HasTile || Main.tile[i, num2].LiquidAmount > 0)
				{
					num2--;
				}
				if (Main.tile[i, num2 + 1].HasTile)
				{
					ushort num3 = (ushort)WorldGen.genRand.Next(75, 77);
					ushort wallType = (ushort)WallType<QuartzBrickWallUnsafe>();
					if (num3 == 75)
					{
						num3 = (ushort)TileType<ArqueriteBricks>();
					}
					if (num3 == 76)
					{
						num3 = (ushort)TileType<QuartzBricks>();
					}
					if (num3 == TileType<QuartzBricks>())
					{
						wallType = (ushort)WallType<QuartzBrickWallUnsafe>();
					}
					if (WorldGen.getGoodWorldGen)
					{
						num3 = (ushort)TileType<ArqueriteBricks>();
					}
					if (num3 == (ushort)TileType<ArqueriteBricks>())
					{
						wallType = (ushort)WallType<ArqueriteBrickWallUnsafe>();
					}
					ArqueriteCastleLeft(i, num2, num3, wallType);
					i += WorldGen.genRand.Next(30, 130);
					if (WorldGen.genRand.Next(10) == 0)
					{
						i += WorldGen.genRand.Next(0, 200);
					}
				}
			}
			float num4 = (Main.maxTilesX) / 4200;
			for (int j = 0; (float)j < 200f * num4; j++)
			{
				int num5 = 0;
				bool flag = false;
				while (!flag)
				{
					num5++;
					int num6 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.2), (int)((double)Main.maxTilesX * 0.8));
					int num7 = WorldGen.genRand.Next(Main.maxTilesY - 300, Main.maxTilesY - 20);
					if (Main.tile[num6, num7].HasTile && (Main.tile[num6, num7].TileType == ModContent.TileType<QuartzBricks>() || Main.tile[num6, num7].TileType == ModContent.TileType<ArqueriteBricks>()))
					{
						int num8 = 0;
						if (Main.tile[num6 - 1, num7].WallType > 0)
						{
							num8 = -1;
						}
						else if (Main.tile[num6 + 1, num7].WallType > 0)
						{
							num8 = 1;
						}
						if (!Main.tile[num6 + num8, num7].HasTile && !Main.tile[num6 + num8, num7 + 1].HasTile)
						{
							bool flag2 = false;
							for (int k = num6 - 8; k < (num6 / 2) + 8; k++)
							{
								for (int l = num7 - 8; l < num7 + 8; l++)
								{
									if (Main.tile[k, l].HasTile && Main.tile[k, l].TileType == ModContent.TileType<GeoTorch>())
									{
										flag2 = true;
										break;
									}
								}
							}
							if (!flag2)
							{
								WorldGen.PlaceTile(num6 + num8, num7, ModContent.TileType<GeoTorch>(), mute: true, forced: true, -1);
								flag = true;
							}
						}
					}
					if (num5 > 1000)
					{
						flag = true;
					}
				}
			}
			double num9 = 4200000.0 / (double)(Main.maxTilesX / 2);
			for (int m = 0; (double)m < num9; m++)
			{
				int num10 = 0;
				int num11 = WorldGen.genRand.Next(num, (Main.maxTilesX / 2) - num);
				int n = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
				while ((Main.tile[num11, n].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num11, n].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>()) || Main.tile[num11, n].HasTile)
				{
					num11 = WorldGen.genRand.Next(num, (Main.maxTilesX / 2) - num);
					n = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
					if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
					{
						num11 = ((WorldGen.genRand.Next(2) != 0) ? WorldGen.genRand.Next((Main.maxTilesX / 2) - num, (Main.maxTilesX / 2) - 50) : WorldGen.genRand.Next(50, num));
					}
					num10++;
					if (num10 > 100000)
					{
						break;
					}
				}
				if (num10 > 100000 || (Main.tile[num11, n].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num11, n].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>()) || Main.tile[num11, n].HasTile)
				{
					continue;
				}
				for (; !WorldGen.SolidTile(num11, n) && n < Main.maxTilesY - 20; n++)
				{
				}
				n--;
				int num12 = num11;
				int num13 = num11;
				while (!Main.tile[num12, n].HasTile && WorldGen.SolidTile(num12, n + 1))
				{
					num12--;
				}
				num12++;
				for (; !Main.tile[num13, n].HasTile && WorldGen.SolidTile(num13, n + 1); num13++)
				{
				}
				num13--;
				int num14 = num13 - num12;
				int num15 = (num13 + num12) / 2;
				if (Main.tile[num15, n].HasTile || (Main.tile[num15, n].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num15, n].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>()) || !WorldGen.SolidTile(num15, n + 1))
				{
					continue;
				}
				int num16 = WorldGen.genRand.Next(13);
				int num17 = 0;
				int num18 = 0;
				if (num16 == 0)
				{
					num17 = 5;
					num18 = 4;
				}
				if (num16 == 1)
				{
					num17 = 4;
					num18 = 3;
				}
				if (num16 == 2)
				{
					num17 = 3;
					num18 = 5;
				}
				if (num16 == 3)
				{
					num17 = 4;
					num18 = 6;
				}
				if (num16 == 4)
				{
					num17 = 3;
					num18 = 3;
				}
				if (num16 == 5)
				{
					num17 = 5;
					num18 = 3;
				}
				if (num16 == 6)
				{
					num17 = 5;
					num18 = 4;
				}
				if (num16 == 7)
				{
					num17 = 5;
					num18 = 4;
				}
				if (num16 == 8)
				{
					num17 = 5;
					num18 = 4;
				}
				if (num16 == 9)
				{
					num17 = 3;
					num18 = 5;
				}
				if (num16 == 10)
				{
					num17 = 5;
					num18 = 3;
				}
				if (num16 == 11)
				{
					num17 = 2;
					num18 = 4;
				}
				if (num16 == 12)
				{
					num17 = 3;
					num18 = 3;
				}
				for (int num19 = num15 - num17; num19 <= num15 + num17; num19++)
				{
					for (int num20 = n - num18; num20 <= n; num20++)
					{
						if (Main.tile[num19, num20].HasTile)
						{
							num16 = -1;
							break;
						}
					}
				}
				if ((double)num14 < (double)num17 * 1.75)
				{
					num16 = -1;
				}
				switch (num16)
				{
					case 0:
						{
							WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzTable>(), mute: true, forced: false, -1);
							int num22 = WorldGen.genRand.Next(6);
							if (num22 < 3)
							{
								WorldGen.PlaceTile(num15 + num22, n - 2, ModContent.TileType<QuartzCandle>(), mute: true, forced: false, -1);
							}
							if (!Main.tile[num15, n].HasTile)
							{
								break;
							}
							if (!Main.tile[num15 - 2, n].HasTile)
							{
								WorldGen.PlaceTile(num15 - 2, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
								if (Main.tile[num15 - 2, n].HasTile)
								{
									Main.tile[num15 - 2, n].TileFrameX += 18;
									Main.tile[num15 - 2, n - 1].TileFrameX += 18;
								}
							}
							if (!Main.tile[num15 + 2, n].HasTile)
							{
								WorldGen.PlaceTile(num15 + 2, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
							}
							break;
						}
					case 1:
						{
							WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzWorkbench>(), mute: true, forced: false, -1);
							int num21 = WorldGen.genRand.Next(4);
							if (num21 < 2)
							{
								WorldGen.PlaceTile(num15 + num21, n - 1, ModContent.TileType<QuartzCandle>(), mute: true, forced: false, -1);
							}
							if (!Main.tile[num15, n].HasTile)
							{
								break;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								if (!Main.tile[num15 - 1, n].HasTile)
								{
									WorldGen.PlaceTile(num15 - 1, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
									if (Main.tile[num15 - 1, n].HasTile)
									{
										Main.tile[num15 - 1, n].TileFrameX += 18;
										Main.tile[num15 - 1, n - 1].TileFrameX += 18;
									}
								}
							}
							else if (!Main.tile[num15 + 2, n].HasTile)
							{
								WorldGen.PlaceTile(num15 + 2, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
							}
							break;
						}
					case 2:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzVase>(), mute: true, forced: false, -1);
						break;
					case 3:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzBookcase>(), mute: true, forced: false, -1);
						break;
					case 4:
						if (WorldGen.genRand.Next(2) == 0)
						{
							WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
							Main.tile[num15, n].TileFrameX += 18;
							Main.tile[num15, n - 1].TileFrameX += 18;
						}
						else
						{
							WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
						}
						break;
					case 5:
						if (WorldGen.genRand.Next(2) == 0)
						{
							WorldGen.Place4x2(num15, n, (ushort)ModContent.TileType<QuartzBed>(), 1);
						}
						else
						{
							WorldGen.Place4x2(num15, n, (ushort)ModContent.TileType<QuartzBed>(), -1);
						}
						break;
					case 6:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzPiano>(), mute: true, forced: false, -1);
						break;
					case 7:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzDresser>(), mute: true, forced: false, -1);
						break;
					case 8:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzSofa>(), mute: true, forced: false, -1);
						break;
					case 9:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzClock>(), mute: true, forced: false, -1);
						break;
					case 10:
						if (WorldGen.genRand.Next(2) == 0)
						{
							WorldGen.Place4x2(num15, n, (ushort)ModContent.TileType<QuartzBath>(), 1);
						}
						else
						{
							WorldGen.Place4x2(num15, n, (ushort)ModContent.TileType<QuartzBath>(), -1);
						}
						break;
					case 11:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzLamp>(), mute: true, forced: false, -1);
						break;
					case 12:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzCandelabra>(), mute: true, forced: false, -1);
						break;
				}
			}
			num9 = 420000.0 / (double)(Main.maxTilesX / 2);
			for (int num23 = 0; (double)num23 < num9; num23++)
			{
				int num24 = 0;
				int num25 = WorldGen.genRand.Next(num, (Main.maxTilesX / 2) - num);
				int num26 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
				while ((Main.tile[num25, num26].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num25, num26].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>()) || Main.tile[num25, num26].HasTile)
				{
					num25 = WorldGen.genRand.Next(num, (Main.maxTilesX / 2) - num);
					num26 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
					if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
					{
						num25 = ((WorldGen.genRand.Next(2) != 0) ? WorldGen.genRand.Next((Main.maxTilesX / 2) - num, (Main.maxTilesX / 2) - 50) : WorldGen.genRand.Next(50, num));
					}
					num24++;
					if (num24 > 100000)
					{
						break;
					}
				}
				if (num24 > 100000)
				{
					continue;
				}
				int num27 = num25;
				int num28 = num25;
				int num29 = num26;
				int num30 = num26;
				int num31 = 0;
				for (int num32 = 0; num32 < 2; num32++)
				{
					num27 = num25;
					num28 = num25;
					while (!Main.tile[num27, num26].HasTile && (Main.tile[num27, num26].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[num27, num26].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>()))
					{
						num27--;
					}
					num27++;
					for (; !Main.tile[num28, num26].HasTile && (Main.tile[num28, num26].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[num28, num26].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>()); num28++)
					{
					}
					num28--;
					num25 = (num27 + num28) / 2;
					num29 = num26;
					num30 = num26;
					while (!Main.tile[num25, num29].HasTile && (Main.tile[num25, num29].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[num25, num29].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>()))
					{
						num29--;
					}
					num29++;
					for (; !Main.tile[num25, num30].HasTile && (Main.tile[num25, num30].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[num25, num30].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>()); num30++)
					{
					}
					num30--;
					num26 = (num29 + num30) / 2;
				}
				num27 = num25;
				num28 = num25;
				while (!Main.tile[num27, num26].HasTile && !Main.tile[num27, num26 - 1].HasTile && !Main.tile[num27, num26 + 1].HasTile)
				{
					num27--;
				}
				num27++;
				for (; !Main.tile[num28, num26].HasTile && !Main.tile[num28, num26 - 1].HasTile && !Main.tile[num28, num26 + 1].HasTile; num28++)
				{
				}
				num28--;
				num29 = num26;
				num30 = num26;
				while (!Main.tile[num25, num29].HasTile && !Main.tile[num25 - 1, num29].HasTile && !Main.tile[num25 + 1, num29].HasTile)
				{
					num29--;
				}
				num29++;
				for (; !Main.tile[num25, num30].HasTile && !Main.tile[num25 - 1, num30].HasTile && !Main.tile[num25 + 1, num30].HasTile; num30++)
				{
				}
				num30--;
				num25 = (num27 + num28) / 2;
				num26 = (num29 + num30) / 2;
				int num33 = num28 - num27;
				num31 = num30 - num29;
				if (num33 <= 7 || num31 <= 5)
				{
					continue;
				}
				int num34 = 0;
				if (TheDepthsWorldGen.nearDepthsPainting(num25, num26))
				{
					num34 = -1;
				}
				if (num34 == 0)
				{
					PaintingEntry paintingEntry = TheDepthsWorldGen.RandDarknessPicture();
					if (!WorldGen.nearPicture(num25, num26))
					{
						WorldGen.PlaceTile(num25, num26, paintingEntry.tileType, mute: true, forced: false, -1, paintingEntry.style);
					}
				}
			}
			num9 = 420000.0 / (double)(Main.maxTilesX / 2);
			for (int num35 = 0; (double)num35 < num9; num35++)
			{
				int num36 = 0;
				int num37;
				int num38;
				do
				{
					num37 = WorldGen.genRand.Next(num, (Main.maxTilesX / 2) - num);
					num38 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
					if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
					{
						num37 = ((WorldGen.genRand.Next(2) != 0) ? WorldGen.genRand.Next((Main.maxTilesX / 2) - num, (Main.maxTilesX / 2) - 50) : WorldGen.genRand.Next(50, num));
					}
					num36++;
				}
				while (num36 <= 100000 && ((Main.tile[num37, num38].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num37, num38].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>()) || Main.tile[num37, num38].HasTile));
				if (num36 > 100000)
				{
					continue;
				}
				while (!WorldGen.SolidTile(num37, num38) && num38 > 10)
				{
					num38--;
				}
				num38++;
				if (Main.tile[num37, num38].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num37, num38].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>())
				{
					continue;
				}
				int num39 = WorldGen.genRand.Next(3);
				int num40;
				int num41;
				switch (num39)
				{
					default:
						num40 = 1;
						num41 = 3;
						break;
					case 1:
						num40 = 3;
						num41 = 3;
						break;
					case 2:
						num40 = 1;
						num41 = 2;
						break;
				}
				for (int num42 = num37 - 1; num42 <= num37 + num40; num42++)
				{
					for (int num43 = num38; num43 <= num38 + num41; num43++)
					{
						Tile tile = Main.tile[num37, num38];
						if (num42 < num37 || num42 == num37 + num40)
						{
							if (tile.HasTile)
							{
								/*switch (tile.TileType)
								{
									case TileType<QuartzDoorClosed>():
									case 11:
									case 34:
									case 42:
									case 91:
										num39 = -1;
										break;
								}*/
							}
						}
						else if (tile.HasTile)
						{
							num39 = -1;
						}
					}
				}
				switch (num39)
				{
					case 0:
						WorldGen.PlaceTile(num37, num38, ModContent.TileType<DepthsVanityBanners>(), mute: true, forced: false, -1, WorldGen.genRand.Next(6));
						break;
					case 1:
						WorldGen.PlaceTile(num37, num38, ModContent.TileType<QuartzChandelier>(), mute: true, forced: false, -1);
						break;
					case 2:
						WorldGen.PlaceTile(num37, num38, ModContent.TileType<QuartzLantern>(), mute: true, forced: false, -1);
						break;
				}
			}
		}

		public static void ArqueriteCastleLeft(int i, int j, ushort tileType = 75, ushort wallType = 14)
		{
			int[] array = new int[5];
			int[] array2 = new int[5];
			int[] array3 = new int[10];
			int[] array4 = new int[10];
			int num = 8;
			int num2 = 20;
			if (WorldGen.drunkWorldGen)
			{
				num /= 2;
				num2 *= 2;
			}
			array[2] = i - WorldGen.genRand.Next(num / 2, num2 / 2);
			array2[2] = i + WorldGen.genRand.Next(num / 2, num2 / 2);
			array[3] = array2[2];
			array2[3] = array[3] + WorldGen.genRand.Next(num, num2);
			array[4] = array2[3];
			array2[4] = array[4] + WorldGen.genRand.Next(num, num2);
			array2[1] = array[2];
			array[1] = array2[1] - WorldGen.genRand.Next(num, num2);
			array2[0] = array[1];
			array[0] = array2[0] - WorldGen.genRand.Next(num, num2);
			num = 6;
			num2 = 12;
			array3[3] = j - WorldGen.genRand.Next(num, num2);
			array4[3] = j;
			for (int k = 4; k < 10; k++)
			{
				array3[k] = array4[k - 1];
				array4[k] = array3[k] + WorldGen.genRand.Next(num, num2);
			}
			for (int num3 = 2; num3 >= 0; num3--)
			{
				array4[num3] = array3[num3 + 1];
				array3[num3] = array4[num3] - WorldGen.genRand.Next(num, num2);
			}
			bool flag = false;
			bool flag2 = false;
			bool[,] array5 = new bool[5, 10];
			int num4 = 3;
			int num5 = 3;
			for (int l = 0; l < 2; l++)
			{
				if (WorldGen.genRand.Next(3) == 0 || WorldGen.drunkWorldGen)
				{
					flag = true;
					int num6 = WorldGen.genRand.Next(10);
					if (num6 < num4)
					{
						num4 = num6;
					}
					if (num6 > num5)
					{
						num5 = num6;
					}
					int num7 = 1;
					if (WorldGen.genRand.Next(2) == 0 || WorldGen.drunkWorldGen)
					{
						array5[0, num6] = true;
						array5[1, num6] = true;
						num7 = 0;
					}
					else
					{
						array5[1, num6] = true;
					}
					int num8 = WorldGen.genRand.Next(2);
					if (num8 == 0)
					{
						num8 = -1;
					}
					int num9 = WorldGen.genRand.Next(10);
					while (num9 > 0 && num6 >= 0 && num6 < 10)
					{
						array5[num7, num6] = true;
						num6 += num8;
					}
				}
				if (WorldGen.genRand.Next(3) == 0 || WorldGen.drunkWorldGen)
				{
					flag2 = true;
					int num10 = WorldGen.genRand.Next(10);
					if (num10 < num4)
					{
						num4 = num10;
					}
					if (num10 > num5)
					{
						num5 = num10;
					}
					int num11 = 3;
					if (WorldGen.genRand.Next(2) == 0 || WorldGen.drunkWorldGen)
					{
						array5[3, num10] = true;
						array5[4, num10] = true;
						num11 = 4;
					}
					else
					{
						array5[3, num10] = true;
					}
					int num12 = WorldGen.genRand.Next(2);
					if (num12 == 0)
					{
						num12 = -1;
					}
					int num13 = WorldGen.genRand.Next(10);
					while (num13 > 0 && num10 >= 0 && num10 < 10)
					{
						array5[num11, num10] = true;
						num10 += num12;
					}
				}
			}
			for (int m = 0; m < 5; m++)
			{
				int num14 = array[m];
				bool flag3 = false;
				if (num14 < 10 || num14 > (Main.maxTilesX / 2) - 10)
				{
					flag3 = true;
				}
				else
				{
					for (int n = Main.UnderworldLayer; n < Main.maxTilesY; n++)
					{
						if (Main.tile[num14, n].WallType > 0)
						{
							flag3 = true;
						}
					}
				}
				if (flag3)
				{
					for (int num15 = 0; num15 < 10; num15++)
					{
						array5[m, num15] = false;
					}
				}
			}
			int num16 = WorldGen.genRand.Next(10);
			if (num16 < num4)
			{
				num4 = num16;
			}
			num16 = WorldGen.genRand.Next(10);
			if (num16 > num5)
			{
				num5 = num16;
			}
			if (!flag && !flag2)
			{
				while (num5 - num4 < 5)
				{
					num16 = WorldGen.genRand.Next(10);
					if (num16 < num4)
					{
						num4 = num16;
					}
					num16 = WorldGen.genRand.Next(10);
					if (num16 > num5)
					{
						num5 = num16;
					}
				}
			}
			for (int num17 = num4; num17 <= num5; num17++)
			{
				array5[2, num17] = true;
			}
			for (int num18 = 0; num18 < 5; num18++)
			{
				for (int num19 = 0; num19 < 10; num19++)
				{
					if (array5[num18, num19] && (array3[num19] < Main.UnderworldLayer || array4[num19] > Main.maxTilesY - 20))
					{
						array5[num18, num19] = false;
					}
				}
			}
			for (int num20 = 0; num20 < 5; num20++)
			{
				for (int num21 = 0; num21 < 10; num21++)
				{
					if (!array5[num20, num21])
					{
						continue;
					}
					for (int num22 = array[num20]; num22 <= array2[num20]; num22++)
					{
						for (int num23 = array3[num21]; num23 <= array4[num21]; num23++)
						{
							if (num22 < 10)
							{
								break;
							}
							if (num22 > (Main.maxTilesX / 2) - 10)
							{
								break;
							}
							Main.tile[num22, num23].LiquidAmount = 0;
							if (num22 == array[num20] || num22 == array2[num20] || num23 == array3[num21] || num23 == array4[num21])
							{
								Tile tile = Main.tile[num22, num23];
								tile.HasTile = true;
								tile.TileType = tileType;
								tile.IsHalfBlock = false;
								tile.Slope = 0;
							}
							else
							{
								Tile tile = Main.tile[num22, num23];
								tile.WallType = wallType;
								tile.HasTile = false;
							}
						}
					}
				}
			}
			for (int num24 = 0; num24 < 4; num24++)
			{
				bool[] array6 = new bool[10];
				bool flag4 = false;
				for (int num25 = 0; num25 < 10; num25++)
				{
					if (array5[num24, num25] && array5[num24 + 1, num25])
					{
						array6[num25] = true;
						flag4 = true;
					}
				}
				while (flag4)
				{
					int num26 = WorldGen.genRand.Next(10);
					if (array6[num26])
					{
						flag4 = false;
						Tile tile1 = Main.tile[array2[num24], array4[num26] - 1];
						tile1.HasTile = false;
						Tile tile2 = Main.tile[array2[num24], array4[num26] - 2];
						tile2.HasTile = false;
						Tile tile3 = Main.tile[array2[num24], array4[num26] - 3];
						tile3.HasTile = false;
						Main.tile[array2[num24], array4[num26] - 1].WallType = wallType;
						Main.tile[array2[num24], array4[num26] - 2].WallType = wallType;
						Main.tile[array2[num24], array4[num26] - 3].WallType = wallType;
						WorldGen.PlaceTile(array2[num24], array4[num26] - 1, ModContent.TileType<QuartzDoorClosed>(), mute: true, forced: false, -1);
					}
				}
			}
			for (int num27 = 0; num27 < 5; num27++)
			{
				for (int num28 = 0; num28 < 10; num28++)
				{
					if (!array5[num27, num28])
					{
						continue;
					}
					if (num28 > 0 && array5[num27, num28 - 1])
					{
						int num29 = WorldGen.genRand.Next(array[num27] + 2, array2[num27] - 1);
						int num30 = WorldGen.genRand.Next(array[num27] + 2, array2[num27] - 1);
						int num31 = 0;
						while (num30 - num29 < 2 || num30 - num29 > 5)
						{
							num29 = WorldGen.genRand.Next(array[num27] + 2, array2[num27] - 1);
							num30 = WorldGen.genRand.Next(array[num27] + 2, array2[num27] - 1);
							num31++;
							if (num31 > 10000)
							{
								break;
							}
						}
						if (num31 > 10000)
						{
							break;
						}
						for (int num32 = num29; num32 <= num30 && num32 >= 20 && num32 <= (Main.maxTilesX / 2) - 20; num32++)
						{
							Tile tile = Main.tile[num32, array3[num28]];
							tile.HasTile = false;
							WorldGen.PlaceTile(num32, array3[num28], ModContent.TileType<QuartzPlatform>(), mute: true, forced: true, -1);
							Main.tile[num32, array3[num28]].WallType = wallType;
						}
					}
					if (num27 < 4 && array5[num27 + 1, num28] && WorldGen.genRand.Next(3) == 0)
					{
						Tile tile1 = Main.tile[array2[num27], array4[num28] - 1];
						tile1.HasTile = false;
						Tile tile2 = Main.tile[array2[num27], array4[num28] - 2];
						tile2.HasTile = false;
						Tile tile3 = Main.tile[array2[num27], array4[num28] - 3];
						tile3.HasTile = false;
						Main.tile[array2[num27], array4[num28] - 1].WallType = wallType;
						Main.tile[array2[num27], array4[num28] - 2].WallType = wallType;
						Main.tile[array2[num27], array4[num28] - 3].WallType = wallType;
						WorldGen.PlaceTile(array2[num27], array4[num28] - 1, ModContent.TileType<QuartzDoorClosed>(), mute: true, forced: false, -1);
					}
				}
			}
			bool flag5 = false;
			for (int num33 = 0; num33 < 5; num33++)
			{
				bool[] array7 = new bool[10];
				for (int num34 = 0; num34 < 10; num34++)
				{
					if (array5[num33, num34])
					{
						flag5 = true;
						array7[num34] = true;
					}
				}
				if (!flag5)
				{
					continue;
				}
				bool flag6 = false;
				for (int num35 = 0; num35 < 10; num35++)
				{
					if (array7[num35])
					{
						if (!Main.tile[array[num33] - 1, array4[num35] - 1].HasTile && !Main.tile[array[num33] - 1, array4[num35] - 2].HasTile && !Main.tile[array[num33] - 1, array4[num35] - 3].HasTile && Main.tile[array[num33] - 1, array4[num35] - 1].LiquidAmount == 0 && Main.tile[array[num33] - 1, array4[num35] - 2].LiquidAmount == 0 && Main.tile[array[num33] - 1, array4[num35] - 3].LiquidAmount == 0)
						{
							flag6 = true;
						}
						else
						{
							array7[num35] = false;
						}
					}
				}
				while (flag6)
				{
					int num36 = WorldGen.genRand.Next(10);
					if (array7[num36])
					{
						flag6 = false;
						Tile tile1 = Main.tile[array[num33], array4[num36] - 1];
						tile1.HasTile = false;
						Tile tile2 = Main.tile[array[num33], array4[num36] - 2];
						tile2.HasTile = false;
						Tile tile3 = Main.tile[array[num33], array4[num36] - 3];
						tile3.HasTile = false;
						WorldGen.PlaceTile(array[num33], array4[num36] - 1, ModContent.TileType<QuartzDoorClosed>(), mute: true, forced: false, -1);
					}
				}
				break;
			}
			bool flag7 = false;
			for (int num37 = 4; num37 >= 0; num37--)
			{
				bool[] array8 = new bool[10];
				for (int num38 = 0; num38 < 10; num38++)
				{
					if (array5[num37, num38])
					{
						flag7 = true;
						array8[num38] = true;
					}
				}
				if (flag7)
				{
					bool flag8 = false;
					for (int num39 = 0; num39 < 10; num39++)
					{
						if (array8[num39])
						{
							if (num37 < 20 || num37 > (Main.maxTilesX / 2) - 20)
							{
								break;
							}
							if (!Main.tile[array2[num37] + 1, array4[num39] - 1].HasTile && !Main.tile[array2[num37] + 1, array4[num39] - 2].HasTile && !Main.tile[array2[num37] + 1, array4[num39] - 3].HasTile && Main.tile[array2[num37] + 1, array4[num39] - 1].LiquidAmount == 0 && Main.tile[array2[num37] + 1, array4[num39] - 2].LiquidAmount == 0 && Main.tile[array2[num37] + 1, array4[num39] - 3].LiquidAmount == 0)
							{
								flag8 = true;
							}
							else
							{
								array8[num39] = false;
							}
						}
					}
					while (flag8)
					{
						int num40 = WorldGen.genRand.Next(10);
						if (array8[num40])
						{
							flag8 = false;
							Tile tile1 = Main.tile[array2[num37], array4[num40] - 1];
							tile1.HasTile = false;
							Tile tile2 = Main.tile[array2[num37], array4[num40] - 2];
							tile2.HasTile = false;
							Tile tile3 = Main.tile[array2[num37], array4[num40] - 3];
							tile3.HasTile = false;
							WorldGen.PlaceTile(array2[num37], array4[num40] - 1, ModContent.TileType<QuartzDoorClosed>(), mute: true, forced: false, -1);
						}
					}
					break;
				}
			}
			bool flag9 = false;
			for (int num41 = 0; num41 < 10; num41++)
			{
				bool[] array9 = new bool[10];
				for (int num42 = 0; num42 < 5; num42++)
				{
					if (array5[num42, num41])
					{
						flag9 = true;
						array9[num42] = true;
					}
				}
				if (!flag9)
				{
					continue;
				}
				bool flag10 = true;
				while (flag10)
				{
					int num43 = WorldGen.genRand.Next(5);
					if (!array9[num43])
					{
						continue;
					}
					int num44 = WorldGen.genRand.Next(array[num43] + 2, array2[num43] - 1);
					int num45 = WorldGen.genRand.Next(array[num43] + 2, array2[num43] - 1);
					int num46 = 0;
					while (num45 - num44 < 2 || num45 - num44 > 5)
					{
						num44 = WorldGen.genRand.Next(array[num43] + 2, array2[num43] - 1);
						num45 = WorldGen.genRand.Next(array[num43] + 2, array2[num43] - 1);
						num46++;
						if (num46 > 10000)
						{
							break;
						}
					}
					if (num46 > 10000)
					{
						break;
					}
					for (int num47 = num44; num47 <= num45 && num47 >= 10 && num47 <= (Main.maxTilesX / 2) - 10; num47++)
					{
						if (Main.tile[num47, array3[num41] - 1].HasTile || Main.tile[num47, array3[num41] - 1].LiquidAmount > 0)
						{
							flag10 = false;
						}
					}
					if (flag10)
					{
						for (int num48 = num44; num48 <= num45 && num48 >= 10 && num48 <= (Main.maxTilesX / 2) - 10; num48++)
						{
							Tile tile = Main.tile[num48, array3[num41]];
							tile.HasTile = false;
							WorldGen.PlaceTile(num48, array3[num41], ModContent.TileType<QuartzPlatform>(), mute: true, forced: true, -1);
						}
					}
					flag10 = false;
				}
				break;
			}
		}
		#endregion

		#endregion

		#region DrunkGenRight

		#region RemixShinanigansRight
		public static void RemixIslandStuffRight(GenerationProgress progress, GameConfiguration configuration)
		{
			if (WorldGen.remixWorldGen)
			{
				for (int k = 0; k < Main.maxTilesX / 2; k++)
				{
					for (int l = Main.maxTilesY - 300; l < Main.maxTilesY; l++)
					{
						if (Main.tile[k, l].TileType == ModContent.TileType<ShaleBlock>())
						{
							WorldGen.KillTile(k, l, false, false, false);
							Main.tile[k, l].TileType = (ushort)TileID.Ash;
							Tile tile = Main.tile[k, l];
							tile.HasTile = true;
						}
						else if (Main.tile[k, l].TileType == ModContent.TileType<NightmareGrass>())
						{
							WorldGen.KillTile(k, l, false, false, false);
							Main.tile[k, l].TileType = (ushort)TileID.AshGrass;
							Tile tile = Main.tile[k, l];
							tile.HasTile = true;
						}
					}
				}

				int num854 = (int)((double)Main.maxTilesX * 0.38);
				int num855 = (int)((double)Main.maxTilesX * 0.62);
				int num856 = num854;
				int num857 = Main.maxTilesY - 1;
				int num858 = Main.maxTilesY - 135;
				int num859 = Main.maxTilesY - 160;
				for (int num864 = num854; num864 < num855 + 15; num864++)
				{
					for (int num865 = Main.maxTilesY - 200; num865 < num858 + 20; num865++)
					{
						if (Main.tile[num864, num865].TileType == 633 && Main.tile[num864, num865].HasTile == true && !Main.tile[num864, num865 - 1].HasTile == true && WorldGen.genRand.Next(3) == 0)
						{
							WorldGen.TryGrowingTreeByType(634, num864, num865);
						}
					}
				}
			}
		}
		#endregion

		#region DrunkDropletReplacerRight
		public static void DrippingQuicksilverTileCleanupRight(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Placing Dripping Quicksilver";
			for (int k = Main.maxTilesX / 2; k < Main.maxTilesX; k++)
			{
				for (int l = 0; l < Main.maxTilesY; l++)
				{
					if (Main.tile[k, l].TileType == TileID.LavaDrip)
					{
						WorldGen.KillTile(k, l);
						Main.tile[k, l].TileType = (ushort)ModContent.TileType<QuicksilverDropletSource>();
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
					}
					if (Main.tile[k, l].WallType == WallID.ObsidianBackUnsafe)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall1>();
					}
					if (Main.tile[k, l].WallType == WallID.LavaUnsafe1 || Main.tile[k, l].WallType == WallID.LavaUnsafe4)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall2>();
					}
					if (Main.tile[k, l].WallType == WallID.LavaUnsafe2)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall3>();
					}
					if (Main.tile[k, l].WallType == WallID.LavaUnsafe3)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall4>();
					}
					if (Main.tile[k, l].TileType == TileID.GeyserTrap)
					{
						WorldGen.KillTile(k, l);
						WorldGen.Place2x1(k, l, (ushort)ModContent.TileType<WaterGeyser>(), 0);
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
					}
					if (Main.tile[k, l].TileType == ModContent.TileType<MercuryMoss>())
					{
						if ((!Main.tileSolid[Main.tile[k, l + 1].TileType] || !Main.tile[k, l + 1].HasTile) && (!Main.tileSolid[Main.tile[k, l - 1].TileType] || !Main.tile[k, l - 1].HasTile) && (!Main.tileSolid[Main.tile[k + 1, l].TileType] || !Main.tile[k + 1, l].HasTile) && (!Main.tileSolid[Main.tile[k - 1, l].TileType] || !Main.tile[k - 1, l].HasTile))
						{
							WorldGen.KillTile(k, l);
						}
					}
				}
			}
		}
		#endregion

		#region DrunkMossRight
		public static void MossGenRight(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Growing Sparkly Moss";
			for (int k = Main.maxTilesX / 2; k < Main.maxTilesX; k++)
			{
				for (int l = 0; l < Main.maxTilesY; l++)
				{
					if (Main.tile[k, l].TileType == TileID.LavaMoss)
					{
						WorldGen.KillTile(k, l);
						Main.tile[k, l].TileType = (ushort)ModContent.TileType<MercuryMoss>();
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
					}
				}
			}
		}
		#endregion

		#region DrunkPotRemovalRight
		public static void KILLTHEPOTSRight(GenerationProgress progress, GameConfiguration configuration)
		{
			for (int X = Main.maxTilesX / 2; X < Main.maxTilesX; X++)
			{
				for (int Y = Main.maxTilesY - 200; Y < Main.maxTilesY; Y++)
				{
					if (Main.tile[X, Y].TileType == TileID.Pots)
					{
						WorldGen.KillTile(X, Y);
					}
				}
			}
		}
		#endregion

		#region DrunkPotsRight
		public static void PotsRight(GenerationProgress progress, GameConfiguration configuration)
		{
			double num444 = (double)((Main.maxTilesX) * Main.maxTilesY) * 0.0008;
			if (Main.starGame)
			{
				num444 *= Main.starGameMath(0.2);
			}
			for (int num445 = Main.maxTilesX / 2; (double)num445 < num444; num445++)
			{
				double num446 = (double)num445 / num444;
				progress.Set(num446);
				bool flag25 = false;
				int num447 = 0;
				while (!flag25)
				{
					int num448 = WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, Main.maxTilesY - 10);
					if (num446 > 0.93)
					{
						num448 = Main.maxTilesY - 150;
					}
					else if (num446 > 0.75)
					{
						num448 = (int)GenVars.worldSurfaceLow;
					}
					int num449 = WorldGen.genRand.Next(20 + (Main.maxTilesX / 2), (Main.maxTilesX) - 20);
					bool flag26 = false;
					for (int num450 = num448; num450 < Main.maxTilesY - 20; num450++)
					{
						if (!flag26)
						{
							if (Main.tile[num449, num450].HasTile && Main.tileSolid[Main.tile[num449, num450].TileType] && !(Main.tile[num449, num450 - 1].LiquidType == LiquidID.Lava) && !(Main.tile[num449, num450 - 1].LiquidType == LiquidID.Shimmer))
							{
								flag26 = true;
							}
						}
						else if (!((double)num450 < Main.worldSurface) || Main.tile[num449, num450].WallType != 0)
						{
							int style = WorldGen.genRand.Next(0, 4);
							int type = 28;
							if (num450 > Main.UnderworldLayer)
							{
								style = 0;
								type = ModContent.TileType<DepthsPot>();
							}
							if (!WorldGen.oceanDepths(num449, num450) && !(Main.tile[num449, num450].LiquidType == LiquidID.Shimmer) && WorldGen.PlacePot(num449, num450, (ushort)type, style))
							{
								flag25 = true;
								break;
							}
							num447++;
							if (num447 >= 10000)
							{
								flag25 = true;
								break;
							}
						}
					}
				}
			}
		}
		#endregion

		#region DrunkGemforgeRight
		public static void GemforgeRight(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Placing Gemforges";
			for (int index1 = Main.maxTilesX / 2; index1 < (Main.maxTilesX / 200); ++index1)
			{
				float num2 = index1 / (Main.maxTilesX / 200);
				progress.Set(num2);
				bool flag = false;
				int num3 = 0;
				while (!flag)
				{
					int i = WorldGen.genRand.Next(1, Main.maxTilesX);
					int index2 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 30);
					try
					{
						if (Main.tile[i, index2].WallType != WallType<ArqueriteBrickWallUnsafe>())
						{
							if (Main.tile[i, index2].WallType != WallType<QuartzBrickWallUnsafe>())
								continue;
						}
						while (!Main.tile[i, index2].HasTile && index2 < Main.maxTilesY - 20)
							++index2;
						int j = index2 - 1;
						WorldGen.PlaceTile(i, j, TileType<Gemforge>());
						if (Main.tile[i, j].TileType == TileType<Gemforge>())
						{
							flag = true;
						}
						else
						{
							++num3;
							if (num3 >= 10000)
								flag = true;
						}
					}
					catch
					{
					}
				}
			}
		}
		#endregion

		#region DrunkRight
		public static void DepthsRight(GenerationProgress progress, GameConfiguration configuration)
		{
			{
				progress.Message = "Creating depths";
				progress.Set(0.0f);
				for (int X999 = Main.maxTilesX / 2; X999 < Main.maxTilesX; X999++)
				{
					for (int Y999 = Main.maxTilesY - 300; Y999 < Main.maxTilesY; Y999++)
					{
						if (Main.tile[X999, Y999].TileType == TileID.ObsidianBrick || Main.tile[X999, Y999].TileType == TileID.HellstoneBrick || Main.tile[X999, Y999].TileType == TileID.Beds || Main.tile[X999, Y999].TileType == TileID.Bathtubs || Main.tile[X999, Y999].TileType == TileID.Tables || Main.tile[X999, Y999].TileType == TileID.WorkBenches || Main.tile[X999, Y999].TileType == TileID.Chairs || Main.tile[X999, Y999].TileType == TileID.Platforms || Main.tile[X999, Y999].TileType == TileID.Candelabras || Main.tile[X999, Y999].TileType == TileID.GrandfatherClocks || Main.tile[X999, Y999].TileType == TileID.Pianos || Main.tile[X999, Y999].TileType == TileID.Bookcases || Main.tile[X999, Y999].TileType == TileID.Hellforge || Main.tile[X999, Y999].TileType == TileID.Chandeliers || Main.tile[X999, Y999].TileType == TileID.Torches || Main.tile[X999, Y999].TileType == 89 || Main.tile[X999, Y999].TileType == TileID.Dressers || Main.tile[X999, Y999].TileType == TileID.Candles || Main.tile[X999, Y999].TileType == TileID.Statues || Main.tile[X999, Y999].TileType == TileID.ClosedDoor || Main.tile[X999, Y999].TileType == TileID.OpenDoor || Main.tile[X999, Y999].TileType == TileID.TreeAsh || Main.tile[X999, Y999].TileType == TileID.AshVines || Main.tile[X999, Y999].TileType == TileID.HangingLanterns || Main.tile[X999, Y999].TileType == TileID.Lamps || Main.tile[X999, Y999].TileType == TileID.Banners || Main.tile[X999, Y999].TileType == TileID.Painting2X3 || Main.tile[X999, Y999].TileType == TileID.Painting3X2 || Main.tile[X999, Y999].TileType == TileID.Painting3X3 || Main.tile[X999, Y999].TileType == TileID.Painting4X3 || Main.tile[X999, Y999].TileType == TileID.Painting6X4)
						{
							Tile tile = Main.tile[X999, Y999];
							tile.HasTile = false;
						}
						if (Main.tile[X999, Y999].WallType == WallID.ObsidianBrickUnsafe || Main.tile[X999, Y999].WallType == WallID.HellstoneBrickUnsafe)
						{
							Tile tile = Main.tile[X999, Y999];
							tile.WallType = 0;
						}
						else if (Main.tile[X999, Y999].TileType == TileID.Ash)
                        {
							WorldGen.KillTile(X999, Y999, false, false, false);
							Main.tile[X999, Y999].TileType = (ushort)ModContent.TileType<ShaleBlock>();
							Tile tile = Main.tile[X999, Y999];
							tile.HasTile = true;
						}
						else if (Main.tile[X999, Y999].TileType == TileID.AshGrass)
						{
							WorldGen.KillTile(X999, Y999, false, false, false);
							Main.tile[X999, Y999].TileType = (ushort)ModContent.TileType<NightmareGrass>();
							Tile tile = Main.tile[X999, Y999];
							tile.HasTile = true;
						}
					}
				}
				int num838 = Main.maxTilesY - WorldGen.genRand.Next(150, 190);
				for (int num839 = Main.maxTilesX / 2; num839 < Main.maxTilesX; num839++)
				{
					num838 += WorldGen.genRand.Next(-3, 4);
					if (num838 < Main.maxTilesY - 190)
					{
						num838 = Main.maxTilesY - 190;
					}
					if (num838 > Main.maxTilesY - 160)
					{
						num838 = Main.maxTilesY - 160;
					}
					for (int num840 = num838 - 20 - WorldGen.genRand.Next(3); num840 < Main.maxTilesY; num840++)
					{
						if (num840 >= num838)
						{
							Tile tile = Main.tile[num839, num840];
							tile.HasTile = false;
							tile.WallType = 0;
							tile.LiquidType = 1;
							Main.tile[num839, num840].LiquidAmount = 0;
						}
						else
						{
							Main.tile[num839, num840].TileType = (ushort)ModContent.TileType<ShaleBlock>();
							Main.tile[num839, num840].WallType = 0;
						}
					}
				}
				int num841 = Main.maxTilesY - WorldGen.genRand.Next(40, 70);
				for (int num842 = 10 + Main.maxTilesX / 2; num842 < Main.maxTilesX - 10; num842++)
				{
					num841 += WorldGen.genRand.Next(-10, 11);
					if (num841 > Main.maxTilesY - 60)
					{
						num841 = Main.maxTilesY - 60;
					}
					if (num841 < Main.maxTilesY - 100)
					{
						num841 = Main.maxTilesY - 120;
					}
					for (int num843 = num841; num843 < Main.maxTilesY - 10; num843++)
					{
						if (!Main.tile[num842, num843].HasTile)
						{
							Tile tile = Main.tile[num842, num843];
							tile.LiquidType = LiquidID.Lava;
							Main.tile[num842, num843].LiquidAmount = byte.MaxValue;
						}
					}
				}
				for (int num844 = Main.maxTilesX / 2; num844 < Main.maxTilesX; num844++)
				{
					if (WorldGen.genRand.Next(50) == 0)
					{
						int num845 = Main.maxTilesY - 65;
						while (!Main.tile[num844, num845].HasTile && num845 > Main.maxTilesY - 135)
						{
							num845--;
						}
						WorldGen.TileRunner(WorldGen.genRand.Next(Main.maxTilesX / 2, Main.maxTilesX), num845 + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(15, 20), 1000, ModContent.TileType<ShaleBlock>(), addTile: true, 0.0, WorldGen.genRand.Next(1, 3), noYChange: true);
					}
				}
				Liquid.QuickWater(-2);
				for (int num846 = Main.maxTilesX / 2; num846 < Main.maxTilesX; num846++)
				{
					double num847 = (double)num846 / (double)(Main.maxTilesX - 1);
					progress.Set(num847 / 2.0 + 0.5);
					if (WorldGen.genRand.Next(13) == 0)
					{
						int num848 = Main.maxTilesY - 65;
						while ((Main.tile[num846, num848].LiquidAmount > 0 || Main.tile[num846, num848].HasTile) && num848 > Main.maxTilesY - 140)
						{
							num848--;
						}
						if ((!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen) || WorldGen.genRand.Next(3) == 0 || !((double)num846 > (double)Main.maxTilesX * 0.4) || !((double)num846 < (double)Main.maxTilesX * 0.6))
						{
							WorldGen.TileRunner(num846, num848 - WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(5, 30), 1000, ModContent.TileType<ShaleBlock>(), addTile: true, 0.0, WorldGen.genRand.Next(1, 3), noYChange: true);
						}
						double num849 = WorldGen.genRand.Next(1, 3);
						if (WorldGen.genRand.Next(3) == 0)
						{
							num849 *= 0.5;
						}
						if ((!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen) || WorldGen.genRand.Next(3) == 0 || !((double)num846 > (double)Main.maxTilesX * 0.4) || !((double)num846 < (double)Main.maxTilesX * 0.6))
						{
							if (WorldGen.genRand.Next(2) == 0)
							{
								WorldGen.TileRunner(num846, num848 - WorldGen.genRand.Next(2, 5), (int)((double)WorldGen.genRand.Next(5, 15) * num849), (int)((double)WorldGen.genRand.Next(10, 15) * num849), ModContent.TileType<ShaleBlock>(), addTile: true, 1.0, 0.3);
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								num849 = WorldGen.genRand.Next(1, 3);
								WorldGen.TileRunner(num846, num848 - WorldGen.genRand.Next(2, 5), (int)((double)WorldGen.genRand.Next(5, 15) * num849), (int)((double)WorldGen.genRand.Next(10, 15) * num849), ModContent.TileType<ShaleBlock>(), addTile: true, -1.0, 0.3);
							}
						}
						WorldGen.TileRunner(num846 + WorldGen.genRand.Next(-10, 10), num848 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(5, 10), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
						if (WorldGen.genRand.Next(3) == 0)
						{
							WorldGen.TileRunner(num846 + WorldGen.genRand.Next(-10, 10), num848 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(10, 30), WorldGen.genRand.Next(10, 20), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
						}
						if (WorldGen.genRand.Next(5) == 0)
						{
							WorldGen.TileRunner(num846 + WorldGen.genRand.Next(-15, 15), num848 + WorldGen.genRand.Next(-15, 10), WorldGen.genRand.Next(15, 30), WorldGen.genRand.Next(5, 20), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
						}
					}
				}
				for (int num850 = Main.maxTilesX / 2; num850 < Main.maxTilesX; num850++)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(20, Main.maxTilesX - 20), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 7), -2);
				}
				if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
				{
					for (int num851 = Main.maxTilesX / 2; num851 < Main.maxTilesX * 2; num851++)
					{
						WorldGen.TileRunner(WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.35), (int)((double)Main.maxTilesX * 0.65)), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(5, 20), WorldGen.genRand.Next(5, 10), -2);
					}
				}
				for (int num852 = Main.maxTilesX / 2; num852 < Main.maxTilesX; num852++)
				{
					if (!Main.tile[num852, Main.maxTilesY - 145].HasTile)
					{
						Tile tile = Main.tile[num852, Main.maxTilesY - 145];
						Main.tile[num852, Main.maxTilesY - 145].LiquidAmount = byte.MaxValue;
						tile.LiquidType = LiquidID.Lava;
					}
					if (!Main.tile[num852, Main.maxTilesY - 144].HasTile)
					{
						Tile tile = Main.tile[num852, Main.maxTilesY - 144];
						Main.tile[num852, Main.maxTilesY - 144].LiquidAmount = byte.MaxValue;
						tile.LiquidType = 1;
					}
				}
				for (int num853 = 0; num853 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008) / 2; num853++)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(Main.maxTilesX / 2, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), TileType<ArqueriteOre>());
				}
				for (int index = 0; index < (int)(Main.maxTilesX * Main.maxTilesY * 0.0008) / 2; ++index)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(Main.maxTilesX / 2, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(5, 9), TileType<Shalestone>(), false, 0.0f, 0.0f, false, true);
				}
				if (!WorldGen.remixWorldGen)
				{
					Gems();
				}
				AddQuartzApartmentsRight();
			}

			void Gems()
			{
				for (int type = 63; type <= 68; ++type)
				{
					int tile = 0;
					float num1 = 0.0f;
					if (type == 67)
					{
						num1 = Main.maxTilesX * 0.5f;
						tile = TileType<ShalestoneAmethyst>();
					}
					else if (type == 66)
					{
						num1 = Main.maxTilesX * 0.45f;
						tile = TileType<ShalestoneTopaz>();
					}
					else if (type == 63)
					{
						num1 = Main.maxTilesX * 0.3f;
						tile = TileType<ShalestoneSapphire>();
					}
					else if (type == 65)
					{
						num1 = Main.maxTilesX * 0.25f;
						tile = TileType<ShalestoneEmerald>();
					}
					else if (type == 64)
					{
						num1 = Main.maxTilesX * 0.1f;
						tile = TileType<ShalestoneRuby>();
					}
					else if (type == 68)
					{
						num1 = Main.maxTilesX * 0.05f;
						tile = TileType<ShalestoneDiamond>();
					}
					float num2 = num1 * 0.2f;
					for (int index = Main.maxTilesX / 2; index < num2; ++index)
					{
						int i = WorldGen.genRand.Next(0, Main.maxTilesX);
						int j;
						for (j = WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY); Main.tile[i, j].TileType != 1; j = WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY))
							i = WorldGen.genRand.Next(0, Main.maxTilesX);
						WorldGen.TileRunner(i, j, WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(3, 7), tile, false, 0.0f, 0.0f, false, true);
					}
				}
				for (int index1 = 0; index1 < 2; ++index1)
				{
					int num1 = 1;
					int num2 = 5;
					int num3 = Main.maxTilesX - 5;
					if (index1 == 1)
					{
						num1 = -1;
						num2 = Main.maxTilesX - 5;
						num3 = 5;
					}
					int index2 = num2;
					while (index2 != num3)
					{
						for (int index3 = 10; index3 < Main.maxTilesY - 10; ++index3)
						{
							if (Main.tile[index2, index3].HasTile && Main.tile[index2, index3 + 1].HasTile && Main.tileSand[Main.tile[index2, index3].TileType] && Main.tileSand[Main.tile[index2, index3 + 1].TileType])
							{
								ushort type = Main.tile[index2, index3].TileType;
								int index4 = index2 + num1;
								int index5 = index3 + 1;
								if (!Main.tile[index4, index3].HasTile && !Main.tile[index4, index3 + 1].HasTile)
								{
									while (!Main.tile[index4, index5].HasTile)
										++index5;
									int index6 = index5 - 1;
									Main.tile[index4, index6].TileType = type;
								}
							}
						}
						index2 += num1;
					}
				}
			}
		}
		#endregion

		#region NightmareGroveRight
		public static void NightmareGroveRight(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Growing bioluminecent plants in very dark areas";
			//Proper Grove Gen
			if (WorldGen.remixWorldGen)
			{
				int num854 = (int)((double)(Main.maxTilesX / 2) * 0.38);
				int num855 = (int)((double)Main.maxTilesX * 0.62);
				int num856 = num854;
				int num857 = Main.maxTilesY - 1;
				int num858 = Main.maxTilesY - 135;
				int num859 = Main.maxTilesY - 160;
				bool flag55 = false;
				Liquid.QuickWater(-2);
				for (int num862 = num854; num862 < num855 + 15; num862++)
				{
					for (int num863 = Main.maxTilesY - 300; num863 < num858 + 20; num863++)
					{
						Main.tile[num862, num863].LiquidAmount = 0;
						if ((Main.tile[num862, num863].TileType == ModContent.TileType<ShaleBlock>() || Main.tile[num862, num863].TileType == ModContent.TileType<Shalestone>()) && Main.tile[num862, num863].HasTile && (!Main.tile[num862 - 1, num863 - 1].HasTile || !Main.tile[num862, num863 - 1].HasTile || !Main.tile[num862 + 1, num863 - 1].HasTile || !Main.tile[num862 - 1, num863].HasTile || !Main.tile[num862 + 1, num863].HasTile || !Main.tile[num862 - 1, num863 + 1].HasTile || !Main.tile[num862, num863 + 1].HasTile || !Main.tile[num862 + 1, num863 + 1].HasTile))
						{
							Main.tile[num862, num863].TileType = (ushort)ModContent.TileType<NightmareGrass>();
						}
					}
				}
				for (int num864 = num854; num864 < num855 + 15; num864++)
				{
					for (int num865 = Main.maxTilesY - 200; num865 < num858 + 20; num865++)
					{
						if (Main.tile[num864, num865].TileType == ModContent.TileType<NightmareGrass>() && Main.tile[num864, num865].HasTile && !Main.tile[num864, num865 - 1].HasTile && WorldGen.genRand.Next(3) == 0)
						{
							WorldGen.GrowTree(num864, num865);
						}
					}
				}
			}
			else if (!WorldGen.drunkWorldGen)
			{
				for (int num866 = (Main.maxTilesX / 2) + 25; num866 < Main.maxTilesX - 25; num866++)
				{
					if ((double)num866 < (double)Main.maxTilesX * 0.17 || (double)num866 > (double)Main.maxTilesX * 0.83)
					{
						for (int num867 = Main.maxTilesY - 300; num867 < Main.maxTilesY - 100 + WorldGen.genRand.Next(-1, 2); num867++)
						{
							if ((Main.tile[num866, num867].TileType == ModContent.TileType<ShaleBlock>() || Main.tile[num866, num867].TileType == ModContent.TileType<Shalestone>()) && Main.tile[num866, num867].HasTile && (!Main.tile[num866 - 1, num867 - 1].HasTile || !Main.tile[num866, num867 - 1].HasTile || !Main.tile[num866 + 1, num867 - 1].HasTile || !Main.tile[num866 - 1, num867].HasTile || !Main.tile[num866 + 1, num867].HasTile || !Main.tile[num866 - 1, num867 + 1].HasTile || !Main.tile[num866, num867 + 1].HasTile || !Main.tile[num866 + 1, num867 + 1].HasTile))
							{
								Main.tile[num866, num867].TileType = (ushort)ModContent.TileType<NightmareGrass>();
							}
						}
					}
				}
				for (int num868 = (Main.maxTilesX / 2) + 25; num868 < Main.maxTilesX - 25; num868++)
				{
					if ((double)num868 < (double)Main.maxTilesX * 0.17 || (double)num868 > (double)Main.maxTilesX * 0.83)
					{
						for (int num869 = Main.maxTilesY - 200; num869 < Main.maxTilesY - 50; num869++)
						{
							if (Main.tile[num868, num869].TileType == ModContent.TileType<NightmareGrass>() && Main.tile[num868, num869].HasTile && !Main.tile[num868, num869 - 1].HasTile && WorldGen.genRand.Next(3) == 0)
							{
								WorldGen.GrowTree(num868, num869);
							}
						}
					}
				}
			}
			else
			{
				for (int num866 = (Main.maxTilesX / 2) + 25; num866 < Main.maxTilesX - 25; num866++)
				{
					for (int num867 = Main.maxTilesY - 300; num867 < Main.maxTilesY - 100 + WorldGen.genRand.Next(-1, 2); num867++)
					{
						if ((Main.tile[num866, num867].TileType == ModContent.TileType<ShaleBlock>() || Main.tile[num866, num867].TileType == ModContent.TileType<Shalestone>()) && Main.tile[num866, num867].HasTile && (!Main.tile[num866 - 1, num867 - 1].HasTile || !Main.tile[num866, num867 - 1].HasTile || !Main.tile[num866 + 1, num867 - 1].HasTile || !Main.tile[num866 - 1, num867].HasTile || !Main.tile[num866 + 1, num867].HasTile || !Main.tile[num866 - 1, num867 + 1].HasTile || !Main.tile[num866, num867 + 1].HasTile || !Main.tile[num866 + 1, num867 + 1].HasTile))
						{
							Main.tile[num866, num867].TileType = (ushort)ModContent.TileType<NightmareGrass>();
						}
					}
				}
				for (int num868 = (Main.maxTilesX / 2) + 25; num868 < Main.maxTilesX / 2 - 25; num868++)
				{
					for (int num869 = Main.maxTilesY - 200; num869 < Main.maxTilesY - 50; num869++)
					{
						if (Main.tile[num868, num869].TileType == ModContent.TileType<NightmareGrass>() && Main.tile[num868, num869].HasTile && !Main.tile[num868, num869 - 1].HasTile && WorldGen.genRand.Next(3) == 0)
						{
							WorldGen.GrowTree(num868, num869);
						}
					}
				}
			}

			//Vines
			for (int num233 = (Main.maxTilesX / 2) + 5; num233 < Main.maxTilesX - 5; num233++)
			{
				int num234 = 0;
				for (int num253 = 0; num253 < Main.maxTilesY; num253++)
				{
					if (num234 > 0 && !Main.tile[num233, num253].HasTile)
					{
						Tile tile = Main.tile[num233, num253];
						tile.HasTile = true;
						Main.tile[num233, num253].TileType = (ushort)ModContent.TileType<NightmareVines>();
						num234--;
					}
					else
					{
						num234 = 0;
					}
					if (Main.tile[num233, num253].HasTile && !Main.tile[num233, num253].BottomSlope && Main.tile[num233, num253].TileType == ModContent.TileType<NightmareGrass>() && TheDepthsWorldGen.GrowMoreVines(num233, num253) && WorldGen.genRand.Next(5) < 3)
					{
						num234 = WorldGen.genRand.Next(1, 10);
					}
				}
			}

			//Foliage
			for (int num263 = (Main.maxTilesX / 2); num263 < Main.maxTilesX; num263++)
			{
				progress.Set((double)num263 / (double)Main.maxTilesX);
				for (int num264 = 1; num264 < Main.maxTilesY; num264++)
				{
					if (Main.tile[num263, num264].TileType == ModContent.TileType<NightmareGrass>() && Main.tile[num263, num264].HasUnactuatedTile && !Main.tile[num263, num264 - 1].HasTile)
					{
						WorldGen.PlaceTile(num263, num264 - 1, ModContent.TileType<NightmareGrass_Foliage>(), mute: true);
					}
				}
			}
		}
		#endregion

		#region DrunkHousesRight
		public static void AddQuartzApartmentsRight()
		{
			int num = (int)((double)Main.maxTilesX * 0.25);
			for (int i = 100 + (Main.maxTilesX / 2); i < Main.maxTilesX - 100; i++)
			{
				if (((WorldGen.drunkWorldGen || WorldGen.remixWorldGen) && i > num && i < Main.maxTilesX - num) || (!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen && (i < num || i > Main.maxTilesX - num)))
				{
					continue;
				}
				int num2 = Main.maxTilesY - 40;
				while (Main.tile[i, num2].HasTile || Main.tile[i, num2].LiquidAmount > 0)
				{
					num2--;
				}
				if (Main.tile[i, num2 + 1].HasTile)
				{
					ushort num3 = (ushort)WorldGen.genRand.Next(75, 77);
					ushort wallType = (ushort)WallType<QuartzBrickWallUnsafe>();
					if (num3 == 75)
					{
						num3 = (ushort)TileType<ArqueriteBricks>();
					}
					if (num3 == 76)
					{
						num3 = (ushort)TileType<QuartzBricks>();
					}
					if (num3 == TileType<QuartzBricks>())
					{
						wallType = (ushort)WallType<QuartzBrickWallUnsafe>();
					}
					if (WorldGen.getGoodWorldGen)
					{
						num3 = (ushort)TileType<ArqueriteBricks>();
					}
					if (num3 == (ushort)TileType<ArqueriteBricks>())
					{
						wallType = (ushort)WallType<ArqueriteBrickWallUnsafe>();
					}
					ArqueriteCastleRight(i, num2, num3, wallType);
					i += WorldGen.genRand.Next(30, 130);
					if (WorldGen.genRand.Next(10) == 0)
					{
						i += WorldGen.genRand.Next(0, 200);
					}
				}
			}
			float num4 = Main.maxTilesX / 4200;
			for (int j = 0; (float)j < 200f * num4; j++)
			{
				int num5 = 0;
				bool flag = false;
				while (!flag)
				{
					num5++;
					int num6 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.2), (int)((double)Main.maxTilesX * 0.8));
					int num7 = WorldGen.genRand.Next(Main.maxTilesY - 300, Main.maxTilesY - 20);
					if (Main.tile[num6, num7].HasTile && (Main.tile[num6, num7].TileType == ModContent.TileType<QuartzBricks>() || Main.tile[num6, num7].TileType == ModContent.TileType<ArqueriteBricks>()))
					{
						int num8 = 0;
						if (Main.tile[num6 - 1, num7].WallType > 0)
						{
							num8 = -1;
						}
						else if (Main.tile[num6 + 1, num7].WallType > 0)
						{
							num8 = 1;
						}
						if (!Main.tile[num6 + num8, num7].HasTile && !Main.tile[num6 + num8, num7 + 1].HasTile)
						{
							bool flag2 = false;
							for (int k = num6 - 8; k < num6 + 8; k++)
							{
								for (int l = num7 - 8; l < num7 + 8; l++)
								{
									if (Main.tile[k, l].HasTile && Main.tile[k, l].TileType == ModContent.TileType<GeoTorch>())
									{
										flag2 = true;
										break;
									}
								}
							}
							if (!flag2)
							{
								WorldGen.PlaceTile(num6 + num8, num7, ModContent.TileType<GeoTorch>(), mute: true, forced: true, -1);
								flag = true;
							}
						}
					}
					if (num5 > 1000)
					{
						flag = true;
					}
				}
			}
			double num9 = 4200000.0 / (double)Main.maxTilesX;
			for (int m = 0; (double)m < num9; m++)
			{
				int num10 = 0;
				int num11 = WorldGen.genRand.Next(num, Main.maxTilesX - num);
				int n = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
				while ((Main.tile[num11, n].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num11, n].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>()) || Main.tile[num11, n].HasTile)
				{
					num11 = WorldGen.genRand.Next(num, Main.maxTilesX - num);
					n = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
					if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
					{
						num11 = ((WorldGen.genRand.Next(2) != 0) ? WorldGen.genRand.Next(Main.maxTilesX - num, Main.maxTilesX - 50) : WorldGen.genRand.Next(50, num));
					}
					num10++;
					if (num10 > 100000)
					{
						break;
					}
				}
				if (num10 > 100000 || (Main.tile[num11, n].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num11, n].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>()) || Main.tile[num11, n].HasTile)
				{
					continue;
				}
				for (; !WorldGen.SolidTile(num11, n) && n < Main.maxTilesY - 20; n++)
				{
				}
				n--;
				int num12 = num11;
				int num13 = num11;
				while (!Main.tile[num12, n].HasTile && WorldGen.SolidTile(num12, n + 1))
				{
					num12--;
				}
				num12++;
				for (; !Main.tile[num13, n].HasTile && WorldGen.SolidTile(num13, n + 1); num13++)
				{
				}
				num13--;
				int num14 = num13 - num12;
				int num15 = (num13 + num12) / 2;
				if (Main.tile[num15, n].HasTile || (Main.tile[num15, n].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num15, n].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>()) || !WorldGen.SolidTile(num15, n + 1))
				{
					continue;
				}
				int num16 = WorldGen.genRand.Next(13);
				int num17 = 0;
				int num18 = 0;
				if (num16 == 0)
				{
					num17 = 5;
					num18 = 4;
				}
				if (num16 == 1)
				{
					num17 = 4;
					num18 = 3;
				}
				if (num16 == 2)
				{
					num17 = 3;
					num18 = 5;
				}
				if (num16 == 3)
				{
					num17 = 4;
					num18 = 6;
				}
				if (num16 == 4)
				{
					num17 = 3;
					num18 = 3;
				}
				if (num16 == 5)
				{
					num17 = 5;
					num18 = 3;
				}
				if (num16 == 6)
				{
					num17 = 5;
					num18 = 4;
				}
				if (num16 == 7)
				{
					num17 = 5;
					num18 = 4;
				}
				if (num16 == 8)
				{
					num17 = 5;
					num18 = 4;
				}
				if (num16 == 9)
				{
					num17 = 3;
					num18 = 5;
				}
				if (num16 == 10)
				{
					num17 = 5;
					num18 = 3;
				}
				if (num16 == 11)
				{
					num17 = 2;
					num18 = 4;
				}
				if (num16 == 12)
				{
					num17 = 3;
					num18 = 3;
				}
				for (int num19 = num15 - num17; num19 <= num15 + num17; num19++)
				{
					for (int num20 = n - num18; num20 <= n; num20++)
					{
						if (Main.tile[num19, num20].HasTile)
						{
							num16 = -1;
							break;
						}
					}
				}
				if ((double)num14 < (double)num17 * 1.75)
				{
					num16 = -1;
				}
				switch (num16)
				{
					case 0:
						{
							WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzTable>(), mute: true, forced: false, -1);
							int num22 = WorldGen.genRand.Next(6);
							if (num22 < 3)
							{
								WorldGen.PlaceTile(num15 + num22, n - 2, ModContent.TileType<QuartzCandle>(), mute: true, forced: false, -1);
							}
							if (!Main.tile[num15, n].HasTile)
							{
								break;
							}
							if (!Main.tile[num15 - 2, n].HasTile)
							{
								WorldGen.PlaceTile(num15 - 2, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
								if (Main.tile[num15 - 2, n].HasTile)
								{
									Main.tile[num15 - 2, n].TileFrameX += 18;
									Main.tile[num15 - 2, n - 1].TileFrameX += 18;
								}
							}
							if (!Main.tile[num15 + 2, n].HasTile)
							{
								WorldGen.PlaceTile(num15 + 2, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
							}
							break;
						}
					case 1:
						{
							WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzWorkbench>(), mute: true, forced: false, -1);
							int num21 = WorldGen.genRand.Next(4);
							if (num21 < 2)
							{
								WorldGen.PlaceTile(num15 + num21, n - 1, ModContent.TileType<QuartzCandle>(), mute: true, forced: false, -1);
							}
							if (!Main.tile[num15, n].HasTile)
							{
								break;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								if (!Main.tile[num15 - 1, n].HasTile)
								{
									WorldGen.PlaceTile(num15 - 1, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
									if (Main.tile[num15 - 1, n].HasTile)
									{
										Main.tile[num15 - 1, n].TileFrameX += 18;
										Main.tile[num15 - 1, n - 1].TileFrameX += 18;
									}
								}
							}
							else if (!Main.tile[num15 + 2, n].HasTile)
							{
								WorldGen.PlaceTile(num15 + 2, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
							}
							break;
						}
					case 2:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzVase>(), mute: true, forced: false, -1);
						break;
					case 3:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzBookcase>(), mute: true, forced: false, -1);
						break;
					case 4:
						if (WorldGen.genRand.Next(2) == 0)
						{
							WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
							Main.tile[num15, n].TileFrameX += 18;
							Main.tile[num15, n - 1].TileFrameX += 18;
						}
						else
						{
							WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzChair>(), mute: true, forced: false, -1);
						}
						break;
					case 5:
						if (WorldGen.genRand.Next(2) == 0)
						{
							WorldGen.Place4x2(num15, n, (ushort)ModContent.TileType<QuartzBed>(), 1);
						}
						else
						{
							WorldGen.Place4x2(num15, n, (ushort)ModContent.TileType<QuartzBed>(), -1);
						}
						break;
					case 6:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzPiano>(), mute: true, forced: false, -1);
						break;
					case 7:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzDresser>(), mute: true, forced: false, -1);
						break;
					case 8:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzSofa>(), mute: true, forced: false, -1);
						break;
					case 9:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzClock>(), mute: true, forced: false, -1);
						break;
					case 10:
						if (WorldGen.genRand.Next(2) == 0)
						{
							WorldGen.Place4x2(num15, n, (ushort)ModContent.TileType<QuartzBath>(), 1);
						}
						else
						{
							WorldGen.Place4x2(num15, n, (ushort)ModContent.TileType<QuartzBath>(), -1);
						}
						break;
					case 11:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzLamp>(), mute: true, forced: false, -1);
						break;
					case 12:
						WorldGen.PlaceTile(num15, n, ModContent.TileType<QuartzCandelabra>(), mute: true, forced: false, -1);
						break;
				}
			}
			num9 = 420000.0 / (double)Main.maxTilesX;
			for (int num23 = 0; (double)num23 < num9; num23++)
			{
				int num24 = 0;
				int num25 = WorldGen.genRand.Next(num, Main.maxTilesX - num);
				int num26 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
				while ((Main.tile[num25, num26].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num25, num26].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>()) || Main.tile[num25, num26].HasTile)
				{
					num25 = WorldGen.genRand.Next(num, Main.maxTilesX - num);
					num26 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
					if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
					{
						num25 = ((WorldGen.genRand.Next(2) != 0) ? WorldGen.genRand.Next(Main.maxTilesX - num, Main.maxTilesX - 50) : WorldGen.genRand.Next(50, num));
					}
					num24++;
					if (num24 > 100000)
					{
						break;
					}
				}
				if (num24 > 100000)
				{
					continue;
				}
				int num27 = num25;
				int num28 = num25;
				int num29 = num26;
				int num30 = num26;
				int num31 = 0;
				for (int num32 = 0; num32 < 2; num32++)
				{
					num27 = num25;
					num28 = num25;
					while (!Main.tile[num27, num26].HasTile && (Main.tile[num27, num26].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[num27, num26].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>()))
					{
						num27--;
					}
					num27++;
					for (; !Main.tile[num28, num26].HasTile && (Main.tile[num28, num26].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[num28, num26].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>()); num28++)
					{
					}
					num28--;
					num25 = (num27 + num28) / 2;
					num29 = num26;
					num30 = num26;
					while (!Main.tile[num25, num29].HasTile && (Main.tile[num25, num29].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[num25, num29].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>()))
					{
						num29--;
					}
					num29++;
					for (; !Main.tile[num25, num30].HasTile && (Main.tile[num25, num30].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[num25, num30].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>()); num30++)
					{
					}
					num30--;
					num26 = (num29 + num30) / 2;
				}
				num27 = num25;
				num28 = num25;
				while (!Main.tile[num27, num26].HasTile && !Main.tile[num27, num26 - 1].HasTile && !Main.tile[num27, num26 + 1].HasTile)
				{
					num27--;
				}
				num27++;
				for (; !Main.tile[num28, num26].HasTile && !Main.tile[num28, num26 - 1].HasTile && !Main.tile[num28, num26 + 1].HasTile; num28++)
				{
				}
				num28--;
				num29 = num26;
				num30 = num26;
				while (!Main.tile[num25, num29].HasTile && !Main.tile[num25 - 1, num29].HasTile && !Main.tile[num25 + 1, num29].HasTile)
				{
					num29--;
				}
				num29++;
				for (; !Main.tile[num25, num30].HasTile && !Main.tile[num25 - 1, num30].HasTile && !Main.tile[num25 + 1, num30].HasTile; num30++)
				{
				}
				num30--;
				num25 = (num27 + num28) / 2;
				num26 = (num29 + num30) / 2;
				int num33 = num28 - num27;
				num31 = num30 - num29;
				if (num33 <= 7 || num31 <= 5)
				{
					continue;
				}
				int num34 = 0;
				if (TheDepthsWorldGen.nearDepthsPainting(num25, num26))
				{
					num34 = -1;
				}
				if (num34 == 0)
				{
					PaintingEntry paintingEntry = TheDepthsWorldGen.RandDarknessPicture();
					if (!WorldGen.nearPicture(num25, num26))
					{
						WorldGen.PlaceTile(num25, num26, paintingEntry.tileType, mute: true, forced: false, -1, paintingEntry.style);
					}
				}
			}
			num9 = 420000.0 / (double)Main.maxTilesX;
			for (int num35 = 0; (double)num35 < num9; num35++)
			{
				int num36 = 0;
				int num37;
				int num38;
				do
				{
					num37 = WorldGen.genRand.Next(num, Main.maxTilesX - num);
					num38 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
					if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
					{
						num37 = ((WorldGen.genRand.Next(2) != 0) ? WorldGen.genRand.Next(Main.maxTilesX - num, Main.maxTilesX - 50) : WorldGen.genRand.Next(50, num));
					}
					num36++;
				}
				while (num36 <= 100000 && ((Main.tile[num37, num38].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num37, num38].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>()) || Main.tile[num37, num38].HasTile));
				if (num36 > 100000)
				{
					continue;
				}
				while (!WorldGen.SolidTile(num37, num38) && num38 > 10)
				{
					num38--;
				}
				num38++;
				if (Main.tile[num37, num38].WallType != ModContent.WallType<QuartzBrickWallUnsafe>() && Main.tile[num37, num38].WallType != ModContent.WallType<ArqueriteBrickWallUnsafe>())
				{
					continue;
				}
				int num39 = WorldGen.genRand.Next(3);
				int num40;
				int num41;
				switch (num39)
				{
					default:
						num40 = 1;
						num41 = 3;
						break;
					case 1:
						num40 = 3;
						num41 = 3;
						break;
					case 2:
						num40 = 1;
						num41 = 2;
						break;
				}
				for (int num42 = num37 - 1; num42 <= num37 + num40; num42++)
				{
					for (int num43 = num38; num43 <= num38 + num41; num43++)
					{
						Tile tile = Main.tile[num37, num38];
						if (num42 < num37 || num42 == num37 + num40)
						{
							if (tile.HasTile)
							{
								/*switch (tile.TileType)
								{
									case TileType<QuartzDoorClosed>():
									case 11:
									case 34:
									case 42:
									case 91:
										num39 = -1;
										break;
								}*/
							}
						}
						else if (tile.HasTile)
						{
							num39 = -1;
						}
					}
				}
				switch (num39)
				{
					case 0:
						WorldGen.PlaceTile(num37, num38, ModContent.TileType<DepthsVanityBanners>(), mute: true, forced: false, -1, WorldGen.genRand.Next(6));
						break;
					case 1:
						WorldGen.PlaceTile(num37, num38, ModContent.TileType<QuartzChandelier>(), mute: true, forced: false, -1);
						break;
					case 2:
						WorldGen.PlaceTile(num37, num38, ModContent.TileType<QuartzLantern>(), mute: true, forced: false, -1);
						break;
				}
			}
		}

		public static void ArqueriteCastleRight(int i, int j, ushort tileType = 75, ushort wallType = 14)
		{
			int[] array = new int[5];
			int[] array2 = new int[5];
			int[] array3 = new int[10];
			int[] array4 = new int[10];
			int num = 8;
			int num2 = 20;
			if (WorldGen.drunkWorldGen)
			{
				num /= 2;
				num2 *= 2;
			}
			array[2] = i - WorldGen.genRand.Next(num / 2, num2 / 2);
			array2[2] = i + WorldGen.genRand.Next(num / 2, num2 / 2);
			array[3] = array2[2];
			array2[3] = array[3] + WorldGen.genRand.Next(num, num2);
			array[4] = array2[3];
			array2[4] = array[4] + WorldGen.genRand.Next(num, num2);
			array2[1] = array[2];
			array[1] = array2[1] - WorldGen.genRand.Next(num, num2);
			array2[0] = array[1];
			array[0] = array2[0] - WorldGen.genRand.Next(num, num2);
			num = 6;
			num2 = 12;
			array3[3] = j - WorldGen.genRand.Next(num, num2);
			array4[3] = j;
			for (int k = 4; k < 10; k++)
			{
				array3[k] = array4[k - 1];
				array4[k] = array3[k] + WorldGen.genRand.Next(num, num2);
			}
			for (int num3 = 2; num3 >= 0; num3--)
			{
				array4[num3] = array3[num3 + 1];
				array3[num3] = array4[num3] - WorldGen.genRand.Next(num, num2);
			}
			bool flag = false;
			bool flag2 = false;
			bool[,] array5 = new bool[5, 10];
			int num4 = 3;
			int num5 = 3;
			for (int l = 0; l < 2; l++)
			{
				if (WorldGen.genRand.Next(3) == 0 || WorldGen.drunkWorldGen)
				{
					flag = true;
					int num6 = WorldGen.genRand.Next(10);
					if (num6 < num4)
					{
						num4 = num6;
					}
					if (num6 > num5)
					{
						num5 = num6;
					}
					int num7 = 1;
					if (WorldGen.genRand.Next(2) == 0 || WorldGen.drunkWorldGen)
					{
						array5[0, num6] = true;
						array5[1, num6] = true;
						num7 = 0;
					}
					else
					{
						array5[1, num6] = true;
					}
					int num8 = WorldGen.genRand.Next(2);
					if (num8 == 0)
					{
						num8 = -1;
					}
					int num9 = WorldGen.genRand.Next(10);
					while (num9 > 0 && num6 >= 0 && num6 < 10)
					{
						array5[num7, num6] = true;
						num6 += num8;
					}
				}
				if (WorldGen.genRand.Next(3) == 0 || WorldGen.drunkWorldGen)
				{
					flag2 = true;
					int num10 = WorldGen.genRand.Next(10);
					if (num10 < num4)
					{
						num4 = num10;
					}
					if (num10 > num5)
					{
						num5 = num10;
					}
					int num11 = 3;
					if (WorldGen.genRand.Next(2) == 0 || WorldGen.drunkWorldGen)
					{
						array5[3, num10] = true;
						array5[4, num10] = true;
						num11 = 4;
					}
					else
					{
						array5[3, num10] = true;
					}
					int num12 = WorldGen.genRand.Next(2);
					if (num12 == 0)
					{
						num12 = -1;
					}
					int num13 = WorldGen.genRand.Next(10);
					while (num13 > 0 && num10 >= 0 && num10 < 10)
					{
						array5[num11, num10] = true;
						num10 += num12;
					}
				}
			}
			for (int m = 0; m < 5; m++)
			{
				int num14 = array[m];
				bool flag3 = false;
				if (num14 < 10 || num14 > Main.maxTilesX - 10)
				{
					flag3 = true;
				}
				else
				{
					for (int n = Main.UnderworldLayer; n < Main.maxTilesY; n++)
					{
						if (Main.tile[num14, n].WallType > 0)
						{
							flag3 = true;
						}
					}
				}
				if (flag3)
				{
					for (int num15 = 0; num15 < 10; num15++)
					{
						array5[m, num15] = false;
					}
				}
			}
			int num16 = WorldGen.genRand.Next(10);
			if (num16 < num4)
			{
				num4 = num16;
			}
			num16 = WorldGen.genRand.Next(10);
			if (num16 > num5)
			{
				num5 = num16;
			}
			if (!flag && !flag2)
			{
				while (num5 - num4 < 5)
				{
					num16 = WorldGen.genRand.Next(10);
					if (num16 < num4)
					{
						num4 = num16;
					}
					num16 = WorldGen.genRand.Next(10);
					if (num16 > num5)
					{
						num5 = num16;
					}
				}
			}
			for (int num17 = num4; num17 <= num5; num17++)
			{
				array5[2, num17] = true;
			}
			for (int num18 = 0; num18 < 5; num18++)
			{
				for (int num19 = 0; num19 < 10; num19++)
				{
					if (array5[num18, num19] && (array3[num19] < Main.UnderworldLayer || array4[num19] > Main.maxTilesY - 20))
					{
						array5[num18, num19] = false;
					}
				}
			}
			for (int num20 = 0; num20 < 5; num20++)
			{
				for (int num21 = 0; num21 < 10; num21++)
				{
					if (!array5[num20, num21])
					{
						continue;
					}
					for (int num22 = array[num20]; num22 <= array2[num20]; num22++)
					{
						for (int num23 = array3[num21]; num23 <= array4[num21]; num23++)
						{
							if (num22 < 10)
							{
								break;
							}
							if (num22 > Main.maxTilesX - 10)
							{
								break;
							}
							Main.tile[num22, num23].LiquidAmount = 0;
							if (num22 == array[num20] || num22 == array2[num20] || num23 == array3[num21] || num23 == array4[num21])
							{
								Tile tile = Main.tile[num22, num23];
								tile.HasTile = true;
								tile.TileType = tileType;
								tile.IsHalfBlock = false;
								tile.Slope = 0;
							}
							else
							{
								Tile tile = Main.tile[num22, num23];
								tile.WallType = wallType;
								tile.HasTile = false;
							}
						}
					}
				}
			}
			for (int num24 = 0; num24 < 4; num24++)
			{
				bool[] array6 = new bool[10];
				bool flag4 = false;
				for (int num25 = 0; num25 < 10; num25++)
				{
					if (array5[num24, num25] && array5[num24 + 1, num25])
					{
						array6[num25] = true;
						flag4 = true;
					}
				}
				while (flag4)
				{
					int num26 = WorldGen.genRand.Next(10);
					if (array6[num26])
					{
						flag4 = false;
						Tile tile1 = Main.tile[array2[num24], array4[num26] - 1];
						tile1.HasTile = false;
						Tile tile2 = Main.tile[array2[num24], array4[num26] - 2];
						tile2.HasTile = false;
						Tile tile3 = Main.tile[array2[num24], array4[num26] - 3];
						tile3.HasTile = false;
						Main.tile[array2[num24], array4[num26] - 1].WallType = wallType;
						Main.tile[array2[num24], array4[num26] - 2].WallType = wallType;
						Main.tile[array2[num24], array4[num26] - 3].WallType = wallType;
						WorldGen.PlaceTile(array2[num24], array4[num26] - 1, ModContent.TileType<QuartzDoorClosed>(), mute: true, forced: false, -1);
					}
				}
			}
			for (int num27 = 0; num27 < 5; num27++)
			{
				for (int num28 = 0; num28 < 10; num28++)
				{
					if (!array5[num27, num28])
					{
						continue;
					}
					if (num28 > 0 && array5[num27, num28 - 1])
					{
						int num29 = WorldGen.genRand.Next(array[num27] + 2, array2[num27] - 1);
						int num30 = WorldGen.genRand.Next(array[num27] + 2, array2[num27] - 1);
						int num31 = 0;
						while (num30 - num29 < 2 || num30 - num29 > 5)
						{
							num29 = WorldGen.genRand.Next(array[num27] + 2, array2[num27] - 1);
							num30 = WorldGen.genRand.Next(array[num27] + 2, array2[num27] - 1);
							num31++;
							if (num31 > 10000)
							{
								break;
							}
						}
						if (num31 > 10000)
						{
							break;
						}
						for (int num32 = num29 + (Main.maxTilesX); num32 <= num30 && num32 >= 20 && num32 <= Main.maxTilesX - 20; num32++)
						{
							Tile tile = Main.tile[num32, array3[num28]];
							tile.HasTile = false;
							WorldGen.PlaceTile(num32, array3[num28], ModContent.TileType<QuartzPlatform>(), mute: true, forced: true, -1);
							Main.tile[num32, array3[num28]].WallType = wallType;
						}
					}
					if (num27 < 4 && array5[num27 + 1, num28] && WorldGen.genRand.Next(3) == 0)
					{
						Tile tile1 = Main.tile[array2[num27], array4[num28] - 1];
						tile1.HasTile = false;
						Tile tile2 = Main.tile[array2[num27], array4[num28] - 2];
						tile2.HasTile = false;
						Tile tile3 = Main.tile[array2[num27], array4[num28] - 3];
						tile3.HasTile = false;
						Main.tile[array2[num27], array4[num28] - 1].WallType = wallType;
						Main.tile[array2[num27], array4[num28] - 2].WallType = wallType;
						Main.tile[array2[num27], array4[num28] - 3].WallType = wallType;
						WorldGen.PlaceTile(array2[num27], array4[num28] - 1, ModContent.TileType<QuartzDoorClosed>(), mute: true, forced: false, -1);
					}
				}
			}
			bool flag5 = false;
			for (int num33 = 0; num33 < 5; num33++)
			{
				bool[] array7 = new bool[10];
				for (int num34 = 0; num34 < 10; num34++)
				{
					if (array5[num33, num34])
					{
						flag5 = true;
						array7[num34] = true;
					}
				}
				if (!flag5)
				{
					continue;
				}
				bool flag6 = false;
				for (int num35 = 0; num35 < 10; num35++)
				{
					if (array7[num35])
					{
						if (!Main.tile[array[num33] - 1, array4[num35] - 1].HasTile && !Main.tile[array[num33] - 1, array4[num35] - 2].HasTile && !Main.tile[array[num33] - 1, array4[num35] - 3].HasTile && Main.tile[array[num33] - 1, array4[num35] - 1].LiquidAmount == 0 && Main.tile[array[num33] - 1, array4[num35] - 2].LiquidAmount == 0 && Main.tile[array[num33] - 1, array4[num35] - 3].LiquidAmount == 0)
						{
							flag6 = true;
						}
						else
						{
							array7[num35] = false;
						}
					}
				}
				while (flag6)
				{
					int num36 = WorldGen.genRand.Next(10);
					if (array7[num36])
					{
						flag6 = false;
						Tile tile1 = Main.tile[array[num33], array4[num36] - 1];
						tile1.HasTile = false;
						Tile tile2 = Main.tile[array[num33], array4[num36] - 2];
						tile2.HasTile = false;
						Tile tile3 = Main.tile[array[num33], array4[num36] - 3];
						tile3.HasTile = false;
						WorldGen.PlaceTile(array[num33], array4[num36] - 1, ModContent.TileType<QuartzDoorClosed>(), mute: true, forced: false, -1);
					}
				}
				break;
			}
			bool flag7 = false;
			for (int num37 = 4; num37 >= 0; num37--)
			{
				bool[] array8 = new bool[10];
				for (int num38 = 0; num38 < 10; num38++)
				{
					if (array5[num37, num38])
					{
						flag7 = true;
						array8[num38] = true;
					}
				}
				if (flag7)
				{
					bool flag8 = false;
					for (int num39 = 0; num39 < 10; num39++)
					{
						if (array8[num39])
						{
							if (num37 < 20 || num37 > Main.maxTilesX - 20)
							{
								break;
							}
							if (!Main.tile[array2[num37] + 1, array4[num39] - 1].HasTile && !Main.tile[array2[num37] + 1, array4[num39] - 2].HasTile && !Main.tile[array2[num37] + 1, array4[num39] - 3].HasTile && Main.tile[array2[num37] + 1, array4[num39] - 1].LiquidAmount == 0 && Main.tile[array2[num37] + 1, array4[num39] - 2].LiquidAmount == 0 && Main.tile[array2[num37] + 1, array4[num39] - 3].LiquidAmount == 0)
							{
								flag8 = true;
							}
							else
							{
								array8[num39] = false;
							}
						}
					}
					while (flag8)
					{
						int num40 = WorldGen.genRand.Next(10);
						if (array8[num40])
						{
							flag8 = false;
							Tile tile1 = Main.tile[array2[num37], array4[num40] - 1];
							tile1.HasTile = false;
							Tile tile2 = Main.tile[array2[num37], array4[num40] - 2];
							tile2.HasTile = false;
							Tile tile3 = Main.tile[array2[num37], array4[num40] - 3];
							tile3.HasTile = false;
							WorldGen.PlaceTile(array2[num37], array4[num40] - 1, ModContent.TileType<QuartzDoorClosed>(), mute: true, forced: false, -1);
						}
					}
					break;
				}
			}
			bool flag9 = false;
			for (int num41 = 0; num41 < 10; num41++)
			{
				bool[] array9 = new bool[10];
				for (int num42 = 0; num42 < 5; num42++)
				{
					if (array5[num42, num41])
					{
						flag9 = true;
						array9[num42] = true;
					}
				}
				if (!flag9)
				{
					continue;
				}
				bool flag10 = true;
				while (flag10)
				{
					int num43 = WorldGen.genRand.Next(5);
					if (!array9[num43])
					{
						continue;
					}
					int num44 = WorldGen.genRand.Next(array[num43] + 2, array2[num43] - 1);
					int num45 = WorldGen.genRand.Next(array[num43] + 2, array2[num43] - 1);
					int num46 = 0;
					while (num45 - num44 < 2 || num45 - num44 > 5)
					{
						num44 = WorldGen.genRand.Next(array[num43] + 2, array2[num43] - 1);
						num45 = WorldGen.genRand.Next(array[num43] + 2, array2[num43] - 1);
						num46++;
						if (num46 > 10000)
						{
							break;
						}
					}
					if (num46 > 10000)
					{
						break;
					}
					for (int num47 = num44 + (Main.maxTilesX / 2); num47 <= num45 && num47 >= 10 && num47 <= Main.maxTilesX - 10; num47++)
					{
						if (Main.tile[num47, array3[num41] - 1].HasTile || Main.tile[num47, array3[num41] - 1].LiquidAmount > 0)
						{
							flag10 = false;
						}
					}
					if (flag10)
					{
						for (int num48 = num44 + (Main.maxTilesX / 2); num48 <= num45 && num48 >= 10 && num48 <= Main.maxTilesX - 10; num48++)
						{
							Tile tile = Main.tile[num48, array3[num41]];
							tile.HasTile = false;
							WorldGen.PlaceTile(num48, array3[num41], ModContent.TileType<QuartzPlatform>(), mute: true, forced: true, -1);
						}
					}
					flag10 = false;
				}
				break;
			}
		}
		#endregion

		#endregion

		#region DrunkChestStuff
		public static void DepthsShadowchestGenResetterDrunk(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Resetting Shadow Chests";
			List<int> list3 = new List<int> { 274, ModContent.ItemType<Items.Weapons.SilverStar>(), 220, ModContent.ItemType<Items.Weapons.Skyfall>(), 112, ModContent.ItemType<Items.Weapons.WhiteLightning>(), 218, ModContent.ItemType<Items.Weapons.NightFury>(), 3019 };
			if (WorldGen.remixWorldGen)
			{
				list3 = new List<int> { 274, ModContent.ItemType<Items.Weapons.SilverStar>(), 220, ModContent.ItemType<Items.Weapons.Skyfall>(), 683, ModContent.ItemType<Items.Weapons.BlueSphere>(), 218, ModContent.ItemType<Items.Weapons.NightFury>(), 3019 };
			}
			List<int> list4 = new List<int>();
			while (list3.Count > 0)
			{
				int index = WorldGen.genRand.Next(list3.Count);
				int item = list3[index];
				list4.Add(item);
				list3.RemoveAt(index);
			}
			GenVars.hellChestItem = list4.ToArray();
		}

		public static void DepthsBuriedDrunkChests(GenerationProgress progress, GameConfiguration configuration)
		{
			for (int chestID = 0; chestID < (Main.maxChests / 2); chestID++)
			{
				Chest chest = Main.chest[chestID];
				if (chest != null)
				{
					for (int chestslot = 0; chestslot < Chest.maxItems; chestslot++)
					{
						switch (chest.item[chestslot].type)
						{
							case ItemID.LavaCharm:
								chest.item[chestslot].TurnToAir(true);
								chest.item[chestslot].SetDefaults(ModContent.ItemType<Items.Accessories.AmalgamAmulet>());
								break;
							case ItemID.HellMinecart:
								chest.item[chestslot] = new Item(ModContent.ItemType<Items.PhantomFirecart>());
								break;
							case ItemID.HellfireArrow:
								chest.item[chestslot] = new Item(ModContent.ItemType<Items.Weapons.DiamondArrow>());
								chest.item[chestslot].stack = WorldGen.genRand.Next(25) + 50;
								break;
							case ItemID.HellCake:
								chest.item[chestslot] = new Item(ModContent.ItemType<Items.GeodeLazerPointer>());
								break;
							case ItemID.TreasureMagnet:
								chest.item[chestslot].TurnToAir(true);
								chest.item[chestslot].SetDefaults(ModContent.ItemType<Items.Accessories.LodeStone>());
								break;
							case ItemID.ObsidianSkinPotion:
								chest.item[chestslot] = new Item(ModContent.ItemType<Items.CrystalSkinPotion>());
								chest.item[chestslot].stack = WorldGen.genRand.Next(1, 3);
								break;
							case ItemID.InfernoPotion:
								chest.item[chestslot] = new Item(ModContent.ItemType<Items.SilverSpherePotion>());
								chest.item[chestslot].stack = WorldGen.genRand.Next(1, 3);
								break;
							case ItemID.CobaltShield:
								chest.item[chestslot].TurnToAir(true);
								chest.item[chestslot].SetDefaults(ModContent.ItemType<Items.Accessories.PalladiumShield>());
								break;
						}
					}
				}
			}
		}
		#endregion
	}
}
