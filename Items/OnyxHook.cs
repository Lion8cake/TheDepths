using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Items.Placeable;

namespace TheDepths.Items
{
    public class OnyxHook : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SapphireHook);
            Item.shootSpeed = 18f;
            Item.shoot = Mod.Find<ModProjectile>("OnyxHook").Type;
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
