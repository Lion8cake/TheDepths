using TheDepths.Tiles;
using TheDepths.Buffs;
using TheDepths.Items.Placeable;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Accessories
{
	public class CrystalSkull : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Grants immunity to Mercury Poisoning");
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 27000;
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
			Item.defense = 1;
			Item.lifeRegen = 19;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
            player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
		}

		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 30);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();
		}
	}
}