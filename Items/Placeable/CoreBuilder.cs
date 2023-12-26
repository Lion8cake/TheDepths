using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items;
using TheDepths.Items.Weapons;
using TheDepths.Items.Placeable;
using TheDepths.Items.Armor;
using TheDepths.Items.Accessories;
using TheDepths.Items.Banners;
using TheDepths.Tiles;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Placeable
{
	public class CoreBuilder : ModItem
	{
		public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 9999;
			Item.value = 3000;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.CoreBuilderTile>();
			Item.placeStyle = 0;
			Item.rare = ItemRarityID.Green;
		}
		
		public override void AddRecipes()
        {
			CreateRecipe()
                .AddIngredient(ItemID.AshBlock, 50)
                .AddIngredient(ItemID.Hellforge, 1)
                .AddIngredient(ItemID.HellstoneBar, 10)
                .AddTile(TileID.Anvils)
				.Register();

			AddAndReplace<RubyRelic>(ItemID.GuideVoodooDoll);
			AddAndReplace<ShalestoneConch>(ItemID.DemonConch);
		}

		private static void AddAndReplace<TConf>(int hall) where TConf : ModItem
		{
			Recipe recipe = Recipe.Create(hall);
			recipe.AddIngredient(ContentInstance<TConf>.Instance.Type);
			recipe.AddTile(ModContent.TileType<CoreBuilderTile>());
			recipe.DisableDecraft();
			recipe.Register();
			recipe = Recipe.Create(ContentInstance<TConf>.Instance.Type);
			recipe.AddIngredient(hall);
			recipe.AddTile(ModContent.TileType<CoreBuilderTile>());
			recipe.DisableDecraft();
			recipe.Register();
		}
	}
}
