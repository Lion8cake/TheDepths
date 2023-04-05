using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class SilverSpherePotion : ModItem
	{
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
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
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ItemID.FlarefinKoi, 1);
			recipe.AddIngredient(ItemID.Obsidifish, 2);
			recipe.AddIngredient(ModContent.ItemType<Items.ShadowShrub>(), 1);
			recipe.AddTile(13);
			recipe.Register();
		}
    }
}
