using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheDepths.Items.Weapons
{
	public class BlackPhasesaber : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(3764);
			Item.damage = 41;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.width = 40;
			Item.height = 40;
		}
		
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			Lighting.AddLight(player.itemLocation + new Vector2(6f + player.velocity.X, 14f), 0.3f, 0.275f, 0.3f);
		}
	    
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalShard, 50);
			recipe.AddIngredient(ModContent.ItemType<Items.Weapons.BlackPhaseblade>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
