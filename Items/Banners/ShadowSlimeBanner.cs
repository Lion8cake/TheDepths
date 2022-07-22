using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Banners
{
	public class ShadowSlimeBanner : ModItem
	{
		public override void SetDefaults() {
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<DepthsBanners>();
			Item.placeStyle = 1;
		}
	}
}