using TheDepths.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Placeable;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class GeodeChestplate : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.White;
			Item.defense = 6;
			Item.value = 4500;
		}
		
		public override void UpdateEquip(Player player) {
			player.maxMinions++;
		}
		
		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 3);
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			Recipe recipe2= CreateRecipe();
			recipe2.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 3);
			recipe2.AddIngredient(ItemID.ShadowScale, 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}