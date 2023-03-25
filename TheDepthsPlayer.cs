using Terraria.GameContent;
using TheDepths.Buffs;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using System.IO;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;
using TheDepths.Items;
using TheDepths.Projectiles.Nohitweapon;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;
using AltLibrary.Common.Systems;
using TheDepths.Biomes;
using System;
using TheDepths.Items.Weapons;

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
        public bool noHit;
        public bool stoneRose;
        public bool quicksilverSurfboard;
        public int tremblingDepthsScreenshakeTimer;

        public bool geodeCrystal;
        public bool livingShadow;
        public bool miniChasme;
        public bool miniChasmeArms;
        public bool ShadePet;

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
            quicksilverSurfboard = false;

            geodeCrystal = false;
            livingShadow = false;
            miniChasme = false;
            miniChasmeArms = false;
            ShadePet = false;


            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].boss)
                {
                    return;
                }
            }
            noHit = false;
        }

        public override void ModifyScreenPosition()
        {
            if (tremblingDepthsScreenshakeTimer > 0)
            {
                Main.screenPosition += Main.rand.NextVector2Circular(20, 20);
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            Player player = Main.LocalPlayer;

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
                r *= 0.1f;
                g *= 0.2f;
                b *= 0.7f;
                fullBright = true;
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

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (item.CountsAsClass(DamageClass.Melee))
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

        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            if (item.CountsAsClass(DamageClass.Melee))
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

        public override void PostUpdate()
        {
            if (lodeStone)
            {
                Player.defaultItemGrabRange = 107;
            }
            if (tremblingDepthsScreenshakeTimer > 0)
            {
                tremblingDepthsScreenshakeTimer--;
            }
            if (Player.dead)
            {
                MercuryTimer = 0;
            }
            //Main.NewText(MercuryTimer); //For Debugging, posts number of ticks that have passed when the player is on Mercury
        }

        public override void UpdateBadLifeRegen()
        {
            if (merBoiling || merPoison)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                MercuryTimer++;
                int multiplier = 2;
                if (stoneRose)
                {
                    multiplier--;
                }
                Player.lifeRegen -= Utils.Clamp(MercuryTimer / 60, 0, 10) * multiplier;
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
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<BlueSphere>())
            {
                Player.stringColor = PaintID.DeepYellowPaint;
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

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (damageSource.SourceCustomReason == "Gemforge")
            {
                WeightedRandom<string> deathmessage = new();
                deathmessage.Add(Language.GetTextValue(Player.name + " tried to summon a cult in pure light", Player.name));
                deathmessage.Add(Language.GetTextValue(Player.name + " moved the gemforge away from its home", Player.name));
                deathmessage.Add(Language.GetTextValue(Player.name + " tried to burned a relic on the surface", Player.name));
                damageSource = PlayerDeathReason.ByCustomReason(deathmessage);
                return true;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        }

        public override void PostUpdateMiscEffects()
        {
            if (Main.netMode != 2 && Player.whoAmI == Main.myPlayer)
            {
                if (quicksilverSurfboard)
                {
                    TextureAssets.FlyingCarpet = Request<Texture2D>("TheDepths/Assets/FlyingCarpet/SilverSurfboard");
                }
                else
                {
                    TextureAssets.FlyingCarpet = Main.Assets.Request<Texture2D>("Images/FlyingCarpet");
                }
                if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
                {
                    //TextureAssets.Dust = Request<Texture2D>("TheDepths/Lava/Dust");
                    TextureAssets.Liquid[1] = Request<Texture2D>("TheDepths/Lava/Quicksilver_Block");
                    TextureAssets.LiquidSlope[1] = Request<Texture2D>("TheDepths/Lava/Quicksilver_Slope");
                    TextureAssets.Item[207] = Request<Texture2D>("TheDepths/Lava/QuicksilverBucket");
                    TextureAssets.Item[4820] = Request<Texture2D>("TheDepths/Lava/BottomlessQuicksilverBucket");
                    TextureAssets.Item[4872] = Request<Texture2D>("TheDepths/Lava/QuicksilverSponge");

                    //Old Texture/lava layer background
                    int[] bgnum = new int[30] { 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 150, 151, 152, 157, 158, 159, 185, 186, 187 };
                    foreach (int i in bgnum)
                    {
                        TextureAssets.Background[i] = Request<Texture2D>("TheDepths/Backgrounds/Background_" + i);
                    }
                }
                else
                {
                    //TextureAssets.Dust = Main.Assets.Request<Texture2D>("Images/Dust");
                    TextureAssets.Liquid[1] = Main.Assets.Request<Texture2D>("Images/Liquid_1");
                    TextureAssets.LiquidSlope[1] = Main.Assets.Request<Texture2D>("Images/LiquidSlope_1");
                    TextureAssets.Item[207] = Main.Assets.Request<Texture2D>("Images/Item_207");
                    TextureAssets.Item[4820] = Main.Assets.Request<Texture2D>("Images/Item_4820");
                    TextureAssets.Item[4872] = Main.Assets.Request<Texture2D>("Images/Item_4872");

                    //Old Texture/lava layer background
                    int[] bgnumOriginal = new int[30] { 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 150, 151, 152, 157, 158, 159, 185, 186, 187 };
                    foreach (int i in bgnumOriginal)
                    {
                        TextureAssets.Background[i] = Main.Assets.Request<Texture2D>("Images/Background_" + i);
                    }
                }
            }
            if (Player.lavaWet && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
            {
                Player.AddBuff(BuffType<MercuryBoiling>(), 60 * 7, false, false);
                Player.lavaImmune = true;
            }
            /*else if (Player.lavaWet && WorldBiomeManager.WorldHell != "TheDepths/AltDepthsBiome")
            {
                Player.lavaImmune = false;
            }*/
            
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
    }
}