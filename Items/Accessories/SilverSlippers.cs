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
	[AutoloadEquip(new EquipType[] { EquipType.Shoes })]
	public class SilverSlippers : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 500000;
			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.waterWalk2 = true;
			player.fireWalk = true;
			player.waterWalk = true;
			player.GetModPlayer<TheDepthsPlayer>().aAmulet2 = true;
			player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
			player.GetModPlayer<TheDepthsPlayer>().stoneRose = true;
		}
	}
}