using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class SilverSpherePotion : ModItem
	{
        public override void SetStaticDefaults()
        {
		    DisplayName.SetDefault("Silver Sphere Potion");
            Tooltip.SetDefault("Four Silver Spheres summon around you");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = 1000;
            Item.buffType = ModContent.BuffType<Buffs.SilverSphereBuff>();
            Item.buffTime = 14400;
        }
		
		/*public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ItemID.FlarefinKoi, 1).AddIngredient(ItemID.Obsidifish, 2).AddIngredient(ModContent.ItemType<Items.ShadowShrub>(), 1).AddTile(13).Register();
		}*/
    }
}
