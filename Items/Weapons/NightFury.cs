using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Weapons
{
    public class NightFury : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 36;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.rare = 2;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 9f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<Projectiles.DiamondArrow>();
            }
        }
    }
}
