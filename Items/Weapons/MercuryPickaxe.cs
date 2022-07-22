using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Items.Weapons
{
	public class MercuryPickaxe : ModItem
	{
		public override void SetStaticDefaults() {
		}

		public override void SetDefaults() {
			Item.damage = 15;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 5;
			Item.useAnimation = 10;
			Item.pick = 100;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(silver: 54);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}
		
		public override void MeleeEffects(Player player, Rectangle hitbox) {
			if (Main.rand.NextBool(10)) {
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<MercurySparkleDust>());
			}
		}
		
		/*public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<ArqueriteBar>(), 20).AddTile(TileID.Anvils).Register();
		}*/
	}
}