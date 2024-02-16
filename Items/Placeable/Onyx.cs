using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheDepths.Items.Placeable
{
	public class Onyx : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Amber;
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
			Item.createTile = ModContent.TileType<Tiles.PlacedOnyx>();
			Item.value = 15000;
			Item.placeStyle = 1;
		}

		public override bool? UseItem(Player player)
		{
			int i = Player.tileTargetX;
			int j = Player.tileTargetY;
			if ((WorldGen.SolidTile(i - 1, j) || WorldGen.SolidTile(i + 1, j) || WorldGen.SolidTile(i, j - 1) || WorldGen.SolidTile(i, j + 1)))
			{
				Item.createTile = ModContent.TileType<Tiles.PlacedOnyx>();
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
