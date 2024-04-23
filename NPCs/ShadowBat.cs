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
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;
using TheDepths.Items;

namespace TheDepths.NPCs
{
	public class ShadowBat : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = 4;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers
			{
				Position = new Vector2(0f, -20f),
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = -40f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
		}

		public override void SetDefaults() {
			NPC.width = 48;
			NPC.height = 40;
			NPC.damage = 62;
			NPC.defense = 18;
			NPC.lifeMax = 220;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath4;
			NPC.value = 400f;
			NPC.knockBackResist = 0.5f;
			NPC.lavaImmune = true;
			NPC.aiStyle = 14;
			AIType = NPCID.GiantBat;
			AnimationType = NPCID.GiantBat;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<ShadowBatBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.ShadowBat")
			});
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    	{
    		target.AddBuff(BuffID.Blackout, 180);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.hardMode && (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && !Main.remixWorld) || Main.hardMode && (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && (spawnInfo.SpawnTileX < Main.maxTilesX * 0.38 + 50.0 || spawnInfo.SpawnTileX > Main.maxTilesX * 0.62) && Main.remixWorld))
			{
				return 1.25f;
			}
			return 0f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Emerald, 50, 1, 1));
			npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Lamington>(), 150));
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

				for (int i = 0; i < 1; i++)
				{
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("ShadowBatGore").Type);
				}
			}
		}
	}
}
