using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Weapons
{
	public class BlackPhaseblade : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.knockBack = 3f;
			Item.width = 40;
			Item.height = 40;
			Item.damage = 26;
			Item.scale = 1f;
			Item.UseSound = SoundID.Item15;
			Item.rare = ItemRarityID.Blue;
			Item.value = 27000;
			Item.DamageType = DamageClass.Melee;
		}
	
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			Lighting.AddLight(player.itemLocation + new Vector2(6f + player.velocity.X, 14f), 0.058f, 0.061f, 0.06f);
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MeteoriteBar, 15);
			recipe.AddIngredient(ModContent.ItemType<Placeable.Onyx>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SortAfterFirstRecipesOf(ItemID.OrangePhaseblade);
			recipe.Register();
		}
	}
}
