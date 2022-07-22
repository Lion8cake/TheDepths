using TheDepths.Tiles;
using TheDepths.Buffs;
using TheDepths.Items.Placeable;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Accessories
{
[AutoloadEquip(new EquipType[] { EquipType.Shoes })]
	public class CrystalWaterWalkingBoots : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Provides the ability to walk on water\nGrants immunity to Mercury Poisoning");
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 500000;
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
            player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
			player.waterWalk = true;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.WaterWalkingBoots, 1).AddIngredient(ModContent.ItemType<Items.Accessories.CrystalSkull>(), 1).AddTile(114).Register();
		}
	}
}