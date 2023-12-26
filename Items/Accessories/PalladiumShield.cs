using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Buffs;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Accessories
{
[AutoloadEquip(new EquipType[] { EquipType.Shield })]
	public class PalladiumShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Green;
			Item.width = 24;
			Item.value = Item.sellPrice(0, 1, 75, 0);
			Item.accessory = true;
			Item.height = 28;
			Item.defense = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<TheDepthsPlayer>().pShield = true;
		}
	}
}