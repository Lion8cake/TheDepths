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
	[AutoloadEquip(new EquipType[] { EquipType.Neck })]
	public class AmalgamAmulet : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AquaStone>();
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 300000;
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<TheDepthsPlayer>().aAmulet = true;
		}
	}
}