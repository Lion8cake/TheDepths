using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class SilverLiner : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.CanFishInLava[Item.type] = true;
		}

		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 40;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 8;
			Item.useTime = 8;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.fishingPole = 47;
			Item.shootSpeed = 13f;
			Item.shoot = ModContent.ProjectileType<Projectiles.SilverBobber>();
		}

		public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
		{
			lineOriginOffset = new Vector2(47, -31);
		}
	}
}