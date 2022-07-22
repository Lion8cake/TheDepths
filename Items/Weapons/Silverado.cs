//using TheDepths.Tiles;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Weapons
{
	public class Silverado : ModItem
	{
		public override void SetStaticDefaults() {
		}

		public override void SetDefaults() {
			Item.damage = 20;
			Item.DamageType = DamageClass.Ranged; 
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4; 
			Item.value = 50000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = 10; 
			Item.shootSpeed = 16f; 
			Item.useAmmo = AmmoID.Bullet;
		}
		
		    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (type == ProjectileID.Bullet) { 
				type = ProjectileID.BulletHighVelocity;
				}
			}

		/*public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 10).AddIngredient(ItemID.Handgun, 1).AddTile(TileID.Anvils).Register();
		}*/
	}
}
