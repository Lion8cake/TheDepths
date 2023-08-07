using TheDepths.Tiles;
using TheDepths.Buffs;
using TheDepths.Items.Placeable;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheDepths.Items
{
	public class RubyRelic : ModItem
	{
		public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Do you hear something?'" +
                "\nMaybe putting this on a gemforge is not the best idea");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 1000;
			Item.rare = ItemRarityID.White;
			Item.maxStack = 999;
		}
	}
}