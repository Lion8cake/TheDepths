using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	internal class OnyxRobe : ModItem
	{
	
	    public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Onyx Robe");
			Tooltip.SetDefault("20% decreased mana usage"
				+ "\nIncreases maximum mana by 60");
			ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
		}
		
		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 14;
			Item.rare = ItemRarityID.Blue;
			Item.value = 300000;
			Item.defense = 16;
		}

		public override void SetMatch(bool male, ref int equipSlot, ref bool robes) {
			robes = true;
			equipSlot = EquipLoader.GetEquipSlot(Mod, "OnyxRobe_Legs", EquipType.Legs);
		}
		
		public override void UpdateEquip(Player player)
		{
			player.manaCost -= 0.22f;
			player.statManaMax2 += 60;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Robe, 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Onyx>(), 10);
			recipe.AddTile(86);
			recipe.Register();
		}
	}
}
