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
	public class ShadowSlime : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = 2; 
		}
		
		public override void SetDefaults() {
			NPC.width = 32;
			NPC.height = 32;
			NPC.damage = 34;
			NPC.defense = 12;
			NPC.lifeMax = 55;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 400f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 1;
			AIType = NPCID.Crimslime;
			AnimationType = NPCID.Crimslime;
			NPC.lavaImmune = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<ShadowSlimeBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.ShadowSlime")
			});
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    	{
    		target.AddBuff(BuffID.Darkness, 180);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneUnderworldHeight && TheDepthsWorldGen.depthsorHell)
			{
				return 1.75f;
			}
			return 0f;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 10; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ShaleDust>());
				}
			}
		}
	}
}