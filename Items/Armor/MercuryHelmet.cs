using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MercuryHelmet : ModItem
	{
		public override void SetStaticDefaults() {
		Tooltip.SetDefault("4% Increased Melee Critical Strike Chance");
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
			Item.value = 45000;
		}
		
		public override void UpdateEquip(Player player) {
		player.GetCritChance(DamageClass.Generic) += 4;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<MercuryChestplate>() && legs.type == ModContent.ItemType<MercuryGreaves>();
		}

		public override void UpdateArmorSet(Player player) {
			player.setBonus = "3 Defence";
			player.statDefense += 3;
		}

		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}