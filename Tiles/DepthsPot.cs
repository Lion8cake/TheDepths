using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;
using TheDepths.Items;
using TheDepths.Items.Weapons;
using Terraria.DataStructures;
using TheDepths.Dusts;
using TheDepths.Gores;

namespace TheDepths.Tiles
{
	public class DepthsPot : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileOreFinderPriority[Type] = 100;
			Main.tileSpelunker[Type] = true;
			Main.tileCut[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(226, 227, 231), name);
			DustType = ModContent.DustType<QuartzDust>();
			HitSound = SoundID.Shatter;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile tile = Main.tile[i, j];
			if (i > 5 && j > 5 && i < Main.maxTilesX - 5 && j < Main.maxTilesY - 5 && Main.tile[i, j] != null)
			{
				if (WorldGen.SkipFramingBecauseOfGen && !Main.tileFrameImportant[Main.tile[i, j].TileType])
				{
					return false;
				}
				if (tile.HasTile)
				{
					if (noBreak && Main.tileFrameImportant[tile.TileType] && !TileID.Sets.Torch[tile.TileType])
					{
						return false;
					}
					if (Main.tileFrameImportant[tile.TileType])
					{
						CheckPot(i, j);
					}
				}
			}
			return false;
		}

		public void CheckPot(int i, int j)
		{
			if (WorldGen.destroyObject)
			{
				return;
			}
			bool flag = false;
			int num = 0;
			int num2 = j;
			for (num += Main.tile[i, j].TileFrameX / 18; num > 1; num -= 2)
			{
			}
			num *= -1;
			num += i;
			int num3 = Main.tile[i, j].TileFrameY / 18;
			int num4 = 0;
			while (num3 > 1)
			{
				num3 -= 2;
				num4++;
			}
			num2 -= num3;
			for (int k = num; k < num + 2; k++)
			{
				for (int l = num2; l < num2 + 2; l++)
				{
					int num5;
					for (num5 = Main.tile[k, l].TileFrameX / 18; num5 > 1; num5 -= 2)
					{
					}
					if (!Main.tile[k, l].HasTile || num5 != k - num || Main.tile[k, l].TileFrameY != (l - num2) * 18 + num4 * 36)
					{
						flag = true;
					}
				}
				if (!WorldGen.SolidTile2(k, num2 + 2))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			WorldGen.destroyObject = true;
			SoundEngine.PlaySound(SoundID.Shatter, new Vector2(i * 16, j * 16));
			for (int m = num; m < num + 2; m++)
			{
				for (int n = num2; n < num2 + 2; n++)
				{
					if (Main.tile[m, n].HasTile)
					{
						WorldGen.KillTile(m, n);
					}
				}
			}
			Gore.NewGore(new EntitySource_Misc(""), new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), ModContent.GoreType<DepthsPotGore1>());
			Gore.NewGore(new EntitySource_Misc(""), new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), ModContent.GoreType<DepthsPotGore2>());
			Gore.NewGore(new EntitySource_Misc(""), new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), ModContent.GoreType<DepthsPotGore3>());
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				SpawnThingsFromPot(i, j, num, num2);
			}
			WorldGen.destroyObject = false;
		}

		private void SpawnThingsFromPot(int i, int j, int x2, int y2) //Style 14, 15 and 16
		{
			bool flag = (double)j < Main.rockLayer;
			bool flag2 = j < Main.UnderworldLayer;
			if (Main.remixWorld)
			{
				flag = (double)j > Main.rockLayer && j < Main.UnderworldLayer;
				flag2 = (double)j > Main.worldSurface && (double)j < Main.rockLayer;
			}
			float num = 2.1f;
			num = (num * 2f + 1f) / 3f;
			int range = (int)(500f / ((num + 1f) / 2f));
			if (WorldGen.gen)
			{
				return;
			}
			if (Player.GetClosestRollLuck(i, j, range) == 0f)
			{
				if (Main.netMode != 1)
				{
					Projectile.NewProjectile(new EntitySource_Misc(""), i * 16 + 16, j * 16 + 16, 0f, -12f, ProjectileID.CoinPortal, 0, 0f, Main.myPlayer); //Coin Portal
				}
				return;
			}
			if (Main.getGoodWorld && WorldGen.genRand.Next(6) == 0)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), i * 16 + 16, j * 16 + 8, (float)Main.rand.Next(-100, 101) * 0.002f, 0f, ProjectileID.Bomb, 0, 0f, Main.myPlayer, 16f, 16f); //Bomb
				return;
			}
			if (Main.remixWorld && Main.netMode != 1 && WorldGen.genRand.Next(5) == 0)
			{
				Player player = Main.player[Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)];
				if (Main.rand.Next(2) == 0)
				{
					Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.FallenStar); //Fallen star
				}
				else if (player.ZoneJungle)
				{
					int num12 = -1;
					num12 = NPC.NewNPC(new EntitySource_Misc(""), x2 * 16 + 16, y2 * 16 + 32, NPCID.JungleSlime); //Jungle Slime
					if (num12 > -1)
					{
						Main.npc[num12].ai[1] = 75f;
						Main.npc[num12].netUpdate = true;
					}
				}
				else if ((double)j > Main.rockLayer && j < Main.maxTilesY - 350)
				{
					int num13 = -1;
					num13 = ((Main.rand.Next(9) == 0) ? NPC.NewNPC(new EntitySource_Misc(""), x2 * 16 + 16, y2 * 16 + 32, NPCID.PurpleSlime) : ((Main.rand.Next(7) == 0) ? NPC.NewNPC(new EntitySource_Misc(""), x2 * 16 + 16, y2 * 16 + 32, NPCID.RedSlime) : ((Main.rand.Next(6) == 0) ? NPC.NewNPC(new EntitySource_Misc(""), x2 * 16 + 16, y2 * 16 + 32, NPCID.YellowSlime) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(new EntitySource_Misc(""), x2 * 16 + 16, y2 * 16 + 32, NPCID.BlueSlime) : NPC.NewNPC(new EntitySource_Misc(""), x2 * 16 + 16, y2 * 16 + 32, NPCID.GreenSlime)))));
					if (num13 > -1) //slimes
					{
						Main.npc[num13].ai[1] = 75f;
						Main.npc[num13].netUpdate = true;
					}
				}
				else if ((double)j > Main.worldSurface && (double)j <= Main.rockLayer)
				{
					int num14 = -1;
					num14 = NPC.NewNPC(new EntitySource_Misc(""), x2 * 16 + 16, y2 * 16 + 32, NPCID.BlackSlime); //Black Slime
					if (num14 > -1)
					{
						Main.npc[num14].ai[1] = 75f;
						Main.npc[num14].netUpdate = true;
					}
				}
				else
				{
					Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.FallenStar); //Fallen Star Again
				}
				return;
			}
			if (Main.remixWorld && (double)i > (double)Main.maxTilesX * 0.37 && (double)i < (double)Main.maxTilesX * 0.63 && j > Main.maxTilesY - 220)
			{
				int stack = Main.rand.Next(20, 41);
				Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Rope, stack); //Rope
				return;
			}
			if (WorldGen.genRand.Next(45) == 0 || (Main.rand.Next(45) == 0 && Main.expertMode))
			{
				if ((double)j < Main.worldSurface)
				{
					int num16 = WorldGen.genRand.Next(10);
					if (num16 == 0)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.IronskinPotion);
					}
					if (num16 == 1)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ShinePotion);
					}
					if (num16 == 2)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.NightOwlPotion);
					}
					if (num16 == 3)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SwiftnessPotion);
					}
					if (num16 == 4)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.MiningPotion);
					}
					if (num16 == 5)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.CalmingPotion);
					}
					if (num16 == 6)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.BuilderPotion);
					}
					if (num16 >= 7)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.RecallPotion, WorldGen.genRand.Next(1, 3));
					}
				}
				else if (flag)
				{
					int num17 = WorldGen.genRand.Next(11);
					if (num17 == 0)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.RegenerationPotion);
					}
					if (num17 == 1)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ShinePotion);
					}
					if (num17 == 2)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.NightOwlPotion);
					}
					if (num17 == 3)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SwiftnessPotion);
					}
					if (num17 == 4)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ArcheryPotion);
					}
					if (num17 == 5)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.GillsPotion);
					}
					if (num17 == 6)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.TrapsightPotion);
					}
					if (num17 == 7)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.MiningPotion);
					}
					if (num17 == 8)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.TrapsightPotion);
					}
					if (num17 >= 7)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.RecallPotion, WorldGen.genRand.Next(1, 3));
					}
				}
				else if (flag2)
				{
					int num18 = WorldGen.genRand.Next(15);
					if (num18 == 0)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SpelunkerPotion);
					}
					if (num18 == 1)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.FeatherfallPotion);
					}
					if (num18 == 2)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.NightOwlPotion);
					}
					if (num18 == 3)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.WaterWalkingPotion);
					}
					if (num18 == 4)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ArcheryPotion);
					}
					if (num18 == 5)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.GravitationPotion);
					}
					if (num18 == 6)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ThornsPotion);
					}
					if (num18 == 7)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.WaterWalkingPotion); //Relogic what is this doing here twice???
					}
					if (num18 == 8)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.InvisibilityPotion);
					}
					if (num18 == 9)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HunterPotion);
					}
					if (num18 == 10)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.MiningPotion);
					}
					if (num18 == 11)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HeartreachPotion);
					}
					if (num18 == 12)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.FlipperPotion);
					}
					if (num18 == 13)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.TrapsightPotion);
					}
					if (num18 >= 7)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.RecallPotion, WorldGen.genRand.Next(1, 3));
					}
				}
				else
				{
					int num19 = WorldGen.genRand.Next(14);
					if (num19 == 0)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SpelunkerPotion);
					}
					if (num19 == 1)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.FeatherfallPotion);
					}
					if (num19 == 2)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ManaRegenerationPotion);
					}
					if (num19 == 3)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<CrystalSkinPotion>());
					}
					if (num19 == 4)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.MagicPowerPotion);
					}
					if (num19 == 5)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.InvisibilityPotion);
					}
					if (num19 == 6)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HunterPotion);
					}
					if (num19 == 7)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.GravitationPotion);
					}
					if (num19 == 8)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ThornsPotion);
					}
					if (num19 == 9)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.WaterWalkingPotion);
					}
					if (num19 == 10)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<CrystalSkinPotion>()); //Again relogic????
					}
					if (num19 == 11)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.BattlePotion);
					}
					if (num19 == 12)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HeartreachPotion);
					}
					if (num19 == 13)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.TitanPotion);
					}
					if (WorldGen.genRand.Next(5) == 0)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.PotionOfReturn);
					}
				}
				return;
			}
			if (Main.netMode == 2 && Main.rand.Next(30) == 0)
			{
				Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.WormholePotion);
				return;
			}
			int num15 = Main.rand.Next(7);
			if (Main.expertMode)
			{
				num15--;
			}
			Player player2 = Main.player[Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)];
			int num2 = 0;
			int num3 = 20;
			for (int k = 0; k < 50; k++)
			{
				Item item = player2.inventory[k];
				if (!item.IsAir && item.createTile == 4)
				{
					num2 += item.stack;
					if (num2 >= num3)
					{
						break;
					}
				}
			}
			bool flag4 = num2 < num3;
			if (num15 == 0 && player2.statLife < player2.statLifeMax2)
			{
				Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart);
				if (Main.rand.Next(2) == 0)
				{
					Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart);
				}
				if (Main.expertMode)
				{
					if (Main.rand.Next(2) == 0)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart);
					}
					if (Main.rand.Next(2) == 0)
					{
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart);
					}
				}
				return;
			}
			if (num15 == 1 || (num15 == 0 && flag4))
			{
				int num4 = Main.rand.Next(2, 7);
				if (Main.expertMode)
				{
					num4 += Main.rand.Next(1, 7);
				}
				int type = ItemID.Torch;
				int type2 = ItemID.Glowstick;
				if (player2.ZoneHallow)
				{
					num4 += Main.rand.Next(2, 7);
					type = ItemID.HallowedTorch;
				}
				else if (player2.ZoneGlowshroom)
				{
					num4 += Main.rand.Next(2, 7);
					type = ItemID.MushroomTorch;
				}
				if (Main.tile[i, j].LiquidAmount > 0)
				{
					Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type2, num4);
				}
				else
				{
					Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type, num4);
				}
				return;
			}
			switch (num15)
			{
				case 2: //Ammos
					{
						int stack2 = Main.rand.Next(10, 21);
						int type4 = 40;
						if (flag && WorldGen.genRand.Next(2) == 0)
						{
							type4 = ((!Main.hardMode) ? ItemID.Shuriken : ItemID.Grenade);
						}
						if (j > Main.UnderworldLayer)
						{
							type4 = ModContent.ItemType<DiamondArrow>();
						}
						else if (Main.hardMode)
						{
							type4 = ((Main.rand.Next(2) != 0) ? ItemID.UnholyArrow : ((WorldGen.SavedOreTiers.Silver != TileID.Tungsten) ? ItemID.SilverBullet : ItemID.TungstenBullet));
						}
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type4, stack2);
						return;
					}
				case 3:
					{
						int type5 = 28;
						if (j > Main.UnderworldLayer || Main.hardMode)
						{
							type5 = ItemID.HealingPotion;
						}
						int num6 = 1;
						if (Main.expertMode && Main.rand.Next(3) != 0)
						{
							num6++;
						}
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type5, num6);
						return;
					}
				case 4:
					if (flag2)
					{
						int type3 = ItemID.Bomb;
						int num5 = Main.rand.Next(4) + 1;
						if (Main.expertMode)
						{
							num5 += Main.rand.Next(4);
						}
						Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type3, num5);
						return;
					}
					break;
			}
			if ((num15 == 4 || num15 == 5) && j < Main.UnderworldLayer && !Main.hardMode)
			{
				int stack3 = Main.rand.Next(20, 41);
				Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Rope, stack3);
				return;
			}
			float num7 = 200 + WorldGen.genRand.Next(-100, 101);
			if ((double)j < Main.worldSurface)
			{
				num7 *= 0.5f;
			}
			else if (flag)
			{
				num7 *= 0.75f;
			}
			else if (j > Main.maxTilesY - 250)
			{
				num7 *= 1.25f;
			}
			num7 *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
			if (Main.rand.Next(4) == 0)
			{
				num7 *= 1f + (float)Main.rand.Next(5, 11) * 0.01f;
			}
			if (Main.rand.Next(8) == 0)
			{
				num7 *= 1f + (float)Main.rand.Next(10, 21) * 0.01f;
			}
			if (Main.rand.Next(12) == 0)
			{
				num7 *= 1f + (float)Main.rand.Next(20, 41) * 0.01f;
			}
			if (Main.rand.Next(16) == 0)
			{
				num7 *= 1f + (float)Main.rand.Next(40, 81) * 0.01f;
			}
			if (Main.rand.Next(20) == 0)
			{
				num7 *= 1f + (float)Main.rand.Next(50, 101) * 0.01f;
			}
			if (Main.expertMode)
			{
				num7 *= 2.5f;
			}
			if (Main.expertMode && Main.rand.Next(2) == 0)
			{
				num7 *= 1.25f;
			}
			if (Main.expertMode && Main.rand.Next(3) == 0)
			{
				num7 *= 1.5f;
			}
			if (Main.expertMode && Main.rand.Next(4) == 0)
			{
				num7 *= 1.75f;
			}
			num7 *= num;
			if (NPC.downedBoss1)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedBoss2)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedBoss3)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedMechBoss1)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedMechBoss2)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedMechBoss3)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedPlantBoss)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedQueenBee)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedGolemBoss)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedPirates)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedGoblins)
			{
				num7 *= 1.1f;
			}
			if (NPC.downedFrost)
			{
				num7 *= 1.1f;
			}
			while ((int)num7 > 0)
			{
				if (num7 > 1000000f)
				{
					int num8 = (int)(num7 / 1000000f);
					if (num8 > 50 && Main.rand.Next(2) == 0)
					{
						num8 /= Main.rand.Next(3) + 1;
					}
					if (Main.rand.Next(2) == 0)
					{
						num8 /= Main.rand.Next(3) + 1;
					}
					num7 -= (float)(1000000 * num8);
					Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.PlatinumCoin, num8);
					continue;
				}
				if (num7 > 10000f)
				{
					int num9 = (int)(num7 / 10000f);
					if (num9 > 50 && Main.rand.Next(2) == 0)
					{
						num9 /= Main.rand.Next(3) + 1;
					}
					if (Main.rand.Next(2) == 0)
					{
						num9 /= Main.rand.Next(3) + 1;
					}
					num7 -= (float)(10000 * num9);
					Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.GoldCoin, num9);
					continue;
				}
				if (num7 > 100f)
				{
					int num10 = (int)(num7 / 100f);
					if (num10 > 50 && Main.rand.Next(2) == 0)
					{
						num10 /= Main.rand.Next(3) + 1;
					}
					if (Main.rand.Next(2) == 0)
					{
						num10 /= Main.rand.Next(3) + 1;
					}
					num7 -= (float)(100 * num10);
					Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SilverCoin, num10);
					continue;
				}
				int num11 = (int)num7;
				if (num11 > 50 && Main.rand.Next(2) == 0)
				{
					num11 /= Main.rand.Next(3) + 1;
				}
				if (Main.rand.Next(2) == 0)
				{
					num11 /= Main.rand.Next(4) + 1;
				}
				if (num11 < 1)
				{
					num11 = 1;
				}
				num7 -= (float)num11;
				Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.CopperCoin, num11);
			}
		}
	}
}