using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//using TheDepths.Items.Placeable;

namespace TheDepths.Items.Weapons
{
	public class DiamondArrow : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("3 bursts of crystals are shot out in random directions apon impact");
		}

		public override void SetDefaults() {
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true; 
			Item.knockBack = 1.5f;
			Item.value = 80;
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<Projectiles.DiamondArrow>();
			Item.shootSpeed = 7f;
			Item.ammo = AmmoID.Arrow;
		}

		/*public override void AddRecipes() {
			CreateRecipe(33).AddIngredient(40, 33).AddIngredient(ModContent.ItemType<DiamondDust>(), 1).AddTile(TileID.Anvils).Register();
		}*/
	}
}
