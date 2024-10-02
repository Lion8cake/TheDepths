using TheDepths.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Placeable;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class QuartzWinterCoat : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 20;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 5;
			Item.value = 9000;
		}
		
		public override void UpdateEquip(Player player) {
			player.maxMinions++;
		}
	}
}