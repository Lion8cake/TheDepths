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

namespace TheDepths.Items.Placeable
{
	public class CoreBuilder : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Core Builder");
            Tooltip.SetDefault("Allows you to convert hell materials into their depths alternatives and vice versa\n" +
                "'A legendary forge that is rumored to be the reason how the core of each world is created'");
        }

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 99;
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
            CreateRecipe()
                .AddIngredient<ShaleBlock>(50)
                .AddIngredient<Gemforge>(1)
                .AddIngredient<ArqueriteBar>(10)
                .AddTile(TileID.Anvils)
                .Register();

			AddAndReplace<ShaleBlock>(ItemID.AshBlock);
			AddAndReplace<Shalestone>(ItemID.AshBlock);
			AddAndReplace<ArqueriteBar>(ItemID.HellstoneBar);
			AddAndReplace<ArqueriteOre>(ItemID.Hellstone);
			AddAndReplace<QuartzBricks>(ItemID.ObsidianBrick);
			AddAndReplace<ArqueriteBricks>(ItemID.HellstoneBrick);
			AddAndReplace<QuartzBrickWall>(ItemID.ObsidianBrickWall);
			AddAndReplace<ArqueriteBrickWall>(ItemID.HellstoneBrickWall);
			AddAndReplace<RubyRelic>(ItemID.GuideVoodooDoll);
			AddAndReplace<ShadowShrub>(ItemID.Fireblossom);
			AddAndReplace<ShadowShrubSeeds>(ItemID.FireblossomSeeds);
			AddAndReplace<Quartz>(ItemID.Obsidian);
			AddAndReplace<LivingFog>(ItemID.LivingFireBlock);
			AddAndReplace<SilverfallBlock>(ItemID.LavafallBlock);
			AddAndReplace<SilverfallWall>(ItemID.LavafallWall);
			AddAndReplace<ShadowBrick>(ItemID.DemoniteBrick);
			AddAndReplace<ShadowBrick>(ItemID.CrimtaneBrick);
			AddAndReplace<PurplePlumbersHat>(ItemID.PlumbersHat);
			AddAndReplace<Gemforge>(ItemID.Hellforge);
		}

		private static void AddAndReplace<TConf>(int hall) where TConf : ModItem
		{
			Recipe recipe = Recipe.Create(hall);
			recipe.AddIngredient(ContentInstance<TConf>.Instance.Type);
			recipe.AddTile(ModContent.TileType<CoreBuilderTile>());
			recipe.Register();
			recipe = Recipe.Create(ContentInstance<TConf>.Instance.Type);
			recipe.AddIngredient(hall);
			recipe.AddTile(ModContent.TileType<CoreBuilderTile>());
			recipe.Register();
		}

		private static void AddAndReplace<THall, TConf>() where TConf : ModItem where THall : ModItem
		{
			Recipe recipe = Recipe.Create(ContentInstance<THall>.Instance.Type);
			recipe.AddIngredient(ContentInstance<TConf>.Instance.Type);
			recipe.AddTile(ModContent.TileType<CoreBuilderTile>());
			recipe.Register();
			recipe = Recipe.Create(ContentInstance<TConf>.Instance.Type);
			recipe.AddIngredient(ContentInstance<THall>.Instance.Type);
			recipe.AddTile(ModContent.TileType<CoreBuilderTile>());
			recipe.Register();
		}
	}
}
