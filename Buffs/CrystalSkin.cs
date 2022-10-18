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
	public class CrystalSkin : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Crystal Skin");
			Description.SetDefault("Immune to Quicksilver");
			Main.debuff[Type] = false;
	    	Main.pvpBuff[Type] = true;
	    	Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex) {
		player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
		player.buffImmune[ModContent.BuffType<MercuryBoiling>()] = true;
		}
	}
}
