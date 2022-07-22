using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Buffs;
using TheDepths.Projectiles.Summons;

namespace TheDepths.Items.Weapons
{
	public class LivingShadowStaff : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Summons a Silhouette to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults() {
			Item.damage = 25;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 26;
			Item.height = 28;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(0, 0, 100, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item44;
			Item.buffType = ModContent.BuffType<LivingShadowSummonBuff>();
			Item.shoot = ModContent.ProjectileType<LivingShadowSummonProj>();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = Main.MouseWorld;
		}

		/*public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2);
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;
			return false;
		}*/
	}
}
