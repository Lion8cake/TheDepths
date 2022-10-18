using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Placeable
{
	public class ArqueriteBrickWall : ModItem
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
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Walls.ArqueriteBrickWall>();
		}

		public override void AddRecipes() {
			Recipe recipe = CreateRecipe(4);
			recipe.AddIngredient(ModContent.ItemType<ArqueriteBricks>());
			recipe.Register();
		}
	}
}