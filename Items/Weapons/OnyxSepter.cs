using TheDepths.Projectiles;
using TheDepths.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Weapons
{
	public class OnyxSepter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Onyx Septer");
		}
	
		public override void SetDefaults()
		{
			Item.damage = 68;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 6;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.autoReuse = true;
			Item.useStyle = 5;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 0, 80);
			Item.rare = 4;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("OnyxBolt").Type;
			Item.shootSpeed = 7.5f;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 10);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 6);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Onyx>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}