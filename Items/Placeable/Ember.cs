using Terraria.ID;
using TheDepths.Tiles;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
	public class Ember : ModItem
	{
		public override void SetStaticDefaults()
		{
		}

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
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Ember>();
			Item.rare = ItemRarityID.Green;
		}
	}
}
