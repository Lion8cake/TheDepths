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
	public class GoldBat : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Gold Bat");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults() {
			NPC.width = 28;
			NPC.height = 28;
			NPC.damage = 25;
			NPC.defense = 10;
			NPC.lifeMax = 320;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 14;
			AIType = NPCID.GiantBat;
			AnimationType = NPCID.GiantBat;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<GoldBatBanner>();
		}
	}
}
