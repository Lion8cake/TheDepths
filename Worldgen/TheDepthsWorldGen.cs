using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
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
using System.IO;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.GameContent.UI.States;
using Microsoft.Xna.Framework.Graphics;

namespace TheDepths.Worldgen
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

		//The most fitting place since we already are syncing depths world tags here so we might as well have the chasme downed code here too
		public static bool downedChasme = false;

		/// <summary>
		/// Detects if the player is on the depths side of the drunk seed if the depths is on the Right
		/// </summary>
		public static bool IsPlayerInRightDepths => DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2;

		/// <summary>
		///   Detects if the player is on the depths side of the drunk seed if the depths is on the left
		/// </summary>
		public static bool IsPlayerInLeftDepths => DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2;

		/// <summary>
		///   Checks if the player is in the depths part of the world. This is used to reduce repetion within code as previously all the check needed was depthsorHell == true.
		/// </summary>
		public static bool InDepths => (depthsorHell && !Main.drunkWorld || (IsPlayerInLeftDepths || IsPlayerInRightDepths) && Main.drunkWorld);

		public override void OnWorldLoad()
		{
			DrunkDepthsLeft = false;
			DrunkDepthsRight = false;
			downedChasme = false;

			//World Converter (1.4.3 => 1.4.4)
			//Also contains some explinations 
			if (!Main.dedServ) //Make sure that we are not on a server so we dont infinatly get stuck on Syncing Mods
			{
				string twld = Path.ChangeExtension(Main.worldPathName, ".twld"); //gets the world we are updating
				var tag2 = TagIO.FromStream(new MemoryStream(File.ReadAllBytes(twld))); //We read the nbt data of the world .twld file
				if (tag2.ContainsKey("modData"))
				{ //We look for modData here and v there 
					foreach (TagCompound modDataTag in tag2.GetList<TagCompound>("modData"))
					{
						if (modDataTag.Get<string>("mod") == "AltLibrary" && modDataTag.Get<string>("name") == "WorldBiomeManager")
						{ //Here we take two paths, one for if altlib is enabled (imposter mod the original wont return) or if the mod is unloaded 
							TagCompound dataTag = modDataTag.Get<TagCompound>("data");

							if (dataTag.Get<string>("AltLibrary:WorldHell") == "TheDepths/AltDepthsBiome")
							{ //Look for the correct string that WorldHallow is saved under
								depthsorHell = true; //Convert world by giving it the tag
								ModContent.GetInstance<TheDepths>().Logger.Debug("Altlib save!, converting world!"); //Announce converting
							}
							else
							{
								ModContent.GetInstance<TheDepths>().Logger.Debug("non-Altlib save, unable to convert world"); //Announce that the world doesn't have altlib/it didn't work
							}
							break;
						}
						if (modDataTag.Get<string>("mod") == "ModLoader")
						{
							ModContent.GetInstance<TheDepths>().Logger.Debug("Didn't find altlib, Attempting to look in unloaded mods"); //Didn't find altlib so we look in unloaded and announce 
							TagCompound dataTag = modDataTag.Get<TagCompound>("data"); //we look for the first tmod data
							if (dataTag.ContainsKey("list"))
							{ //find a list called list
								ModContent.GetInstance<TheDepths>().Logger.Debug("Found List inside unloaded mods!"); //anounce we have found the list since list can be tricky sometimes
								foreach (TagCompound unloadedList in dataTag.GetList<TagCompound>("list"))
								{ //same here as above ^

									if (unloadedList.Get<string>("mod") == "AltLibrary" && unloadedList.Get<string>("name") == "WorldBiomeManager")
									{ //Look for altlib inside of list
										ModContent.GetInstance<TheDepths>().Logger.Debug("Found Altlib under unloaded mods"); //announce that altlib has been found inside tmod's unloaded data
										TagCompound dataTag2 = (TagCompound)unloadedList["data"]; //We look for the data entry list under list

										if (dataTag2.Get<string>("AltLibrary:WorldHell") == "TheDepths/AltDepthsBiome")
										{ //same as the lines previously when altlib was enabled
											depthsorHell = true;
											ModContent.GetInstance<TheDepths>().Logger.Debug("Altlib save!, converting world!");
										}
										else
										{
											ModContent.GetInstance<TheDepths>().Logger.Debug("non-Altlib save, unable to convert world");
										}
										break;
									}
								}
							}
							break;
						}
					}
				}
			}
		}

		public override void OnWorldUnload()
		{
			depthsorHell = false;
			DrunkDepthsLeft = false;
			DrunkDepthsRight = false;
			downedChasme = false;
		}

		public override void ClearWorld()
		{
			depthsorHell = false;
			DrunkDepthsLeft = false;
			DrunkDepthsRight = false;
			downedChasme = false;
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
			if (downedChasme)
			{
				tag["downedChasme"] = true;
			}
		}

		public override void SaveWorldHeader(TagCompound tag)
		{
			tag["HasDepths"] = depthsorHell;
			tag["DrunkDepthsLeft"] = DrunkDepthsLeft;
			tag["DrunkDepthsRight"] = DrunkDepthsRight;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			depthsorHell = tag.ContainsKey("IsDepths");
			DrunkDepthsLeft = tag.ContainsKey("DepthsIsOnTheLeft");
			DrunkDepthsRight = tag.ContainsKey("DepthsIsOnTheRight");
			downedChasme = tag.ContainsKey("downedChasme");
		}

		public override void NetSend(BinaryWriter writer)
		{
			var flags = new BitsByte();
			flags[0] = depthsorHell;
			flags[1] = DrunkDepthsLeft;
			flags[2] = DrunkDepthsRight;
			flags[2] = downedChasme;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			depthsorHell = flags[0];
			DrunkDepthsLeft = flags[1];
			DrunkDepthsRight = flags[2];
			downedChasme = flags[3];
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
				DrunkDepthsRight = true;
				/*if (Main.rand.NextBool(2))
				{
					DrunkDepthsLeft = true;
					DrunkDepthsRight = false;
				}
				else
                {
					DrunkDepthsRight = true;
					DrunkDepthsLeft = false;
                }*/
			}
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            if (depthsorHell && !Main.drunkWorld)
            {
                int index2 = tasks.FindIndex(genpass => genpass.Name.Equals("Underworld"));
				if (index2 != -1)
                {
                    tasks.Insert(index2 + 1, new PassLegacy("Underworld", new WorldGenLegacyMethod(Depths)));
					tasks.Insert(index2 + 2, new PassLegacy("Remix Middle Island", new WorldGenLegacyMethod(RemixIsland)));
					tasks.RemoveAt(index2);
                }
				int index3 = tasks.FindIndex(genpass => genpass.Name.Equals("Hellforge"));
				if (index3 != -1)
                {
                    tasks.Insert(index3 + 1, new PassLegacy("Hellforge", new WorldGenLegacyMethod(Gemforge)));
                    tasks.Insert(index3 + 2, new PassLegacy("Petrifying Trees", new WorldGenLegacyMethod(TreeGen)));
                    tasks.RemoveAt(index3);
                }
				int index4 = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
				if (index4 != -1)
				{
					tasks.Insert(index4 + 1, new PassLegacy("Nightmare Grove", new WorldGenLegacyMethod(NightmareGrove)));
					tasks.Insert(index4 + 2, new PassLegacy("Pots", new WorldGenLegacyMethod(Pots)));
					tasks.RemoveAt(index4);
				}
				int index5 = tasks.FindIndex(genpass => genpass.Name.Equals("Buried Chests"));
				if (index5 != -1)
				{
					tasks.Insert(index5 + 1, new PassLegacy("Buried Chests", new WorldGenLegacyMethod(DepthsBuriedChests))); 
					tasks.RemoveAt(index5);
				}
				int index6 = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
                if (index6 != -1)
                {
                    tasks.Insert(index6 - 2, new PassLegacy("Depths Remix Shuffling", new WorldGenLegacyMethod(FinishDepthsRemix)));
					tasks.Insert(index6 - 1, new PassLegacy("Quicksilver Droplets", new WorldGenLegacyMethod(DrippingQuicksilverTileCleanup)));
                }
				int index7 = tasks.FindIndex(genpass => genpass.Name.Equals("Reset"));
				if (index7 != -1)
				{
					tasks.Insert(index7 + 2, new PassLegacy("Shadow Chest Shuffler", new WorldGenLegacyMethod(DepthsShadowchestGenResetter)));
				}
			}
			if (DrunkDepthsLeft && Main.drunkWorld)
			{
				int index2 = tasks.FindIndex(genpass => genpass.Name.Equals("Underworld"));
				if (index2 != -1)
				{
					tasks.Insert(index2 + 1, new PassLegacy("DepthsLeft", new WorldGenLegacyMethod(TheDepthsDrunkGen.DepthsLeft)));
					tasks.Insert(index2 + 2, new PassLegacy("Remix Middle Island Left", new WorldGenLegacyMethod(TheDepthsDrunkGen.RemixIslandLeft)));
				}
				int index3 = tasks.FindIndex(genpass => genpass.Name.Equals("Hellforge"));
				if (index3 != -1)
				{
					tasks.Insert(index3 + 1, new PassLegacy("GemForges", new WorldGenLegacyMethod(TheDepthsDrunkGen.GemforgeLeft)));
					tasks.Insert(index3 + 2, new PassLegacy("PetrifyingTrees", new WorldGenLegacyMethod(TheDepthsDrunkGen.TreeGenLeft)));
				}
				int index4 = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
				if (index4 != -1)
				{
					tasks.Insert(index4 - 2, new PassLegacy("Nightmare Grove", new WorldGenLegacyMethod(NightmareGrove)));
					tasks.Insert(index4 - 1, new PassLegacy("Remix Middle Island Left Again", new WorldGenLegacyMethod(TheDepthsDrunkGen.RemixIslandStuffLeft)));
					tasks.Insert(index4 + 1, new PassLegacy("DepthsPots", new WorldGenLegacyMethod(TheDepthsDrunkGen.PotsLeft)));
				}
				int index5 = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
				if (index5 != -1)
				{
					tasks.Insert(index5 - 3, new PassLegacy("Depths Remix Shuffling", new WorldGenLegacyMethod(FinishDepthsRemix)));
					tasks.Insert(index5 - 2, new PassLegacy("KillingHellPots", new WorldGenLegacyMethod(TheDepthsDrunkGen.KILLTHEPOTSLeft)));
					tasks.Insert(index5 - 1, new PassLegacy("Quicksilver Droplets", new WorldGenLegacyMethod(TheDepthsDrunkGen.DrippingQuicksilverTileCleanupLeft)));
				}
				int index6 = tasks.FindIndex(genpass => genpass.Name.Equals("Reset"));
				if (index6 != -1)
				{
					tasks.Insert(index6 + 2, new PassLegacy("Shadow Chest Shuffler", new WorldGenLegacyMethod(TheDepthsDrunkGen.DepthsShadowchestGenResetterDrunk)));
				}
			}
			if (DrunkDepthsRight && Main.drunkWorld)
			{
				int index2 = tasks.FindIndex(genpass => genpass.Name.Equals("Underworld"));
				if (index2 != -1)
				{
					tasks.Insert(index2 + 1, new PassLegacy("DepthsRight", new WorldGenLegacyMethod(TheDepthsDrunkGen.DepthsRight)));
					tasks.Insert(index2 + 2, new PassLegacy("Remix Middle Island Right", new WorldGenLegacyMethod(RemixIsland)));
				}
				int index3 = tasks.FindIndex(genpass => genpass.Name.Equals("Hellforge"));
				if (index3 != -1)
				{
					tasks.Insert(index3 + 1, new PassLegacy("GemForges", new WorldGenLegacyMethod(Gemforge)));
					tasks.Insert(index3 + 2, new PassLegacy("PetrifyingTrees", new WorldGenLegacyMethod(TheDepthsDrunkGen.TreeGenRight)));
				}
				int index4 = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
				if (index4 != -1)
				{
					tasks.Insert(index4 - 2, new PassLegacy("Nightmare Grove", new WorldGenLegacyMethod(TheDepthsDrunkGen.NightmareGroveRight)));
					tasks.Insert(index4 - 1, new PassLegacy("Remix Middle Island Right Again", new WorldGenLegacyMethod(TheDepthsDrunkGen.RemixIslandStuffRight)));
					tasks.Insert(index4 + 1, new PassLegacy("DepthsPots", new WorldGenLegacyMethod(TheDepthsDrunkGen.PotsRight)));
				}
				int index5 = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
				if (index5 != -1)
				{
					tasks.Insert(index5 - 3, new PassLegacy("Depths Remix Shuffling", new WorldGenLegacyMethod(FinishDepthsRemix)));
					tasks.Insert(index5 - 2, new PassLegacy("KillingHellPots", new WorldGenLegacyMethod(TheDepthsDrunkGen.KILLTHEPOTSRight)));
					tasks.Insert(index5 - 1, new PassLegacy("Quicksilver Droplets", new WorldGenLegacyMethod(TheDepthsDrunkGen.DrippingQuicksilverTileCleanupRight)));
				}
				int index6 = tasks.FindIndex(genpass => genpass.Name.Equals("Reset"));
				if (index6 != -1)
				{
					tasks.Insert(index6 + 2, new PassLegacy("Shadow Chest Shuffler", new WorldGenLegacyMethod(TheDepthsDrunkGen.DepthsShadowchestGenResetterDrunk)));
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

		public static void DepthsShadowchestGenResetter(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Resetting Shadow Chests";
			List<int> list3 = new List<int> { 274, ModContent.ItemType<Items.Weapons.SilverStar>(), ModContent.ItemType<Items.Weapons.Skyfall>(), ModContent.ItemType<Items.Weapons.WhiteLightning>(), ModContent.ItemType<Items.Weapons.NightFury>() };
			if (WorldGen.remixWorldGen)
			{
				list3 = new List<int> { 274, ModContent.ItemType<Items.Weapons.SilverStar>(), ModContent.ItemType<Items.Weapons.Skyfall>(), ModContent.ItemType<Items.Weapons.BlueSphere>(), ModContent.ItemType<Items.Weapons.NightFury>() };
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

		public static void DrippingQuicksilverTileCleanup(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Making Quicksilver seep through cracks";
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

		public static void DepthsBuriedChests(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Lang.gen[30].Value;
			Main.tileSolid[226] = true;
			Main.tileSolid[162] = true;
			Main.tileSolid[225] = true;
			UndergroundHouse.CaveHouseBiome caveHouseBiome = GenVars.configuration.CreateBiome<UndergroundHouse.CaveHouseBiome>();
			int random6 = configuration.Get<WorldGenRange>("CaveHouseCount").GetRandom(WorldGen.genRand);
			int random7 = configuration.Get<WorldGenRange>("UnderworldChestCount").GetRandom(WorldGen.genRand);
			int num546 = configuration.Get<WorldGenRange>("CaveChestCount").GetRandom(WorldGen.genRand);
			int random8 = configuration.Get<WorldGenRange>("AdditionalDesertHouseCount").GetRandom(WorldGen.genRand);
			if (Main.starGame)
			{
				num546 = (int)((double)num546 * Main.starGameMath(0.2));
			}
			int num547 = random6 + random7 + num546 + random8;
			int num548 = 10000;
			for (int num549 = 0; num549 < num546; num549++)
			{
				if (num548 <= 0)
				{
					break;
				}
				progress.Set((double)num549 / (double)num547);
				int num550 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
				int num551 = WorldGen.genRand.Next((int)((GenVars.worldSurfaceHigh + 20.0 + Main.rockLayer) / 2.0), Main.maxTilesY - 230);
				if (WorldGen.remixWorldGen)
				{
					num551 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 400);
				}
				ushort wall = Main.tile[num550, num551].WallType;
				if (Main.wallDungeon[wall] || wall == 87 || WorldGen.oceanDepths(num550, num551) || !AddBuriedChest(num550, num551, 0, notNearOtherChests: false, -1, trySlope: false, 0))
				{
					num548--;
					num549--;
				}
			}
			num548 = 10000;
			for (int num552 = 0; num552 < random7; num552++)
			{
				if (num548 <= 0)
				{
					break;
				}
				progress.Set((double)(num552 + num546) / (double)num547);
				int num553 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
				int num554 = WorldGen.genRand.Next(Main.UnderworldLayer, Main.maxTilesY - 50);
				if (Main.wallDungeon[Main.tile[num553, num554].WallType] || !AddBuriedChest(num553, num554, 0, notNearOtherChests: false, -1, trySlope: false, 0))
				{
					num548--;
					num552--;
				}
			}
			num548 = 10000;
			for (int num555 = 0; num555 < random6; num555++)
			{
				if (num548 <= 0)
				{
					break;
				}
				progress.Set((double)(num555 + num546 + random7) / (double)num547);
				int x11 = WorldGen.genRand.Next(80, Main.maxTilesX - 80);
				int y9 = WorldGen.genRand.Next((int)(GenVars.worldSurfaceHigh + 20.0), Main.maxTilesY - 230);
				if (WorldGen.remixWorldGen)
				{
					y9 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 400);
				}
				if (WorldGen.oceanDepths(x11, y9) || !caveHouseBiome.Place(new Point(x11, y9), GenVars.structures))
				{
					num548--;
					num555--;
				}
			}
			num548 = 10000;
			Rectangle undergroundDesertHiveLocation = GenVars.UndergroundDesertHiveLocation;
			if ((double)undergroundDesertHiveLocation.Y < Main.worldSurface + 26.0)
			{
				int num556 = (int)Main.worldSurface + 26 - undergroundDesertHiveLocation.Y;
				undergroundDesertHiveLocation.Y += num556;
				undergroundDesertHiveLocation.Height -= num556;
			}
			for (int num557 = 0; num557 < random8; num557++)
			{
				if (num548 <= 0)
				{
					break;
				}
				progress.Set((double)(num557 + num546 + random7 + random6) / (double)num547);
				if (!caveHouseBiome.Place(WorldGen.RandomRectanglePoint(undergroundDesertHiveLocation), GenVars.structures))
				{
					num548--;
					num557--;
				}
			}
			Main.tileSolid[226] = false;
			Main.tileSolid[162] = false;
			Main.tileSolid[225] = false;
		}

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
				progress.Set(0.0);
				int num838 = Main.maxTilesY - WorldGen.genRand.Next(150, 190);
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
					for (int num840 = num838 - 20 - WorldGen.genRand.Next(3); num840 < Main.maxTilesY; num840++)
					{
						if (num840 >= num838)
						{
							Tile tile = Main.tile[num839, num840];
							tile.HasTile = false;
							tile.LiquidType = -1;
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
				for (int num844 = 0; num844 < Main.maxTilesX; num844++)
				{
					if (WorldGen.genRand.Next(50) == 0)
					{
						int num845 = Main.maxTilesY - 65;
						while (!Main.tile[num844, num845].HasTile && num845 > Main.maxTilesY - 135)
						{
							num845--;
						}
						WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), num845 + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(15, 20), 1000, ModContent.TileType<ShaleBlock>(), addTile: true, 0.0, WorldGen.genRand.Next(1, 3), noYChange: true);
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
				for (int num850 = 0; num850 < Main.maxTilesX; num850++)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(20, Main.maxTilesX - 20), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 7), -2);
				}
				if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
				{
					for (int num851 = 0; num851 < Main.maxTilesX * 2; num851++)
					{
						WorldGen.TileRunner(WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.35), (int)((double)Main.maxTilesX * 0.65)), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(5, 20), WorldGen.genRand.Next(5, 10), -2);
					}
				}
				for (int num852 = 0; num852 < Main.maxTilesX; num852++)
				{
					if (!Main.tile[num852, Main.maxTilesY - 145].HasTile)
					{
						Main.tile[num852, Main.maxTilesY - 145].LiquidAmount = byte.MaxValue;
						Tile tile = Main.tile[num852, Main.maxTilesY - 145];
						tile.LiquidType = LiquidID.Lava;
					}
					if (!Main.tile[num852, Main.maxTilesY - 144].HasTile)
					{
						Main.tile[num852, Main.maxTilesY - 144].LiquidAmount = byte.MaxValue;
						Tile tile = Main.tile[num852, Main.maxTilesY - 144];
						tile.LiquidType = LiquidID.Lava;
					}
				}
				for (int num853 = 0; num853 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num853++)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), TileType<ArqueriteOre>());
				}
				for (int index = 0; index < (int)(Main.maxTilesX * Main.maxTilesY * 0.0008); ++index)
				{
					WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(5, 9), TileType<Shalestone>(), false, 0.0f, 0.0f, false, true);
				}
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
							tile = new Tile();
							tile.HasTile = true;
							Main.tile[num856, num861].TileType = (ushort)ModContent.TileType<ShaleBlock>();
						}
					}
				}
				Gems();
				AddQuartzApartments();
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

		public static void FinishDepthsRemix(GenerationProgress progress, GameConfiguration configuration)
		{
			if (WorldGen.remixWorldGen)
			{
				for (int i = 25; i < Main.maxTilesX - 25; i++)
				{
					for (int j = 25; j < Main.maxTilesY - 25; j++)
					{
						if (WorldGen.notTheBees && (double)j < Main.worldSurface)
						{
							if (Main.tile[i, j].TileType == ModContent.TileType<Tiles.GlitterBlock>() || Main.tile[i, j].TileType == ModContent.TileType<Tiles.Quartz>())
							{
								WorldGen.KillTile(i, j);
							}
						}
						if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == ModContent.TileType<Tiles.Quartz>())
						{
							WorldGen.KillTile(i, j);
						}
					}
				}
				Liquid.QuickWater(-2);
				int num = (int)((double)Main.maxTilesX * 0.38);
				int num2 = (int)((double)Main.maxTilesX * 0.62);
				_ = Main.maxTilesY;
				int num3 = Main.maxTilesY - 135;
				_ = Main.maxTilesY;
				for (int k = num; k < num2 + 15; k++)
				{
					for (int l = Main.maxTilesY - 200; l < num3 + 10; l++)
					{
						Tile tile = Main.tile[k, l];
						tile.LiquidType = 0;
						if (Main.tile[k, l].TileType == ModContent.TileType<Tiles.ArqueriteOre>() || Main.tile[k, l].TileType == ModContent.TileType<Tiles.Shalestone>())
						{
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.ShaleBlock>();
						}
					}
				}
				WorldGen.AddTrees(undergroundOnly: true);
			}
		}

		public static void RemixIsland(GenerationProgress progress, GameConfiguration configuration)
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
		public static void NightmareGrove(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Growing bioluminecent plants in very dark areas";
			//Proper Grove Gen
			if (WorldGen.remixWorldGen)
			{
				int num854 = (int)((double)Main.maxTilesX * 0.38);
				int num855 = (int)((double)Main.maxTilesX * 0.62);
				int num856 = num854;
				int num857 = Main.maxTilesY - 1;
				int num858 = Main.maxTilesY - 135;
				int num859 = Main.maxTilesY - 160;
				bool flag55 = false;
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
				for (int num866 = 25; num866 < Main.maxTilesX - 25; num866++)
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
				for (int num868 = 25; num868 < Main.maxTilesX - 25; num868++)
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
				for (int num866 = 25; num866 < Main.maxTilesX - 25; num866++)
				{
					for (int num867 = Main.maxTilesY - 300; num867 < Main.maxTilesY - 100 + WorldGen.genRand.Next(-1, 2); num867++)
					{
						if ((Main.tile[num866, num867].TileType == ModContent.TileType<ShaleBlock>() || Main.tile[num866, num867].TileType == ModContent.TileType<Shalestone>()) && Main.tile[num866, num867].HasTile && (!Main.tile[num866 - 1, num867 - 1].HasTile || !Main.tile[num866, num867 - 1].HasTile || !Main.tile[num866 + 1, num867 - 1].HasTile || !Main.tile[num866 - 1, num867].HasTile || !Main.tile[num866 + 1, num867].HasTile || !Main.tile[num866 - 1, num867 + 1].HasTile || !Main.tile[num866, num867 + 1].HasTile || !Main.tile[num866 + 1, num867 + 1].HasTile))
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

			//Vines
			for (int num233 = 5; num233 < Main.maxTilesX - 5; num233++)
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
					if (Main.tile[num233, num253].HasTile && !Main.tile[num233, num253].BottomSlope && Main.tile[num233, num253].TileType == ModContent.TileType<NightmareGrass>() && GrowMoreVines(num233, num253) && WorldGen.genRand.Next(5) < 3)
					{
						num234 = WorldGen.genRand.Next(1, 10);
					}
				}
			}

			//Foliage
			for (int num263 = 0; num263 < Main.maxTilesX; num263++)
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

		public static bool GrowMoreVines(int x, int y)
		{
			if (!WorldGen.InWorld(x, y, 30))
			{
				return false;
			}
			int num = 4;
			int num2 = 6;
			int num3 = 10;
			int num4 = 60;
			int num5 = 0;
			for (int i = x - num; i <= x + num; i++)
			{
				for (int j = y - num2; j <= y + num3; j++)
				{
					if (TileID.Sets.IsVine[Main.tile[i, j].TileType])
					{
						num5++;
						if (j > y && Collision.CanHitLine(new Vector2(x * 16, y * 16), 1, 1, new Vector2(i * 16, j * 16), 1, 1))
						{
							num5 = ((Main.tile[i, j].TileType != 528) ? (num5 + (j - y) * 2) : (num5 + (j - y) * 20));
						}
						if (num5 > num4)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		public static void AddQuartzApartments()
		{
			int num = (int)((double)Main.maxTilesX * 0.25);
			for (int i = 100; i < Main.maxTilesX - 100; i++)
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
					ArqueriteCastle(i, num2, num3, wallType);
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
				/*int num34 = 0;
				if (WorldGen.nearPicture2(num25, num26))
				{
					num34 = -1;
				}
				if (num34 == 0)
				{
					PaintingEntry paintingEntry = WorldGen.RandHellPicture();
					if (!WorldGen.nearPicture(num25, num26))
					{
						WorldGen.PlaceTile(num25, num26, paintingEntry.tileType, mute: true, forced: false, -1, paintingEntry.style);
					}
				}*/
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

		public static void ArqueriteCastle(int i, int j, ushort tileType = 75, ushort wallType = 14)
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
						for (int num32 = num29; num32 <= num30 && num32 >= 20 && num32 <= Main.maxTilesX - 20; num32++)
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
					for (int num47 = num44; num47 <= num45 && num47 >= 10 && num47 <= Main.maxTilesX - 10; num47++)
					{
						if (Main.tile[num47, array3[num41] - 1].HasTile || Main.tile[num47, array3[num41] - 1].LiquidAmount > 0)
						{
							flag10 = false;
						}
					}
					if (flag10)
					{
						for (int num48 = num44; num48 <= num45 && num48 >= 10 && num48 <= Main.maxTilesX - 10; num48++)
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

		public static void QuartzApartment(int i, int j, byte type = 76, byte wall = 13)
		{
			int num = WorldGen.genRand.Next(8, 20);
			int num2 = WorldGen.genRand.Next(1, 3);
			int num3 = WorldGen.genRand.Next(4, 13);
			int num4 = j;
			for (int k = 0; k < num2; k++)
			{
				int num5 = WorldGen.genRand.Next(5, 9);
				DepthsRoom(i, num4, num, num5, type, wall);
				num4 -= num5;
			}
			num4 = j;
			for (int l = 0; l < num3; l++)
			{
				int num6 = WorldGen.genRand.Next(5, 9);
				num4 += num6;
				DepthsRoom(i, num4, num, num6, type, wall);
			}
			for (int m = i - num / 2; m <= i + num / 2; m++)
			{
				for (num4 = j; num4 < Main.maxTilesY && ((Main.tile[m, num4].HasTile && (Main.tile[m, num4].TileType == ModContent.TileType<ArqueriteBricks>() || Main.tile[m, num4].TileType == ModContent.TileType<QuartzBricks>())) || Main.tile[i, num4].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[i, num4].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>()); num4++)
				{
				}
				int num7 = 6 + WorldGen.genRand.Next(3);
				while (num4 < Main.maxTilesY && !Main.tile[m, num4].HasTile)
				{
					num7--;
					Tile tile = Main.tile[m, num4];
					tile.HasTile = true;
					Main.tile[m, num4].TileType = (ushort)ModContent.TileType<QuartzBricks>();
					num4++;
					if (num7 <= 0)
					{
						break;
					}
				}
			}
			int num8 = 0;
			int num9 = 0;
			for (num4 = j; num4 < Main.maxTilesY && ((Main.tile[i, num4].HasTile && (Main.tile[i, num4].TileType == ModContent.TileType<ArqueriteBricks>() || Main.tile[i, num4].TileType == ModContent.TileType<QuartzBricks>())) || Main.tile[i, num4].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[i, num4].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>()); num4++)
			{
			}
			num4--;
			num9 = num4;
			while ((Main.tile[i, num4].HasTile && (Main.tile[i, num4].TileType == ModContent.TileType<ArqueriteBricks>() || Main.tile[i, num4].TileType == ModContent.TileType<QuartzBricks>())) || Main.tile[i, num4].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[i, num4].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>())
			{
				num4--;
				if (!Main.tile[i, num4].HasTile || (Main.tile[i, num4].TileType != ModContent.TileType<ArqueriteBricks>() && Main.tile[i, num4].TileType != ModContent.TileType<QuartzBricks>()))
				{
					continue;
				}
				int num10 = WorldGen.genRand.Next(i - num / 2 + 1, i + num / 2 - 1);
				int num11 = WorldGen.genRand.Next(i - num / 2 + 1, i + num / 2 - 1);
				if (num10 > num11)
				{
					int num12 = num10;
					num10 = num11;
					num11 = num12;
				}
				if (num10 == num11)
				{
					if (num10 < i)
					{
						num11++;
					}
					else
					{
						num10--;
					}
				}
				for (int n = num10; n <= num11; n++)
				{
					if (Main.tile[n, num4 - 1].WallType == ModContent.WallType<QuartzBrickWallUnsafe>())
					{
						Main.tile[n, num4].WallType = (ushort)ModContent.WallType<QuartzBrickWallUnsafe>();
					}
					if (Main.tile[n, num4 - 1].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>())
					{
						Main.tile[n, num4].WallType = (ushort)ModContent.WallType<ArqueriteBrickWallUnsafe>();
					}
					Main.tile[n, num4].TileType = (ushort)ModContent.TileType<QuartzPlatform>();
					Tile tile = Main.tile[n, num4]; 
					tile.HasTile = true;
				}
				num4--;
			}
			num8 = num4;
			double num13 = (double)((num9 - num8) * num) * 0.02;
			for (int num14 = 0; (double)num14 < num13; num14++)
			{
				int num15 = WorldGen.genRand.Next(i - num / 2, i + num / 2 + 1);
				int num16 = WorldGen.genRand.Next(num8, num9);
				int num17 = WorldGen.genRand.Next(3, 8);
				for (int num18 = num15 - num17; num18 <= num15 + num17; num18++)
				{
					for (int num19 = num16 - num17; num19 <= num16 + num17; num19++)
					{
						double num20 = Math.Abs(num18 - num15);
						double num21 = Math.Abs(num19 - num16);
						if (!(Math.Sqrt(num20 * num20 + num21 * num21) < (double)num17 * 0.4))
						{
							continue;
						}
						try
						{
							if (Main.tile[num18, num19].TileType == ModContent.TileType<ArqueriteBricks>() || Main.tile[num18, num19].TileType == ModContent.TileType<QuartzPlatform>())
							{
								Tile tile = Main.tile[num18, num19]; 
								tile.HasTile = false;
							}
							Main.tile[num18, num19].WallType = 0;
						}
						catch
						{
						}
					}
				}
			}
		}

		public static void DepthsRoom(int i, int j, int width, int height, byte type = 76, byte wall = 13)
		{
			if (j > Main.maxTilesY - 40)
			{
				return;
			}
			for (int k = i - width / 2; k <= i + width / 2; k++)
			{
				for (int l = j - height; l <= j; l++)
				{
					try
					{
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
						Main.tile[k, l].TileType = type;
						Main.tile[k, l].LiquidAmount = 0;
						tile.LiquidType = -1;
					}
					catch
					{
					}
				}
			}
			for (int m = i - width / 2 + 1; m <= i + width / 2 - 1; m++)
			{
				for (int n = j - height + 1; n <= j - 1; n++)
				{
					try
					{
						Tile tile = Main.tile[m, n];
						tile.HasTile = false;
						Main.tile[m, n].WallType = wall;
						Main.tile[m, n].LiquidAmount = 0;
						tile.LiquidType = -1;
					}
					catch
					{
					}
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

		private static bool IsUndergroundDesert(int x, int y)
		{
			if ((double)y < Main.worldSurface)
			{
				return false;
			}
			if ((double)x < (double)Main.maxTilesX * 0.15 || (double)x > (double)Main.maxTilesX * 0.85)
			{
				return false;
			}
			if (WorldGen.remixWorldGen && (double)y > Main.rockLayer)
			{
				return false;
			}
			int num = 15;
			for (int i = x - num; i <= x + num; i++)
			{
				for (int j = y - num; j <= y + num; j++)
				{
					if (Main.tile[i, j].WallType == 187 || Main.tile[i, j].WallType == 216)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsDungeon(int x, int y)
		{
			if ((double)y < Main.worldSurface)
			{
				return false;
			}
			if (x < 0 || x > Main.maxTilesX)
			{
				return false;
			}
			if (Main.wallDungeon[Main.tile[x, y].WallType])
			{
				return true;
			}
			return false;
		}

		public static bool AddBuriedChest(int i, int j, int contain = 0, bool notNearOtherChests = false, int Style = -1, bool trySlope = false, ushort chestTileType = 0)
		{
			if (chestTileType == 0)
			{
				chestTileType = 21;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			bool flag10 = false;
			int num = 15;
			if (WorldGen.tenthAnniversaryWorldGen)
			{
				num *= 3;
			}
			for (int k = j; k < Main.maxTilesY - 10; k++)
			{
				int num2 = -1;
				int num3 = -1;
				if (Main.tile[i, k].LiquidType == LiquidID.Shimmer)
				{
					return false;
				}
				if (trySlope && Main.tile[i, k].HasTile && Main.tileSolid[Main.tile[i, k].TileType] && !Main.tileSolidTop[Main.tile[i, k].TileType])
				{
					if (Style == 17)
					{
						int num4 = 30;
						for (int l = i - num4; l <= i + num4; l++)
						{
							for (int m = k - num4; m <= k + num4; m++)
							{
								if (!WorldGen.InWorld(l, m, 5))
								{
									return false;
								}
								if (Main.tile[l, m].HasTile && (Main.tile[l, m].TileType == 21 || Main.tile[l, m].TileType == 467))
								{
									return false;
								}
							}
						}
					}
					if (Main.tile[i - 1, k].TopSlope)
					{
						num2 = (int)Main.tile[i - 1, k].Slope;
						Tile Slope1 = Main.tile[i - 1, k];
						Slope1.Slope = 0;
					}
					if (Main.tile[i, k].TopSlope)
					{
						num3 = (int)Main.tile[i, k].Slope;
						Tile Slope2 = Main.tile[i, k];
						Slope2.Slope = 0;
					}
				}
				if (WorldGen.remixWorldGen && (double)i > (double)Main.maxTilesX * 0.37 && (double)i < (double)Main.maxTilesX * 0.63 && k > Main.maxTilesY - 250)
				{
					return false;
				}
				int num5 = 2;
				for (int n = i - num5; n <= i + num5; n++)
				{
					for (int num6 = k - num5; num6 <= k + num5; num6++)
					{
						if (Main.tile[n, num6].HasTile && (TileID.Sets.Boulders[Main.tile[n, num6].TileType] || Main.tile[n, num6].TileType == 26 || Main.tile[n, num6].TileType == 237))
						{
							return false;
						}
					}
				}
				if (!WorldGen.SolidTile(i, k))
				{
					continue;
				}
				bool flag11 = false;
				int num7 = k;
				int num8 = -1;
				int num9 = 0;
				bool flag12 = (double)num7 >= Main.worldSurface + 25.0;
				if (WorldGen.remixWorldGen)
				{
					flag12 = num7 < Main.maxTilesY - 400;
				}
				if (flag12 || contain > 0)
				{
					num9 = 1;
				}
				if (Style >= 0)
				{
					num9 = Style;
				}
				if ((chestTileType == 467 && num9 == 10) || (contain == 0 && num7 <= Main.maxTilesY - 205 && IsUndergroundDesert(i, k)))
				{
					flag2 = true;
					num9 = 10;
					chestTileType = 467;
					contain = ((num7 <= (GenVars.desertHiveHigh * 3 + GenVars.desertHiveLow * 4) / 7) ? Utils.SelectRandom(WorldGen.genRand, new short[4] { 4056, 4055, 4262, 4263 }) : Utils.SelectRandom(WorldGen.genRand, new short[3] { 4061, 4062, 4276 }));
					if (WorldGen.getGoodWorldGen && WorldGen.genRand.Next(num) == 0)
					{
						contain = 52;
					}
				}
				if (chestTileType == 21 && (num9 == 11 || (contain == 0 && (double)num7 >= Main.worldSurface + 25.0 && num7 <= Main.maxTilesY - 205 && (Main.tile[i, k].TileType == 147 || Main.tile[i, k].TileType == 161 || Main.tile[i, k].TileType == 162))))
				{
					flag = true;
					num9 = 11;
					contain = WorldGen.genRand.Next(6) switch
					{
						0 => 670,
						1 => 724,
						2 => 950,
						3 => (!WorldGen.remixWorldGen) ? 1319 : 725,
						4 => 987,
						_ => 1579,
					};
					if (WorldGen.genRand.Next(20) == 0)
					{
						contain = 997;
					}
					if (WorldGen.genRand.Next(50) == 0)
					{
						contain = 669;
					}
					if (WorldGen.getGoodWorldGen && WorldGen.genRand.Next(num) == 0)
					{
						contain = 52;
					}
				}
				if (chestTileType == 21 && (Style == 10 || contain == 211 || contain == 212 || contain == 213 || contain == 753))
				{
					flag3 = true;
					num9 = 10;
					if (WorldGen.getGoodWorldGen && WorldGen.genRand.Next(num) == 0)
					{
						contain = 52;
					}
				}
				if (chestTileType == 21 && num7 > Main.maxTilesY - 205 && contain == 0)
				{
					flag7 = true;
					contain = GenVars.hellChestItem[GenVars.hellChest];
					num9 = 4;
					flag11 = true;
					if (WorldGen.getGoodWorldGen && WorldGen.genRand.Next(num) == 0)
					{
						contain = 52;
					}
				}
				if (chestTileType == 21 && num9 == 17)
				{
					flag4 = true;
					if (WorldGen.getGoodWorldGen && WorldGen.genRand.Next(num) == 0)
					{
						contain = 52;
					}
				}
				if (chestTileType == 21 && num9 == 12)
				{
					flag5 = true;
					if (WorldGen.getGoodWorldGen && WorldGen.genRand.Next(num) == 0)
					{
						contain = 52;
					}
				}
				if (chestTileType == 21 && num9 == 32)
				{
					flag6 = true;
					if (WorldGen.getGoodWorldGen && WorldGen.genRand.Next(num) == 0)
					{
						contain = 52;
					}
				}
				if (chestTileType == 21 && num9 != 0 && IsDungeon(i, k))
				{
					flag8 = true;
				}
				if (chestTileType == 21 && num9 != 0 && (contain == 848 || contain == 857 || contain == 934))
				{
					flag9 = true;
				}
				if (chestTileType == 21 && (num9 == 13 || contain == 159 || contain == 65 || contain == 158 || contain == 2219))
				{
					flag10 = true;
					if (WorldGen.remixWorldGen && !WorldGen.getGoodWorldGen)
					{
						if (WorldGen.crimson)
						{
							num9 = 43;
						}
						else
						{
							chestTileType = 467;
							num9 = 3;
						}
					}
				}
				if (WorldGen.noTrapsWorldGen && num9 == 1 && chestTileType == 21 && (!WorldGen.remixWorldGen || WorldGen.genRand.Next(3) == 0))
				{
					num9 = 4;
					chestTileType = 467;
				}
				num8 = ((chestTileType != 467) ? WorldGen.PlaceChest(i - 1, num7 - 1, chestTileType, notNearOtherChests, num9) : WorldGen.PlaceChest(i - 1, num7 - 1, chestTileType, notNearOtherChests, num9));
				if (num8 >= 0)
				{
					if (flag11)
					{
						GenVars.hellChest++;
						if (GenVars.hellChest >= GenVars.hellChestItem.Length)
						{
							GenVars.hellChest = 0;
						}
					}
					Chest chest = Main.chest[num8];
					int num10 = 0;
					while (num10 == 0)
					{
						bool flag13 = (double)num7 < Main.worldSurface + 25.0;
						if (WorldGen.remixWorldGen)
						{
							flag13 = (double)num7 >= (Main.rockLayer + (double)((Main.maxTilesY - 350) * 2)) / 3.0;
						}
						if ((num9 == 0 && flag13) || flag9)
						{
							if (contain > 0)
							{
								chest.item[num10].SetDefaults(contain);
								chest.item[num10].Prefix(-1);
								num10++;
								switch (contain)
								{
									case 848:
										chest.item[num10].SetDefaults(866);
										num10++;
										break;
									case 832:
										chest.item[num10].SetDefaults(933);
										num10++;
										if (WorldGen.genRand.Next(6) == 0)
										{
											int num11 = WorldGen.genRand.Next(2);
											switch (num11)
											{
												case 0:
													num11 = 4429;
													break;
												case 1:
													num11 = 4427;
													break;
											}
											chest.item[num10].SetDefaults(num11);
											num10++;
										}
										break;
								}
								if (Main.tenthAnniversaryWorld && flag9)
								{
									chest.item[num10++].SetDefaults(848);
									chest.item[num10++].SetDefaults(866);
								}
							}
							else
							{
								int num12 = WorldGen.genRand.Next(10);
								if (num12 == 0)
								{
									chest.item[num10].SetDefaults(280);
									chest.item[num10].Prefix(-1);
								}
								if (num12 == 1)
								{
									chest.item[num10].SetDefaults(281);
									chest.item[num10].Prefix(-1);
								}
								if (num12 == 2)
								{
									chest.item[num10].SetDefaults(284);
									chest.item[num10].Prefix(-1);
								}
								if (num12 == 3)
								{
									chest.item[num10].SetDefaults(285);
									chest.item[num10].Prefix(-1);
								}
								if (num12 == 4)
								{
									chest.item[num10].SetDefaults(953);
									chest.item[num10].Prefix(-1);
								}
								if (num12 == 5)
								{
									chest.item[num10].SetDefaults(946);
									chest.item[num10].Prefix(-1);
								}
								if (num12 == 6)
								{
									chest.item[num10].SetDefaults(3068);
									chest.item[num10].Prefix(-1);
								}
								if (num12 == 7)
								{
									if (WorldGen.remixWorldGen)
									{
										chest.item[num10].SetDefaults(517);
										chest.item[num10].Prefix(-1);
									}
									else
									{
										chest.item[num10].SetDefaults(3069);
										chest.item[num10].Prefix(-1);
									}
								}
								if (num12 == 8)
								{
									chest.item[num10].SetDefaults(3084);
									chest.item[num10].Prefix(-1);
								}
								if (num12 == 9)
								{
									chest.item[num10].SetDefaults(4341);
									chest.item[num10].Prefix(-1);
								}
								num10++;
							}
							if (WorldGen.genRand.Next(6) == 0)
							{
								int stack = WorldGen.genRand.Next(40, 76);
								chest.item[num10].SetDefaults(282);
								chest.item[num10].stack = stack;
								num10++;
							}
							if (WorldGen.genRand.Next(6) == 0)
							{
								int stack2 = WorldGen.genRand.Next(150, 301);
								chest.item[num10].SetDefaults(279);
								chest.item[num10].stack = stack2;
								num10++;
							}
							if (WorldGen.genRand.Next(6) == 0)
							{
								chest.item[num10].SetDefaults(3093);
								chest.item[num10].stack = 1;
								if (WorldGen.genRand.Next(5) == 0)
								{
									chest.item[num10].stack += WorldGen.genRand.Next(2);
								}
								if (WorldGen.genRand.Next(10) == 0)
								{
									chest.item[num10].stack += WorldGen.genRand.Next(3);
								}
								num10++;
							}
							if (WorldGen.genRand.Next(6) == 0)
							{
								chest.item[num10].SetDefaults(4345);
								chest.item[num10].stack = 1;
								if (WorldGen.genRand.Next(5) == 0)
								{
									chest.item[num10].stack += WorldGen.genRand.Next(2);
								}
								if (WorldGen.genRand.Next(10) == 0)
								{
									chest.item[num10].stack += WorldGen.genRand.Next(3);
								}
								num10++;
							}
							if (WorldGen.genRand.Next(3) == 0)
							{
								chest.item[num10].SetDefaults(168);
								chest.item[num10].stack = WorldGen.genRand.Next(3, 6);
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int num13 = WorldGen.genRand.Next(2);
								int stack3 = WorldGen.genRand.Next(8) + 3;
								if (num13 == 0)
								{
									chest.item[num10].SetDefaults(GenVars.copperBar);
								}
								if (num13 == 1)
								{
									chest.item[num10].SetDefaults(GenVars.ironBar);
								}
								chest.item[num10].stack = stack3;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int stack4 = WorldGen.genRand.Next(50, 101);
								chest.item[num10].SetDefaults(965);
								chest.item[num10].stack = stack4;
								num10++;
							}
							if (WorldGen.genRand.Next(3) != 0)
							{
								int num14 = WorldGen.genRand.Next(2);
								int stack5 = WorldGen.genRand.Next(26) + 25;
								if (num14 == 0)
								{
									chest.item[num10].SetDefaults(40);
								}
								if (num14 == 1)
								{
									chest.item[num10].SetDefaults(42);
								}
								chest.item[num10].stack = stack5;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int stack6 = WorldGen.genRand.Next(3) + 3;
								chest.item[num10].SetDefaults(28);
								chest.item[num10].stack = stack6;
								num10++;
							}
							if (WorldGen.genRand.Next(3) != 0)
							{
								chest.item[num10].SetDefaults(2350);
								chest.item[num10].stack = WorldGen.genRand.Next(3, 6);
								num10++;
							}
							if (WorldGen.genRand.Next(3) > 0)
							{
								int num15 = WorldGen.genRand.Next(6);
								int stack7 = WorldGen.genRand.Next(1, 3);
								if (num15 == 0)
								{
									chest.item[num10].SetDefaults(292);
								}
								if (num15 == 1)
								{
									chest.item[num10].SetDefaults(298);
								}
								if (num15 == 2)
								{
									chest.item[num10].SetDefaults(299);
								}
								if (num15 == 3)
								{
									chest.item[num10].SetDefaults(290);
								}
								if (num15 == 4)
								{
									chest.item[num10].SetDefaults(2322);
								}
								if (num15 == 5)
								{
									chest.item[num10].SetDefaults(2325);
								}
								chest.item[num10].stack = stack7;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int num16 = WorldGen.genRand.Next(2);
								int stack8 = WorldGen.genRand.Next(11) + 10;
								if (num16 == 0)
								{
									chest.item[num10].SetDefaults(8);
								}
								if (num16 == 1)
								{
									chest.item[num10].SetDefaults(31);
								}
								chest.item[num10].stack = stack8;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								chest.item[num10].SetDefaults(72);
								chest.item[num10].stack = WorldGen.genRand.Next(10, 30);
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								chest.item[num10].SetDefaults(9);
								chest.item[num10].stack = WorldGen.genRand.Next(50, 100);
								num10++;
							}
						}
						else if ((!WorldGen.remixWorldGen && (double)num7 < Main.rockLayer) || (WorldGen.remixWorldGen && (double)num7 > Main.rockLayer && num7 < Main.maxTilesY - 250))
						{
							if (contain > 0)
							{
								if (contain == 832)
								{
									chest.item[num10].SetDefaults(933);
									num10++;
								}
								chest.item[num10].SetDefaults(contain);
								chest.item[num10].Prefix(-1);
								num10++;
								if (flag4)
								{
									if (WorldGen.genRand.Next(2) == 0)
									{
										chest.item[num10++].SetDefaults(4425);
									}
									if (WorldGen.genRand.Next(2) == 0)
									{
										chest.item[num10++].SetDefaults(4460);
									}
								}
								if (flag10 && WorldGen.genRand.Next(40) == 0)
								{
									chest.item[num10++].SetDefaults(4978);
								}
								if (flag5 && WorldGen.genRand.Next(10) == 0)
								{
									int num17 = WorldGen.genRand.Next(2);
									switch (num17)
									{
										case 0:
											num17 = 4429;
											break;
										case 1:
											num17 = 4427;
											break;
									}
									chest.item[num10].SetDefaults(num17);
									num10++;
								}
								if (flag8 && (!GenVars.generatedShadowKey || WorldGen.genRand.Next(3) == 0))
								{
									GenVars.generatedShadowKey = true;
									chest.item[num10].SetDefaults(329);
									num10++;
								}
							}
							else
							{
								switch (WorldGen.genRand.Next(6))
								{
									case 0:
										chest.item[num10].SetDefaults(49);
										chest.item[num10].Prefix(-1);
										break;
									case 1:
										chest.item[num10].SetDefaults(50);
										chest.item[num10].Prefix(-1);
										break;
									case 2:
										chest.item[num10].SetDefaults(53);
										chest.item[num10].Prefix(-1);
										break;
									case 3:
										chest.item[num10].SetDefaults(54);
										chest.item[num10].Prefix(-1);
										break;
									case 4:
										chest.item[num10].SetDefaults(5011);
										chest.item[num10].Prefix(-1);
										break;
									default:
										chest.item[num10].SetDefaults(975);
										chest.item[num10].Prefix(-1);
										break;
								}
								num10++;
								if (WorldGen.genRand.Next(20) == 0)
								{
									chest.item[num10].SetDefaults(997);
									chest.item[num10].Prefix(-1);
									num10++;
								}
								else if (WorldGen.genRand.Next(20) == 0)
								{
									chest.item[num10].SetDefaults(930);
									chest.item[num10].Prefix(-1);
									num10++;
									chest.item[num10].SetDefaults(931);
									chest.item[num10].stack = WorldGen.genRand.Next(26) + 25;
									num10++;
								}
								if (flag6 && WorldGen.genRand.Next(2) == 0)
								{
									chest.item[num10].SetDefaults(4450);
									num10++;
								}
								if (flag6 && WorldGen.genRand.Next(3) == 0)
								{
									chest.item[num10].SetDefaults(4779);
									num10++;
									chest.item[num10].SetDefaults(4780);
									num10++;
									chest.item[num10].SetDefaults(4781);
									num10++;
								}
							}
							if (flag2)
							{
								if (WorldGen.genRand.Next(3) == 0)
								{
									chest.item[num10].SetDefaults(4423);
									chest.item[num10].stack = WorldGen.genRand.Next(10, 20);
									num10++;
								}
							}
							else if (WorldGen.genRand.Next(3) == 0)
							{
								chest.item[num10].SetDefaults(166);
								chest.item[num10].stack = WorldGen.genRand.Next(10, 20);
								num10++;
							}
							if (WorldGen.genRand.Next(5) == 0)
							{
								chest.item[num10].SetDefaults(52);
								num10++;
							}
							if (WorldGen.genRand.Next(3) == 0)
							{
								int stack9 = WorldGen.genRand.Next(50, 101);
								chest.item[num10].SetDefaults(965);
								chest.item[num10].stack = stack9;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int num18 = WorldGen.genRand.Next(2);
								int stack10 = WorldGen.genRand.Next(10) + 5;
								if (num18 == 0)
								{
									chest.item[num10].SetDefaults(GenVars.ironBar);
								}
								if (num18 == 1)
								{
									chest.item[num10].SetDefaults(GenVars.silverBar);
								}
								chest.item[num10].stack = stack10;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int num19 = WorldGen.genRand.Next(2);
								int stack11 = WorldGen.genRand.Next(25) + 25;
								if (num19 == 0)
								{
									chest.item[num10].SetDefaults(40);
								}
								if (num19 == 1)
								{
									chest.item[num10].SetDefaults(42);
								}
								chest.item[num10].stack = stack11;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int stack12 = WorldGen.genRand.Next(3) + 3;
								chest.item[num10].SetDefaults(28);
								chest.item[num10].stack = stack12;
								num10++;
							}
							if (WorldGen.genRand.Next(3) > 0)
							{
								int num20 = WorldGen.genRand.Next(9);
								int stack13 = WorldGen.genRand.Next(1, 3);
								if (num20 == 0)
								{
									chest.item[num10].SetDefaults(289);
								}
								if (num20 == 1)
								{
									chest.item[num10].SetDefaults(298);
								}
								if (num20 == 2)
								{
									chest.item[num10].SetDefaults(299);
								}
								if (num20 == 3)
								{
									chest.item[num10].SetDefaults(290);
								}
								if (num20 == 4)
								{
									chest.item[num10].SetDefaults(303);
								}
								if (num20 == 5)
								{
									chest.item[num10].SetDefaults(291);
								}
								if (num20 == 6)
								{
									chest.item[num10].SetDefaults(304);
								}
								if (num20 == 7)
								{
									chest.item[num10].SetDefaults(2322);
								}
								if (num20 == 8)
								{
									chest.item[num10].SetDefaults(2329);
								}
								chest.item[num10].stack = stack13;
								num10++;
							}
							if (WorldGen.genRand.Next(3) != 0)
							{
								int stack14 = WorldGen.genRand.Next(2, 5);
								chest.item[num10].SetDefaults(2350);
								chest.item[num10].stack = stack14;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int stack15 = WorldGen.genRand.Next(11) + 10;
								if (num9 == 11)
								{
									chest.item[num10].SetDefaults(974);
								}
								else
								{
									chest.item[num10].SetDefaults(8);
								}
								chest.item[num10].stack = stack15;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								chest.item[num10].SetDefaults(72);
								chest.item[num10].stack = WorldGen.genRand.Next(50, 90);
								num10++;
							}
						}
						else if (num7 < Main.maxTilesY - 250 || (WorldGen.remixWorldGen && (Style == 7 || Style == 14)))
						{
							if (contain > 0)
							{
								chest.item[num10].SetDefaults(contain);
								chest.item[num10].Prefix(-1);
								num10++;
								if (flag && WorldGen.genRand.Next(5) == 0)
								{
									chest.item[num10].SetDefaults(3199);
									num10++;
								}
								if (flag2)
								{
									if (WorldGen.genRand.Next(7) == 0)
									{
										chest.item[num10].SetDefaults(4346);
										num10++;
									}
									if (WorldGen.genRand.Next(15) == 0)
									{
										chest.item[num10].SetDefaults(4066);
										num10++;
									}
								}
								if (flag3 && WorldGen.genRand.Next(6) == 0)
								{
									chest.item[num10++].SetDefaults(3360);
									chest.item[num10++].SetDefaults(3361);
								}
								if (flag3 && WorldGen.genRand.Next(10) == 0)
								{
									chest.item[num10++].SetDefaults(4426);
								}
								if (flag4)
								{
									if (WorldGen.genRand.Next(2) == 0)
									{
										chest.item[num10++].SetDefaults(4425);
									}
									if (WorldGen.genRand.Next(2) == 0)
									{
										chest.item[num10++].SetDefaults(4460);
									}
								}
								if (flag8 && (!GenVars.generatedShadowKey || WorldGen.genRand.Next(3) == 0))
								{
									GenVars.generatedShadowKey = true;
									chest.item[num10].SetDefaults(329);
									num10++;
								}
							}
							else
							{
								int num21 = WorldGen.genRand.Next(7);
								bool flag14 = num7 > GenVars.lavaLine;
								if (WorldGen.remixWorldGen)
								{
									flag14 = (double)num7 > Main.worldSurface && (double)num7 < Main.rockLayer;
								}
								int maxValue = 20;
								if (WorldGen.tenthAnniversaryWorldGen)
								{
									maxValue = 15;
								}
								if (WorldGen.genRand.Next(maxValue) == 0 && flag14)
								{
									chest.item[num10].SetDefaults(ModContent.ItemType<Items.Accessories.AmalgamAmulet>());
									chest.item[num10].Prefix(-1);
								}
								else if (WorldGen.genRand.Next(15) == 0)
								{
									chest.item[num10].SetDefaults(997);
									chest.item[num10].Prefix(-1);
								}
								else
								{
									if (num21 == 0)
									{
										chest.item[num10].SetDefaults(49);
										chest.item[num10].Prefix(-1);
									}
									if (num21 == 1)
									{
										chest.item[num10].SetDefaults(50);
										chest.item[num10].Prefix(-1);
									}
									if (num21 == 2)
									{
										chest.item[num10].SetDefaults(53);
										chest.item[num10].Prefix(-1);
									}
									if (num21 == 3)
									{
										chest.item[num10].SetDefaults(54);
										chest.item[num10].Prefix(-1);
									}
									if (num21 == 4)
									{
										chest.item[num10].SetDefaults(5011);
										chest.item[num10].Prefix(-1);
									}
									if (num21 == 5)
									{
										chest.item[num10].SetDefaults(975);
										chest.item[num10].Prefix(-1);
									}
									if (num21 == 6)
									{
										chest.item[num10].SetDefaults(930);
										chest.item[num10].Prefix(-1);
										num10++;
										chest.item[num10].SetDefaults(931);
										chest.item[num10].stack = WorldGen.genRand.Next(26) + 25;
									}
								}
								num10++;
								if (flag6)
								{
									if (WorldGen.genRand.Next(2) == 0)
									{
										chest.item[num10].SetDefaults(4450);
										num10++;
									}
									else
									{
										chest.item[num10].SetDefaults(4779);
										num10++;
										chest.item[num10].SetDefaults(4780);
										num10++;
										chest.item[num10].SetDefaults(4781);
										num10++;
									}
								}
							}
							if (WorldGen.genRand.Next(5) == 0)
							{
								chest.item[num10].SetDefaults(43);
								num10++;
							}
							if (WorldGen.genRand.Next(3) == 0)
							{
								chest.item[num10].SetDefaults(167);
								num10++;
							}
							if (WorldGen.genRand.Next(4) == 0)
							{
								chest.item[num10].SetDefaults(51);
								chest.item[num10].stack = WorldGen.genRand.Next(26) + 25;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int num22 = WorldGen.genRand.Next(2);
								int stack16 = WorldGen.genRand.Next(8) + 3;
								if (num22 == 0)
								{
									chest.item[num10].SetDefaults(GenVars.goldBar);
								}
								if (num22 == 1)
								{
									chest.item[num10].SetDefaults(GenVars.silverBar);
								}
								chest.item[num10].stack = stack16;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int num23 = WorldGen.genRand.Next(2);
								int stack17 = WorldGen.genRand.Next(26) + 25;
								if (num23 == 0)
								{
									chest.item[num10].SetDefaults(41);
								}
								if (num23 == 1)
								{
									chest.item[num10].SetDefaults(279);
								}
								chest.item[num10].stack = stack17;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int stack18 = WorldGen.genRand.Next(3) + 3;
								chest.item[num10].SetDefaults(188);
								chest.item[num10].stack = stack18;
								num10++;
							}
							if (WorldGen.genRand.Next(3) > 0)
							{
								int num24 = WorldGen.genRand.Next(6);
								int stack19 = WorldGen.genRand.Next(1, 3);
								if (num24 == 0)
								{
									chest.item[num10].SetDefaults(296);
								}
								if (num24 == 1)
								{
									chest.item[num10].SetDefaults(295);
								}
								if (num24 == 2)
								{
									chest.item[num10].SetDefaults(299);
								}
								if (num24 == 3)
								{
									chest.item[num10].SetDefaults(302);
								}
								if (num24 == 4)
								{
									chest.item[num10].SetDefaults(303);
								}
								if (num24 == 5)
								{
									chest.item[num10].SetDefaults(305);
								}
								chest.item[num10].stack = stack19;
								num10++;
							}
							if (WorldGen.genRand.Next(3) > 1)
							{
								int num25 = WorldGen.genRand.Next(6);
								int stack20 = WorldGen.genRand.Next(1, 3);
								if (num25 == 0)
								{
									chest.item[num10].SetDefaults(301);
								}
								if (num25 == 1)
								{
									chest.item[num10].SetDefaults(297);
								}
								if (num25 == 2)
								{
									chest.item[num10].SetDefaults(304);
								}
								if (num25 == 3)
								{
									chest.item[num10].SetDefaults(2329);
								}
								if (num25 == 4)
								{
									chest.item[num10].SetDefaults(2351);
								}
								if (num25 == 5)
								{
									chest.item[num10].SetDefaults(2326);
								}
								chest.item[num10].stack = stack20;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int stack21 = WorldGen.genRand.Next(2, 5);
								chest.item[num10].SetDefaults(2350);
								chest.item[num10].stack = stack21;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int num26 = WorldGen.genRand.Next(2);
								int stack22 = WorldGen.genRand.Next(15) + 15;
								if (num26 == 0)
								{
									if (num9 == 11)
									{
										chest.item[num10].SetDefaults(974);
									}
									else
									{
										chest.item[num10].SetDefaults(8);
									}
								}
								if (num26 == 1)
								{
									chest.item[num10].SetDefaults(282);
								}
								chest.item[num10].stack = stack22;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								chest.item[num10].SetDefaults(73);
								chest.item[num10].stack = WorldGen.genRand.Next(1, 3);
								num10++;
							}
						}
						else
						{
							if (contain > 0)
							{
								chest.item[num10].SetDefaults(contain);
								chest.item[num10].Prefix(-1);
								num10++;
								if (flag7 && WorldGen.genRand.Next(5) == 0)
								{
									chest.item[num10].SetDefaults(ModContent.ItemType<Items.Accessories.LodeStone>());
									num10++;
								}
								if (flag7 && WorldGen.genRand.Next(10) == 0)
								{
									chest.item[num10].SetDefaults(ModContent.ItemType<Items.PhantomFirecart>());
									num10++;
								}
								if (flag7 && WorldGen.genRand.Next(10) == 0)
								{
									chest.item[num10].SetDefaults(4737);
									num10++;
								}
								if (flag7 && WorldGen.genRand.Next(10) == 0)
								{
									chest.item[num10].SetDefaults(ModContent.ItemType<Pets.ShadowCat.GeodeLazerPointer>());
									num10++;
								}
							}
							else
							{
								int num27 = WorldGen.genRand.Next(4);
								if (num27 == 0)
								{
									chest.item[num10].SetDefaults(49);
									chest.item[num10].Prefix(-1);
								}
								if (num27 == 1)
								{
									chest.item[num10].SetDefaults(50);
									chest.item[num10].Prefix(-1);
								}
								if (num27 == 2)
								{
									chest.item[num10].SetDefaults(53);
									chest.item[num10].Prefix(-1);
								}
								if (num27 == 3)
								{
									chest.item[num10].SetDefaults(54);
									chest.item[num10].Prefix(-1);
								}
								num10++;
							}
							if (WorldGen.genRand.Next(3) == 0)
							{
								chest.item[num10].SetDefaults(167);
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int num28 = WorldGen.genRand.Next(2);
								int stack23 = WorldGen.genRand.Next(15) + 15;
								if (num28 == 0)
								{
									chest.item[num10].SetDefaults(117);
								}
								if (num28 == 1)
								{
									chest.item[num10].SetDefaults(GenVars.goldBar);
								}
								chest.item[num10].stack = stack23;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int num29 = WorldGen.genRand.Next(2);
								int stack24 = WorldGen.genRand.Next(25) + 50;
								if (num29 == 0)
								{
									chest.item[num10].SetDefaults(ModContent.ItemType<Items.Weapons.DiamondArrow>());
								}
								if (num29 == 1)
								{
									if (WorldGen.SavedOreTiers.Silver == 168)
									{
										chest.item[num10].SetDefaults(4915);
									}
									else
									{
										chest.item[num10].SetDefaults(278);
									}
								}
								chest.item[num10].stack = stack24;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int stack25 = WorldGen.genRand.Next(6) + 15;
								chest.item[num10].SetDefaults(227);
								chest.item[num10].stack = stack25;
								num10++;
							}
							if (WorldGen.genRand.Next(4) > 0)
							{
								int num30 = WorldGen.genRand.Next(8);
								int stack26 = WorldGen.genRand.Next(1, 3);
								if (num30 == 0)
								{
									chest.item[num10].SetDefaults(296);
								}
								if (num30 == 1)
								{
									chest.item[num10].SetDefaults(295);
								}
								if (num30 == 2)
								{
									chest.item[num10].SetDefaults(293);
								}
								if (num30 == 3)
								{
									chest.item[num10].SetDefaults(ModContent.ItemType<Items.CrystalSkinPotion>());
								}
								if (num30 == 4)
								{
									chest.item[num10].SetDefaults(294);
								}
								if (num30 == 5)
								{
									chest.item[num10].SetDefaults(297);
								}
								if (num30 == 6)
								{
									chest.item[num10].SetDefaults(304);
								}
								if (num30 == 7)
								{
									chest.item[num10].SetDefaults(2323);
								}
								chest.item[num10].stack = stack26;
								num10++;
							}
							if (WorldGen.genRand.Next(3) > 0)
							{
								int num31 = WorldGen.genRand.Next(8);
								int stack27 = WorldGen.genRand.Next(1, 3);
								if (num31 == 0)
								{
									chest.item[num10].SetDefaults(305);
								}
								if (num31 == 1)
								{
									chest.item[num10].SetDefaults(301);
								}
								if (num31 == 2)
								{
									chest.item[num10].SetDefaults(302);
								}
								if (num31 == 3)
								{
									chest.item[num10].SetDefaults(ModContent.ItemType<Items.CrystalSkinPotion>());
								}
								if (num31 == 4)
								{
									chest.item[num10].SetDefaults(300);
								}
								if (num31 == 5)
								{
									chest.item[num10].SetDefaults(2351);
								}
								if (num31 == 6)
								{
									chest.item[num10].SetDefaults(ModContent.ItemType<Items.SilverSpherePotion>());
								}
								if (num31 == 7)
								{
									chest.item[num10].SetDefaults(2345);
								}
								chest.item[num10].stack = stack27;
								num10++;
							}
							if (WorldGen.genRand.Next(3) == 0)
							{
								int stack28 = WorldGen.genRand.Next(1, 3);
								if (WorldGen.genRand.Next(2) == 0)
								{
									chest.item[num10].SetDefaults(2350);
								}
								else
								{
									chest.item[num10].SetDefaults(4870);
								}
								chest.item[num10].stack = stack28;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								int num32 = WorldGen.genRand.Next(2);
								int stack29 = WorldGen.genRand.Next(15) + 15;
								if (num32 == 0)
								{
									chest.item[num10].SetDefaults(8);
								}
								if (num32 == 1)
								{
									chest.item[num10].SetDefaults(282);
								}
								chest.item[num10].stack = stack29;
								num10++;
							}
							if (WorldGen.genRand.Next(2) == 0)
							{
								chest.item[num10].SetDefaults(73);
								chest.item[num10].stack = WorldGen.genRand.Next(2, 5);
								num10++;
							}
						}
						if (num10 > 0 && chestTileType == 21)
						{
							if (num9 == 10 && WorldGen.genRand.Next(4) == 0)
							{
								chest.item[num10].SetDefaults(2204);
								num10++;
							}
							if (num9 == 11 && WorldGen.genRand.Next(7) == 0)
							{
								chest.item[num10].SetDefaults(2198);
								num10++;
							}
							if (flag10 && WorldGen.genRand.Next(3) == 0)
							{
								chest.item[num10].SetDefaults(2197);
								num10++;
							}
							if (flag10)
							{
								int num33 = WorldGen.genRand.Next(6);
								if (num33 == 0)
								{
									chest.item[num10].SetDefaults(5258);
								}
								if (num33 == 1)
								{
									chest.item[num10].SetDefaults(5226);
								}
								if (num33 == 2)
								{
									chest.item[num10].SetDefaults(5254);
								}
								if (num33 == 3)
								{
									chest.item[num10].SetDefaults(5238);
								}
								if (num33 == 4)
								{
									chest.item[num10].SetDefaults(5255);
								}
								if (num33 == 5)
								{
									chest.item[num10].SetDefaults(5388);
								}
								num10++;
							}
							if (flag10)
							{
								chest.item[num10].SetDefaults(751);
								chest.item[num10].stack = WorldGen.genRand.Next(50, 101);
								num10++;
							}
							if (num9 == 16)
							{
								chest.item[num10].SetDefaults(2195);
								num10++;
							}
							if (Main.wallDungeon[Main.tile[i, k].WallType] && WorldGen.genRand.Next(8) == 0)
							{
								chest.item[num10].SetDefaults(2192);
								num10++;
							}
							if ((num9 == 23 || num9 == 24 || num9 == 25 || num9 == 26 || num9 == 27) && WorldGen.genRand.Next(2) == 0)
							{
								chest.item[num10].SetDefaults(5234);
								num10++;
							}
							if (num9 == 16)
							{
								if (WorldGen.genRand.Next(5) == 0)
								{
									chest.item[num10].SetDefaults(2767);
									num10++;
								}
								else
								{
									chest.item[num10].SetDefaults(2766);
									chest.item[num10].stack = WorldGen.genRand.Next(3, 8);
									num10++;
								}
							}
						}
						if (num10 <= 0 || chestTileType != 467)
						{
							continue;
						}
						if (flag10 && WorldGen.genRand.Next(3) == 0)
						{
							chest.item[num10].SetDefaults(2197);
							num10++;
						}
						if (flag10)
						{
							int num34 = WorldGen.genRand.Next(5);
							if (num34 == 0)
							{
								chest.item[num10].SetDefaults(5258);
							}
							if (num34 == 1)
							{
								chest.item[num10].SetDefaults(5226);
							}
							if (num34 == 2)
							{
								chest.item[num10].SetDefaults(5254);
							}
							if (num34 == 3)
							{
								chest.item[num10].SetDefaults(5238);
							}
							if (num34 == 4)
							{
								chest.item[num10].SetDefaults(5255);
							}
							num10++;
						}
						if (flag10)
						{
							chest.item[num10].SetDefaults(751);
							chest.item[num10].stack = WorldGen.genRand.Next(50, 101);
							num10++;
						}
						if (num9 == 13 && WorldGen.genRand.Next(2) == 0)
						{
							chest.item[num10].SetDefaults(5234);
							num10++;
						}
					}
					return true;
				}
				if (trySlope)
				{
					if (num2 > -1)
					{
						Tile Slope1 = Main.tile[i - 1, k]; 
						Slope1.Slope = (SlopeType)(byte)num2;
					}
					if (num3 > -1)
					{
						Tile Slope2 = Main.tile[i, k]; 
						Slope2.Slope = (SlopeType)(byte)num3;
					}
				}
				return false;
			}
			return false;
		}
	}
}