using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class CrystalCrown : ModItem
	{
		public override void SetStaticDefaults() {
		Tooltip.SetDefault("20% Increased Magic Critical Strike Chance");
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 6;
			Item.value = 100000;
		}
		
		public override void UpdateEquip(Player player) {
		player.GetCritChance(DamageClass.Magic) += 20;
		}
	}
}