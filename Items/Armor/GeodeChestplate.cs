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
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 5;
			Item.value = 4500;
		}
		
		public override void UpdateEquip(Player player) {
			player.maxMinions++;
		}
		
		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}