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
			NPC.value = 250000f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 1;
			AIType = NPCID.Crimslime;
			AnimationType = NPCID.Crimslime;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<AchromaBanner>();
		}
	}
}