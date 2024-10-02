using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Items.Placeable;

namespace TheDepths.Items.Weapons
{
	public class QuicksilverRocket : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults() {
			Item.damage = 40;
			Item.width = 20;
			Item.height = 14;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.ammo = AmmoID.Rocket;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 0, 10);
			Item.DamageType = DamageClass.Ranged;
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
		{
			if (weapon.type == ItemID.GrenadeLauncher || type == ProjectileID.GrenadeI)
			{
				type = ModContent.ProjectileType<Projectiles.QuicksilverGrenade>();
			}
			else if (weapon.type == ItemID.ProximityMineLauncher || type == ProjectileID.ProximityMineI)
			{
				type = ModContent.ProjectileType<Projectiles.QuicksilverMine>();
			}
			else if (weapon.type == ItemID.SnowmanCannon)
			{
				type = ModContent.ProjectileType<Projectiles.QuicksilverSnowmanRocket>();
			}
			else if (weapon.type == ItemID.FireworksLauncher)
			{
				type = ProjectileID.Celeb2Rocket + Main.rand.Next(0, 4);
			}
			else if (weapon.type == ItemID.ElectrosphereLauncher)
			{
				type = ProjectileID.ElectrosphereMissile;
			}
			else if (weapon.type == ItemID.Celeb2)
			{
				type = ProjectileID.Celeb2Rocket;
			}
			else
			{
				type = ModContent.ProjectileType<Projectiles.QuicksilverRocket>();
			}
		}
	}
}
