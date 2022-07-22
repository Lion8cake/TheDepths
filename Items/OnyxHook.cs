using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
//using TheDepths.Items.Placeable;

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
		
		/*public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Onyx>(), 15).AddTile(TileID.Anvils).Register();
        }*/
    }
}
