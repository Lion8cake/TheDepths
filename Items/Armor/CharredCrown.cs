using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class CharredCrown : ModItem
	{
		public override void SetStaticDefaults() {
		Tooltip.SetDefault("18% Increased Magic Damage");
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 8;
			Item.value = 100000;
		}
		
		public override void UpdateEquip(Player player) {
		player.GetDamage(DamageClass.Magic) += 0.18f;
		}
	}
}