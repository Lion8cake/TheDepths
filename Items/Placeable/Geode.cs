using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
	public class Geode : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults() {
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.PlacedGems>();
			Item.placeStyle = 1;
		}

		public override bool? UseItem(Player player)
		{
			int i = Player.tileTargetX;
			int j = Player.tileTargetY;
			if ((WorldGen.SolidTile(i - 1, j) || WorldGen.SolidTile(i + 1, j) || WorldGen.SolidTile(i, j - 1) || WorldGen.SolidTile(i, j + 1)))
			{
				Item.createTile = ModContent.TileType<Tiles.PlacedGems>();
				Item.consumable = true;
			}
			else
			{
				Item.createTile = -1;
				Item.consumable = false;
			}
			return null;
		}
	}
}
