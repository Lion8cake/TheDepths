using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheDepths.Items.Banners;
using TheDepths.Items.Placeable;
using Terraria.GameContent.ItemDropRules;
using TheDepths.Items.Armor;
using TheDepths.Items.Weapons;
using TheDepths.Items.Accessories;
using AltLibrary.Common.Systems;
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;

namespace TheDepths.NPCs
{
	public class Geomancer : ModNPC
	{
		public static int PraiseTheRelic;

		public static int TheRelicMadeHimExplode;

		public bool shouldFrameCounterIncrease;

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Geomancer");
			Main.npcFrameCount[NPC.type] = 20; 
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

        public override void AI() //AI was supposed to freeze the npc in place and then force him into a warshipping animation which he would just die when the screenflash is over, ill leave this in the code for now
        {
            if (PraiseTheRelic == 1)
            {
				NPC.ai[3] = 125f;
            }
			if (NPC.ai[3] >= 125f)
            {
				NPC.aiStyle = 0;
				AIType = NPCID.BoundGoblin;
				//AnimationType = NPCID.CultistDevote;
			}
			if (TheRelicMadeHimExplode == 1)
            {
				NPC.life = -1;
				AnimationType = NPCID.ChaosElemental;
				PraiseTheRelic = 0;
			}
        }

        public override void FindFrame(int frameHeight)
        {
			if (PraiseTheRelic == 1)
			{
				if (NPC.frameCounter <= 0)
				{
					shouldFrameCounterIncrease = true;
				}
				if (NPC.frameCounter >= 50)
				{
					shouldFrameCounterIncrease = false;
				}

				NPC.frameCounter += shouldFrameCounterIncrease ? 1 : -1; //thanks absoluteAquarian

                if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 16 * frameHeight;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 17 * frameHeight;
                }
                else if (NPC.frameCounter < 30)
                {
                    NPC.frame.Y = 18 * frameHeight;
                }
                else if (NPC.frameCounter < 40)
                {
                    NPC.frame.Y = 19 * frameHeight;
                }
				Main.NewText(NPC.frameCounter);
			}
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Loyal cult members well-practiced in geomancy. Legends have it that they worship a living shadow-flame being")
			});
		}

		public override void HitEffect(int hitDirection, double damage)
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
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PurplePlumbersHat>(), 5000));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Geode>(), 1));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShadowSphere>(), 50));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LivingShadowStaff>(), 50));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StoneRose>(), 50));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneUnderworldHeight && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
			{
				return 1f;
			}
			return 0f;
		}
	}
}
