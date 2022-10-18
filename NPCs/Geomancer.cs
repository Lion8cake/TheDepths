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

namespace TheDepths.NPCs
{
	public class Geomancer : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Geomancer");
			Main.npcFrameCount[NPC.type] = 16; 
		}

		public override void SetDefaults() {
			NPC.width = 40;
			NPC.height = 80;
			NPC.damage = 40;
			NPC.defense = 16;
			NPC.lifeMax = 85;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			//Sound 3 and 4 
			NPC.lavaImmune = true;
			NPC.value = 700f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 3;
			AIType = NPCID.ChaosElemental;
			AnimationType = NPCID.ChaosElemental;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<GeomancerBanner>();
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
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 13);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 12);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 11);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PurplePlumbersHat>(), 5000, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Geode>(), 1, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShadowSphere>(), 50, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LivingShadowStaff>(), 50, 1, 1));
			//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StoneRose>(), 50, 1, 1));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneUnderworldHeight && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
			{
				return 1.5f;
			}
			return 0f;
		}
	}
}
