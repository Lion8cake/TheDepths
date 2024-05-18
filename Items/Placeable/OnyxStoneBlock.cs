using Terraria.ID;
using TheDepths.Tiles;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria;

namespace TheDepths.Items.Placeable
{
	public class OnyxStoneBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 9999;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.OnyxStone>();
			Item.SetShopValues(ItemRarityID.White, Item.sellPrice(0, 0, 1));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe()
				.AddIngredient(ModContent.ItemType<Onyx>())
				.AddIngredient(ItemID.StoneBlock)
				.AddCondition(Condition.InGraveyard)
				.AddTile(TileID.HeavyWorkBench)
				.SortAfterFirstRecipesOf(ItemID.AmberStoneBlock)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<OnyxStoneWall>(), 4)
				.AddTile(TileID.WorkBenches)
				.Register()
				.SortAfter(recipe);
		}
	}
}
