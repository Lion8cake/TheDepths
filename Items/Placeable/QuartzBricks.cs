using Terraria;
using TheDepths.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Placeable
{
	public class QuartzBricks : ModItem
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
			Item.createTile = ModContent.TileType<Tiles.QuartzBricks>();
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.Green;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(5);
			recipe.AddIngredient(ItemID.StoneBlock, 5);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 1);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ModContent.ItemType<Items.Placeable.Furniture.QuartzPlatform>(), 2);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ModContent.ItemType<Items.Placeable.QuartzBrickWall>(), 4);
			recipe3.AddTile(TileID.WorkBenches);
			recipe3.Register();
		}
	}
}
