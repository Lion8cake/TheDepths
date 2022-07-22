using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace TheDepths.Pets.ShadePet
{
	public class ShadePetItem : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Cheese And Crackers");
			Tooltip.SetDefault("Summons a Shade Pet to follow you");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish); // Copy the Defaults of the Zephyr Fish Item.

			Item.shoot = ModContent.ProjectileType<ShadePetProjectile>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<ShadePetBuff>(); // Apply buff upon usage of the Item.
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600);
			}
		}
	}
}
