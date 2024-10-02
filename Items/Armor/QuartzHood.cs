using TheDepths.Tiles;
using TheDepths.Projectiles.GeodeSetBonus;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Items.Placeable;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class QuartzHood : ModItem
	{ 
		public override void SetStaticDefaults() {
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 20;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 4;
			Item.value = 9000;
		}
		
		public override void UpdateEquip(Player player) {
			player.whipRangeMultiplier += 0.15f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<QuartzWinterCoat>() && legs.type == ModContent.ItemType<QuartzLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.TheDepths.SetBonus.QuartzArmor");
			player.whipRangeMultiplier += 0.35f;
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.35f;
		}
	}
}