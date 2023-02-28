using TheDepths.Projectiles;
using TheDepths.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Weapons
{
	public class DiamondBolt : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 78;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 8;
			Item.width = 48;
			Item.height = 44;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.autoReuse = true;
			Item.useStyle = 5;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 3f;
			Item.value = Item.sellPrice(0, 1, 2);
			Item.rare = 4;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.DiamondBolt>();
			Item.shootSpeed = 7.5f;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DiamondStaff);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.DiamondDust>(), 30);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}