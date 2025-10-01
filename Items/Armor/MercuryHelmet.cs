using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Buffs;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MercuryHelmet : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
			Item.value = 45000;
		}
		
		public override void UpdateEquip(Player player) {
			player.GetCritChance(DamageClass.Generic) += 6;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<MercuryChestplate>() && legs.type == ModContent.ItemType<MercuryGreaves>();
		}

		public override void UpdateArmorSet(Player player) {
			player.setBonus = Language.GetTextValue("Mods.TheDepths.SetBonus.MercuryHelmet");
			player.statDefense += 3;
			if (Main.player[Main.myPlayer].GetModPlayer<TheDepthsPlayer>().QuicksilverTimer <= 0)
			{
				player.buffImmune[ModContent.BuffType<MercuryBoiling>()] = true;
				player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
			}
		}
	}
}