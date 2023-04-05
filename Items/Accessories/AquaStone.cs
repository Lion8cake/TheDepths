using TheDepths.Tiles;
using TheDepths.Buffs;
using TheDepths.Items.Placeable;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Accessories
{
	public class AquaStone : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 100000;
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<TheDepthsPlayer>().aStone = true;
		}
	}
}