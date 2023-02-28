/*using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace TheDepths.Items.Weapons
{
	public class WhiteLightning : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Tooltip.SetDefault("Summons up to 3 Lightning orbs that connect to each other");
		}

		public override void SetDefaults() {
			Item.damage = 16;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 5;
			Item.width = 38;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<Projectiles.WhiteLightningOrb>();
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 3;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int tileX = (int)((Main.mouseX + Main.screenPosition.X) / 16);
			int tileY = (int)((Main.mouseY + Main.screenPosition.Y) / 16);
			if (player.ownedProjectileCounts[Item.shoot] < 3 && (!Main.tile[tileX, tileY].HasTile || !Main.tileSolid[Main.tile[tileX, tileY].TileType]))
			{
				Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}*/