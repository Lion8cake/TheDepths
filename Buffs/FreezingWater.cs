using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.Utilities;
using TheDepths.Dusts;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Terraria.ModLoader.ModContent;

namespace TheDepths.Buffs
{
	public class FreezingWater : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Freezing Water");
			Description.SetDefault("Moment has been completely disabled");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<TheDepthsPlayer>().slowWater = true;
			player.moveSpeed = 0f;
			for (int d = 0; d < 30; d++)
            {
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, DustType<SlowingWaterFire>())];
                dust.velocity *= 3;
            }
		}
		
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<NPCs.TheDepthsGlobalNPC>().slowWater = true;
     	}
	}
}
