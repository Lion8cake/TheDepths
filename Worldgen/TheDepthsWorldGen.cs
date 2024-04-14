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
using TheDepths.Tiles.Furniture;
using System.IO;
using TheDepths.Tiles.Trees;
using TheDepths.Worldgen.Generation;
using static Terraria.ModLoader.ModContent;

namespace TheDepths.Worldgen
{    
    public enum UnderworldOptions {
	    Random,
	    Underworld,
	    Depths,
    }
    
    public class TheDepthsWorldGen : ModSystem {
	    public UnderworldOptions SelectedUnderworldOption { get; set; } = UnderworldOptions.Random;
		public static bool isWorldDepths;

		public static bool DrunkDepthsLeft;
		public static bool DrunkDepthsRight;

		//The most fitting place since we already are syncing depths world tags here so we might as well have the chasme downed code here too
		public static bool downedChasme = false;

		/// <summary>
		/// Detects if the player is on the depths side of the drunk seed if the depths is on the Right
		/// </summary>
		public static bool IsPlayerInRightDepths(Player player) => DrunkDepthsRight && Math.Abs(player.position.ToTileCoordinates().X) > Main.maxTilesX / 2;

		/// <summary>
		///   Detects if the player is on the depths side of the drunk seed if the depths is on the left
		/// </summary>
		public static bool IsPlayerInLeftDepths(Player player) => DrunkDepthsLeft && Math.Abs(player.position.ToTileCoordinates().X) < Main.maxTilesX / 2;

		/// <summary>
		///   Checks if the player is in the depths part of the world. This is used to reduce repetion within code as previously all the check needed was depthsorHell == true.
		/// </summary>
		public static bool InDepths(Player player) => ((isWorldDepths && !DrunkDepthsLeft && !DrunkDepthsRight) || (IsPlayerInLeftDepths(player) || IsPlayerInRightDepths(player)));

		public override void OnWorldLoad()
		{
			DrunkDepthsLeft = false;
			DrunkDepthsRight = false;
			downedChasme = false;
			//World Converter (1.4.3 => 1.4.4)
			//Also contains some explinations 
			if (!Main.dedServ) //Make sure that we are not on a server so we dont infinatly get stuck on Syncing Mods
			{
				try
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
									isWorldDepths = true; //Convert world by giving it the tag
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
												isWorldDepths = true;
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
				catch
				{
					ModContent.GetInstance<TheDepths>().Logger.Debug("Could not get the world file, you are either joining a server or world directory is false!");
				}
			}
			else
			{
				ModContent.GetInstance<TheDepths>().Logger.Debug("Could not get the world file, you are either joining a server or world directory is false!");
			}
		}

		public override void OnWorldUnload()
		{
			ModContent.GetInstance<PetrifiedWoodChandelier>().Coordinates = new(); 
			ModContent.GetInstance<PetrifiedWoodLantern>().Coordinates = new();
			isWorldDepths = false;
			DrunkDepthsLeft = false;
			DrunkDepthsRight = false;
			downedChasme = false;
		}

		public override void ClearWorld()
		{
			ModContent.GetInstance<PetrifiedWoodChandelier>().Coordinates = new();
			ModContent.GetInstance<PetrifiedWoodLantern>().Coordinates = new();
			isWorldDepths = false;
			DrunkDepthsLeft = false;
			DrunkDepthsRight = false;
			downedChasme = false;
		}

		public override void SaveWorldData(TagCompound tag)
		{
			if (isWorldDepths)
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
			tag["HasDepths"] = isWorldDepths;
			tag["DrunkDepthsLeft"] = DrunkDepthsLeft;
			tag["DrunkDepthsRight"] = DrunkDepthsRight;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			isWorldDepths = tag.ContainsKey("IsDepths");
			DrunkDepthsLeft = tag.ContainsKey("DepthsIsOnTheLeft");
			DrunkDepthsRight = tag.ContainsKey("DepthsIsOnTheRight");
			downedChasme = tag.ContainsKey("downedChasme");
		}

		public override void NetSend(BinaryWriter writer)
		{
			var flags = new BitsByte();
			flags[0] = isWorldDepths;
			flags[1] = DrunkDepthsLeft;
			flags[2] = DrunkDepthsRight;
			flags[3] = downedChasme;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			isWorldDepths = flags[0];
			DrunkDepthsLeft = flags[1];
			DrunkDepthsRight = flags[2];
			downedChasme = flags[3];
		}

		public override void PreWorldGen() {
			isWorldDepths = SelectedUnderworldOption switch {
				UnderworldOptions.Random => Main.rand.NextBool(),
				UnderworldOptions.Underworld => false,
				UnderworldOptions.Depths => true,
				_ => throw new ArgumentOutOfRangeException(),
			};

			if (Main.drunkWorld || ModSupport.DepthsModCalling.FargoBoBWSupport)
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
			if (isWorldDepths || WorldGen.drunkWorldGen || ModSupport.DepthsModCalling.FargoBoBWSupport)
			{
				int baseUnderWorldIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Underworld"));
				if (baseUnderWorldIndex >= 0)
				{
					if (WorldGen.drunkWorldGen || ModSupport.DepthsModCalling.FargoBoBWSupport || WorldGen.remixWorldGen)
					{
						if (WorldGen.drunkWorldGen)
							tasks.Insert(baseUnderWorldIndex + 1, new PassLegacy("Depths: Depths", DepthsGen.SpecialGenerate)); // Overwrite some amount of space with the Depths
						else
							tasks[baseUnderWorldIndex] = new PassLegacy("Underworld", DepthsGen.SpecialGenerate); // Replace the entire Underworld with only Depths for ddu
					}
					else
						tasks[baseUnderWorldIndex] = new PassLegacy("Underworld", DepthsGen.Generate); // Replace the Underworld entirely with the Depths
				}//When replacing the Underworld we must set the Pass name to "Underworld" otherwise mods that rely on the underworld pass will fail causing a crash or worldgen to not generate

				int hellforgeIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Hellforge"));
				if (hellforgeIndex >= 0)
					tasks[hellforgeIndex] = new PassLegacy("Hellforge", Gemforge);

				int potsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
				if (potsIndex >= 0)
				{
					tasks.Insert(potsIndex + 1, new PassLegacy("Depths: Depths Pots", Pots)); // Adds the Pots method in addition to the vanilla method. It has a check for if it should skip all other pots in drunk world gen, so it doesn't add extra pots everywhere.
                }

				int buriedChestIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Buried Chests"));
				if (buriedChestIndex >= 0)
					tasks.Insert(buriedChestIndex + 1, new PassLegacy("Depths: Item Replacer", DepthsBuriedChests)); //Replaces some chests with (drunk) or all (depths only) with depth's loot such as diamond arrows

				int resetIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Reset"));
				if (resetIndex >= 0)
					tasks.Insert(resetIndex + 1, new PassLegacy("Depths: Shadow Chest Shuffler", DepthsShadowchestGenResetter)); //Changes the lootpool of the shadowchests to include some shadow chest items

				int mossIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Moss Grass"));
				if (mossIndex >= 0)
				{
					tasks.Insert(mossIndex + 1, new PassLegacy("Depths: Quicksilver Moss Foliage", MossFoliageGen)); //Adds foliage to the Quicksilver moss
					tasks.Insert(mossIndex + 1, new PassLegacy("Depths: Quicksilver moss", MossGen)); //Replaces Lava Moss
				}

				int cleanupIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
				if (cleanupIndex >= 0)
				{
					tasks.Insert(cleanupIndex - 1, new PassLegacy("Depths: Remix Shuffling", FinishDepthsRemix)); //Finalises worldgen for both remix and normal
					tasks.Insert(cleanupIndex - 1, new PassLegacy("Depths: Tile Resetting", DrippingQuicksilverTileCleanup));
				}
			}
        }

        public override void ModifyHardmodeTasks(List<GenPass> list)
		{
			if (isWorldDepths || WorldGen.drunkWorldGen || ModSupport.DepthsModCalling.FargoBoBWSupport)
			{
				list.Add(new PassLegacy("The Depths: Onyx Shalestone", new WorldGenLegacyMethod(OnyxShale)));
			}
		}

		private static void OnyxShale(GenerationProgress progres, GameConfiguration configurations)
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

		public static void DepthsShadowchestGenResetter(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Resetting Shadow Chests";
			List<int> list3;
			if (WorldGen.drunkWorldGen || ModSupport.DepthsModCalling.FargoBoBWSupport)
			{
				list3 = new List<int> { 274, ModContent.ItemType<Items.Weapons.SilverStar>(), 220, ModContent.ItemType<Items.Weapons.Skyfall>(), 112, ModContent.ItemType<Items.Weapons.WhiteLightning>(), 218, ModContent.ItemType<Items.Weapons.NightFury>(), 3019 };
				if (WorldGen.remixWorldGen)
				{
					list3 = new List<int> { 274, ModContent.ItemType<Items.Weapons.SilverStar>(), 220, ModContent.ItemType<Items.Weapons.Skyfall>(), 683, ModContent.ItemType<Items.Weapons.BlueSphere>(), 218, ModContent.ItemType<Items.Weapons.NightFury>(), 3019 };
				}
			}
			else
			{
				list3 = new List<int> { 274, ModContent.ItemType<Items.Weapons.SilverStar>(), ModContent.ItemType<Items.Weapons.Skyfall>(), ModContent.ItemType<Items.Weapons.WhiteLightning>(), ModContent.ItemType<Items.Weapons.NightFury>() };
				if (WorldGen.remixWorldGen)
				{
					list3 = new List<int> { 274, ModContent.ItemType<Items.Weapons.SilverStar>(), ModContent.ItemType<Items.Weapons.Skyfall>(), ModContent.ItemType<Items.Weapons.BlueSphere>(), ModContent.ItemType<Items.Weapons.NightFury>() };
				}
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
			progress.Message = "Mercury-ifying the lower caverns";
			for (int k = (DrunkDepthsRight ? Main.maxTilesX / 2 : 0); k < (DrunkDepthsLeft ? Main.maxTilesX / 2 : Main.maxTilesX); k++)
			{
				for (int l = 0; l < Main.maxTilesY; l++)
				{
					if (Main.tile[k, l].WallType == WallID.ObsidianBackUnsafe)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall1>();
					}
					else if (Main.tile[k, l].WallType == WallID.LavaUnsafe1 || Main.tile[k, l].WallType == WallID.LavaUnsafe4)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall2>();
					}
					else if (Main.tile[k, l].WallType == WallID.LavaUnsafe2)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall3>();
					}
					else if (Main.tile[k, l].WallType == WallID.LavaUnsafe3)
					{
						WorldGen.KillWall(k, l);
						Main.tile[k, l].WallType = (ushort)ModContent.WallType<Walls.NaturalQuicksilverWall4>();
					}
					if (Main.tile[k, l].TileType == TileID.LavaDrip)
					{
						WorldGen.KillTile(k, l);
						Main.tile[k, l].TileType = (ushort)ModContent.TileType<QuicksilverDropletSource>();
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
					}
					else if (Main.tile[k, l].TileType == TileID.GeyserTrap)
					{
						WorldGen.KillTile(k, l);
						WorldGen.Place2x1(k, l, (ushort)ModContent.TileType<WaterGeyser>(), 0);
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<MercuryMoss>())
					{
						if ((!Main.tileSolid[Main.tile[k, l + 1].TileType] || !Main.tile[k, l + 1].HasTile) && (!Main.tileSolid[Main.tile[k, l - 1].TileType] || !Main.tile[k, l - 1].HasTile) && 
							(!Main.tileSolid[Main.tile[k + 1, l].TileType] || !Main.tile[k + 1, l].HasTile) && (!Main.tileSolid[Main.tile[k - 1, l].TileType] || !Main.tile[k - 1, l].HasTile))
						{
							WorldGen.KillTile(k, l);
						}
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<QuartzDoorClosed>())
					{
						Tile tile = Main.tile[k, l + 1];
						tile.Slope = SlopeType.Solid;
						tile.IsHalfBlock = false;
						Tile tile2 = Main.tile[k, l - 1];
						tile2.Slope = SlopeType.Solid;
						tile2.IsHalfBlock = false;
					}
				}
			}
		}

		public static void DepthsBuriedChests(GenerationProgress progress, GameConfiguration configuration)
		{
			for (int chestID = 0; chestID < (!WorldGen.drunkWorldGen && !ModSupport.DepthsModCalling.FargoBoBWSupport ? Main.maxChests : Main.maxChests / 2); chestID++)
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

		private void MossGen(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Growing Sparkly Moss";
			for (int k = (DrunkDepthsRight ? Main.maxTilesX / 2 : 0); k < (DrunkDepthsLeft ? Main.maxTilesX / 2 : Main.maxTilesX); k++)
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

		private void MossFoliageGen(GenerationProgress progress, GameConfiguration configuration)
		{
			for (int num163 = 5; num163 < Main.maxTilesX - 5; num163++)
			{
				for (int num164 = 5; num164 < Main.maxTilesY - 5; num164++)
				{
					if (Main.tile[num163, num164].HasTile && Main.tile[num163, num164].TileType == (ushort)ModContent.TileType<MercuryMoss>())
					{
						for (int num165 = 0; num165 < 4; num165++)
						{
							int num166 = num163;
							int num167 = num164;
							if (num165 == 0)
							{
								num166--;
							}
							if (num165 == 1)
							{
								num166++;
							}
							if (num165 == 2)
							{
								num167--;
							}
							if (num165 == 3)
							{
								num167++;
							}
							if (!Main.tile[num166, num167].HasTile)
							{
								WorldGen.PlaceTile(num166, num167, ModContent.TileType<MercuryMoss_Foliage>(), mute: true);
							}
						}
					}
				}
			}
		}

		private void Pots(GenerationProgress progress, GameConfiguration configuration)
		{
			//int potsBroken = 0;
			for (int X = (DrunkDepthsRight ? Main.maxTilesX / 2 : 0); X < (DrunkDepthsLeft ? Main.maxTilesX / 2 : Main.maxTilesX); X++)
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
			double num444 = (double)((DrunkDepthsLeft ? Main.maxTilesX / 2 : Main.maxTilesX) * Main.maxTilesY) * 0.0008;
			if (Main.starGame)
			{
				num444 *= Main.starGameMath(0.2);
			}
			for (int num445 = (DrunkDepthsRight ? Main.maxTilesX / 2 : 0); (double)num445 < num444; num445++)
			{
				double num446 = (double)num445 / num444;
				progress.Set(num446);
				bool flag25 = false;
				int num447 = 0;
				while (!flag25)
				{
					int num449 = WorldGen.genRand.Next((DrunkDepthsRight ? (Main.maxTilesX / 2) + 20: 20), (DrunkDepthsLeft ? Main.maxTilesX / 2 : Main.maxTilesX) - 20);
					bool flag26 = false;
					for (int num450 = Main.maxTilesY - 200; num450 < Main.maxTilesY - 20; num450++)
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
							if (num450 > Main.UnderworldLayer)
							{
								int type = ModContent.TileType<DepthsPot>();
								if (!WorldGen.oceanDepths(num449, num450) && !(Main.tile[num449, num450].LiquidType == LiquidID.Shimmer) && WorldGen.PlacePot(num449, num450, (ushort)type))
								{
									flag25 = true;
									break;
								}
								num447++;
								if (num447 >= 833)
								{
									flag25 = true;
									break;
								}
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
						tile.LiquidType = -1;
						tile.LiquidAmount = 0;
						if (Main.tile[k, l].TileType == ModContent.TileType<Tiles.ArqueriteOre>() || Main.tile[k, l].TileType == ModContent.TileType<Tiles.Shalestone>())
						{
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.ShaleBlock>();
						}
					}
				}
				WorldGen.AddTrees(undergroundOnly: true);
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
	}
}