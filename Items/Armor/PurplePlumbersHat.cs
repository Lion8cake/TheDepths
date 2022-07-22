using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class PurplePlumbersHat : ModItem
	{ 
	    public override void SetStaticDefaults() {
			DisplayName.SetDefault("Purple Plumber's Hat");
		}
		
		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.White;
			Item.vanity = true;
			Item.value = 10000;
		}
	}
}