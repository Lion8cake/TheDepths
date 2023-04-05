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
	public class MercuryFooting : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
	    	Main.pvpBuff[Type] = false;
	    	Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex) 
		{
			player.moveSpeed += 0.2f;
		}
	}
}
