using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
	public class BlackGemspark : ModItem
	{
		public override void SetStaticDefaults() {
		}

		public override void SetDefaults() {
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.BlackGemspark>();
		}
		
		/*public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Glass, 20).AddIngredient(ModContent.ItemType<Items.Placeable.Onyx>(), 1).AddTile(TileID.WorkBenches).Register();
		}*/
	}
}
