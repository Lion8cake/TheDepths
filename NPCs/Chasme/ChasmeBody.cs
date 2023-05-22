using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Biomes;

namespace TheDepths.NPCs.Chasme;

public class ChasmeBody : ChasmeBodyPart
{
	public override bool IsLoadingEnabled(Mod mod) => true;

	protected override Vector2 BaseOffset => new(x: -98, y: 0);

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
		base.SetDefaults();
		NPC.width = 364;
		NPC.height = 208;
		NPC.defense = 18;
		NPC.lifeMax = 9999;
		NPC.damage = 40;
		NPC.dontTakeDamage = true; //make it invincible 
		SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
	}

	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHeart>()], quickUnlock: true);
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.Chasme")
			});
	}
}
