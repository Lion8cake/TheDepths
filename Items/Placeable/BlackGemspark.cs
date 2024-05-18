using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
	public class BlackGemspark : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults() {
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.BlackGemspark>();
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(20);
			recipe.AddIngredient(ItemID.Glass, 20);
			recipe.AddIngredient(ModContent.ItemType<Onyx>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SortAfterFirstRecipesOf(ItemID.AmberGemsparkBlock);
			recipe.Register();

			Recipe recipe2 = CreateRecipe(1);
			recipe2.AddIngredient(ModContent.ItemType<Items.Placeable.BlackGemsparkWall>(), 4);
			recipe2.AddTile(TileID.WorkBenches);
			recipe2.Register();
			recipe2.SortAfter(recipe);

			Recipe recipe3 = CreateRecipe(1);
			recipe3.AddIngredient(ModContent.ItemType<Items.Placeable.BlackGemsparkWallOffline>(), 4);
			recipe3.AddTile(TileID.WorkBenches);
			recipe3.Register();
			recipe3.SortAfter(recipe2);
		}
	}
}
