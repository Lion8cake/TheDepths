using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Placeable
{
	public class ShadowShrubSeeds : ModItem
	{
		public override void SetDefaults() {
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.placeStyle = 0;
			Item.width = 12;
			Item.height = 14;
			Item.value = 80;
			Item.createTile = ModContent.TileType<Tiles.ShadowShrub>();
		}
	}
}