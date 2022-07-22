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
	public class Geomancer : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Geomancer");
			Main.npcFrameCount[NPC.type] = 16; 
		}

		public override void SetDefaults() {
			NPC.width = 40;
			NPC.height = 80;
			NPC.damage = 40;
			NPC.defense = 16;
			NPC.lifeMax = 85;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 700f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 3;
			AIType = NPCID.ChaosElemental;
			AnimationType = NPCID.ChaosElemental;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<GeomancerBanner>();
		}
	}
}
