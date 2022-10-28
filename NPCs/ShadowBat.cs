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
using AltLibrary.Common.Systems;
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;

namespace TheDepths.NPCs
{
	public class ShadowBat : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Shadow Bat");
			Main.npcFrameCount[NPC.type] = 4;

			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Position = new Vector2(0f, -20f),
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = -40f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
		}

		public override void SetDefaults() {
			NPC.width = 48;
			NPC.height = 40;
			NPC.damage = 62;
			NPC.defense = 18;
			NPC.lifeMax = 220;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
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

				new FlavorTextBestiaryInfoElement("This half black half emerald bat is a stonger varient of the bats found in the depths, Is it a bat's shadow or a shadow made of bats?")
			});
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
    	{
    		target.AddBuff(BuffID.Blackout, 180);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.hardMode && spawnInfo.Player.ZoneUnderworldHeight && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
			{
				return 1.5f;
			}
			return 0f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Emerald, 50, 1, 1));
		}
	}
}
