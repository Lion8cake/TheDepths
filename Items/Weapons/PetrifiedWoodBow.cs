using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Weapons
{
    public class PetrifiedWoodBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = 5;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.width = 12;
            Item.height = 28;
            Item.shoot = 1;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.damage = 8;
            Item.shootSpeed = 6.2f;
            Item.noMelee = true;
            Item.value = 100;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Placeable.PetrifiedWood>(), 8)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
