using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class PurplePlumbersShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purple Plumber's Shirt");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.vanity = true;
            Item.rare = ItemRarityID.White;
            Item.value = 250000;
        }
    }
}