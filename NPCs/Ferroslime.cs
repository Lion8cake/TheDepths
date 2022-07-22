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
    }
}