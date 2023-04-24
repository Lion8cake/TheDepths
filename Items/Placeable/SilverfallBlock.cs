using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TheDepths.Items.Placeable
{
    public class SilverfallBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LavafallBlock);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.SilverfallBlock>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Glass);
            recipe.AddTile(TileID.CrystalBall);
            recipe.AddCondition(Language.GetOrRegister("Mods.TheDepths.Recipes.NearQuicksilver"), () => TheDepthsWorldGen.depthsorHell && Main.LocalPlayer.adjLava);
            recipe.Register();
        }
    }
}
