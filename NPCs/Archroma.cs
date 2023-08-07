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
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;
using TheDepths.Dusts;

namespace TheDepths.NPCs
{
	public class Archroma : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Archroma");
			Main.npcFrameCount[NPC.type] = 2; 
		}
		
		public override void SetDefaults() {
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 75;
			NPC.defense = 35;
			NPC.lifeMax = 800;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 1500000f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 1;
			AIType = NPCID.Crimslime;
			AnimationType = NPCID.Crimslime;
			NPC.lavaImmune = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<AchromaBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("A cousin of pinky with a similar density causing them to be very strong, they eat money")
			});
		}

		/*public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.hardMode && spawnInfo.Player.ZoneUnderworldHeight && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
			{
				return 0.1f;
			}
			return 0f;
		}*/

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 10; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<MercurySparkleDust>());
				}
			}
		}
	}
}