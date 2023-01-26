using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Accessories
{
	public class QuickSilverSurfboard : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Allows the owner to float for a few seconds");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 20;
			Item.value = 50000;
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.carpet = true;
			player.GetModPlayer<TheDepthsPlayer>().quicksilverSurfboard = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.DiamondDust>(), 20);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 20);
			recipe.AddIngredient(ItemID.SoulofFlight, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}