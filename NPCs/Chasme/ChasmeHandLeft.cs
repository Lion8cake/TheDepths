﻿using Microsoft.Xna.Framework;
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

public class ChasmeHandLeft : ChasmeHand
{
    public override void SetStaticDefaults()
    {
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
		SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
	}

	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHeart>()], quickUnlock: true);
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.ChasmeHands")
			});
	}
}