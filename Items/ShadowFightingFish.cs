using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
    public class ShadowFightingFish : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 38;
            Item.value = 6000;
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 9999;
        }
    }
}