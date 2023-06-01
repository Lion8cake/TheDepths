using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Items.Weapons
{
	public class QuicksilverBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}
	
		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.shootSpeed = 5f;
			Item.shoot = ModContent.ProjectileType<Projectiles.QuicksilverBomb>();
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.useAnimation = 25;
			Item.noUseGraphic = true;
			Item.useTime = 25;
			Item.value = Item.sellPrice(0, 0, 5);
			Item.rare = ItemRarityID.Blue;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DryBomb);
			recipe.AddCondition(Language.GetOrRegister("Mods.TheDepths.Recipes.NearQuicksilver"), () => Worldgen.TheDepthsWorldGen.depthsorHell && Main.LocalPlayer.adjLava);
			recipe.Register();
		}
	}
}