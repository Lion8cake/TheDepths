using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Weapons
{
    public class Shaderang : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.damage = 30;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 14;
            Item.useStyle = 1;
            Item.useTime = 14;
            Item.knockBack = 10f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.height = 38;
            Item.value = Item.buyPrice(0, 0, 10);
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<Projectiles.Shaderang>();
            Item.shootSpeed = 16f;
        }

        public override bool CanUseItem(Player player)
        {
            int stack = Item.stack;
            bool canuse = true;
            for (int m = 0; m < 1000; m++)
            {
                if (Main.projectile[m].active && Main.projectile[m].owner == Main.myPlayer && Main.projectile[m].type == Item.shoot)
                    stack -= 1;
            }
            if (stack <= 0) canuse = false;
            return canuse;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 3)
                .AddIngredient(ItemID.ThornChakram)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
