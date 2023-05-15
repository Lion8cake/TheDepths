using TheDepths.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.Localization;

namespace TheDepths.Items.Placeable
{
	public class GlitterBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.GlitterBlock>();
			Item.width = 16;
			Item.height = 16;
		}
	}
}
