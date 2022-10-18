using TheDepths.Tiles;
using TheDepths.Buffs;
using TheDepths.Items.Placeable;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items
{
	public class RubyRelic : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("'Do you hear something?'");
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 1000;
			Item.rare = ItemRarityID.White;
			Item.accessory = true;
		}
	}
}