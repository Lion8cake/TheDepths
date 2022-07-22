using TheDepths.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
	public class ArqueriteBricks : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.ArqueriteBricks>();
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.Green;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.StoneBlock, 1).AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteOre>(), 1).AddTile(TileID.Furnaces).Register();
		}
	}
}
