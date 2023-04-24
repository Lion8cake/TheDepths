using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
    public class QuartzFeeder : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.value = 2000;
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 9999;
        }
    }
}