using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Weapons
{
    public class NightwoodBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.width = 12;
            Item.height = 28;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.damage = 11;
            Item.shootSpeed = 6.6f;
            Item.noMelee = true;
            Item.value = 100;
            /*if (Item.Variant == ItemVariants.WeakerVariant)
            {
                Item.damage = 7;
                Item.useAnimation = 29;
                Item.useTime = 29;
            }*/
        }

        public override void UpdateInventory(Player player)
        {
            if (Main.remixWorld)
            {
                Item.damage = 7;
                Item.useTime = 29;
                Item.useAnimation = 29;
            }
            else
            {
                Item.damage = 11;
                Item.useTime = 25;
                Item.useAnimation = 25;
            }
        }
    }
}
