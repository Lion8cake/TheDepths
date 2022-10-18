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
using Terraria.Utilities;
using Terraria.WorldBuilding;

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
		}
	}
}
