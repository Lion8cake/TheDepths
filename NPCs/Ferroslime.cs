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
using AltLibrary.Common.Systems;

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
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 1;
			AIType = NPCID.Crimslime;
			AnimationType = NPCID.Crimslime;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<FerroslimeBanner>();
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
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LodeStone>(), 33, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 5, 15));
		}
	}
}