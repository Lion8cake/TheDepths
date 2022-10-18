using TheDepths.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class MercuryChestplate : ModItem
	{
		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			DisplayName.SetDefault("Mercury Chestplate");
			Tooltip.SetDefault("8% Increased Melee Damage");
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 8;
			Item.value = 30000;
		}
		
		public override void UpdateEquip(Player player) {
		player.GetDamage(DamageClass.Melee) += 0.08f;
		}

		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}