using Terraria.ID;
using TheDepths.Tiles;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria;

namespace TheDepths.Items.Placeable
{
	public class QuicksilverSensor : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.value = 500;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.QuicksilverSensor>();
			Item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Cog, 5);
			recipe.AddIngredient(ModContent.ItemType<MagicQuicksilverDropper>());
			recipe.AddIngredient(ItemID.Wire);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SortAfterFirstRecipesOf(ItemID.LogicSensor_Lava);
			recipe.Register();
		}
	}
}
