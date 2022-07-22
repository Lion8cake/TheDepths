using Terraria.ID;
using TheDepths.Tiles;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
	public class ArqueriteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 90;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 99;
			Item.value = 20000;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.ArqueriteBar>();
			Item.placeStyle = 0;
			Item.rare = ItemRarityID.Green;
		}

		/*public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<ArqueriteOre>(), 3).AddIngredient(ModContent.ItemType<Quartz>(), 1).AddTile(ModContent.TileType<Tiles.Gemforge>()).Register();
		}*/
	}
}
