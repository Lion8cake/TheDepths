using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using TheDepths.Items.Banners;
using TheDepths.Items.Placeable;
using Terraria.GameContent.ItemDropRules;
using TheDepths.Items.Armor;
using TheDepths.Items.Weapons;
using TheDepths.Items.Accessories;
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;

namespace TheDepths.NPCs
{
	public class Geomancer : ModNPC
	{
		public static bool PraiseTheRelic;

		public static bool TheRelicMadeHimExplode;

		public bool shouldFrameCounterIncrease;

		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = 20;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Velocity = 1f,
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {
			NPC.width = 40;
			NPC.height = 46;
			NPC.damage = 40;
			NPC.defense = 16;
			NPC.lifeMax = 85;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.lavaImmune = true;
			NPC.value = 700f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 3;
			AIType = NPCID.ChaosElemental;
			AnimationType = NPCID.ChaosElemental;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<GeomancerBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
		}

		public override void AI()
        {
            if (PraiseTheRelic == true)
            {
				NPC.aiStyle = 0;
				AIType = NPCID.BoundGoblin;
				AnimationType = 0;
			}
			if (TheRelicMadeHimExplode == true)
            {
				NPC.life = -1;
				AnimationType = NPCID.ChaosElemental;
				PraiseTheRelic = false;
			}
        }

        public override void FindFrame(int frameHeight)
        {
			if (PraiseTheRelic == true)
			{
				if (shouldFrameCounterIncrease)
				{
					NPC.frameCounter++;
				}
				else if (!shouldFrameCounterIncrease)
                {
					NPC.frameCounter--;
                }

				if (NPC.frameCounter >= 25)
                {
					shouldFrameCounterIncrease = false;
                }
				else if (NPC.frameCounter <= 0)
                {
					shouldFrameCounterIncrease = true;
                }

                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 16 * frameHeight;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 17 * frameHeight;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = 18 * frameHeight;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 19 * frameHeight;
                }
			}
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.Geomancer")
			});
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 3; i++)
				{
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 63);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 62);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PurplePlumbersHat>(), 250));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Geode>(), 1));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShadowSphere>(), 35));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LivingShadowStaff>(), 35));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StoneRose>(), 50));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if ((spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths && !Main.remixWorld) || (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths && (spawnInfo.SpawnTileX < Main.maxTilesX * 0.38 + 50.0 || spawnInfo.SpawnTileX > Main.maxTilesX * 0.62) && Main.remixWorld))
			{
				return 1f;
			}
			return 0f;
		}
	}
}
