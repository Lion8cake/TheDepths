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
        public static RecipeGroup DepthRecipeGroup;

        public override void Unload()
        {
            DepthRecipeGroup = null;
        }

        public override void AddRecipeGroups()
        {
            DepthRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.TerrasparkBoots)}",
            ItemID.TerrasparkBoots);

            RecipeGroup.RegisterGroup("The Depths: Terraspark Boot recipe", DepthRecipeGroup);
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ItemID.TerrasparkBoots);
                recipe.AddIngredient(ItemID.FrostsparkBoots)
                .AddIngredient(ModContent.ItemType<SilverSlippers>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}