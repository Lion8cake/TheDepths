using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Accessories
{
	public class QuicksilverproofFishingHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Lime;
			Item.width = 20;
			Item.accessory = true;
			Item.value = Item.sellPrice(0, 2);
			Item.height = 30;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.accLavaFishing = true;
		}
	}
}