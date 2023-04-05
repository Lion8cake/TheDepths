using TheDepths.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Weapons
{
	public class BlueSphere : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.Yoyo[Item.type] = true;
			ItemID.Sets.GamepadExtraRange[Item.type] = 15;
			ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 24;
			Item.height = 24;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 16f;
			Item.knockBack = 2.5f;
			Item.damage = 50;
			Item.rare = ItemRarityID.LightRed;

			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			

			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(silver: 400);
			Item.shoot = ModContent.ProjectileType<BlueSphereYoyo>();
		}
	}
}
