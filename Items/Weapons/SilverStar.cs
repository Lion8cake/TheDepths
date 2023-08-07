using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Projectiles;
using Microsoft.Xna.Framework;

namespace TheDepths.Items.Weapons
{
	internal class SilverStar : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Silver Star");
			SacrificeTotal = 1;

			ItemID.Sets.ToolTipDamageMultiplier[Type] = 2f;
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 45; 
			Item.useTime = 45;
			Item.knockBack = 5f;
			Item.width = 34;
			Item.height = 34;
			Item.damage = 37;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.SilverStar>();
			Item.shootSpeed = 12f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Orange; //maybe
			Item.value = Item.sellPrice(gold: 2, silver: 50); //change later
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.channel = true;
			Item.noMelee = true;
		}
	}
}
