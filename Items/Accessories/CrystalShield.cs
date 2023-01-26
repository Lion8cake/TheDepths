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
	class CrystalShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Shield");
			Tooltip.SetDefault("Increases damage reduction by 10%" 
				+"\nImmune to knockback"
				+"\nImmune to mercury radiation");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.LightRed;
			Item.width = 20;
			Item.value = 100000;
			Item.accessory = true;
			Item.height = 20;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance = 0.1f;
			player.noKnockback = true;
			player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Accessories.CrystalSkull>(), 1);
			recipe.AddIngredient(ItemID.CobaltShield, 1);
			recipe.AddTile(114);
			recipe.Register();
		}
	}
}