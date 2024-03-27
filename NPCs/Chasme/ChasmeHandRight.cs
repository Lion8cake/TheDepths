using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.NPCs.Chasme;

public class ChasmeHandRight : ModNPC
{
	public int HeartID;

	public override void SetStaticDefaults()
	{
		NPCID.Sets.TrailCacheLength[Type] = 10;
		NPCID.Sets.TrailingMode[Type] = 1;
		NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
		{
			Hide = true
		};
		NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
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

	public override void AI()
	{
		bool Regenerating = false;
		if (Main.npc[HeartID].type != ModContent.NPCType<ChasmeHeart>())
		{
			NPC.active = false;
		}
		NPC chasmeSoul = Main.npc[HeartID];
		//Positioning
		NPC.spriteDirection = NPC.direction = chasmeSoul.direction;
		NPC.Center = chasmeSoul.Center + new Vector2(136 * NPC.direction, -89);

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

		//regen
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
	}
}