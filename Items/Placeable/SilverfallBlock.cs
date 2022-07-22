using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TheDepths.Items.Placeable
{
    public class SilverfallBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silverfall Block");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LavafallBlock);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.SilverfallBlock>();
        }
    }
}
