using Terraria.ID;
using TheDepths.Tiles;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria;

namespace TheDepths.Items.Placeable
{
	public class MagicQuicksilverDropper : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(0, 0, 0, 40);
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.QuicksilverDropletSource>();
			Item.placeStyle = 0;
			Item.rare = ItemRarityID.White;
		}
	}
}
