using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Placeable
{
	public class BlackStainedGlass : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.autoReuse = true;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.width = 12;
			Item.height = 12;
			Item.value = Item.sellPrice(0, 0, 1, 60);
			Item.createWall = ModContent.WallType<Walls.BlackStainedGlass>();
		}
	}
}