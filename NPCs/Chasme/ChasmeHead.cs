using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheDepths.Dusts;
using TheDepths.Projectiles;
using TheDepths.Projectiles.Chasme;
namespace TheDepths.NPCs.Chasme
{
    public class ChasmeHead : ModNPC
    {
        public int HeartID;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 272;
            NPC.height = 170;
            NPC.defense = 18;
            NPC.lifeMax = 2500;
            NPC.damage = 40;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.Item70;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
        }

        public override bool CheckDead()
        {
            if (Main.npc[HeartID].life > 0)
            {
                NPC.life = 1;
                return false;
            }
            else
                return true;
        }

		public override bool CheckActive()
		{
			return false;
		}

		public override void AI()
        {
			//ai[0] Ruby Ray timer
			//ai[1] Crying Timer
			//ai[2] unused
			//ai[3] unused

			if (Main.npc[HeartID].type != ModContent.NPCType<ChasmeHeart>() || !Main.npc[HeartID].active)
			{
				NPC.active = false;
                return;
			}
			NPC chasmeSoul = Main.npc[HeartID];
            //Positioning
            NPC.spriteDirection = NPC.direction = chasmeSoul.direction;
            NPC.Center = chasmeSoul.Center + new Vector2((Main.getGoodWorld ? 56 : 36) * NPC.direction, Main.getGoodWorld ? -162 : - 150);

            //Damage scaling
            float damagePer = Main.getGoodWorld ? 1 : (float)(1.00 - (float)(chasmeSoul.life) / (float)(chasmeSoul.lifeMax));
            NPC.damage = (int)MathHelper.Lerp(NPC.defDamage, (float)(NPC.defDamage * 1.5), damagePer);
			if (chasmeSoul.ai[3] > 0)
				NPC.damage = 0;

			//targetting
			NPC.target = chasmeSoul.target;
            Player player = Main.player[NPC.target];

            //Death checks
            if (chasmeSoul.life <= 0)
            {
                NPC.life = 0;
                NPC.checkDead();
            }
            else if (NPC.life <= 0)
            {
                NPC.life = 1;
            }
            NPC.dontTakeDamage = (NPC.life == 1 || chasmeSoul.ai[1] != 0);

            //Ruby Ray attacks
            if (chasmeSoul.ai[3] == 0) //Make sure the boss isnt dead
            {
                NPC.ai[0]++;
                if (NPC.ai[0] >= (NPC.dontTakeDamage ? 4.5 : 3) * 60)
                {
                    if (Main.netMode != 1)
                    {
                        Vector2 accuracy = NPC.dontTakeDamage ? new Vector2(Main.rand.Next(-128, 128), Main.rand.Next(-128, 128)) : Vector2.Zero; //Fuck up the accuracy when the core it out
                        int projDamage = (int)MathHelper.Lerp(70, (float)(70 * 1.5), damagePer) / 2; //divided by 2 because projectiles multiply the damage by 2 for some dumbass reason
                        Vector2 val = player.Center + new Vector2(NPC.Center.X + 60 * NPC.direction, NPC.Center.Y + 16) + accuracy;
                        Vector2 val2 = NPC.Center + new Vector2(NPC.Center.X + 60 * NPC.direction, NPC.Center.Y + 16);
                        float shootSpeed = (float)Math.Atan2(val2.Y - val.Y, val2.X - val.X);
                        Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X + 60 * NPC.direction, NPC.Center.Y + 16, (float)(Math.Cos(shootSpeed) * 14.0 * -1.0), (float)(Math.Sin(shootSpeed) * 14.0 * -1.0), ModContent.ProjectileType<ChasmeRay>(), projDamage, 0f, 0, NPC.life <= NPC.lifeMax / 2 ? 1f : 0f); //ai[0] for the projectile is whether it summons the shockwaves or not
                    }
                    NPC.ai[0] = 0;
                }

                //Quicksilver tears
                if (chasmeSoul.life <= chasmeSoul.lifeMax / 4)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] >= 15)
                    {
                        if (Main.netMode != 1)
                        {
                            for (int i = 0; i < 40; i++)
                            {
                                for (int j = 0; j < 5; j++)
                                {
                                    if (Main.rand.NextBool(200))
                                    {
                                        int projDamage = (int)MathHelper.Lerp(35, (float)(35 * 1.5), damagePer) / 2; //divided by 2 because projectiles multiply the damage by 2 for some dumbass reason
                                        float tearsXSpeed = Main.rand.Next(-25, 25) / 10;
                                        int projID = Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X + ((50 + i) * NPC.direction), NPC.Center.Y + 24 + j, tearsXSpeed, 1, ModContent.ProjectileType<QuicksilverTears>(), projDamage, 0f, 0);
                                        Projectile proj = Main.projectile[projID];
                                        proj.friendly = false;
                                        proj.hostile = true;
                                    }
                                }
                            }
                        }
                        NPC.ai[1] = 0;
                    }
                }
            }
            // Legendary/FTW Mode Changes
            if (Main.getGoodWorld)
            {
                NPC.scale = (float)(ContentSamples.NpcsByNetId[Type].scale * 0.8);
            }

            //Death
            if (chasmeSoul.ai[3] == 120 || chasmeSoul.ai[3] == 160)
            {
				SoundEngine.PlaySound(SoundID.Item70, NPC.position);
			}
            if (chasmeSoul.ai[3] == 341)
            {
				SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, NPC.position);
				for (int goreX = 64; goreX < NPC.width - 64; goreX++)
				{
					for (int goreY = 64; goreY < NPC.height - 64; goreY++)
					{
                        if (Main.rand.NextBool(400))
                        {
							Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(goreX, goreY), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)), Mod.Find<ModGore>("ShaleStoneGore" + Main.rand.Next(1, 7)).Type);
						}
					}
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
            int frame = NPC.frame.Y / frameHeight;
			if (NPC.life <= NPC.lifeMax / 2)
            {
                if (frame < 3)
                {
					NPC.frameCounter++;
					if (NPC.frameCounter >= 6)
					{
						frame++;
						NPC.frameCounter = 0;
					}
				}
			}
            else
            {
                if (frame > 0 && frame < 5)
                {
					NPC.frameCounter++;
					if (NPC.frameCounter >= 6)
					{
						frame++;
						NPC.frameCounter = 0;
					}
				}
            }
            if (frame >= 5)
            {
                frame = 0;
            }
            NPC.frame.Y = frame * frameHeight;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            NPC chasmeSoul = Main.npc[HeartID];
            string TextureExtention = "";
            if (chasmeSoul.life <= chasmeSoul.lifeMax / 4)
			{
                TextureExtention += "_Crying";
			}
            if (NPC.dontTakeDamage)
            {
                TextureExtention += "_BrokenEyes";
            }

		    if (chasmeSoul.ai[3] >= 340)
			{
				return false;
			}
			else if (chasmeSoul.ai[3] >= 120)
            {
				Texture2D asset = ModContent.Request<Texture2D>(Texture + "_Cracks", AssetRequestMode.ImmediateLoad).Value;
                Rectangle frame = chasmeSoul.ai[3] >= 180 ? new Rectangle(0, asset.Height / 2, asset.Width, asset.Height / 2) : new Rectangle(0, 0, asset.Width, asset.Height / 2);
				SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				float num66 = Main.NPCAddHeight(NPC);
				Vector2 origin = new Vector2(TextureAssets.Npc[NPC.type].Width() / 2, TextureAssets.Npc[NPC.type].Height() / Main.npcFrameCount[NPC.type] / 2);
				Vector2 pos = new Vector2(NPC.position.X - screenPos.X + (NPC.width / 2) - (TextureAssets.Npc[NPC.type].Width() * NPC.scale / 2f) + (origin.X * NPC.scale), NPC.position.Y - Main.screenPosition.Y + NPC.height - (TextureAssets.Npc[NPC.type].Height() * NPC.scale / Main.npcFrameCount[NPC.type]) + 4f + (origin.Y * NPC.scale) + num66);
				Main.spriteBatch.Draw(asset, pos, frame, drawColor, NPC.rotation, origin, NPC.scale, effects, 0f);
				return false;
			}
			else if (TextureExtention != "")
			{
                Texture2D asset = ModContent.Request<Texture2D>(Texture + TextureExtention, AssetRequestMode.ImmediateLoad).Value;
				SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				float num66 = Main.NPCAddHeight(NPC);
				Vector2 origin = new Vector2(TextureAssets.Npc[NPC.type].Width() / 2, TextureAssets.Npc[NPC.type].Height() / Main.npcFrameCount[NPC.type] / 2);
				Vector2 pos = new Vector2(NPC.position.X - screenPos.X + (NPC.width / 2) - (TextureAssets.Npc[NPC.type].Width() * NPC.scale / 2f) + (origin.X * NPC.scale), NPC.position.Y - Main.screenPosition.Y + NPC.height - (TextureAssets.Npc[NPC.type].Height() * NPC.scale / Main.npcFrameCount[NPC.type]) + 4f + (origin.Y * NPC.scale) + num66);
                Main.spriteBatch.Draw(asset, pos, NPC.frame, drawColor, NPC.rotation, origin, NPC.scale, effects, 0f);
                return false;
            }
            else
			{
                return true;
			}
        }

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			NPC chasmeSoul = Main.npc[HeartID];
            if (chasmeSoul.ai[3] >= 220 && chasmeSoul.ai[3] <= 340)
            {
                float percent = (float)(1f - ((float)(chasmeSoul.ai[3] < 340 ? chasmeSoul.ai[3] : 340) - 220) / 120f);
                Color color = Color.White * (chasmeSoul.ai[3] >= 220 ? MathHelper.Lerp(1, 0, percent) : 0);

				Texture2D asset = ModContent.Request<Texture2D>(Texture + "_CrackingOverlay", AssetRequestMode.ImmediateLoad).Value;
                Rectangle frame = new Rectangle(0, 0, asset.Width, asset.Height);
                SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                float num66 = Main.NPCAddHeight(NPC);
                Vector2 origin = new Vector2(TextureAssets.Npc[NPC.type].Width() / 2, TextureAssets.Npc[NPC.type].Height() / Main.npcFrameCount[NPC.type] / 2);
                Vector2 pos = new Vector2(NPC.position.X - screenPos.X + (NPC.width / 2) - (TextureAssets.Npc[NPC.type].Width() * NPC.scale / 2f) + (origin.X * NPC.scale), NPC.position.Y - Main.screenPosition.Y + NPC.height - (TextureAssets.Npc[NPC.type].Height() * NPC.scale / Main.npcFrameCount[NPC.type]) + 4f + (origin.Y * NPC.scale) + num66);
                spriteBatch.Draw(asset, pos, frame, color, NPC.rotation, origin, NPC.scale, effects, 0f);
            }
		}

		public override void DrawEffects(ref Color drawColor)
		{
            bool invincible = NPC.life == 1;
            if (NPC.dontTakeDamage != invincible && NPC.life <= 1)
			{
				for (int i = 0; i < 40; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						if (Main.rand.NextBool(20))
						{
                            Dust.NewDust(new Vector2(NPC.Center.X + ((48 + i) * NPC.direction), NPC.Center.Y + 18 + j), 6, 6, DustID.LifeCrystal);
						}
					}
				}
			}
		}
	}
}