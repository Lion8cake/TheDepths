using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Biomes;

namespace TheDepths.NPCs.Chasme;

public class ChasmeHandLeft : ModNPC
{
	public int HeartID;

	public override void SetStaticDefaults()
	{
		NPCID.Sets.TrailCacheLength[Type] = 10;
		NPCID.Sets.TrailingMode[Type] = 1;
		NPCID.Sets.BossBestiaryPriority.Add(Type);

		NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
		{
			CustomTexturePath = "TheDepths/NPCs/Chasme/ChasmeHand_Preview",
			PortraitScale = 0.75f,
			Scale = 0.75f,
			Position = new Vector2(50f, 80f),
			PortraitPositionXOverride = 10f,
			PortraitPositionYOverride = 40f
		};
		NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
	}

    public override void SetDefaults()
	{
		NPC.width = 160;
		NPC.height = 166;
		NPC.defense = 14;
		NPC.lifeMax = 350;
		NPC.damage = 35;
		NPC.HitSound = SoundID.Item70;
		NPC.DeathSound = SoundID.NPCDeath14;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.aiStyle = -1;
		NPC.noTileCollide = true;
		SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
	}

	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHeart>()], quickUnlock: true);
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.ChasmeHands")
		});
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

	public override void AI()
	{
		//ai[0] Regenerating
		//ai[1] Dashing timer
		//ai[2] Shooting timer
		//ai[3] unused

		bool Regenerating = false;
		if (Main.npc[HeartID].type != ModContent.NPCType<ChasmeHeart>())
		{
			NPC.active = false;
		}
		NPC chasmeSoul = Main.npc[HeartID];
		//Positioning
		NPC.spriteDirection = NPC.direction = chasmeSoul.direction;
		NPC.Center = chasmeSoul.Center + new Vector2(114 * NPC.direction, 95);

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
		if (NPC.ai[0] != 0f)
		{
			Regenerating = true;
		}

		//Regen
		if (Regenerating)
		{
			NPC.ai[0]++;
			if (NPC.ai[0] >= 11)
			{
				if (NPC.life < NPC.lifeMax)
				{
					NPC.life++;
				}
				if (NPC.life >= NPC.lifeMax)
				{
					NPC.life = NPC.lifeMax;
				}
				if (NPC.life == NPC.lifeMax)
				{
					NPC.ai[0] = 0f;
				}
				NPC.ai[0] = 1;
			}
		}

		NPC.dontTakeDamage = (NPC.life == 1 || Regenerating);

		Main.BestiaryTracker.Kills.SetKillCountDirectly(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHandLeft>()], Main.BestiaryTracker.Kills.GetKillCount(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHeart>()]));
	}
}