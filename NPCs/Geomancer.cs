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

        /*public override void AI() //AI was supposed to freeze the npc in place and then force him into a warshipping animation which he would just die when the screenflash is over, ill leave this in the code for now
        {
            if (PraiseTheRelic == 1)
            {
				NPC.ai[3] = 125f;
				PraiseTheRelic = 0;
            }
			if (NPC.ai[3] <= 125f)
            {
				NPC.ai[0] = 0f;
                NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
				NPC.velocity = Vector2.Zero;
				NPC.frame.Y = 16 * 56;
				AnimationType = NPCID.CultistDevote;
			}
			if (TheRelicMadeHimExplode == 1)
            {
				OnKill();
				NPC.frame.Y = 0;
				AnimationType = NPCID.ChaosElemental;
				TheRelicMadeHimExplode = 0;
			}
        }*/

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
