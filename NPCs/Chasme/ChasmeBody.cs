using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				CustomTexturePath = "TheDepths/NPCs/Chasme/Chasme_Preview",
				PortraitScale = 0.5f,
				Scale = 0.5f,
				Position = new Vector2(50f, 50f),
				PortraitPositionYOverride = 30f,
				PortraitPositionXOverride = 50f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.width = 364;
			NPC.height = 208;
			NPC.defense = 18;
			NPC.lifeMax = 9999;
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

		public override void AI()
		{
			if (Main.npc[HeartID].type != ModContent.NPCType<ChasmeHeart>())
			{
				NPC.active = false;
			}
			NPC chasmeSoul = Main.npc[HeartID];
			NPC.spriteDirection = NPC.direction = chasmeSoul.direction;
			NPC.Center = chasmeSoul.Center + new Vector2(-98 * NPC.direction, 0);
			if (chasmeSoul.life <= 0)
			{
				NPC.life = 0;
				NPC.checkDead();
			}
			Main.BestiaryTracker.Kills.SetKillCountDirectly(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeBody>()], Main.BestiaryTracker.Kills.GetKillCount(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHeart>()]));
		}
	}
}