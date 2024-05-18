using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Placeable
{
	public class MercuryMossBricks : ModItem
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
			Item.createTile = ModContent.TileType<Tiles.MercuryMossBricks>();
			Item.width = 12;
			Item.height = 12;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(10);
			recipe.AddIngredient(ModContent.ItemType<MercuryMoss>(), 1);
			recipe.AddIngredient(ItemID.ClayBlock, 10);
			recipe.AddTile(TileID.Furnaces);
			recipe.SortAfterFirstRecipesOf(ItemID.VioletMossBlock);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ModContent.ItemType<MercuryMossBrickWall>(), 4);
			recipe2.AddTile(TileID.WorkBenches);
			recipe2.Register();
			recipe2.SortAfter(recipe);
		}
	}
}
