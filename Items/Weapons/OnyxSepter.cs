using TheDepths.Projectiles;
using TheDepths.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Weapons
{
	public class OnyxSepter : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 68;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 6;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 27;
			Item.useAnimation = 27;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 0, 80);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<OnyxBolt>();
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