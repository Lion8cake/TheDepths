using Terraria.DataStructures;
using TheDepths.Projectiles.Summons;
using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Weapons
{
	public class LivingShadowStaff : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
			Item.shoot = ModContent.ProjectileType<LivingShadowSummonProj>();
			Item.buffType = ModContent.BuffType<Buffs.LivingShadowSummonBuff>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}
	}
}
