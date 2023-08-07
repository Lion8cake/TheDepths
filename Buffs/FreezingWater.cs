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
using TheDepths.NPCs;

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
		}
		
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<NPCs.TheDepthsGlobalNPC>().slowWater = true;
     	}
	}
}
