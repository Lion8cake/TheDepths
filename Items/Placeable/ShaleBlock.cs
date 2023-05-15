using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
	public class ShaleBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

        public override LocalizedText DisplayName
		{
			get
			{
				if (ModContent.GetInstance<TheDepthsClientConfig>().SlateConfig)
				{
					return Language.GetOrRegister("Mods.TheDepths.ShaleConfig.SlateBlock");
				}
				return Language.GetOrRegister("Mods.TheDepths.ShaleConfig.ShaleBlock");
			}
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
			Item.createTile = ModContent.TileType<Tiles.ShaleBlock>();
		}
	}
}
