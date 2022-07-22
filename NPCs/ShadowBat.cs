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

namespace TheDepths.NPCs
{
	public class ShadowBat : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Shadow Bat");
			Main.npcFrameCount[NPC.type] = 4;
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
			NPC.aiStyle = 14;
			AIType = NPCID.GiantBat;
			AnimationType = NPCID.GiantBat;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<ShadowBatBanner>();
		}
		
		public override void OnHitPlayer(Player target, int damage, bool crit)
    	{
    		target.AddBuff(BuffID.Blackout, 180);
		}
	}
}
