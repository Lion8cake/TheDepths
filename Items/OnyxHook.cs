using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Items.Placeable;
using Terraria.GameContent.Creative;

namespace TheDepths.Items
{
    public class OnyxHook : ModItem
    {
		public override void SetStaticDefaults()
		{
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override void SetDefaults()
        {
			Item.noUseGraphic = true;
			Item.damage = 0;
			Item.knockBack = 7f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 18;
			Item.height = 28;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Blue;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.shootSpeed = 13f;
            Item.shoot = ModContent.ProjectileType<Projectiles.OnyxHook>();
        }
		
		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Onyx>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
