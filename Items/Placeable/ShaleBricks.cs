using Terraria;
using TheDepths.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace TheDepths.Items.Placeable
{
	public class ShaleBricks : ModItem
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
					return Language.GetOrRegister("Mods.TheDepths.ShaleConfig.SlateBricks");
				}
				return Language.GetOrRegister("Mods.TheDepths.ShaleConfig.ShaleBricks");
			}
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
			Item.createTile = ModContent.TileType<Tiles.ShaleBricks>();
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.White;
		}
	}
}
