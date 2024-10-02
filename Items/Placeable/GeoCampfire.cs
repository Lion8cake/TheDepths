using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
    public class GeoCampfire : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 18;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.rare = ItemRarityID.White;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.GeoCampfire>();
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.ShimmerCampfire;
		}
    }
}