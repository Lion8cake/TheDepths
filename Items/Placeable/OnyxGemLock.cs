using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Tiles;

namespace TheDepths.Items.Placeable
{
    public class OnyxGemLock : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Onyx Gem Lock");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GemLockRuby);
            Item.createTile = ModContent.TileType<GemlockTile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Onyx>(), 5);
            recipe.AddIngredient(ItemID.StoneBlock, 10);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.Register();
        }
    }
}
