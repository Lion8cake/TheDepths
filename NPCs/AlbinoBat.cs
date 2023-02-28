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
//using AltLibrary.Common.Systems;
using TheDepths.Biomes;
using Terraria.GameContent.Bestiary;

namespace TheDepths.NPCs
{
	public class AlbinoBat : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Albino Bat");
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
			NPC.width = 26;
			NPC.height = 16;
			NPC.damage = 24;
			NPC.defense = 9;
			NPC.lifeMax = 52;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath4;
			NPC.value = 120f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 14;
			NPC.lavaImmune = true;
			AIType = NPCID.GiantBat;
			AnimationType = NPCID.GiantBat;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<AlbinoBatBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("A victum of a rare pigment mutation this bat roams the deeper cavern system to blend in with the surrounding mercury in order to survive")
			});
		}

		/*public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneUnderworldHeight && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
			{
				return 1.25f;
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
				var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 1; i++)
				{
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("AlbinoBatGore").Type);
				}
			}
		}
	}
}
