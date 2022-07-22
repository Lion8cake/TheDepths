using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Accessories
{
	class LodeStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lode Stone");
			Tooltip.SetDefault("Increases item pickup range");
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
			Item.width = 10;
			Item.accessory = true;
			Item.value = 100000;
			Item.height = 10;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<TheDepthsPlayer>().lodeStone = true;
		}
	}
}