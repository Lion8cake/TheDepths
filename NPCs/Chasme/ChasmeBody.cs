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
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Biomes;

namespace TheDepths.NPCs.Chasme
{
	public class ChasmeBody : ModNPC
	{
		public int HeartID;

		public override void SetStaticDefaults()
		{
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				CustomTexturePath = "TheDepths/NPCs/Chasme/Chasme_Preview",
				PortraitScale = 0.5f,
				Scale = 0.5f,
				Position = new Vector2(50f, 50f),
				PortraitPositionYOverride = 30f,
				PortraitPositionXOverride = 50f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.MercuryBoiling>()] = true; //How would you even inflict the body with this debuff
		}

		public override void SetDefaults()
		{
			NPC.width = 364;
			NPC.height = 208;
			NPC.defense = 18;
			NPC.lifeMax = 2500;
			NPC.damage = 40;
			NPC.knockBackResist = 0f;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.Item70;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHeart>()], quickUnlock: true);
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.Chasme")
			});
		}

		public override bool CheckActive()
		{
			return false;
		}

		public override void AI()
		{
			NPC.netUpdate = true;
			if (Main.npc[HeartID].type != ModContent.NPCType<ChasmeHeart>() || !Main.npc[HeartID].active)
			{
				NPC.active = false;
				return;
			}
			NPC chasmeSoul = Main.npc[HeartID];
			NPC.spriteDirection = NPC.direction = chasmeSoul.direction;
			NPC.Center = chasmeSoul.Center + new Vector2((Main.getGoodWorld ? -138 : -98) * NPC.direction, Main.getGoodWorld ? 46 : 0);
			if (chasmeSoul.life <= 0)
			{
				NPC.life = 0;
				NPC.checkDead();
			}

			//Damage scaling
			float damagePer = Main.getGoodWorld ? 1 : (float)(1.00 - (float)(chasmeSoul.life) / (float)(chasmeSoul.lifeMax));
			int damage = (int)MathHelper.Lerp(NPC.defDamage, (float)(NPC.defDamage * 1.5), damagePer);
			NPC.damage = !chasmeSoul.dontTakeDamage ? damage / 2 : damage;
			if (chasmeSoul.ai[3] > 0)
				NPC.damage = 0;

			// Legendary/FTW Mode Changes
			if (Main.getGoodWorld)
			{
				NPC.scale = (float)(ContentSamples.NpcsByNetId[Type].scale * 1.3);
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

			Main.BestiaryTracker.Kills.SetKillCountDirectly(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeBody>()], Main.BestiaryTracker.Kills.GetKillCount(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHeart>()]));
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
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			HeartID = reader.ReadInt32();
		}
	}
}