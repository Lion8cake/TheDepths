using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable.Furniture
{
    public class QuartzSink : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 30;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Terraria.Item.sellPrice(copper: 60);
            Item.createTile = ModContent.TileType<Tiles.Furniture.QuartzSink>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 4).AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteOre>(), 2).AddIngredient(ItemID.WaterBucket, 1).AddTile(TileID.WorkBenches).Register();
        }
    }
}