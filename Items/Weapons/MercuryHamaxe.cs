using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Weapons
{
	public class MercuryHamaxe : ModItem
	{
		public override void SetStaticDefaults() {
		}

		public override void SetDefaults() {
			Item.damage = 23;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.axe = 30;
			Item.hammer = 65;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 15000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		/*public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 20).AddTile(TileID.Anvils).Register();
		}*/

		public override void MeleeEffects(Player player, Rectangle hitbox) {
			if (Main.rand.NextBool(10)) {
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<MercurySparkleDust>());
			}
		}
	}
}
