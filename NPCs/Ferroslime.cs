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
using TheDepths.Items.Accessories;
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;
using TheDepths.Dusts;

namespace TheDepths.NPCs
{
	public class Ferroslime : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ferroslime");
			Main.npcFrameCount[NPC.type] = 2; 
		}
		
		public override void SetDefaults() {
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 90;
			NPC.defense = 28;
			NPC.lifeMax = 450;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.lavaImmune = true;
			NPC.value = 2000f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 1;
			AIType = NPCID.Crimslime;
			AnimationType = NPCID.Crimslime;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<FerroslimeBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("This slime may appear spiky, but that's only its ferrofluid spiking up to come off as more threatening than it is.")
			});
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
    	{
    		target.AddBuff(BuffID.Blackout, 180);
    	}

        /*public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			if (Main.hardMode && spawnInfo.Player.ZoneUnderworldHeight && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
            {
				return 1.3f;
            }
            return 0f;
        }*/

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LodeStone>(), 33, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 5, 15));
		}

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
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ShaleDust>());
				}
			}
		}
	}
}