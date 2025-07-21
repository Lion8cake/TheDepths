using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModLiquidLib;
using ModLiquidLib.Utils;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Generation;
using Terraria.GameContent.Liquid;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheDepths.Biomes;
using TheDepths.Buffs;
using TheDepths.Dusts;
using TheDepths.Gores;
using TheDepths.Items;
using TheDepths.Items.Weapons;
using TheDepths.Liquids;
using TheDepths.NPCs.Chasme;
using TheDepths.Projectiles;
using TheDepths.Tiles;
using TheDepths.Tiles.Trees;
using static Terraria.ModLoader.ExtraJump;
using static Terraria.ModLoader.ModContent;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace TheDepths
{
    public class TheDepthsPlayer : ModPlayer
    {
        public int MercuryTimer;
        public BitsByte largeGems;
        public BitsByte ownedLargeGems;
        public BitsByte hasLargeGems;
        public bool merPoison;
        public bool slowWater;
        public bool merBoiling;
        public bool merImbue;
        public bool aStone;
        public bool lodeStone;
        public bool stoneRose;
        public bool HasAquaQuiver;
        /// <summary>
		///   Used for First Tier Updrages to the Amalgam Amulet, This contains all items such as Crying Skull, Silver Charm, Azurite Rose and Alagam Amulet
		/// </summary>
        public bool aAmulet;
        /// <summary>
		///   Used For Second Tier Upgrades to the amalgam Amulet, This contains all items such as the Silver Slippers and Terraspark Boots
		/// </summary>
        public bool aAmulet2;
        /// <summary>
		///   Used for third party items not related to the amalgam amulet but still use its effects. Only used by the Geomancer's mining Cart
		/// </summary>
        public bool aAmulet3;
        public bool sEmbers;
        public bool nFlare;
        public bool pShield;
        private int pShieldReduction = 1;
        private int pShieldTimer = 0;
        public bool Gslam;
        public bool GslamVanity;
        public bool quicksilverSurfboard;
        public int tremblingDepthsScreenshakeTimer;
        public int QuicksilverTimer;
        public int AmuletTimer;
        public bool cSkin;
        public bool AmuletTimerCap;
        public bool quicksilverWet;
        public int EmberTimer;
        public bool NightwoodBuff;
        public bool GSlamkeybindPressed = false;

        public int cShadowFlame;

        public bool isSlamming;
        public bool geodeCrystal;
        public bool livingShadow;
        public bool miniChasme;
        public bool ShadePet;
        public bool shadowCat;
        public bool FogMonolith;

        public bool engageChasme;

        public override void ResetEffects()
        {
            largeGems = 0;
            merPoison = false;
            slowWater = false;
            merBoiling = false;
            merImbue = false;
            aStone = false;
            lodeStone = false;
            stoneRose = false;
            HasAquaQuiver = false;
            aAmulet = false;
            aAmulet2 = false;
            aAmulet3 = false;
            sEmbers = false;
            nFlare = false;
            quicksilverSurfboard = false;
            quicksilverWet = false;
            cSkin = false;
            NightwoodBuff = false;
            pShield = false;
            Gslam = false;

            if (isSlamming)
            {
                Player.maxFallSpeed = 20f;
            }
			geodeCrystal = false;
            livingShadow = false;
            miniChasme = false;
            ShadePet = false;
            shadowCat = false;
            if (tremblingDepthsScreenshakeTimer < 0)
            {
				tremblingDepthsScreenshakeTimer = 0;
			}
            else
            {
                tremblingDepthsScreenshakeTimer--;
            }
			FogMonolith = false;
        }

        public override void ModifyScreenPosition()
        {
            if (tremblingDepthsScreenshakeTimer > 0)
            {
                Main.screenPosition += Main.rand.NextVector2Circular(20, 20);
            }
        }

		public override void Unload()
		{
            if (Main.netMode != NetmodeID.Server)
            {
                int[] bgnumOriginal = new int[30] { 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 150, 151, 152, 157, 158, 159, 185, 186, 187 };
                foreach (int i in bgnumOriginal)
                {
                    TextureAssets.Background[i] = Main.Assets.Request<Texture2D>("Images/Background_" + i);
                }

                TextureAssets.Item[3729] = Main.Assets.Request<Texture2D>("Images/Item_3729");
                TextureAssets.Tile[423] = Main.Assets.Request<Texture2D>("Images/Tiles_423");
			}
        }

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            Player player = Player;

            if (merPoison)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f && !player.dead)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f, 2f), Player.width + 4, Player.height + 4, ModContent.DustType<MercuryFire>(), Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default(Color), 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    drawInfo.DustCache.Add(dust);
                }
            }
            if (slowWater)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f && !player.dead)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f, 2f), Player.width + 4, Player.height + 4, ModContent.DustType<SlowingWaterFire>(), Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default(Color), 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    drawInfo.DustCache.Add(dust);
                }
            }
            if (merBoiling)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f && !player.dead)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f, 2f), Player.width + 4, Player.height + 4, ModContent.DustType<MercuryFire>(), Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default(Color), 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    drawInfo.DustCache.Add(dust);
                }
            }
        }

        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (merImbue && Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<MercuryFire>(), Player.velocity.X * 0.2f + (float)Player.direction * 3f, Player.velocity.Y * 0.2f, 100, default(Color), 0.75f);
            }
            if (aStone && Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<SlowingWaterFire>(), Player.velocity.X * 0.2f + (float)Player.direction * 3f, Player.velocity.Y * 0.2f, 100, default(Color), 0.75f);
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (item.CountsAsClass(DamageClass.Melee) || item.CountsAsClass<SummonMeleeSpeedDamageClass>())
            {
                if (merImbue)
                {
                    target.AddBuff(ModContent.BuffType<MercuryBoiling>(), 360 * Main.rand.Next(1, 1));
                }
                if (aStone)
                {
                    target.AddBuff(ModContent.BuffType<FreezingWater>(), 360 * Main.rand.Next(1, 1));
                }
            }
        }

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
            if (proj.CountsAsClass(DamageClass.Melee) || proj.CountsAsClass<SummonMeleeSpeedDamageClass>())
            {
                if (merImbue)
                {
                    target.AddBuff(ModContent.BuffType<MercuryBoiling>(), 360 * Main.rand.Next(1, 1));
                }
                if (aStone)
                {
                    target.AddBuff(ModContent.BuffType<FreezingWater>(), 360 * Main.rand.Next(1, 1));
                }
            }
        }

		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
		{
            if (pShield)
            {
                float percentage = 0.0f;
                pShieldTimer = 60 * 3;
                if (pShieldReduction < 6)
                    pShieldReduction++;
                switch (pShieldReduction)
                {
                    case 1:
                        percentage = 0.5f;
                        break;
                    case 2:
                        percentage = 0.45f;
                        break;
                    case 3:
                        percentage = 0.35f;
                        break;
                    case 4:
                        percentage = 0.20f;
                        break;
					case 5:
						percentage = 0.1f;
						break;
					case 6:
						percentage = 0.0f;
						break;
				}
				modifiers.FinalDamage *= 1f - percentage;
				if (!TheDepthsIDs.Sets.UnreflectiveProjectiles[proj.type] && Main.rand.NextBool(4))
			    {
                    proj.hostile = false;
                    proj.friendly = true;
                    proj.velocity = -proj.oldVelocity;
                    proj.owner = Player.whoAmI;
					SoundEngine.PlaySound(in SoundID.Item150, Player.position);
					ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
					{
						PositionInWorld = proj.Center,
						MovementVector = Vector2.Zero
					}, Player.whoAmI);
				}
            }
		}

		public override void GetDyeTraderReward(List<int> rewardPool)
        {
            rewardPool.Add(ModContent.ItemType<Items.LivingFogDye>());
        }

        public override void CatchFish(FishingAttempt fisher, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            Player player = Player;
			if (Main.tile[fisher.X, fisher.Y].LiquidType == ModLiquidLib.ModLiquidLib.LiquidType<Quicksilver>())
			{
				if (fisher.CanFishInLava)
				{
					if (fisher.crate && Main.rand.Next(6) == 0)
					{
						itemDrop = (Main.hardMode ? ItemType<Items.Placeable.ArqueriteCrate>() : ItemType<Items.Placeable.QuartzCrate>());
					}
					else if (fisher.legendary && Main.hardMode && Main.rand.Next(3) == 0)
					{
						itemDrop = Main.rand.NextFromList(new int[4] { ItemType<ShalestoneConch>(), ItemType<BottomlessQuicksilverBucket>(), ItemType<QuicksilverAbsorbantSponge>(), ItemType<Items.Weapons.Steelocanth>() });
					}
					else if (fisher.legendary && !Main.hardMode && Main.rand.Next(3) == 0)
					{
						itemDrop = Main.rand.NextFromList(new int[3] { ItemType<ShalestoneConch>(), ItemType<BottomlessQuicksilverBucket>(), ItemType<QuicksilverAbsorbantSponge>() });
					}
					else if (fisher.veryrare)
					{
						itemDrop = ItemType<ShadowFightingFish>();
					}
					else if (fisher.rare)
					{
						itemDrop = ItemType<QuartzFeeder>();
					}
                    else
                    {
						itemDrop = 0;
                    }
                    if (ModSupport.DepthsModCalling.Achievements != null)
                    {
                        ModSupport.DepthsModCalling.Achievements.Call("Event", "FishingInQuicksilver");
                    }
				}
			}
			
            if (fisher.questFish == ModContent.ItemType<Chasmefish>())
            {
                if (Player.ZoneRockLayerHeight && Worldgen.TheDepthsWorldGen.InDepths(player) && fisher.uncommon || Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(player) && fisher.uncommon)
                {
                    itemDrop = ModContent.ItemType<Chasmefish>();
                    return;
                }
            }
            if (fisher.questFish == ModContent.ItemType<Relicarp>())
            {
                if (Player.ZoneRockLayerHeight && Worldgen.TheDepthsWorldGen.InDepths(player) && fisher.uncommon || Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(player) && fisher   .uncommon)
                {
                    itemDrop = ModContent.ItemType<Relicarp>();
                    return;
                }
            }
            if (fisher.questFish == ModContent.ItemType<GlimmerDepthFish>())
            {
                if (Player.ZoneRockLayerHeight && Worldgen.TheDepthsWorldGen.InDepths(player) && fisher.uncommon || Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(player) && fisher.uncommon)
                {
                    itemDrop = ModContent.ItemType<GlimmerDepthFish>();
                    return;
                }
            }
        }

		public override void UpdateDyes()
		{
            for (int i = 0; i < 20; i++)
            {
                if (Player.IsItemSlotUnlockedAndUsable(i))
                {
                    int num = i % 10;
                    UpdateItemDye(i < 10, Player.hideVisibleAccessory[num], Player.armor[i], Player.dye[num]);
                }
            }
        }

        internal void UpdateItemDye(bool isNotInVanitySlot, bool isSetToHidden, Item armorItem, Item dyeItem)
        {
            if (!armorItem.IsAir || !isSetToHidden)
            {
                if (armorItem.type == ModContent.ItemType<Items.Accessories.ShadowflameEmberedTreads>() || armorItem.type == ModContent.ItemType<Items.Accessories.NightmareFlareTreads>())
                {
                    cShadowFlame = dyeItem.dye;
                }
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (TheDepths.GroundSlamKeybind.JustPressed)
            {
                GSlamkeybindPressed = true;
            }
        }

        public override void PostUpdate()
        {
            Player player = Player;

            ChasmesCurse();

            bool SlammingInteruptions = (player.velocity.Y == 0 || player.position == player.oldPosition);
			if (Gslam || GslamVanity)
            {
                if (player.afkCounter > 60 * 10 && player.ownedProjectileCounts[ModContent.ProjectileType<ShalestoneHand>()] == 0 && Main.rand.NextBool(1200))
                {
                    Projectile.NewProjectile(new EntitySource_Misc(""), player.position, Vector2.Zero, ModContent.ProjectileType<ShalestoneHand>(), 0, 0, player.whoAmI, 1f);
                }
            }
            if (Gslam)
            {
                if ((GSlamkeybindPressed) && !SlammingInteruptions)
                {
                    Player.canRocket = false;
                    isSlamming = true;
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<ShalestoneHand>()] == 0)
                    {
                        Projectile.NewProjectile(new EntitySource_Misc(""), player.position, Vector2.Zero, ModContent.ProjectileType<ShalestoneHand>(), 0, 0, player.whoAmI);
                    }
                }
            }
            if (GSlamkeybindPressed)
            {
				GSlamkeybindPressed = false;
			}
            if (SlammingInteruptions)
            {
                if (isSlamming && player.velocity.Y == 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, player.position);
                    Projectile.NewProjectile(new EntitySource_Misc(""), player.position, Vector2.Zero, ModContent.ProjectileType<ShalestoneSlam>(), 0, 0);
                }
                isSlamming = false;
            }
            if (isSlamming)
            {
                player.velocity.X = 0;
                player.velocity.Y = 20;
            }

			if (pShieldTimer <= 0)
			{
                if (pShieldReduction > 1)
                {
                    pShieldTimer = 60 * 2;
                    pShieldReduction--;
                }
                else
                {
					pShieldTimer = 0;
				}
			}
			else
			{
				pShieldTimer--;
			}
            if (pShieldReduction > 6)
            {
                pShieldReduction = 6;
            }
			if (pShieldReduction <= 0)
			{
				pShieldReduction = 1;
			}

            if (sEmbers)
            {
                if ((Main.tile[(int)(Player.position.X / 16f), (int)(Player.position.Y / 16f) + 3].HasTile && Main.tileSolid[Main.tile[(int)(Player.position.X / 16f), (int)(Player.position.Y / 16f) + 3].TileType]) || (Main.tile[(int)(Player.position.X / 16f) + 1, (int)(Player.position.Y / 16f) + 3].HasTile && Main.tileSolid[Main.tile[(int)(Player.position.X / 16f) + 1, (int)(Player.position.Y / 16f) + 3].TileType] && Player.velocity.Y == 0f))
                {
                    if (Player.velocity.X > 0 || Player.velocity.X < 0)
                    {
                        EmberTimer++;
                        if (EmberTimer <= 1)
                        {
                            if (Main.rand.NextBool(3))
                            {
                                Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(Player.Center.X, Player.Center.Y + Player.height / 2 - 5), new Vector2(0), ModContent.ProjectileType<ShadowflameEmber1>(), 0, 0);
                            }
                            else if (Main.rand.NextBool(3))
                            {
                                Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(Player.Center.X, Player.Center.Y + Player.height / 2 - 5), new Vector2(0), ModContent.ProjectileType<ShadowflameEmber2>(), 0, 0);
                            }
                            else
                            {
                                Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(Player.Center.X, Player.Center.Y + Player.height / 2 - 5), new Vector2(0), ModContent.ProjectileType<ShadowflameEmber3>(), 0, 0);
                            }
                        }
                        if (EmberTimer >= 3)
                        {
                            EmberTimer = 0;
                        }
                    }
                    else
                    {
                        EmberTimer = 0;
                    }
                }
            }

            if (nFlare)
            {
                Player.sailDash = false;
                Player.coldDash = false;
                Player.desertDash = false;
                Player.fairyBoots = false;
                Player.hellfireTreads = false;
                Player.vanityRocketBoots = 3;
            }

            if (tremblingDepthsScreenshakeTimer > 0)
            {
                tremblingDepthsScreenshakeTimer--;
            }
            if (player.dead)
            {
                MercuryTimer = 0;
            }
            if (stoneRose)
            {
                if (QuicksilverTimer >= 60 * 4 && AmuletTimer == 0)
                {
                    if (NightwoodBuff == true)
                    {
                        Player.AddBuff(BuffType<MercuryBoiling>(), 60 * 3, false, false);
                    }
                    else
                    {
                        Player.AddBuff(BuffType<MercuryBoiling>(), 60 * 7, false, false);
                    }
                    QuicksilverTimer = 60 * 4;
                }
            }
            else
            {
                if (QuicksilverTimer >= 60 * 2 && AmuletTimer == 0)
                {
                    if (NightwoodBuff == true)
                    {
                        Player.AddBuff(BuffType<MercuryBoiling>(), 60 * 3, false, false);
                    }
                    else
                    {
                        Player.AddBuff(BuffType<MercuryBoiling>(), 60 * 7, false, false);
                    }
                    QuicksilverTimer = 60 * 2;
                }
            }
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player target = Main.player[i];
				if (target.active)
				{
                    if (target.team == Player.team && target.hostile && Player.hostile)
                    {
                        if (target.position.WithinRange(Player.position, 40) && Player.HasBuff(ModContent.BuffType<Buffs.MercuryContagion>()))
                        {
                            if (!target.HasBuff(ModContent.BuffType<Buffs.MercuryContagion>()) && target != Player)
                            {
                                target.AddBuff(ModContent.BuffType<Buffs.MercuryContagion>(), Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<Buffs.MercuryContagion>())]);
                            }
                        }
                    }
				}
			}
			int AmuletsActive = 0;
            if ((aAmulet && !aAmulet2 && !aAmulet3) || (!aAmulet && !aAmulet2 && aAmulet3) || (!aAmulet && aAmulet2 && !aAmulet3))
            {
                AmuletsActive = 1;
            }
            else if ((aAmulet && aAmulet2 && !aAmulet3) || (aAmulet && !aAmulet2 && aAmulet3) || (!aAmulet && aAmulet2 && aAmulet3))
			{
                AmuletsActive = 2;
			}
            else if ((aAmulet && aAmulet2 && aAmulet3))
            {
                AmuletsActive = 3;
            }
            if (AmuletTimer < 60 * 4 * AmuletsActive)
            {
                if (AmuletsActive > 0 && !quicksilverWet)
				{
                    AmuletTimer++;
                }
                AmuletTimerCap = false;
            }
            if (AmuletTimer <= 60 * 4 * AmuletsActive && (AmuletsActive > 0) && quicksilverWet && !cSkin)
            {
                AmuletTimer--;
            }
            if (AmuletTimer >= 60 * 4 * AmuletsActive)
            {
                AmuletTimer = 60 * 4 * AmuletsActive;
                AmuletTimerCap = true;
            }
            if (AmuletTimer <= 0 || (AmuletsActive <= 0) || Main.LocalPlayer.dead)
            {
                AmuletTimer = 0;
            }
            //Main.NewText(AmuletTimer);
            //Main.NewText(MercuryTimer); //For Debugging, posts number of ticks that have passed when the player is on Mercury
            //Main.NewText("Depths in on the left: " + Worldgen.TheDepthsWorldGen.IsPlayerInLeftDepths(player)); //Debugging for the drunkseed tag checker
			//Main.NewText("Depths in on the Right: " + Worldgen.TheDepthsWorldGen.IsPlayerInRightDepths(player));
			//Main.NewText("World Tag DrunkDepthsLeft: " + Worldgen.TheDepthsWorldGen.DrunkDepthsLeft);
			//Main.NewText("World Tag DrunkDepthsRight: " + Worldgen.TheDepthsWorldGen.DrunkDepthsRight);
			//Main.NewText("World Tag depthsorHell: " + Worldgen.TheDepthsWorldGen.depthsorHell);

			//Shalestone Conch and shellphone
			Item item = Player.inventory[Player.selectedItem];
            if (!Player.JustDroppedAnItem)
            {
                if ((item.type == ModContent.ItemType<ShalestoneConch>() || item.type == ModContent.ItemType<ShellPhoneDepths>()) && Player.itemAnimation > 0 && Worldgen.TheDepthsWorldGen.InDepths(player))
                {
                    Vector2 vector2 = Vector2.UnitY.RotatedBy((float)Player.itemAnimation * ((float)Math.PI * 2f) / 30f) * new Vector2(15f, 0f);
                    for (int num = 0; num < 2; num++)
                    {
                        if (Main.rand.NextBool(3))
                        {
                            Dust dust2 = Dust.NewDustPerfect(Player.Bottom + vector2, ModContent.DustType<QuicksilverTeleportFire>()); //Change the dust to be unique
                            dust2.velocity.Y *= 0f;
                            dust2.velocity.Y -= 4.5f;
                            dust2.velocity.X *= 1.5f;
                            dust2.scale = 0.8f;
                            dust2.alpha = 130;
                            dust2.noGravity = true;
                            dust2.fadeIn = 1.1f;
                        }
                    }
                    if (Player.ItemTimeIsZero)
                    {
                        Player.ApplyItemTime(item);
                    }
                    else if (Player.itemTime == item.useTime / 2)
                    {
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            ShalestoneConch(Player);
                        }
                        else if (Main.netMode == NetmodeID.MultiplayerClient && Player.whoAmI == Main.myPlayer)
                        {
                            NetMessage.SendData(MessageID.RequestTeleportationByServer, -1, -1, null, 2);
                        }
                    }
                }
                if (TheDepthsIDs.Sets.AxesAbleToBreakStone[item.type])
                {
                    Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
                    if (tile.TileType == ModContent.TileType<PetrifiedTree>() && ((Main.playerInventory && TheDepthsIDs.Sets.AxesAbleToBreakStone[player.inventory[58].type]) || !Main.playerInventory))
                    {
                        item.pick = item.axe * 5;
                    }
                    else
                    {
                        item.pick = 0;
                    }
                }
                for (int slot = 0; slot < Main.InventorySlotsTotal; slot++)
                {
                    if (TheDepthsIDs.Sets.AxesAbleToBreakStone[player.inventory[slot].type] && slot != Player.selectedItem)
                    {
                        player.inventory[slot].pick = 0;
                    }
                }
            } 

            if (player.InModBiome<DepthsBiome>())
			{
                if (ModSupport.DepthsModCalling.Achievements != null)
                {
                    ModSupport.DepthsModCalling.Achievements.Call("Event", "WalkedIntoTheDepths");
                }
            }
        }

        public static void ShalestoneConch(Player player)
        {
            bool canSpawn = false;
            int num = Main.maxTilesX / 2;
            int num2 = 100;
            int num3 = num2 / 2;
            int teleportStartY = Main.UnderworldLayer + 20;
            int teleportRangeY = 80;
            Player.RandomTeleportationAttemptSettings settings = new Player.RandomTeleportationAttemptSettings
            {
                mostlySolidFloor = true,
                avoidAnyLiquid = true,
                avoidLava = true,
                avoidHurtTiles = true,
                avoidWalls = true,
                attemptsBeforeGivingUp = 1000,
                maximumFallDistanceFromOrignalPoint = 30
            };
            Vector2 vector = player.CheckForGoodTeleportationSpot(ref canSpawn, num - num3, num2, teleportStartY, teleportRangeY, settings);
            if (!canSpawn)
            {
                vector = player.CheckForGoodTeleportationSpot(ref canSpawn, num - num2, num3, teleportStartY, teleportRangeY, settings);
            }
            if (!canSpawn)
            {
                vector = player.CheckForGoodTeleportationSpot(ref canSpawn, num + num3, num3, teleportStartY, teleportRangeY, settings);
            }
            if (canSpawn)
            {
                Vector2 newPos = vector;
                player.Teleport(newPos, 13);
                player.velocity = Vector2.Zero;
                if (Main.netMode == NetmodeID.Server)
                {
                    RemoteClient.CheckSection(player.whoAmI, player.position);
                    NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, newPos.X, newPos.Y, 7);
                }
            }
            else
            {
                Vector2 newPos2 = player.position;
                player.Teleport(newPos2, 13);
                player.velocity = Vector2.Zero;
                if (Main.netMode == NetmodeID.Server)
                {
                    RemoteClient.CheckSection(player.whoAmI, player.position);
                    NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, newPos2.X, newPos2.Y, 7, 1);
                }
            }
        }

		public void ChasmesCurse()
		{
            int chasmeNPCIndex = -1;
            for (int nPC = 0; nPC < Main.maxNPCs; nPC++)
            {
                if (Main.npc[nPC].type == ModContent.NPCType<ChasmeHeart>())
                {
					chasmeNPCIndex = nPC;
                    continue;
				}
            }
			if (chasmeNPCIndex < 0 || !Main.npc[chasmeNPCIndex].active)
			{
                engageChasme = false;
				return;
			}
            if (Player.ZoneUnderworldHeight)
            {
                engageChasme = true;
            }
			Vector2 center = Player.Center;
			float num3 = Main.npc[chasmeNPCIndex].position.X + (float)(Main.npc[chasmeNPCIndex].width / 2) - center.X;
			float num2 = Main.npc[chasmeNPCIndex].position.Y + (float)(Main.npc[chasmeNPCIndex].height / 2) - center.Y;
			if ((float)Math.Sqrt(num3 * num3 + num2 * num2) > 20000f && engageChasme)
			{
				Player.KillMe(PlayerDeathReason.ByOther(11), 1000.0, 0);
			}
		}

		public override void PostUpdateRunSpeeds()
        {
            if (nFlare)
            {
                float num = (Player.accRunSpeed + Player.maxRunSpeed) / 2f;
                if (Player.controlLeft && Player.velocity.X > 0f - Player.accRunSpeed && Player.dashDelay >= 0)
                {
                    if (Player.velocity.X < 0f - num && Player.velocity.Y == 0f && !Player.mount.Active)
                    {
                        NightmareFlareParticles();
                    }
                }
                else if (Player.controlRight && Player.velocity.X < Player.accRunSpeed && Player.dashDelay >= 0)
                {
                    if (Player.velocity.X > num && Player.velocity.Y == 0f && !Player.mount.Active)
                    {
                        NightmareFlareParticles();
                    }
                }
            }
        }

        public void NightmareFlareParticles()
        {
            int num = 0;
            if (Player.gravDir == -1f)
            {
                num -= Player.height;
            }
            if (Player.runSoundDelay == 0 && Player.velocity.Y == 0f)
            {
                SoundEngine.PlaySound(Player.hermesStepSound.Style, new Vector2((int)Player.position.X, (int)Player.position.Y));
                Player.runSoundDelay = Player.hermesStepSound.IntendedCooldown;
            }
            int num6 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num), Player.width + 8, 4, ModContent.DustType<ShadowflameEmber>(), (0f - Player.velocity.X) * 0.5f, Player.velocity.Y * 0.5f, 50, default(Color), 2f);
            Main.dust[num6].velocity.X = Main.dust[num6].velocity.X * 0.2f;
            Main.dust[num6].velocity.Y = -1.5f - Main.rand.NextFloat() * 0.5f;
            Main.dust[num6].fadeIn = 0.5f;
            Main.dust[num6].noGravity = true;
            Main.dust[num6].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
        }

        public override void UpdateBadLifeRegen()
        {
            Player player = Player;
            if (merPoison || merBoiling)
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                MercuryTimer++;
                int multiplier = 2;
                if (stoneRose)
                {
                    multiplier--;
                }
                player.lifeRegen -= Utils.Clamp(MercuryTimer / 60, 0, 10) * multiplier;
            }
            if (merBoiling)
            {
                if (Main.remixWorld)
                {
                    if (player.lifeRegen > 0)
                    {
                        player.lifeRegen = 0;
                    }
                    player.lifeRegen -= 10;
                }
            }
            if (!merPoison && !merBoiling && MercuryTimer >= 1)
            {
                MercuryTimer--;
            }
            if (MercuryTimer <= 1 && !merPoison && !merBoiling)
            {
                MercuryTimer = 0;
            }
        }

        public override void PostUpdateEquips()
        {
            if (Player.HeldItem.type == ModContent.ItemType<BlueSphere>())
            {
                Player.stringColor = PaintID.DeepYellowPaint;
            }
            if (Player.HeldItem.type == ModContent.ItemType<SilverLiner>())
            {
                Player.stringColor = PaintID.WhitePaint;
            }
            for (int index = 0; index < 59; ++index)
            {
                if (Player.inventory[index].type == ItemType<LargeOnyx>())
                {
                    largeGems[0] = true;
                }
            }
            if (Player.gemCount == 0)
            {
                if (largeGems > 0)
                {
                    ownedLargeGems = (byte)Player.ownedLargeGems;
                    hasLargeGems = (byte)largeGems;
                    Player.ownedLargeGems = 0;
                    Player.gem = -1;
                }
                else
                {
                    ownedLargeGems = 0;
                    hasLargeGems = 0;
                }
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (damageSource.SourceCustomReason == "Gemforge")
            {
                WeightedRandom<string> deathmessage = new();
                deathmessage.Add(Language.GetTextValue("Mods.TheDepths.PlayerDeathReason.Gemforge.0", Player.name));
                deathmessage.Add(Language.GetTextValue("Mods.TheDepths.PlayerDeathReason.Gemforge.1", Player.name));
                deathmessage.Add(Language.GetTextValue("Mods.TheDepths.PlayerDeathReason.Gemforge.2", Player.name));
                damageSource = PlayerDeathReason.ByCustomReason(deathmessage);
                return true;
            }
            else if (damageSource.SourceCustomReason == "Mercury" || (merPoison || merBoiling))
            {
                WeightedRandom<string> deathmessage = new();
                deathmessage.Add(Language.GetTextValue("Mods.TheDepths.PlayerDeathReason.MercuryPoisoning.0", Player.name));
                deathmessage.Add(Language.GetTextValue("Mods.TheDepths.PlayerDeathReason.MercuryPoisoning.1", Player.name));
                deathmessage.Add(Language.GetTextValue("Mods.TheDepths.PlayerDeathReason.MercuryPoisoning.2", Player.name));
                deathmessage.Add(Language.GetTextValue("Mods.TheDepths.PlayerDeathReason.MercuryPoisoning.3", Player.name));
                damageSource = PlayerDeathReason.ByCustomReason(deathmessage);
                return true;
            }
            else if (damageSource.SourceCustomReason == "Chasme")
            {
                WeightedRandom<string> deathmessage = new();
                deathmessage.Add(Language.GetTextValue("Mods.TheDepths.PlayerDeathReason.ChasmeHead.0", Player.name));
                deathmessage.Add(Language.GetTextValue("Mods.TheDepths.PlayerDeathReason.ChasmeHead.1", Player.name));
                deathmessage.Add(Language.GetTextValue("Mods.TheDepths.PlayerDeathReason.ChasmeHead.2", Player.name));
                damageSource = PlayerDeathReason.ByCustomReason(deathmessage);
                return true;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void PostUpdateMiscEffects()
        {
            Player player = Player;
            if (Main.netMode != NetmodeID.Server && Player.whoAmI == Main.myPlayer)
            {
                TextureAssets.Item[3729] = Request<Texture2D>("TheDepths/Assets/Retextures/LiquidSensor");
                TextureAssets.Tile[423] = Request<Texture2D>("TheDepths/Assets/Retextures/LiquidSensorTile");

                if (quicksilverSurfboard)
                {
                    TextureAssets.FlyingCarpet = Request<Texture2D>("TheDepths/Items/Accessories/QuickSilverSurfboard_Carpet");
                }
                else
                {
                    TextureAssets.FlyingCarpet = Main.Assets.Request<Texture2D>("Images/FlyingCarpet");
                }
                if (Worldgen.TheDepthsWorldGen.InDepths(player))
                {
                    //Old Texture/lava layer background
                    int[] bgnum = new int[30] { 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 150, 151, 152, 157, 158, 159, 185, 186, 187 };
                    foreach (int i in bgnum)
                    {
                        TextureAssets.Background[i] = Request<Texture2D>("TheDepths/Backgrounds/Background_" + i);
                    }
                }
                else
                {
                    //Old Texture/lava layer background
                    int[] bgnumOriginal = new int[30] { 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 150, 151, 152, 157, 158, 159, 185, 186, 187 };
                    foreach (int i in bgnumOriginal)
                    {
                        TextureAssets.Background[i] = Main.Assets.Request<Texture2D>("Images/Background_" + i);
                    }
                }
            }
            else
			{
                TextureAssets.Item[3729] = Main.Assets.Request<Texture2D>("Images/Item_3729");
                TextureAssets.Tile[423] = Main.Assets.Request<Texture2D>("Images/Tiles_423");
            }
            if (player.GetModPlayer<ModLiquidPlayer>().moddedWet[ModLiquidLib.ModLiquidLib.LiquidType<Quicksilver>() - LiquidID.Count])
            {
                if (Main.remixWorld)
                {
                    player.lavaTime = 1000;
                    player.buffImmune[BuffID.OnFire] = true;
                    player.buffImmune[BuffID.OnFire3] = true;
                    quicksilverWet = true;
                    if (AmuletTimer == 0)
                    {
                        if (NightwoodBuff == true)
                        {
                            player.AddBuff(BuffType<MercuryBoiling>(), 60 * (int)3.5, false, false);
                        }
                        else
                        {
                            player.AddBuff(BuffType<MercuryBoiling>(), 60 * 7, false, false);
                        }
                    }
                }
                else
                {
                    if (NightwoodBuff == true)
                    {
                        player.AddBuff(BuffType<MercuryFooting>(), 60 * 60, false, false);
                    }
                    else
                    {
                        player.AddBuff(BuffType<MercuryFooting>(), 60 * 30, false, false);
                    }
                    player.lavaTime = 1000;
                    player.buffImmune[BuffID.OnFire] = true;
                    player.buffImmune[BuffID.OnFire3] = true;
                    quicksilverWet = true;
                    if (AmuletTimer == 0)
                    {
                        QuicksilverTimer++;
                    }
                }
            }
            else
            {
                QuicksilverTimer = 0;
                quicksilverWet = false;
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (Main.myPlayer == Player.whoAmI && Player.difficulty == 0)
            {
                List<int> intList = new List<int>()
                {
                    ModContent.ItemType<LargeOnyx>(),
                };
                for (int index = 0; index < 59; ++index)
                {
                    if (Player.inventory[index].stack > 0 && intList.Contains(Player.inventory[index].type))
                    {
                        int number = Item.NewItem(new EntitySource_Misc(""), Player.getRect(), Player.inventory[index].type, 1, false, 0, false, false);
                        Main.item[number].Prefix(Player.inventory[index].prefix);
                        Main.item[number].stack = Player.inventory[index].stack;
                        Main.item[number].velocity.Y = (float)(Main.rand.Next(-20, 1) * 0.200000002980232);
                        Main.item[number].velocity.X = (float)(Main.rand.Next(-20, 21) * 0.200000002980232);
                        Main.item[number].noGrabDelay = 100;
                        Main.item[number].favorited = false;
                        Main.item[number].newAndShiny = false;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.SyncItem, number: number);
                        Player.inventory[index].SetDefaults(0, false);
                    }
                }
            }
        }

        public override void OnEnterWorld()
        {
            AmuletTimer = 0;
        }
    }

	public class NightwoodHelmetGlowmask : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Head);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}

            if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NightwoodHelmet>() || (drawPlayer.armor[10].type == ItemID.None && drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NightwoodHelmet>()))
            {
                Color color = drawPlayer.GetImmuneAlphaPure(Color.White, drawInfo.shadow);

                Texture2D texture = ModContent.Request<Texture2D>("TheDepths/Items/Armor/NightwoodHelmet_Head_Glow").Value;
                Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.bodyFrame.Width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition;
                Vector2 headVect = drawInfo.headVect;
                DrawData drawData = new DrawData(texture, drawPos.Floor() + headVect, drawPlayer.bodyFrame, color, drawPlayer.headRotation, headVect, 1f, drawInfo.playerEffect, 0)
                {
                    shader = drawInfo.cHead
                };
                drawInfo.DrawDataCache.Add(drawData);
            }
		}
	}

	public class NightwoodChestplateGlowmask : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Torso);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}

			if (drawPlayer.armor[11].type == ModContent.ItemType<Items.Armor.NightwoodBreastplate>() || (drawPlayer.armor[11].type == ItemID.None && drawPlayer.armor[1].type == ModContent.ItemType<Items.Armor.NightwoodBreastplate>()))
			{
				Color color = drawPlayer.GetImmuneAlphaPure(Color.White, drawInfo.shadow);

				Texture2D texture = ModContent.Request<Texture2D>("TheDepths/Items/Armor/NightwoodBreastplate_Body_Glow").Value;
				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame = new(0, 0, 40, 56);
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56))
				{
					frame = new(0, 2, 40, 56); //walking bop
				}
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 6, 40, 56))
				{
					frame = new(40, 0, 40, 56); //jumping frame
				}
				if (!drawPlayer.Male)
				{
					frame = new(0, 112, 40, 56);
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56))
					{
						frame = new(0, 114, 40, 56); //walking bop
					}
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 6, 40, 56))
					{
						frame = new(40, 112, 40, 56); //jumping frame
					}
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, color, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	public class NightwoodBrestpadGlowmask : PlayerDrawLayer
	{ //Shoulder Drawing
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.ArmOverItem);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}

			if (drawPlayer.armor[11].type == ModContent.ItemType<Items.Armor.NightwoodBreastplate>() || (drawPlayer.armor[11].type == ItemID.None && drawPlayer.armor[1].type == ModContent.ItemType<Items.Armor.NightwoodBreastplate>()))
			{
				Color color = drawPlayer.GetImmuneAlphaPure(Color.White, drawInfo.shadow);

				Texture2D texture = ModContent.Request<Texture2D>("TheDepths/Items/Armor/NightwoodBreastplate_Body_Glow").Value;

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame = new(0, 56, 40, 56);
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56))
				{
					frame = new(0, 58, 40, 56); //walking bop
				}
				if (!drawPlayer.Male)
				{
					frame = new(0, 168, 40, 56);
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56))
					{
						frame = new(0, 170, 40, 56); //walking bop
					}
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, color, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}

	public class NightwoodLimbGlowmaskRemover : PlayerDrawLayer
	{ //Arm Drawing
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.ArmOverItem);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}

			if (drawPlayer.armor[11].type == ModContent.ItemType<Items.Armor.NightwoodBreastplate>() || (drawPlayer.armor[11].type == ItemID.None && drawPlayer.armor[1].type == ModContent.ItemType<Items.Armor.NightwoodBreastplate>()))
			{
				Texture2D texture = ModContent.Request<Texture2D>("TheDepths/Items/Armor/NightwoodBreastplate_Body").Value;

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame;
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 5, 40, 56))
				{
					frame = new(80, 56, 40, 56); //Jumping
				}
				else if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 1, 40, 56))
				{
					frame = new(120, 0, 40, 56); //Use1
				}
				else if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 2, 40, 56))
				{
					frame = new(160, 0, 40, 56); //Use2
				}
				else
				{
					frame = new(0, 0, 0, 0); //None
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorBody, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}

	public class NightwoodLeggingsGlowmask : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Leggings);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}

			if (drawPlayer.armor[12].type == ModContent.ItemType<Items.Armor.NightwoodGreaves>() || (drawPlayer.armor[12].type == ItemID.None && drawPlayer.armor[2].type == ModContent.ItemType<Items.Armor.NightwoodGreaves>()))
			{
				Color color = drawPlayer.GetImmuneAlphaPure(Color.White, drawInfo.shadow);

				Texture2D texture = ModContent.Request<Texture2D>("TheDepths/Items/Armor/NightwoodGreaves_Legs_Glow").Value;
				Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.legFrame.Width / 2, drawPlayer.height - drawPlayer.legFrame.Height + 4f) + drawPlayer.legPosition;
				Vector2 legsOffset = drawInfo.legsOffset;
				DrawData drawData = new DrawData(texture, drawPos.Floor() + legsOffset, drawPlayer.legFrame, color, drawPlayer.legRotation, legsOffset, 1f, drawInfo.playerEffect, 0)
				{
					shader = drawInfo.cLegs
				};
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}