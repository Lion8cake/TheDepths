using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Projectiles.Chasme;

namespace TheDepths.NPCs.Chasme;

public class ChasmeHandRight : ModNPC
{
	public int HeartID;

	private Vector2 DirectionToTarget = Vector2.Zero;

	public override void SetStaticDefaults()
	{
		NPCID.Sets.TrailCacheLength[Type] = 10;
		NPCID.Sets.TrailingMode[Type] = 1;
		NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
		{
			Hide = true
		};
		NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
	}

	public override void SetDefaults()
	{
		NPC.width = 176;
		NPC.height = 130;
		NPC.defense = 14;
		NPC.lifeMax = 350;
		NPC.damage = 35;
		NPC.HitSound = SoundID.Item70;
		NPC.DeathSound = SoundID.NPCDeath14;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.aiStyle = -1;
		NPC.noTileCollide = true;
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

	public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
	{
		if (Main.expertMode)
		{
			NPC.lifeMax /= 2;
		}
	}

	public override void AI()
	{
		//ai[0] Regenerating
		//ai[1] Dashing timer
		//ai[2] Dashing delay between dashes
		//ai[3] Shooting timer
		NPC.netUpdate = true;
		int xOff = 136;
		int yOff = -89;
		float magnitude = 1;
		bool Regenerating = false;
		bool Dashing = false;
		bool DashingStarted = false;
		bool Returning = false;
		if (Main.npc[HeartID].type != ModContent.NPCType<ChasmeHeart>() || !Main.npc[HeartID].active)
		{
			NPC.active = false;
			return;
		}
		NPC chasmeSoul = Main.npc[HeartID];

		//Extra hand offsets
		if ((chasmeSoul.ModNPC as ChasmeHeart).ChasmePartIDs[5] != -1 && Main.npc[(chasmeSoul.ModNPC as ChasmeHeart).ChasmePartIDs[5]].whoAmI == NPC.whoAmI)
		{
			xOff = 136 + 30;
			yOff = -89 - 65;
		}
		else if ((chasmeSoul.ModNPC as ChasmeHeart).ChasmePartIDs[7] != -1 && Main.npc[(chasmeSoul.ModNPC as ChasmeHeart).ChasmePartIDs[7]].whoAmI == NPC.whoAmI)
		{
			xOff = 136 + 50;
			yOff = -89 - 110;
		}
		else if ((chasmeSoul.ModNPC as ChasmeHeart).ChasmePartIDs[9] != -1 && Main.npc[(chasmeSoul.ModNPC as ChasmeHeart).ChasmePartIDs[9]].whoAmI == NPC.whoAmI)
		{
			xOff = 136 + 70;
			yOff = -89 - 155;
		}

		//targetting
		NPC.target = chasmeSoul.target;
		Player player = Main.player[NPC.target];

		//Damage scaling
		float damagePer = Main.getGoodWorld ? 1 : (float)(1.00 - (float)(chasmeSoul.life) / (float)(chasmeSoul.lifeMax));
		int damage = (int)MathHelper.Lerp(NPC.defDamage, (float)(NPC.defDamage * 1.5), damagePer);
		NPC.damage = !chasmeSoul.dontTakeDamage ? damage / 2 : damage;
		if (chasmeSoul.ai[3] > 0)
			NPC.damage = 0;

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
		if (NPC.ai[0] != 0f && NPC.ai[1] <= 0)
		{
			Regenerating = true;
		}
		NPC.dontTakeDamage = (NPC.life == 1 || Regenerating);

		if (chasmeSoul.ai[3] <= 0)
		{
			//Regen
			if (Regenerating)
			{
				if (chasmeSoul.dontTakeDamage)
				{
					NPC.ai[0]++;
					if (NPC.ai[0] >= 4f)
					{
						NPC.ai[0] = 1f;
						if (NPC.life < NPC.lifeMax)
						{
							NPC.life++;
						}
						if (NPC.life >= NPC.lifeMax)
						{
							NPC.life = NPC.lifeMax;
							NPC.ai[0] = 0f;
							NPC.ai[1] = 0f;
							NPC.ai[2] = 0f;
						}
					}
				}
			}
			else
			{
				//Dashing Movement
				if (NPC.ai[1] == 0)
					NPC.ai[2]++;

				if (NPC.ai[2] >= 60 * 5)
				{
					if (NPC.ai[1] < 1)
						DashingStarted = true;

					Dashing = true;
					NPC.ai[2] = 60 * 5;
				}
				if (NPC.ai[1] >= (float)(60 * 1.5))
					Returning = true;

				if (Returning)
				{
					NPC.ai[2] = 0;
					magnitude = Math.Clamp(magnitude * 1.03f, 10, 100);

					if (NPC.Center.WithinRange(chasmeSoul.Center + new Vector2(xOff * NPC.direction, yOff), 16))
					{
						NPC.ai[1] = -1f;
						magnitude = 1;
					}
					else
						NPC.ai[1] = (float)(60 * 1.5);

					NPC.velocity = NPC.DirectionTo(chasmeSoul.Center + new Vector2(xOff * NPC.direction, yOff)) * magnitude;
				}
				if (DashingStarted)
				{
					NPC.velocity = NPC.velocity.RotatedBy(NPC.AngleTo(player.Center) / 20);
					DirectionToTarget = NPC.DirectionTo(player.Center);
				}
				if (Dashing)
				{
					NPC.ai[1]++;
					NPC.velocity = DirectionToTarget * 18;
					if (DirectionToTarget.X * 14 > NPC.Center.X)
					{
						NPC.spriteDirection = 1;
					}
				}
			}

			//Emerald Energry Shots
			NPC.ai[3]++;
			if (NPC.ai[3] >= (NPC.dontTakeDamage ? Main.rand.Next(4, 16) : Main.rand.Next(2, 8)) * 60 && NPC.ai[1] == 0)
			{
				if (Main.netMode != 1)
				{
					Vector2 accuracy = !chasmeSoul.dontTakeDamage ? new Vector2(Main.rand.Next(-128, 128), Main.rand.Next(-128, 128)) : Vector2.Zero; //Fuck up the accuracy when the core it out
					int damage2 = (int)MathHelper.Lerp(55, (float)(55 * 1.5), damagePer);
					int projDamage = (!chasmeSoul.dontTakeDamage ? damage2 / 2 : damage2) / 2; //divided by 2 because projectiles multiply the damage by 2 for some dumbass reason
					Vector2 val = player.Center + new Vector2(NPC.Center.X + 38 * NPC.direction, NPC.Center.Y + 16) + accuracy;
					Vector2 val2 = NPC.Center + new Vector2(NPC.Center.X + 38 * NPC.direction, NPC.Center.Y + 16);
					float shootSpeed = (float)Math.Atan2(val2.Y - val.Y, val2.X - val.X);
					int proj = Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X + 38 * NPC.direction, NPC.Center.Y + 16, (float)(Math.Cos(shootSpeed) * 14.0 * -1.0), (float)(Math.Sin(shootSpeed) * 14.0 * -1.0), ModContent.ProjectileType<EmeraldEnergy>(), projDamage, 0f, 0);
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
				}
				NPC.ai[3] = 0;
			}
		}

		//Positioning
		NPC.spriteDirection = NPC.direction = chasmeSoul.direction;
		if (!Dashing && !Returning)
			NPC.Center = chasmeSoul.Center + new Vector2(xOff * NPC.direction, yOff);

		// Legendary/FTW Mode Changes
		if (Main.getGoodWorld)
		{
			NPC.scale = (float)(ContentSamples.NpcsByNetId[Type].scale * 0.9);
		}

		//Death
		if (chasmeSoul.ai[3] == 341)
		{
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

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		NPC chasmeSoul = Main.npc[HeartID];
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
		else
		{
			SpriteEffects spriteEffects = (SpriteEffects)0;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = (SpriteEffects)1;
			}
			Vector2 halfSize = new((float)(TextureAssets.Npc[NPC.type].Width() / 2), (float)(TextureAssets.Npc[NPC.type].Height() / Main.npcFrameCount[NPC.type] / 2));
			float num306 = Main.NPCAddHeight(NPC);
			if ((int)NPC.ai[1] > 0)
			{
				for (int num177 = 1; num177 < NPC.oldPos.Length; num177++)
				{
					_ = ref NPC.oldPos[num177];
					Color newColor5 = drawColor;
					newColor5.R = (byte)(0.5 * (int)(newColor5.R * (double)(10 - num177) / 20.0));
					newColor5.G = (byte)(0.5 * (int)(newColor5.G * (double)(10 - num177) / 20.0));
					newColor5.B = (byte)(0.5 * (int)(newColor5.B * (double)(10 - num177) / 20.0));
					newColor5.A = (byte)(0.5 * (int)(newColor5.A * (double)(10 - num177) / 20.0));
					newColor5 = NPC.GetShimmerColor(newColor5);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, new Vector2(NPC.oldPos[num177].X - screenPos.X + (float)(NPC.width / 2) - (float)TextureAssets.Npc[NPC.type].Width() * NPC.scale / 2f + halfSize.X * NPC.scale, NPC.oldPos[num177].Y - screenPos.Y + (float)NPC.height - (float)TextureAssets.Npc[NPC.type].Height() * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + halfSize.Y * NPC.scale + num306), (Rectangle?)NPC.frame, newColor5, NPC.rotation, halfSize, NPC.scale, spriteEffects, 0f);
				}
			}
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

	public override void SendExtraAI(BinaryWriter writer)
	{
		writer.Write(HeartID);
		writer.Write(DirectionToTarget.X);
		writer.Write(DirectionToTarget.Y);
	}

	public override void ReceiveExtraAI(BinaryReader reader)
	{
		HeartID = reader.ReadInt32();
		DirectionToTarget.X = reader.ReadSingle();
		DirectionToTarget.Y = reader.ReadSingle();
	}
}