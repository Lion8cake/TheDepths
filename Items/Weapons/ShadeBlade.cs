using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using TheDepths.Projectiles;
using Terraria.DataStructures;

namespace TheDepths.Items.Weapons
{
	public class ShadeBlade : ModItem
	{
		public static bool handsReturning;

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 49;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 10;
			Item.value = Item.buyPrice(gold: 5);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.crit = 4;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<ShadeBladeCalculator>();
			Item.shootSpeed = 10f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ShadeBladeLeft>()] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<ShadeBladeRight>()] == 0)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<Projectiles.ShadeBladeRight>(), damage, knockback, Main.LocalPlayer.whoAmI);
			}
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ShadeBladeLeft>()] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<ShadeBladeRight>()] == 1)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<Projectiles.ShadeBladeLeft>(), damage, knockback, Main.LocalPlayer.whoAmI);
			}
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ShadeBladeLeft>()] == 1 && player.ownedProjectileCounts[ModContent.ProjectileType<ShadeBladeRight>()] == 1 && player.ownedProjectileCounts[ModContent.ProjectileType<ShadeBladeCalculator>()] == 0 && handsReturning == false)
			{
				return true;
			}
			return false;
        }
    }
}
