using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
    public class HangingShadowShrub : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 36;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Terraria.Item.sellPrice(silver: 25);
            Item.createTile = ModContent.TileType<Tiles.HangingShadowShrub>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.PotSuspended).AddIngredient(ModContent.ItemType<ShadowShrub>()).Register();
        }
    }
}
