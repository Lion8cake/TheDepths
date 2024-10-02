using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Buffs;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class PetrifiedWoodHelmet : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 22;
			Item.defense = 2;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<PetrifiedWoodBreastplate>() && legs.type == ModContent.ItemType<PetrifiedWoodGreaves>();
		}

		public override void UpdateArmorSet(Player player) {
			player.setBonus = Language.GetTextValue("Mods.TheDepths.SetBonus.PetrifiedWoodHelmet");
			player.endurance = 0.1f;
		}
	}
}