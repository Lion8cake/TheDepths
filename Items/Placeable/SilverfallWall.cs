using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TheDepths.Items.Placeable
{
    public class SilverfallWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LavafallWall);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = WallType<Walls.SilverfallWall>();
        }
    }
}
