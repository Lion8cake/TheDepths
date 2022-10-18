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

namespace TheDepths.Items.Accessories
{
[AutoloadEquip(new EquipType[] { EquipType.Shield })]
	class SanctusShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanctus Shield");
			Tooltip.SetDefault("Provides immunity to most debuffs\nProvides Knockback and Mercury Poisoning immunity");
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Lime;
			Item.width = 20;
			Item.value = 250000;
			Item.accessory = true;
			Item.height = 20;
			Item.defense = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.noKnockback = true;
			player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
			player.buffImmune[46] = true;
			player.buffImmune[44] = true;
			player.buffImmune[33] = true;
			player.buffImmune[36] = true;
			player.buffImmune[30] = true;
			player.buffImmune[20] = true;
			player.buffImmune[32] = true;
			player.buffImmune[31] = true;
			player.buffImmune[35] = true;
			player.buffImmune[23] = true;
			player.buffImmune[22] = true;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Accessories.CrystalShield>(), 1);
			recipe.AddIngredient(ItemID.AnkhCharm, 1);
			recipe.AddTile(114);
			recipe.Register();
		}
	}
}