using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Items.Accessories;
using TheDepths.Items.Placeable;

namespace TheDepths
{
    internal class RecipeGroups : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ItemID.TerrasparkBoots);
                recipe.AddIngredient(ItemID.FrostsparkBoots)
                .AddIngredient(ModContent.ItemType<SilverSlippers>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            Recipe recipe2 = Recipe.Create(ItemID.LogicSensor_Liquid);
                recipe2.AddIngredient(ItemID.Cog, 5)
                .AddIngredient(ItemID.MagicWaterDropper)
                .AddIngredient(ModContent.ItemType<MagicQuicksilverDropper>())
                .AddIngredient(ItemID.MagicHoneyDropper)
                .AddIngredient(ItemID.Wire)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            Recipe recipe3 = Recipe.Create(ItemID.DryBomb);
                recipe3.AddIngredient(ModContent.ItemType<Items.Weapons.QuicksilverBomb>())
                .Register();
            Recipe recipe4 = Recipe.Create(ItemID.DryRocket);
                recipe4.AddIngredient(ModContent.ItemType<Items.Weapons.QuicksilverRocket>())
                .Register();
            Recipe recipe5 = Recipe.Create(ItemID.SeafoodDinner);
                recipe5.AddIngredient(ModContent.ItemType<Items.ShadowFightingFish>(), 2)
                .AddTile(TileID.CookingPots)
                .Register();
            Recipe recipe6 = Recipe.Create(ItemID.SeafoodDinner);
                recipe6.AddIngredient(ModContent.ItemType<Items.QuartzFeeder>(), 2)
                .AddTile(TileID.CookingPots)
                .Register();
            Recipe recipe7 = Recipe.Create(ItemID.PotionOfReturn);
                recipe7.AddIngredient(ModContent.ItemType<Items.QuartzFeeder>())
                .AddIngredient(ItemID.RecallPotion)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
}