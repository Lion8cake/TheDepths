using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class FlaskofMercury : ModItem
	{
        public override void SetStaticDefaults()
        {
		    DisplayName.SetDefault("Flask of Mercury");
            Tooltip.SetDefault("Melee attacks boils enemies with mercury");
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
            Item.rare = ItemRarityID.LightRed;
            Item.value = 2500;
            Item.buffType = ModContent.BuffType<Buffs.FlaskofMercury>();
            Item.buffTime = 72000;
        }
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteOre>(), 3);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.Register();
		}
    }
}
