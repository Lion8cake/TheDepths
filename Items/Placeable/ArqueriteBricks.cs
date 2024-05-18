using Terraria;
using TheDepths.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Placeable
{
	public class ArqueriteBricks : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.ArqueriteBricks>();
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.Green;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(5);
			recipe.AddIngredient(ItemID.StoneBlock, 5);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteOre>(), 1);
			recipe.AddTile(TileID.Furnaces);
			recipe.DisableDecraft();
			recipe.SortAfterFirstRecipesOf(ItemID.HellstoneBrick);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBrickWall>(), 4);
			recipe2.AddTile(TileID.WorkBenches);
			recipe2.Register();
			recipe2.SortAfter(recipe);
		}
	}
}
