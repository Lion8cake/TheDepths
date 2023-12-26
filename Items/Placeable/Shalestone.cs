using TheDepths.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.Localization;

namespace TheDepths.Items.Placeable
{
	public class Shalestone : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
		}

		public override LocalizedText DisplayName
		{
			get
			{
				if (ModContent.GetInstance<TheDepthsClientConfig>().SlateConfig)
				{
					return Language.GetOrRegister("Mods.TheDepths.ShaleConfig.Slatestone");
				}
				return Language.GetOrRegister("Mods.TheDepths.ShaleConfig.Shalestone");
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
			Item.createTile = ModContent.TileType<Tiles.Shalestone>();
			Item.width = 12;
			Item.height = 12;
		}

		public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
		{
			if (Main.rand.NextBool(15) && Main.hardMode)
			{
				resultType = ModContent.ItemType<Onyx>();
				resultStack = 1;
			}
		}
	}
}
