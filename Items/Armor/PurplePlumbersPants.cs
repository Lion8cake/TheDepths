using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class PurplePlumbersPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.vanity = true;
            Item.rare = ItemRarityID.White;
            Item.value = 250000;
        }
    }
}