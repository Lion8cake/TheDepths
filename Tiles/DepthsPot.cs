using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Items;
using TheDepths.Items.Weapons;
using Terraria.DataStructures;
using TheDepths.Dusts;

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
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Pot");
			AddMapEntry(new Color(226, 227, 231), name);
			DustType = ModContent.DustType<QuartzDust>();
			HitSound = SoundID.Shatter;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			_ = j - Main.tile[i, j].TileFrameY / 18;
			_ = i - Main.tile[i, j].TileFrameX / 18;
			int x = i - Main.tile[i, j].TileFrameX / 18 % 2;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 2;
			SoundEngine.PlaySound(SoundID.Shatter, new Vector2(i * 16, j * 16));
			Gore.NewGore(new EntitySource_Misc(""), new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), Mod.Find<ModGore>("DepthsPotGore1").Type);
			Gore.NewGore(new EntitySource_Misc(""), new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), Mod.Find<ModGore>("DepthsPotGore2").Type);
			Gore.NewGore(new EntitySource_Misc(""), new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), Mod.Find<ModGore>("DepthsPotGore3").Type);
			if (!WorldGen.gen && Main.netMode != 1)
			{
				if (Main.rand.NextBool(190))
				{
					Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(x, y).ToWorldCoordinates() + new Vector2(8, 8 - 64), Vector2.Zero, ProjectileID.CoinPortal, 0, 0f, Main.myPlayer); //Coin Portal
				}
				else if (Main.getGoodWorld && Main.rand.NextBool(4))
				{
					Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(x, y).ToWorldCoordinates() + new Vector2(8, 8), Vector2.Zero, ProjectileID.Bomb, 0, 0f, Main.myPlayer); //FTW Bomb spawning
				}
				else if (Main.rand.NextBool(25) || Main.expertMode && Main.rand.NextBool(12))
				{
					int num = Main.rand.Next(13);
					if (num == 0)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SpelunkerPotion); //Splunker Potion
					}
					if (num == 1)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.FeatherfallPotion); //Feather Fall Potion
					}
					if (num == 2)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ManaRegenerationPotion); //Mana Regen Potion
					}
					if (num == 3)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<CrystalSkinPotion>()); //Crystal Skin Potion
					}
					if (num == 4)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.MagicPowerPotion); //Mana Power Potion
					}
					if (num == 5)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.InvisibilityPotion); //Invis Potion
					}
					if (num == 6)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HunterPotion); //Hunter Potion
					}
					if (num == 7)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.GravitationPotion); //Gravity Potion
					}
					if (num == 8)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ThornsPotion); //Thorns Potion
					}
					if (num == 9)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.WaterWalkingPotion); //Water Walking Potion
					}
					if (num == 10)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.BattlePotion); //Battle Potion
					}
					if (num == 11)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HeartreachPotion); //Heart REACH Potion
					}
					if (num == 12)
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.TitanPotion); //Titan Potion
					}
					else if (Main.rand.NextBool(5))
					{
						Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.PotionOfReturn); //Return Potion
					}
				}
				else if (Main.netMode == NetmodeID.MultiplayerClient && Main.rand.NextBool(30))
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.WormholePotion); //Wormhole Potion
				}
				else if (Main.rand.NextBool(6))
				{
					int S = Main.rand.Next(5);
					if (S == 0)
					{
						if (Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statLife < Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statLifeMax2 && !Main.expertMode)
						{
							Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart, Main.rand.Next(1, 2)); //Heart
						}
						else if (Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statLife < Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statLifeMax2 && Main.expertMode)
						{
							Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart, Main.rand.Next(1, 4)); // Heart Expert Amount
						}
						else if (Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statLife > Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statLifeMax2)
						{
							S = 1; //Arrows
						}
						else
						{
							Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.CopperCoin, Main.rand.Next(0, 100)); //Coins
							Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SilverCoin, Main.rand.Next(3, 6));
						}
					}
					else if (S == 1)
					{
						if (Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statLife <= int.MaxValue && !Main.expertMode)
						{
							if (Main.LocalPlayer.ZoneCorrupt && Main.tile[i, j].LiquidAmount < 0)
							{
								Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.CorruptTorch, Main.rand.Next(4, 12)); //Corrupt Torches
							}
							else if (Main.LocalPlayer.ZoneCrimson && Main.tile[i, j].LiquidAmount < 0)
							{
								Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.CrimsonTorch, Main.rand.Next(4, 12)); //Crimson Torches
							}
							else if (Main.LocalPlayer.ZoneHallow && Main.tile[i, j].LiquidAmount < 0)
							{
								Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HallowedTorch, Main.rand.Next(4, 12)); //Hallow Torches
							}
							else if (Main.tile[i, j].LiquidAmount > 0 && !Main.LocalPlayer.ZoneSnow && !Main.LocalPlayer.ZoneRockLayerHeight)
							{
								Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Glowstick, Main.rand.Next(2, 5)); //Glowstick
							}
							else if (Main.tile[i, j].LiquidAmount > 0 && Main.LocalPlayer.ZoneSnow && Main.LocalPlayer.ZoneRockLayerHeight)
							{
								Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.StickyGlowstick, Main.rand.Next(2, 5)); //Sticky Glowstick
							}
							else
							{
								Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Torch, Main.rand.Next(2, 5)); //Torches
							}
						}
						else if (Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statLife <= int.MaxValue && Main.expertMode)
						{
							if (Main.LocalPlayer.ZoneCorrupt)
							{
								Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.CorruptTorch, Main.rand.Next(5, 18)); //Corrupt Torches Expert Amount
							}
							else if (Main.LocalPlayer.ZoneCrimson)
							{
								Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.CrimsonTorch, Main.rand.Next(5, 18)); //Crimson Torches Expert Amount
							}
							else if (Main.LocalPlayer.ZoneHallow)
							{
								Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HallowedTorch, Main.rand.Next(5, 18)); //Hallowed Torches Expert Amount
							}
							else if (Main.tile[i, j].LiquidAmount > 0 && Main.LocalPlayer.ZoneSnow && Main.LocalPlayer.ZoneRockLayerHeight)
							{
								Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.StickyGlowstick, Main.rand.Next(3, 11)); //Sticky Glowstick Expert Amount
							}
							else if (Main.tile[i, j].LiquidAmount > 0)
							{
								Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Glowstick, Main.rand.Next(3, 11)); //Glowstick Expert Amount
							}
							else
							{
								Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Torch, Main.rand.Next(3, 11)); //Torches Expert amount
							}
						}
					}
					else if (S == 2)
					{
						if (!Main.hardMode)
						{
							Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<DiamondArrow>(), Main.rand.Next(10, 20)); //Diamond Arrows
						}
						else if (Main.rand.NextBool(2))
						{
							Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.UnholyArrow, Main.rand.Next(10, 20)); //Unholy Arrows
						}
						else if (WorldGen.silver == ItemID.SilverOre)
						{
							Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SilverBullet, Main.rand.Next(10, 20)); //Silver bullet
						}
						else
						{
							Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.TungstenBullet, Main.rand.Next(10, 20)); //Tungsten Bullet
						}
					}
					else if (S == 3)
					{
						if (Main.expertMode)
						{
							Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HealingPotion, Main.rand.Next(1, 3)); //Healing Potion Expert Amount
						}
						else
						{
							Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HealingPotion); //Healing Potion 
						}
					}
					else if (S == 4)
					{
						if (Main.expertMode)
						{
							Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Bomb, Main.rand.Next(1, 7)); //Bomb Expert amount
						}
						else
						{
							Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Bomb, Main.rand.Next(1, 4)); //Bomb
						}
					}
				}
				else
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.CopperCoin, Main.rand.Next(0, 100)); //Coins
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SilverCoin, Main.rand.Next(3, 6));
				}
			}
		}
	}
}