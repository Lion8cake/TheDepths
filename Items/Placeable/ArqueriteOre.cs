using TheDepths.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.GameContent;

namespace TheDepths.Items.Placeable
{
	public class ArqueriteOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Hellstone;

			ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 1, ItemID.Hellstone, 1);
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
			Item.createTile = ModContent.TileType<Tiles.ArqueriteOre>();
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<LeakyMercuryWall>(), 4)
				.AddTile(TileID.WorkBenches)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<MercuryBubbleWall>(), 4)
				.AddTile(TileID.WorkBenches)
				.Register();
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<DottedQuicksilverWall>(), 4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
