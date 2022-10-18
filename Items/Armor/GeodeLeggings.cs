using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Items.Placeable;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class GeodeLeggings : ModItem
	{
		public override void SetStaticDefaults() {
		Tooltip.SetDefault("Incressed maximum amount of minions");
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.White;
			Item.defense = 5;
			Item.value = 4500;
		}
		
		public override void UpdateEquip(Player player) {
		player.maxMinions++;
		}
		
		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}