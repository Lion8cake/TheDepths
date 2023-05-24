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

namespace TheDepths
{    
    public enum UnderworldOptions {
	    Random,
	    Underworld,
	    Depths,
    }
    
    public class TheDepthsWorldGen : ModSystem {
	    public UnderworldOptions SelectedUnderworldOption { get; set; } = UnderworldOptions.Random;
		public static bool depthsorHell;

		public static bool DrunkDepthsLeft;
		public static bool DrunkDepthsRight;

		/// <summary>
		/// Detects if the player is on the depths side of the drunk seed if the depths is on the Right
		/// </summary>
		public static bool IsPlayerInRightDepths => DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2;

		/// <summary>
		///   Detects if the player is on the depths side of the drunk seed if the depths is on the left
		/// </summary>
		public static bool IsPlayerInLeftDepths => DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2;

		/// <summary>
		///   Checks if the player is in the depths part of the world. This is used to reduce repition within code as previously all the check needed was depthsorHell == true.
		/// </summary>
		public static bool InDepths => (depthsorHell && !Main.drunkWorld || (IsPlayerInLeftDepths || IsPlayerInRightDepths) && Main.drunkWorld);

		public override void OnWorldLoad()
		{
			depthsorHell = false;
			DrunkDepthsLeft = false;
			DrunkDepthsRight = false;
		}

		public override void OnWorldUnload()
		{
			depthsorHell = false;
			DrunkDepthsLeft = false;
			DrunkDepthsRight = false;
		}

		public override void SaveWorldData(TagCompound tag)
		{
			if (depthsorHell)
			{
				tag["IsDepths"] = true;
			}
			if (DrunkDepthsLeft)
			{
				tag["DepthsIsOnTheLeft"] = true;
			}
			if (DrunkDepthsRight)
			{
				tag["DepthsIsOnTheRight"] = true;
			}
		}

		public override void LoadWorldData(TagCompound tag)
		{
			depthsorHell = tag.ContainsKey("IsDepths");
			DrunkDepthsLeft = tag.ContainsKey("DepthsIsOnTheLeft");
			DrunkDepthsRight = tag.ContainsKey("DepthsIsOnTheRight");
		}

		public override void PreWorldGen() {
			depthsorHell = SelectedUnderworldOption switch {
				UnderworldOptions.Random => Main.rand.NextBool(),
				UnderworldOptions.Underworld => false,
				UnderworldOptions.Depths => true,
				_ => throw new ArgumentOutOfRangeException(),
			};

			if (Main.drunkWorld)
            {
				if (Main.rand.NextBool(2))
				{
					DrunkDepthsLeft = true;
					DrunkDepthsRight = false;
				}
				else
                {
					DrunkDepthsRight = true;
					DrunkDepthsLeft = false;
                }
            }
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            if (depthsorHell && !Main.drunkWorld)
            {
                int index2 = tasks.FindIndex(genpass => genpass.Name.Equals("Underworld"));
                int index3 = tasks.FindIndex(genpass => genpass.Name.Equals("Hellforge"));
                int index4 = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
                if (index2 != -1)
                {
                    tasks.Insert(index2 + 1, new PassLegacy("Underworld", new WorldGenLegacyMethod(Depths)));
                    tasks.RemoveAt(index2);
                    index2 = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
                    tasks.Insert(index2 + 1, new PassLegacy("Pots", new WorldGenLegacyMethod(Pots)));
                    tasks.RemoveAt(index2);
                }

                if (index3 != -1)
                {
                    tasks.Insert(index3 + 1, new PassLegacy("Hellforge", new WorldGenLegacyMethod(Gemforge)));
                    tasks.Insert(index3 + 2, new PassLegacy("The Depths: Trees", new WorldGenLegacyMethod(TreeGen)));
                    tasks.RemoveAt(index3);
                }

				if (index4 != -1)
				{
					tasks.Insert(index4 + 1, new PassLegacy("Quicksilver Droplets", new WorldGenLegacyMethod(DrippingQuicksilverTileCleanup)));
				}
			}
			if (DrunkDepthsLeft && Main.drunkWorld)
			{
				int index2 = tasks.FindIndex(genpass => genpass.Name.Equals("Underworld"));
				int index3 = tasks.FindIndex(genpass => genpass.Name.Equals("Hellforge"));
				int index4 = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
				int index5 = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
				if (index2 != -1)
				{
					tasks.Insert(index2 + 1, new PassLegacy("DepthsLeft", new WorldGenLegacyMethod(TheDepthsDrunkGen.DepthsLeft)));
				}

				if (index3 != -1)
				{
					tasks.Insert(index3 + 1, new PassLegacy("GemForges", new WorldGenLegacyMethod(TheDepthsDrunkGen.GemforgeLeft)));
					tasks.Insert(index3 + 2, new PassLegacy("PetrifyingTrees", new WorldGenLegacyMethod(TheDepthsDrunkGen.TreeGenLeft)));
				}

				if (index4 != -1)
				{
					tasks.Insert(index4 + 1, new PassLegacy("DepthsPots", new WorldGenLegacyMethod(TheDepthsDrunkGen.PotsLeft)));
				}

				if (index5 != -1)
				{
					tasks.Insert(index5 + 1, new PassLegacy("KillingHellPots", new WorldGenLegacyMethod(TheDepthsDrunkGen.KILLTHEPOTSLeft)));
					tasks.Insert(index5 + 2, new PassLegacy("Quicksilver Droplets", new WorldGenLegacyMethod(TheDepthsDrunkGen.DrippingQuicksilverTileCleanupLeft)));
				}
			}
			if (DrunkDepthsRight && Main.drunkWorld)
			{
				int index2 = tasks.FindIndex(genpass => genpass.Name.Equals("Underworld"));
				int index3 = tasks.FindIndex(genpass => genpass.Name.Equals("Hellforge"));
				int index4 = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
				int index5 = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
				if (index2 != -1)
				{
					tasks.Insert(index2 + 1, new PassLegacy("DepthsRight", new WorldGenLegacyMethod(TheDepthsDrunkGen.DepthsRight)));
				}

				if (index3 != -1)
				{
					tasks.Insert(index3 + 1, new PassLegacy("GemForges", new WorldGenLegacyMethod(TheDepthsDrunkGen.GemforgeRight)));
					tasks.Insert(index3 + 2, new PassLegacy("PetrifyingTrees", new WorldGenLegacyMethod(TheDepthsDrunkGen.TreeGenRight)));
				}

				if (index4 != -1)
				{
					tasks.Insert(index4 + 1, new PassLegacy("DepthsPots", new WorldGenLegacyMethod(TheDepthsDrunkGen.PotsRight)));
				}

				if (index5 != -1)
				{
					tasks.Insert(index5 + 1, new PassLegacy("KillingHellPots", new WorldGenLegacyMethod(TheDepthsDrunkGen.KILLTHEPOTSRight)));
					tasks.Insert(index5 + 2, new PassLegacy("Quicksilver Droplets", new WorldGenLegacyMethod(TheDepthsDrunkGen.DrippingQuicksilverTileCleanupRight)));
				}
			}
		}

        public override void ModifyHardmodeTasks(List<GenPass> list)
		{
			if (depthsorHell || WorldGen.drunkWorldGen)
			{
				list.Add(new PassLegacy("The Depths: Onyx Shalestone", new WorldGenLegacyMethod(OnyxShale)));
			}
		}

		private static void OnyxShale(GenerationProgress progres, GameConfiguration configurations)
		{
			OnyxShale();
		}

		public static void DrippingQuicksilverTileCleanup(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Placing Dripping Quicksilver";
			for (int k = 0; k < Main.maxTilesX; k++)
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
				}
			}
		}

		/*public static void AmalgamAmuletReplacor(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Shuffling Chests";
			for (int k = 0; k < Main.maxTilesX; k++)
			{
				for (int l = 0; l < Main.maxTilesY; l++)
				{
					if (Main.tile[l, k].TileType == 21)
					{
						if (Main.chest[ItemID.LavaCharm].item[906].type == ItemID.LavaCharm)
                        {

                        }
					}
				}
			}
		}*/

		private void Pots(GenerationProgress progress, GameConfiguration configuration)
		{
			Main.tileSolid[137] = true;
			Main.tileSolid[130] = true;
			progress.Message = Lang.gen[35].Value;
			if (WorldGen.noTrapsWorldGen)
			{
				Main.tileSolid[138] = true;
				int num440 = (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0004);
				if (WorldGen.remixWorldGen)
				{
					num440 /= 2;
				}
				for (int num441 = 0; num441 < num440; num441++)
				{
					int num442 = WorldGen.genRand.Next(50, Main.maxTilesX - 50);
					int num443;
					for (num443 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 250); !Main.tile[num442, num443].HasTile && num443 < Main.maxTilesY - 250; num443++)
					{
					}
					num443--;
					if (!(Main.tile[num442, num443].LiquidType == LiquidID.Shimmer))
					{
						WorldGen.PlaceTile(num442, num443, 138, mute: true);
						WorldGen.PlaceTile(num442 + 2, num443, 138, mute: true);
						WorldGen.PlaceTile(num442 + 1, num443 - 2, 138, mute: true);
					}
				}
				Main.tileSolid[138] = false;
			}
			double num444 = (double)(Main.maxTilesX * Main.maxTilesY) * 0.0008;
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
					int num449 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
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
							int num451 = 0;
							int num452 = 0;
							if (num450 < Main.maxTilesY - 5)
							{
								num451 = Main.tile[num449, num450 + 1].TileType;
								num452 = Main.tile[num449, num450].WallType;
							}
							if (num451 == 147 || num451 == 161 || num451 == 162)
							{
								style = WorldGen.genRand.Next(4, 7);
							}
							if (num451 == 60)
							{
								style = WorldGen.genRand.Next(7, 10);
							}
							if (Main.wallDungeon[Main.tile[num449, num450].WallType])
							{
								style = WorldGen.genRand.Next(10, 13);
							}
							if (num451 == 41 || num451 == 43 || num451 == 44 || num451 == 481 || num451 == 482 || num451 == 483)
							{
								style = WorldGen.genRand.Next(10, 13);
							}
							if (num451 == 22 || num451 == 23 || num451 == 25)
							{
								style = WorldGen.genRand.Next(16, 19);
							}
							if (num451 == 199 || num451 == 203 || num451 == 204 || num451 == 200)
							{
								style = WorldGen.genRand.Next(22, 25);
							}
							if (num451 == 367)
							{
								style = WorldGen.genRand.Next(31, 34);
							}
							if (num451 == 226)
							{
								style = WorldGen.genRand.Next(28, 31);
							}
							if (num452 == 187 || num452 == 216)
							{
								style = WorldGen.genRand.Next(34, 37);
							}
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

		public static void Gemforge(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Placing gemforges";
            for (int index1 = 0; index1 < Main.maxTilesX / 200; ++index1)
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

		public static void OnyxShale()
		{
			float num1 = Main.maxTilesX * 0.5f;
			float num2 = num1 * 0.2f;
			for (int index = 0; index < num2; ++index)
			{
				int i = WorldGen.genRand.Next(0, Main.maxTilesX);
				int j;
				for (j = WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY); Main.tile[i, j].TileType != TileType<Shalestone>(); j = WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY))
					i = WorldGen.genRand.Next(0, Main.maxTilesX);
				WorldGen.TileRunner(i, j, WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(3, 7), TileType<OnyxShalestone>(), false, 0.0f, 0.0f, false, true);
			}
		}

        private static void Depths(GenerationProgress progress, GameConfiguration configuration)
        {
            {
				progress.Message = "Creating depths";
				progress.Set(0.0f);
				int num838 = Main.maxTilesY -WorldGen.genRand.Next(150, 190);
				for (int num839 = 0; num839 < Main.maxTilesX; num839++)
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
					for (int num840 = num838 - 20 -WorldGen.genRand.Next(3); num840 < Main.maxTilesY; num840++)
					{
						if (num840 >= num838)
						{
							Tile tile = Main.tile[num839, num840];
							tile.HasTile = false;
							tile.LiquidType = 1;
							Main.tile[num839, num840].LiquidAmount = 0;
						}
						else
						{
							Main.tile[num839, num840].TileType = (ushort)ModContent.TileType<ShaleBlock>();
						}
					}
				}
				int num841 = Main.maxTilesY - WorldGen.genRand.Next(40, 70);
				for (int num842 = 10; num842 < Main.maxTilesX - 10; num842++)
				{
					num841 +=WorldGen.genRand.Next(-10, 11);
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
				for (int num844 = 0; num844 < Main.maxTilesX; num844++)
				{
					if (WorldGen.genRand.Next(50) == 0)
					{
						int num845 = Main.maxTilesY - 65;
						while (!Main.tile[num844, num845].HasTile && num845 > Main.maxTilesY - 135)
						{
							num845--;
						}
						WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), num845 +WorldGen.genRand.Next(20, 50),WorldGen.genRand.Next(15, 20), 1000, ModContent.TileType<ShaleBlock>(), addTile: true, 0.0,WorldGen.genRand.Next(1, 3), noYChange: true);
					}
				}
				Liquid.QuickWater(-2);
				for (int num846 = 0; num846 < Main.maxTilesX; num846++)
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
						if ((!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen) ||WorldGen.genRand.Next(3) == 0 || !((double)num846 > (double)Main.maxTilesX * 0.4) || !((double)num846 < (double)Main.maxTilesX * 0.6))
						{
							WorldGen.TileRunner(num846, num848 -WorldGen.genRand.Next(2, 5),WorldGen.genRand.Next(5, 30), 1000, ModContent.TileType<ShaleBlock>(), addTile: true, 0.0,WorldGen.genRand.Next(1, 3), noYChange: true);
						}
						double num849 =WorldGen.genRand.Next(1, 3);
						if (WorldGen.genRand.Next(3) == 0)
						{
							num849 *= 0.5;
						}
						if ((!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen) ||WorldGen.genRand.Next(3) == 0 || !((double)num846 > (double)Main.maxTilesX * 0.4) || !((double)num846 < (double)Main.maxTilesX * 0.6))
						{
							if (WorldGen.genRand.Next(2) == 0)
							{
								WorldGen.TileRunner(num846, num848 -WorldGen.genRand.Next(2, 5), (int)((double)WorldGen.genRand.Next(5, 15) * num849), (int)((double)WorldGen.genRand.Next(10, 15) * num849), ModContent.TileType<ShaleBlock>(), addTile: true, 1.0, 0.3);
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								num849 = WorldGen.genRand.Next(1, 3);
								WorldGen.TileRunner(num846, num848 -WorldGen.genRand.Next(2, 5), (int)((double)WorldGen.genRand.Next(5, 15) * num849), (int)((double)WorldGen.genRand.Next(10, 15) * num849), ModContent.TileType<ShaleBlock>(), addTile: true, -1.0, 0.3);
							}
						}
						WorldGen.TileRunner(num846 +WorldGen.genRand.Next(-10, 10), num848 +WorldGen.genRand.Next(-10, 10),WorldGen.genRand.Next(5, 15),WorldGen.genRand.Next(5, 10), -2, addTile: false,WorldGen.genRand.Next(-1, 3),WorldGen.genRand.Next(-1, 3));
						if (WorldGen.genRand.Next(3) == 0)
						{
							WorldGen.TileRunner(num846 +WorldGen.genRand.Next(-10, 10), num848 +WorldGen.genRand.Next(-10, 10),WorldGen.genRand.Next(10, 30),WorldGen.genRand.Next(10, 20), -2, addTile: false,WorldGen.genRand.Next(-1, 3),WorldGen.genRand.Next(-1, 3));
						}
						if (WorldGen.genRand.Next(5) == 0)
						{
							WorldGen.TileRunner(num846 +WorldGen.genRand.Next(-15, 15), num848 +WorldGen.genRand.Next(-15, 10),WorldGen.genRand.Next(15, 30),WorldGen.genRand.Next(5, 20), -2, addTile: false,WorldGen.genRand.Next(-1, 3),WorldGen.genRand.Next(-1, 3));
						}
					}
				}
				for (int num850 = 0; num850 < Main.maxTilesX; num850++)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(20, Main.maxTilesX - 20),WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10),WorldGen.genRand.Next(2, 7),WorldGen.genRand.Next(2, 7), -2);
				}
				if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
				{
					for (int num851 = 0; num851 < Main.maxTilesX * 2; num851++)
					{
						WorldGen.TileRunner(WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.35), (int)((double)Main.maxTilesX * 0.65)),WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10),WorldGen.genRand.Next(5, 20),WorldGen.genRand.Next(5, 10), -2);
					}
				}
				for (int num852 = 0; num852 < Main.maxTilesX; num852++)
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
				for (int num853 = 0; num853 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num853++)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY),WorldGen.genRand.Next(2, 7),WorldGen.genRand.Next(3, 7), TileType<ArqueriteOre>());
				}
				for (int index = 0; index < (int)(Main.maxTilesX * Main.maxTilesY * 0.0008); ++index)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(5, 9), TileType<Shalestone>(), false, 0.0f, 0.0f, false, true);
				}
				Gems();
				if (WorldGen.remixWorldGen)
				{
					int num854 = (int)((double)Main.maxTilesX * 0.38);
					int num855 = (int)((double)Main.maxTilesX * 0.62);
					int num856 = num854;
					int num857 = Main.maxTilesY - 1;
					int num858 = Main.maxTilesY - 135;
					int num859 = Main.maxTilesY - 160;
					bool flag55 = false;
					Liquid.QuickWater(-2);
					for (; num857 < Main.maxTilesY - 1 || num856 < num855; num856++)
					{
						if (!flag55)
						{
							num857 -=WorldGen.genRand.Next(1, 4);
							if (num857 < num858)
							{
								flag55 = true;
							}
						}
						else if (num856 >= num855)
						{
							num857 +=WorldGen.genRand.Next(1, 4);
							if (num857 > Main.maxTilesY - 1)
							{
								num857 = Main.maxTilesY - 1;
							}
						}
						else
						{
							if ((num856 <= Main.maxTilesX / 2 - 5 || num856 >= Main.maxTilesX / 2 + 5) &&WorldGen.genRand.Next(4) == 0)
							{
								if (WorldGen.genRand.Next(3) == 0)
								{
									num857 +=WorldGen.genRand.Next(-1, 2);
								}
								else if (WorldGen.genRand.Next(6) == 0)
								{
									num857 +=WorldGen.genRand.Next(-2, 3);
								}
								else if (WorldGen.genRand.Next(8) == 0)
								{
									num857 +=WorldGen.genRand.Next(-4, 5);
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
							tile = new Tile();
							tile.HasTile = true;
							Main.tile[num856, num861].TileType = (ushort)TileType<ShaleBlock>();
						}
					}
					Liquid.QuickWater(-2);
					for (int num862 = num854; num862 < num855 + 15; num862++)
					{
						for (int num863 = Main.maxTilesY - 300; num863 < num858 + 20; num863++)
						{
							Main.tile[num862, num863].LiquidAmount = 0;
							if (Main.tile[num862, num863].TileType == ModContent.TileType<ShaleBlock>() && Main.tile[num862, num863].HasTile && (!Main.tile[num862 - 1, num863 - 1].HasTile || !Main.tile[num862, num863 - 1].HasTile || !Main.tile[num862 + 1, num863 - 1].HasTile || !Main.tile[num862 - 1, num863].HasTile || !Main.tile[num862 + 1, num863].HasTile || !Main.tile[num862 - 1, num863 + 1].HasTile || !Main.tile[num862, num863 + 1].HasTile || !Main.tile[num862 + 1, num863 + 1].HasTile))
							{
								Main.tile[num862, num863].TileType = (ushort)ModContent.TileType<NightmareGrass>();
							}
						}
					}
					for (int num864 = num854; num864 < num855 + 15; num864++)
					{
						for (int num865 = Main.maxTilesY - 200; num865 < num858 + 20; num865++)
						{
							if (Main.tile[num864, num865].TileType == ModContent.TileType<NightmareGrass>() && Main.tile[num864, num865].HasTile && !Main.tile[num864, num865 - 1].HasTile &&WorldGen.genRand.Next(3) == 0)
							{
								WorldGen.GrowTree(num864, num865);
							}
						}
					}
				}
				else if (!WorldGen.drunkWorldGen)
				{
					for (int num866 = 25; num866 < Main.maxTilesX - 25; num866++)
					{
						if ((double)num866 < (double)Main.maxTilesX * 0.17 || (double)num866 > (double)Main.maxTilesX * 0.83)
						{
							for (int num867 = Main.maxTilesY - 300; num867 < Main.maxTilesY - 100 +WorldGen.genRand.Next(-1, 2); num867++)
							{
								if (Main.tile[num866, num867].TileType == ModContent.TileType<ShaleBlock>() && Main.tile[num866, num867].HasTile && (!Main.tile[num866 - 1, num867 - 1].HasTile || !Main.tile[num866, num867 - 1].HasTile || !Main.tile[num866 + 1, num867 - 1].HasTile || !Main.tile[num866 - 1, num867].HasTile || !Main.tile[num866 + 1, num867].HasTile || !Main.tile[num866 - 1, num867 + 1].HasTile || !Main.tile[num866, num867 + 1].HasTile || !Main.tile[num866 + 1, num867 + 1].HasTile))
								{
									Main.tile[num866, num867].TileType = (ushort)ModContent.TileType<NightmareGrass>();
								}
							}
						}
					}
					for (int num868 = 25; num868 < Main.maxTilesX - 25; num868++)
					{
						if ((double)num868 < (double)Main.maxTilesX * 0.17 || (double)num868 > (double)Main.maxTilesX * 0.83)
						{
							for (int num869 = Main.maxTilesY - 200; num869 < Main.maxTilesY - 50; num869++)
							{
								if (Main.tile[num868, num869].TileType == ModContent.TileType<NightmareGrass>() && Main.tile[num868, num869].HasTile && !Main.tile[num868, num869 - 1].HasTile &&WorldGen.genRand.Next(3) == 0)
								{
									WorldGen.GrowTree(num868, num869);
								}
							}
						}
					}
				}
				else
				{
					for (int num866 = 25; num866 < Main.maxTilesX - 25; num866++)
					{
						for (int num867 = Main.maxTilesY - 300; num867 < Main.maxTilesY - 100 + WorldGen.genRand.Next(-1, 2); num867++)
						{
							if (Main.tile[num866, num867].TileType == ModContent.TileType<ShaleBlock>() && Main.tile[num866, num867].HasTile && (!Main.tile[num866 - 1, num867 - 1].HasTile || !Main.tile[num866, num867 - 1].HasTile || !Main.tile[num866 + 1, num867 - 1].HasTile || !Main.tile[num866 - 1, num867].HasTile || !Main.tile[num866 + 1, num867].HasTile || !Main.tile[num866 - 1, num867 + 1].HasTile || !Main.tile[num866, num867 + 1].HasTile || !Main.tile[num866 + 1, num867 + 1].HasTile))
							{
								Main.tile[num866, num867].TileType = (ushort)ModContent.TileType<NightmareGrass>();
							}
						}
					}
					for (int num868 = 25; num868 < Main.maxTilesX / 2 - 25; num868++)
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
				AddDepthHouses();
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

		public static void AddDepthHouses()
		{
			int num = (int)((double)Main.maxTilesX * 0.25);
			for (int i = 100; i < Main.maxTilesX - 100; i++)
			{
				if (((!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen) || i <= num || i >= Main.maxTilesX - num) && (WorldGen.drunkWorldGen || WorldGen.remixWorldGen || (i >= num && i <= Main.maxTilesX - num)))
				{
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
						DepthFort(i, num2, num3, wallType);
						i += WorldGen.genRand.Next(30, 130);
						if (WorldGen.genRand.Next(10) == 0)
						{
							i += WorldGen.genRand.Next(0, 200);
						}
					}
				}
			}
			float num4 = (float)(Main.maxTilesX / 4200);
			int num5 = 0;
			while ((float)num5 < 200f * num4)
			{
				int num6 = 0;
				bool flag = false;
				while (!flag)
				{
					num6++;
					int num7 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.2), (int)((double)Main.maxTilesX * 0.8));
					int num8 = WorldGen.genRand.Next(Main.maxTilesY - 300, Main.maxTilesY - 20);
					if (Main.tile[num7, num8].HasTile && (Main.tile[num7, num8].TileType == TileType<QuartzBricks>() || Main.tile[num7, num8].TileType == TileType<ArqueriteBricks>()))
					{
						int num9 = 0;
						if (Main.tile[num7 - 1, num8].WallType > 0)
						{
							num9 = -1;
						}
						else if (Main.tile[num7 + 1, num8].WallType > 0)
						{
							num9 = 1;
						}
						if (!Main.tile[num7 + num9, num8].HasTile && !Main.tile[num7 + num9, num8 + 1].HasTile)
						{
							bool flag2 = false;
							for (int j = num7 - 8; j < num7 + 8; j++)
							{
								for (int k = num8 - 8; k < num8 + 8; k++)
								{
									if (Main.tile[j, k].HasTile && Main.tile[j, k].TileType == 4)
									{
										flag2 = true;
										break;
									}
								}
							}
							if (!flag2)
							{
								WorldGen.PlaceTile(num7 + num9, num8, TileType<GeoTorch>(), true, true, -1);
								flag = true;
							}
						}
					}
					if (num6 > 1000)
					{
						flag = true;
					}
				}
				num5++;
			}
			double num10 = 4200000.0 / (double)Main.maxTilesX;
			int num11 = 0;
			while ((double)num11 < num10)
			{
				int num12 = 0;
				int num13 = WorldGen.genRand.Next(num, Main.maxTilesX - num);
				int num14 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
				while ((Main.tile[num13, num14].WallType != 13 && Main.tile[num13, num14].WallType != 14) || Main.tile[num13, num14].HasTile)
				{
					num13 = WorldGen.genRand.Next(num, Main.maxTilesX - num);
					num14 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
					if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
					{
						if (WorldGen.genRand.Next(2) == 0)
						{
							num13 = WorldGen.genRand.Next(50, num);
						}
						else
						{
							num13 = WorldGen.genRand.Next(Main.maxTilesX - num, Main.maxTilesX - 50);
						}
					}
					num12++;
					if (num12 > 100000)
					{
						break;
					}
				}
				if (num12 <= 100000 && (Main.tile[num13, num14].WallType == WallType<QuartzBrickWallUnsafe>() || Main.tile[num13, num14].WallType == WallType<ArqueriteBrickWallUnsafe>()) && !Main.tile[num13, num14].HasTile)
				{
					while (!WorldGen.SolidTile(num13, num14, false) && num14 < Main.maxTilesY - 20)
					{
						num14++;
					}
					num14--;
					int num15 = num13;
					int num16 = num13;
					while (!Main.tile[num15, num14].HasTile && WorldGen.SolidTile(num15, num14 + 1, false))
					{
						num15--;
					}
					num15++;
					while (!Main.tile[num16, num14].HasTile && WorldGen.SolidTile(num16, num14 + 1, false))
					{
						num16++;
					}
					num16--;
					int num17 = num16 - num15;
					int num18 = (num16 + num15) / 2;
					if (!Main.tile[num18, num14].HasTile && (Main.tile[num18, num14].WallType == 13 || Main.tile[num18, num14].WallType == 14) && WorldGen.SolidTile(num18, num14 + 1, false))
					{
						int style = 16;
						int style2 = 13;
						int style3 = 14;
						int style4 = 49;
						int style5 = 4;
						int style6 = 8;
						int style7 = 15;
						int style8 = 9;
						int style9 = 10;
						int style10 = 17;
						int style11 = 25;
						int style12 = 25;
						int style13 = 23;
						int style14 = 25;
						int num19 = WorldGen.genRand.Next(13);
						int num20 = 0;
						int num21 = 0;
						if (num19 == 0)
						{
							num20 = 5;
							num21 = 4;
						}
						if (num19 == 1)
						{
							num20 = 4;
							num21 = 3;
						}
						if (num19 == 2)
						{
							num20 = 3;
							num21 = 5;
						}
						if (num19 == 3)
						{
							num20 = 4;
							num21 = 6;
						}
						if (num19 == 4)
						{
							num20 = 3;
							num21 = 3;
						}
						if (num19 == 5)
						{
							num20 = 5;
							num21 = 3;
						}
						if (num19 == 6)
						{
							num20 = 5;
							num21 = 4;
						}
						if (num19 == 7)
						{
							num20 = 5;
							num21 = 4;
						}
						if (num19 == 8)
						{
							num20 = 5;
							num21 = 4;
						}
						if (num19 == 9)
						{
							num20 = 3;
							num21 = 5;
						}
						if (num19 == 10)
						{
							num20 = 5;
							num21 = 3;
						}
						if (num19 == 11)
						{
							num20 = 2;
							num21 = 4;
						}
						if (num19 == 12)
						{
							num20 = 3;
							num21 = 3;
						}
						for (int l = num18 - num20; l <= num18 + num20; l++)
						{
							for (int m = num14 - num21; m <= num14; m++)
							{
								if (Main.tile[l, m].HasTile)
								{
									num19 = -1;
									break;
								}
							}
						}
						if ((double)num17 < (double)num20 * 1.75)
						{
							num19 = -1;
						}


						if (num19 == 0)
						{
							WorldGen.PlaceTile(num18, num14, 14, true, false, -1, style2);
							int num22 = WorldGen.genRand.Next(6);
							if (num22 < 3)
							{
								WorldGen.PlaceTile(num18 + num22, num14 - 2, 33, true, false, -1, style12);
							}
							if (Main.tile[num18, num14].HasTile)
							{
								if (!Main.tile[num18 - 2, num14].HasTile)
								{
									WorldGen.PlaceTile(num18 - 2, num14, 15, true, false, -1, style);
									if (Main.tile[num18 - 2, num14].HasTile)
									{
										Tile tile = Main.tile[num18 - 2, num14];
										tile.TileFrameX += 18;
										Tile tile2 = Main.tile[num18 - 2, num14 - 1];
										tile2.TileFrameX += 18;
									}
								}
								if (!Main.tile[num18 + 2, num14].HasTile)
								{
									WorldGen.PlaceTile(num18 + 2, num14, 15, true, false, -1, style);
								}
							}
						}
						else if (num19 == 1)
						{
							WorldGen.PlaceTile(num18, num14, 18, true, false, -1, style3);
							int num23 = WorldGen.genRand.Next(4);
							if (num23 < 2)
							{
								WorldGen.PlaceTile(num18 + num23, num14 - 1, 33, true, false, -1, style12);
							}
							if (Main.tile[num18, num14].HasTile)
							{
								if (WorldGen.genRand.Next(2) == 0)
								{
									if (!Main.tile[num18 - 1, num14].HasTile)
									{
										WorldGen.PlaceTile(num18 - 1, num14, 15, true, false, -1, style);
										if (Main.tile[num18 - 1, num14].HasTile)
										{
											Tile tile3 = Main.tile[num18 - 1, num14];
											tile3.TileFrameX += 18;
											Tile tile4 = Main.tile[num18 - 1, num14 - 1];
											tile4.TileFrameX += 18;
										}
									}
								}
								else if (!Main.tile[num18 + 2, num14].HasTile)
								{
									WorldGen.PlaceTile(num18 + 2, num14, 15, true, false, -1, style);
								}
							}
						}
						else if (num19 == 2)
						{
							WorldGen.PlaceTile(num18, num14, 105, true, false, -1, style4);
						}
						else if (num19 == 3)
						{
							WorldGen.PlaceTile(num18, num14, 101, true, false, -1, style5);
						}
						else if (num19 == 4)
						{
							if (WorldGen.genRand.Next(2) == 0)
							{
								WorldGen.PlaceTile(num18, num14, 15, true, false, -1, style);
								Tile tile5 = Main.tile[num18, num14];
								tile5.TileFrameX += 18;
								Tile tile6 = Main.tile[num18, num14 - 1];
								tile6.TileFrameX += 18;
							}
							else
							{
								WorldGen.PlaceTile(num18, num14, 15, true, false, -1, style);
							}
						}
						else if (num19 == 5)
						{
							if (WorldGen.genRand.Next(2) == 0)
							{
								WorldGen.Place4x2(num18, num14, 79, 1, style6);
							}
							else
							{
								WorldGen.Place4x2(num18, num14, 79, -1, style6);
							}
						}
						else if (num19 == 6)
						{
							WorldGen.PlaceTile(num18, num14, 87, true, false, -1, style7);
						}
						else if (num19 == 7)
						{
							WorldGen.PlaceTile(num18, num14, 88, true, false, -1, style8);
						}
						else if (num19 == 8)
						{
							WorldGen.PlaceTile(num18, num14, 89, true, false, -1, style9);
						}
						else if (num19 == 9)
						{
							WorldGen.PlaceTile(num18, num14, 104, true, false, -1, style10);
						}
						else if (num19 == 10)
						{
							if (WorldGen.genRand.Next(2) == 0)
							{
								WorldGen.Place4x2(num18, num14, 90, 1, style14);
							}
							else
							{
								WorldGen.Place4x2(num18, num14, 90, -1, style14);
							}
						}
						else if (num19 == 11)
						{
							WorldGen.PlaceTile(num18, num14, 93, true, false, -1, style13);
						}
						else if (num19 == 12)
						{
							WorldGen.PlaceTile(num18, num14, 100, true, false, -1, style11);
						}
					}
				}
				num11++;
			}
			num10 = 420000.0 / (double)Main.maxTilesX;
			int num24 = 0;
			while ((double)num24 < num10)
			{
				int num25 = 0;
				int num26 = WorldGen.genRand.Next(num, Main.maxTilesX - num);
				int num27 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
				while ((Main.tile[num26, num27].WallType != 13 && Main.tile[num26, num27].WallType != 14) || Main.tile[num26, num27].HasTile)
				{
					num26 = WorldGen.genRand.Next(num, Main.maxTilesX - num);
					num27 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
					if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
					{
						if (WorldGen.genRand.Next(2) == 0)
						{
							num26 = WorldGen.genRand.Next(50, num);
						}
						else
						{
							num26 = WorldGen.genRand.Next(Main.maxTilesX - num, Main.maxTilesX - 50);
						}
					}
					num25++;
					if (num25 > 100000)
					{
						break;
					}
				}
				if (num25 <= 100000)
				{
					int num28;
					int num29;
					int num30;
					int num31;
					for (int n = 0; n < 2; n++)
					{
						num28 = num26;
						num29 = num26;
						while (!Main.tile[num28, num27].HasTile && (Main.tile[num28, num27].WallType == 13 || Main.tile[num28, num27].WallType == 14))
						{
							num28--;
						}
						num28++;
						while (!Main.tile[num29, num27].HasTile && (Main.tile[num29, num27].WallType == 13 || Main.tile[num29, num27].WallType == 14))
						{
							num29++;
						}
						num29--;
						num26 = (num28 + num29) / 2;
						num30 = num27;
						num31 = num27;
						while (!Main.tile[num26, num30].HasTile && (Main.tile[num26, num30].WallType == 13 || Main.tile[num26, num30].WallType == 14))
						{
							num30--;
						}
						num30++;
						while (!Main.tile[num26, num31].HasTile && (Main.tile[num26, num31].WallType == 13 || Main.tile[num26, num31].WallType == 14))
						{
							num31++;
						}
						num31--;
						num27 = (num30 + num31) / 2;
					}
					num28 = num26;
					num29 = num26;
					while (!Main.tile[num28, num27].HasTile && !Main.tile[num28, num27 - 1].HasTile && !Main.tile[num28, num27 + 1].HasTile)
					{
						num28--;
					}
					num28++;
					while (!Main.tile[num29, num27].HasTile && !Main.tile[num29, num27 - 1].HasTile && !Main.tile[num29, num27 + 1].HasTile)
					{
						num29++;
					}
					num29--;
					num30 = num27;
					num31 = num27;
					while (!Main.tile[num26, num30].HasTile && !Main.tile[num26 - 1, num30].HasTile && !Main.tile[num26 + 1, num30].HasTile)
					{
						num30--;
					}
					num30++;
					while (!Main.tile[num26, num31].HasTile && !Main.tile[num26 - 1, num31].HasTile && !Main.tile[num26 + 1, num31].HasTile)
					{
						num31++;
					}
					num31--;
					num26 = (num28 + num29) / 2;
					num27 = (num30 + num31) / 2;
					int num32 = num29 - num28;
					int num33 = num31 - num30;
					if (num32 > 7 && num33 > 5)
					{
						int num34 = 0;
						if (WorldGen.nearPicture2(num26, num27))
						{
							num34 = -1;
						}
						if (num34 == 0)
						{
							/*PaintingEntry paintingEntry = WorldGen.RandHellPicture();
							if (!WorldGen.nearPicture(num26, num27))
							{
								WorldGen.PlaceTile(num26, num27, paintingEntry.tileType, true, false, -1, paintingEntry.style);
							}*/
						}
					}
				}
				num24++;
			}
			int[] array = new int[]
			{
				WorldGen.genRand.Next(16, 22),
				WorldGen.genRand.Next(16, 22),
				WorldGen.genRand.Next(16, 22)
			};
			while (array[1] == array[0])
			{
				array[1] = WorldGen.genRand.Next(16, 22);
			}
			while (array[2] == array[0] || array[2] == array[1])
			{
				array[2] = WorldGen.genRand.Next(16, 22);
			}
			num10 = 420000.0 / (double)Main.maxTilesX;
			int num35 = 0;
			while ((double)num35 < num10)
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
						if (WorldGen.genRand.Next(2) == 0)
						{
							num37 = WorldGen.genRand.Next(50, num);
						}
						else
						{
							num37 = WorldGen.genRand.Next(Main.maxTilesX - num, Main.maxTilesX - 50);
						}
					}
					num36++;
				}
				while (num36 <= 100000 && ((Main.tile[num37, num38].WallType != 13 && Main.tile[num37, num38].WallType != 14) || Main.tile[num37, num38].HasTile));
				if (num36 <= 100000)
				{
					while (!WorldGen.SolidTile(num37, num38, false) && num38 > 10)
					{
						num38--;
					}
					num38++;
					if (Main.tile[num37, num38].WallType == 13 || Main.tile[num37, num38].WallType == 14)
					{
						int num39 = WorldGen.genRand.Next(3);
						int style15 = 32;
						int style16 = 32;
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
								Tile tile7 = Main.tile[num37, num38];
								if (num42 < num37 || num42 == num37 + num40)
								{
									if (tile7.HasTile)
									{
										ushort type = tile7.TileType;
										if (type <= 34)
										{
											if (type - 10 > 1 && type != 34)
											{
												goto IL_11B9;
											}
										}
										else if (type != 42 && type != 91)
										{
											goto IL_11B9;
										}
										num39 = -1;
									}
								}
								else if (tile7.HasTile)
								{
									num39 = -1;
								}
							IL_11B9:;
							}
						}
						switch (num39)
						{
							case 0:
								WorldGen.PlaceTile(num37, num38, 91, true, false, -1, array[WorldGen.genRand.Next(3)]);
								break;
							case 1:
								WorldGen.PlaceTile(num37, num38, 34, true, false, -1, style15);
								break;
							case 2:
								WorldGen.PlaceTile(num37, num38, 42, true, false, -1, style16);
								break;
						}
					}
				}
				num35++;
			}
		}

		public static void DepthFort(int i, int j, ushort tileType = 75, ushort wallType = 14)
		{
			int[] array14 = new int[5];
			int[] array13 = new int[5];
			int[] array12 = new int[10];
			int[] array11 = new int[10];
			int num65 = 8;
			int num64 = 20;
			if (WorldGen.drunkWorldGen)
			{
				num65 /= 2;
				num64 *= 2;
			}
			array14[2] = i - WorldGen.genRand.Next(num65 / 2, num64 / 2);
			array13[2] = i + WorldGen.genRand.Next(num65 / 2, num64 / 2);
			array14[3] = array13[2];
			array13[3] = array14[3] + WorldGen.genRand.Next(num65, num64);
			array14[4] = array13[3];
			array13[4] = array14[4] + WorldGen.genRand.Next(num65, num64);
			array13[1] = array14[2];
			array14[1] = array13[1] - WorldGen.genRand.Next(num65, num64);
			array13[0] = array14[1];
			array14[0] = array13[0] - WorldGen.genRand.Next(num65, num64);
			num65 = 6;
			num64 = 12;
			array12[3] = j - WorldGen.genRand.Next(num65, num64);
			array11[3] = j;
			for (int n = 4; n < 10; n++)
			{
				array12[n] = array11[n - 1];
				array11[n] = array12[n] + WorldGen.genRand.Next(num65, num64);
			}
			for (int num61 = 2; num61 >= 0; num61--)
			{
				array11[num61] = array12[num61 + 1];
				array12[num61] = array11[num61] - WorldGen.genRand.Next(num65, num64);
			}
			bool flag12 = false;
			bool flag11 = false;
			bool[,] array10 = new bool[5, 10];
			int num60 = 3;
			int num59 = 3;
			for (int m = 0; m < 2; m++)
			{
				if (WorldGen.genRand.Next(3) == 0 || WorldGen.drunkWorldGen)
				{
					flag12 = true;
					int num20 = WorldGen.genRand.Next(10);
					if (num20 < num60)
					{
						num60 = num20;
					}
					if (num20 > num59)
					{
						num59 = num20;
					}
					int num19 = 1;
					if (WorldGen.genRand.Next(2) == 0 || WorldGen.drunkWorldGen)
					{
						array10[0, num20] = true;
						array10[1, num20] = true;
						num19 = 0;
					}
					else
					{
						array10[1, num20] = true;
					}
					int num18 = WorldGen.genRand.Next(2);
					if (num18 == 0)
					{
						num18 = -1;
					}
					int num17 = WorldGen.genRand.Next(10);
					while (num17 > 0 && num20 >= 0 && num20 < 10)
					{
						array10[num19, num20] = true;
						num20 += num18;
					}
				}
				if (WorldGen.genRand.Next(3) == 0 || WorldGen.drunkWorldGen)
				{
					flag11 = true;
					int num16 = WorldGen.genRand.Next(10);
					if (num16 < num60)
					{
						num60 = num16;
					}
					if (num16 > num59)
					{
						num59 = num16;
					}
					int num15 = 3;
					if (WorldGen.genRand.Next(2) == 0 || WorldGen.drunkWorldGen)
					{
						array10[3, num16] = true;
						array10[4, num16] = true;
						num15 = 4;
					}
					else
					{
						array10[3, num16] = true;
					}
					int num14 = WorldGen.genRand.Next(2);
					if (num14 == 0)
					{
						num14 = -1;
					}
					int num13 = WorldGen.genRand.Next(10);
					while (num13 > 0 && num16 >= 0 && num16 < 10)
					{
						array10[num15, num16] = true;
						num16 += num14;
					}
				}
			}
			Tile tile;
			for (int l = 0; l < 5; l++)
			{
				int num22 = array14[l];
				bool flag3 = false;
				if (num22 < 10 || num22 > Main.maxTilesX - 10)
				{
					flag3 = true;
				}
				else
				{
					for (int k = Main.UnderworldLayer; k < Main.maxTilesY; k++)
					{
						tile = Main.tile[num22, k];
						if (tile.WallType > 0)
						{
							flag3 = true;
						}
					}
				}
				if (flag3)
				{
					for (int num21 = 0; num21 < 10; num21++)
					{
						array10[l, num21] = false;
					}
				}
			}
			int num58 = WorldGen.genRand.Next(10);
			if (num58 < num60)
			{
				num60 = num58;
			}
			num58 = WorldGen.genRand.Next(10);
			if (num58 > num59)
			{
				num59 = num58;
			}
			if (!flag12 && !flag11)
			{
				while (num59 - num60 < 5)
				{
					num58 = WorldGen.genRand.Next(10);
					if (num58 < num60)
					{
						num60 = num58;
					}
					num58 = WorldGen.genRand.Next(10);
					if (num58 > num59)
					{
						num59 = num58;
					}
				}
			}
			for (int index1 = 0; index1 < 5; ++index1)
			{
				for (int index2 = 0; index2 < 10; ++index2)
				{
					if (array10[index1, index2] && (array12[index2] < Main.maxTilesY - 200 || array11[index2] > Main.maxTilesY - 20))
						array10[index1, index2] = false;
				}
			}
			for (int index1 = 0; index1 < 5; ++index1)
			{
				for (int index2 = 0; index2 < 10; ++index2)
				{
					if (array10[index1, index2])
					{
						for (int index3 = array14[index1]; index3 <= array13[index1]; ++index3)
						{
							for (int index4 = array12[index2]; index4 <= array11[index2]; ++index4)
							{
								if (index3 == array14[index1] || index3 == array13[index1] || (index4 == array12[index2] || index4 == array11[index2]))
								{
									if (tileType != 77)
									{
										if (tileType == 76)
											Main.tile[index3, index4].TileType = (ushort)TileType<ArqueriteBricks>();
										else
											if (tileType == 75)
											Main.tile[index3, index4].TileType = (ushort)TileType<QuartzBricks>();
										else
											Main.tile[index3, index4].TileType = tileType;
									}
									else
										Main.tile[index3, index4].TileType = (ushort)TileType<Gemforge>();
								}
								else
								{
									Main.tile[index3, index4].WallType = wallType;
								}
							}
						}
					}
				}
			}

			for (int num54 = num60; num54 <= num59; num54++)
			{
				array10[2, num54] = true;
			}
			for (int num53 = 0; num53 < 5; num53++)
			{
				for (int num23 = 0; num23 < 10; num23++)
				{
					if (array10[num53, num23] && (array12[num23] < Main.UnderworldLayer || array11[num23] > Main.maxTilesY - 20))
					{
						array10[num53, num23] = false;
					}
				}
			}
			for (int num52 = 0; num52 < 5; num52++)
			{
				for (int num26 = 0; num26 < 10; num26++)
				{
					if (array10[num52, num26])
					{
						for (int num25 = array14[num52]; num25 <= array13[num52]; num25++)
						{
							for (int num24 = array12[num26]; num24 <= array11[num26]; num24++)
							{
								if (num25 < 10)
								{
									break;
								}
								if (num25 > Main.maxTilesX - 10)
								{
									break;
								}
								tile = Main.tile[num25, num24];
								tile.LiquidAmount = 0;
								if (num25 == array14[num52] || num25 == array13[num52] || num24 == array12[num26] || num24 == array11[num26])
								{
									tile = Main.tile[num25, num24];
									tile.HasTile = (true);
									tile = Main.tile[num25, num24];
									tile.TileType = tileType;
									tile = Main.tile[num25, num24];
									tile.IsHalfBlock = (false);
									tile = Main.tile[num25, num24];
									tile.Slope = (0);
								}
								else
								{
									tile = Main.tile[num25, num24];
									tile.WallType = wallType;
									tile = Main.tile[num25, num24];
									tile.HasTile = (false);
								}
							}
						}
					}
				}
			}
			int style3 = 19;
			int style2 = 13;
			for (int num51 = 0; num51 < 4; num51++)
			{
				bool[] array6 = new bool[10];
				bool flag4 = false;
				for (int num28 = 0; num28 < 10; num28++)
				{
					if (array10[num51, num28] && array10[num51 + 1, num28])
					{
						array6[num28] = true;
						flag4 = true;
					}
				}
				while (flag4)
				{
					int num27 = WorldGen.genRand.Next(10);
					if (array6[num27])
					{
						flag4 = false;
						tile = Main.tile[array13[num51], array11[num27] - 1];
						tile.HasTile = (false);
						tile = Main.tile[array13[num51], array11[num27] - 2];
						tile.HasTile = (false);
						tile = Main.tile[array13[num51], array11[num27] - 3];
						tile.HasTile = (false);
						tile = Main.tile[array13[num51], array11[num27] - 1];
						tile.WallType = wallType;
						tile = Main.tile[array13[num51], array11[num27] - 2];
						tile.WallType = wallType;
						tile = Main.tile[array13[num51], array11[num27] - 3];
						tile.WallType = wallType;
						WorldGen.PlaceTile(array13[num51], array11[num27] - 1, TileType<QuartzDoorClosed>(), true, false, -1);
					}
				}
			}
			for (int num50 = 0; num50 < 5; num50++)
			{
				for (int num33 = 0; num33 < 10; num33++)
				{
					if (array10[num50, num33])
					{
						if (num33 > 0 && array10[num50, num33 - 1])
						{
							int num32 = WorldGen.genRand.Next(array14[num50] + 2, array13[num50] - 1);
							int num31 = WorldGen.genRand.Next(array14[num50] + 2, array13[num50] - 1);
							int num30 = 0;
							do
							{
								if (num31 - num32 >= 2 && num31 - num32 <= 5)
								{
									break;
								}
								num32 = WorldGen.genRand.Next(array14[num50] + 2, array13[num50] - 1);
								num31 = WorldGen.genRand.Next(array14[num50] + 2, array13[num50] - 1);
								num30++;
							}
							while (num30 <= 10000);
							if (num30 > 10000)
							{
								break;
							}
							for (int num29 = num32; num29 <= num31 && num29 >= 20 && num29 <= Main.maxTilesX - 20; num29++)
							{
								tile = Main.tile[num29, array12[num33]];
								tile.HasTile = (false);
								WorldGen.PlaceTile(num29, array12[num33], TileType<QuartzPlatform>(), true, true, -1);
								tile = Main.tile[num29, array12[num33]];
								tile.WallType = wallType;
							}
						}
						if (num50 < 4 && array10[num50 + 1, num33] && WorldGen.genRand.Next(3) == 0)
						{
							tile = Main.tile[array13[num50], array11[num33] - 1];
							tile.HasTile = (false);
							tile = Main.tile[array13[num50], array11[num33] - 2];
							tile.HasTile = (false);
							tile = Main.tile[array13[num50], array11[num33] - 3];
							tile.HasTile = (false);
							tile = Main.tile[array13[num50], array11[num33] - 1];
							tile.WallType = wallType;
							tile = Main.tile[array13[num50], array11[num33] - 2];
							tile.WallType = wallType;
							tile = Main.tile[array13[num50], array11[num33] - 3];
							tile.WallType = wallType;
							WorldGen.PlaceTile(array13[num50], array11[num33] - 1, TileType<QuartzDoorClosed>(), true, false, -1);
						}
					}
				}
			}
			bool flag10 = false;
			for (int num49 = 0; num49 < 5; num49++)
			{
				bool[] array9 = new bool[10];
				for (int num48 = 0; num48 < 10; num48++)
				{
					if (array10[num49, num48])
					{
						flag10 = true;
						array9[num48] = true;
					}
				}
				if (flag10)
				{
					bool flag9 = false;
					for (int num47 = 0; num47 < 10; num47++)
					{
						if (array9[num47])
						{
							tile = Main.tile[array14[num49] - 1, array11[num47] - 1];
							if (!tile.HasTile)
							{
								tile = Main.tile[array14[num49] - 1, array11[num47] - 2];
								if (!tile.HasTile)
								{
									tile = Main.tile[array14[num49] - 1, array11[num47] - 3];
									if (!tile.HasTile)
									{
										tile = Main.tile[array14[num49] - 1, array11[num47] - 1];
										if (tile.LiquidAmount == 0)
										{
											tile = Main.tile[array14[num49] - 1, array11[num47] - 2];
											if (tile.LiquidAmount == 0)
											{
												tile = Main.tile[array14[num49] - 1, array11[num47] - 3];
												if (tile.LiquidAmount == 0)
												{
													flag9 = true;
													continue;
												}
											}
										}
									}
								}
							}
							array9[num47] = false;
						}
					}
					while (flag9)
					{
						int num34 = WorldGen.genRand.Next(10);
						if (array9[num34])
						{
							flag9 = false;
							tile = Main.tile[array14[num49], array11[num34] - 1];
							tile.HasTile = (false);
							tile = Main.tile[array14[num49], array11[num34] - 2];
							tile.HasTile = (false);
							tile = Main.tile[array14[num49], array11[num34] - 3];
							tile.HasTile = (false);
							WorldGen.PlaceTile(array14[num49], array11[num34] - 1, TileType<QuartzDoorClosed>(), true, false, -1);
						}
					}
					break;
				}
			}
			bool flag8 = false;
			for (int num46 = 4; num46 >= 0; num46--)
			{
				bool[] array8 = new bool[10];
				for (int num45 = 0; num45 < 10; num45++)
				{
					if (array10[num46, num45])
					{
						flag8 = true;
						array8[num45] = true;
					}
				}
				if (flag8)
				{
					bool flag7 = false;
					for (int num44 = 0; num44 < 10; num44++)
					{
						if (array8[num44])
						{
							if (num46 < 20)
							{
								break;
							}
							if (num46 > Main.maxTilesX - 20)
							{
								break;
							}
							tile = Main.tile[array13[num46] + 1, array11[num44] - 1];
							if (!tile.HasTile)
							{
								tile = Main.tile[array13[num46] + 1, array11[num44] - 2];
								if (!tile.HasTile)
								{
									tile = Main.tile[array13[num46] + 1, array11[num44] - 3];
									if (!tile.HasTile)
									{
										tile = Main.tile[array13[num46] + 1, array11[num44] - 1];
										if (tile.LiquidAmount == 0)
										{
											tile = Main.tile[array13[num46] + 1, array11[num44] - 2];
											if (tile.LiquidAmount == 0)
											{
												tile = Main.tile[array13[num46] + 1, array11[num44] - 3];
												if (tile.LiquidAmount == 0)
												{
													flag7 = true;
													continue;
												}
											}
										}
									}
								}
							}
							array8[num44] = false;
						}
					}
					while (flag7)
					{
						int num35 = WorldGen.genRand.Next(10);
						if (array8[num35])
						{
							flag7 = false;
							tile = Main.tile[array13[num46], array11[num35] - 1];
							tile.HasTile = (false);
							tile = Main.tile[array13[num46], array11[num35] - 2];
							tile.HasTile = (false);
							tile = Main.tile[array13[num46], array11[num35] - 3];
							tile.HasTile = (false);
							WorldGen.PlaceTile(array13[num46], array11[num35] - 1, TileType<QuartzDoorClosed>(), true, false, -1);
						}
					}
					break;
				}
			}
			bool flag6 = false;
			int num43 = 0;
			bool[] array7;
			while (true)
			{
				if (num43 < 10)
				{
					array7 = new bool[10];
					for (int num42 = 0; num42 < 5; num42++)
					{
						if (array10[num42, num43])
						{
							flag6 = true;
							array7[num42] = true;
						}
					}
					if (flag6)
					{
						break;
					}
					num43++;
					continue;
				}
				return;
			}
			bool flag5 = true;
			while (flag5)
			{
				int num41 = WorldGen.genRand.Next(5);
				if (array7[num41])
				{
					int num40 = WorldGen.genRand.Next(array14[num41] + 2, array13[num41] - 1);
					int num39 = WorldGen.genRand.Next(array14[num41] + 2, array13[num41] - 1);
					int num38 = 0;
					do
					{
						if (num39 - num40 >= 2 && num39 - num40 <= 5)
						{
							break;
						}
						num40 = WorldGen.genRand.Next(array14[num41] + 2, array13[num41] - 1);
						num39 = WorldGen.genRand.Next(array14[num41] + 2, array13[num41] - 1);
						num38++;
					}
					while (num38 <= 10000);
					if (num38 > 10000)
					{
						break;
					}
					for (int num37 = num40; num37 <= num39 && num37 >= 10 && num37 <= Main.maxTilesX - 10; num37++)
					{
						tile = Main.tile[num37, array12[num43] - 1];
						if (!tile.HasTile)
						{
							tile = Main.tile[num37, array12[num43] - 1];
							if (tile.LiquidAmount > 0)
							{
								goto IL_0e80;
							}
							continue;
						}
						goto IL_0e80;
					IL_0e80:
						flag5 = false;
					}
					if (flag5)
					{
						for (int num36 = num40; num36 <= num39 && num36 >= 10 && num36 <= Main.maxTilesX - 10; num36++)
						{
							tile = Main.tile[num36, array12[num43]];
							tile.HasTile = false;
							WorldGen.PlaceTile(num36, array12[num43], TileType<QuartzPlatform>(), true, true, -1);
						}
					}
					flag5 = false;
				}
			}
		}


		public static void TreeGen(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Petrifying Trees";
            progress.Set(0.0f);
            for (int hbx = 50; hbx < Main.maxTilesX - 50; hbx++)
            {
                for (int hby = Main.maxTilesY - 200; hby < Main.maxTilesY - 50; hby++)
                {
                    if (Main.tile[hbx, hby].HasTile && !Main.tile[hbx, hby - 1].HasTile ||
                        Main.tile[hbx, hby].HasTile && !Main.tile[hbx, hby + 1].HasTile ||
                        Main.tile[hbx, hby].HasTile && !Main.tile[hbx - 1, hby].HasTile ||
                        Main.tile[hbx, hby].HasTile && !Main.tile[hbx + 1, hby].HasTile)
                    {
                        if (Main.tile[hbx, hby].TileType == (ushort)TileType<ShaleBlock>())
                        {
                            if (WorldGen.genRand.Next(1) == 0)
                            {
                                WorldGen.GrowTree(hbx, hby - 1);
                            }
                        }
                    }
                }
            }
            progress.Set(1f);
        }
    }
}
