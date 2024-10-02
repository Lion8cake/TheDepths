using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Weapons
{
    public class NightwoodHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.hammer = 47;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 0, 0, 10);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            /*if (Item.Variant == ItemVariants.WeakerVariant)
            {
                Item.damage = 4;
                Item.hammer = 37;
            }*/
        }

        public override void UpdateInventory(Player player)
        {
            if (Main.remixWorld)
            {
                Item.damage = 4;
                Item.hammer = 37;
            }
            else
            {
                Item.damage = 10;
                Item.hammer = 47;
            }
        }
    }
}
