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
	public class CrystalWaterWalkingBoots : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 500000;
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.waterWalk = true;
			player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.WaterWalkingBoots, 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Accessories.CrystalSkull>(), 1);
			recipe.AddTile(114);
			recipe.Register();
		}
	}
}