using Terraria.ID;
using TheDepths.Tiles;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
	public class Geode : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 99;
			Item.value = 500;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			//Item.consumable = true;
			//Item.createTile = ModContent.TileType<Tiles.PlacedGems>();
			Item.rare = ItemRarityID.White;
		}
	}
}
