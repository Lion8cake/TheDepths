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
	public class MercuryPoisoning : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Mercury Poisoning");
			Description.SetDefault("Losing life and slowed movement");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<TheDepthsPlayer>().merPoison = true;
			int extra = player.buffTime[buffIndex] / 60;
			player.lifeRegen = -60;
			player.moveSpeed -= 0.5f;
			if (Main.rand.Next(4) < 3)
            {
                Dust dust = Main.dust[Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, ModContent.DustType<MercuryFire>(), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default(Color), 3f)];
                dust.noGravity = true;
                dust.velocity *= 1.8f;
                dust.velocity.Y -= 0.5f;
            }
		}
	}
}
