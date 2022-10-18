using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheDepths.Items.Banners;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using TheDepths.Items.Armor;
using TheDepths.Items.Placeable;
using AltLibrary.Common.Systems;

namespace TheDepths.NPCs
{
    public class CrystalKing : ModNPC
    {

        public bool attacking;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal King");
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 74;
            NPC.damage = 50;
            NPC.defense = 34;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.lavaImmune = true;
            NPC.value = 1200f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CrystalKingBanner>();
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y > 0f)
            {
                NPC.frame.Y = 2 * frameHeight;
            }
            else if (attacking)
            {
                NPC.frame.Y = frameHeight;
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override void AI()
        {
            Lighting.AddLight(NPC.position, 0.1f, 0.1f, 0.3f);
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            Vector2 val = Main.player[NPC.target].Center + new Vector2(NPC.Center.X, NPC.Center.Y);
            Vector2 val2 = NPC.Center + new Vector2(NPC.Center.X, NPC.Center.Y);
            NPC.netUpdate = true;
            if (player.position.X > NPC.position.X)
            {
                NPC.spriteDirection = 1;
            }
            else if (player.position.X < NPC.position.X)
            {
                NPC.spriteDirection = -1;
            }
            NPC.TargetClosest();
            NPC.velocity.X = NPC.velocity.X * 0.93f;
            if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
            {
                NPC.velocity.X = 0f;
            }
            if (NPC.ai[0] == 0f)
            {
                NPC.ai[0] = 500f;
            }
            if (NPC.ai[2] != 0f && NPC.ai[3] != 0f)
            {
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                for (int i = 0; i < 50; i++)
                {
                    int num = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 92, 0f, 0f, 100, default(Color), 1.8f);
                    Dust obj = Main.dust[num];
                    obj.velocity *= 3f;
                    Main.dust[num].noGravity = true;
                }
                NPC.position.X = NPC.ai[2] * 16f - (float)(NPC.width / 2) + 8f;
                NPC.position.Y = NPC.ai[3] * 16f - (float)NPC.height;
                NPC.velocity.X = 0f;
                NPC.velocity.Y = 0f;
                NPC.ai[2] = 0f;
                NPC.ai[3] = 0f;
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                for (int j = 0; j < 50; j++)
                {
                    int num2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 92, 0f, 0f, 100, default(Color), 1.8f);
                    Dust obj2 = Main.dust[num2];
                    obj2.velocity *= 3f;
                    Main.dust[num2].noGravity = true;
                }
            }
            NPC.ai[0] += 1f;
            NPC.netUpdate = true;
            if (NPC.ai[0] >= 650f && Main.netMode != 1)
            {
                NPC.ai[0] = 1f;
                int num3 = (int)Main.player[NPC.target].position.X / 16;
                int num4 = (int)Main.player[NPC.target].position.Y / 16;
                int num5 = (int)NPC.position.X / 16;
                int num6 = (int)NPC.position.Y / 16;
                int num7 = 20;
                int num8 = 0;
                bool flag = false;
                if (Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000f)
                {
                    num8 = 100;
                    flag = true;
                }
                while (!flag && num8 < 100)
                {
                    num8++;
                    int num9 = Main.rand.Next(num3 - num7, num3 + num7);
                    for (int k = Main.rand.Next(num4 - num7, num4 + num7); k < num4 + num7; k++)
                    {
                        if ((k < num4 - 4 || k > num4 + 4 || num9 < num3 - 4 || num9 > num3 + 4) && (k < num6 - 1 || k > num6 + 1 || num9 < num5 - 1 || num9 > num5 + 1) && Main.tile[num9, k].HasUnactuatedTile)
                        {
                            bool flag2 = true;
                            if ((Main.tile[num9, k - 1].LiquidType == LiquidID.Lava))
                            {
                                flag2 = false;
                            }
                            if (flag2 && Main.tileSolid[Main.tile[num9, k].TileType] && !Collision.SolidTiles(num9 - 1, num9 + 1, k - 4, k - 1))
                            {
                                NPC.ai[2] = num9;
                                NPC.ai[3] = k;
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                NPC.netUpdate = true;
            }
            if (!player.dead && Vector2.Distance(player.Center, NPC.Center) < 1000f)
            {
                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= 150f)
                {
                    attacking = true;
                }
                if (NPC.ai[1] == 150f)
                {
                    for (int l = 0; l < 10; l++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, 92, Main.rand.Next(-6, 6), Main.rand.Next(-6, 6), 0, default(Color), 1.4f);
                    }
                }
                if (NPC.ai[1] >= 180f)
                {
                    SoundEngine.PlaySound(SoundID.Item42, NPC.position);
                    float num10 = (float)Math.Atan2(val2.Y - val.Y, val2.X - val.X);
                    int num11 = Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X, NPC.Center.Y, (float)(Math.Cos(num10) * 14.0 * -1.0), (float)(Math.Sin(num10) * 14.0 * -1.0), Mod.Find<ModProjectile>("CrystalBall").Type, 25, 0f, 0);
                    Main.projectile[num11].friendly = false;
                    Main.projectile[num11].hostile = true;
                    Main.projectile[num11].timeLeft = 120;
                    attacking = false;
                    NPC.ai[1] = 0f;
                }
            }
            NPC.netUpdate = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode && spawnInfo.Player.ZoneUnderworldHeight && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
            {
                return 1.5f;
            }
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DiamondDust>(), 1, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrystalCrown>(), 100, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.Diamond, 50, 1, 1));
        }
    }
}