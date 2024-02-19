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
		
		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 20);
			recipe.AddIngredient(ItemID.TissueSample, 10);
			recipe.AddTile(TileID.Hellforge);
			recipe.Register();
			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.Silk, 10);
			recipe2.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 20);
			recipe2.AddIngredient(ItemID.ShadowScale, 10);
			recipe2.AddTile(TileID.Hellforge);
			recipe2.Register();
		}
	}
}