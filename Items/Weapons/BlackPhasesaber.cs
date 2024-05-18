using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Weapons
{
	public class BlackPhasesaber : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3f;
			Item.width = 40;
			Item.height = 40;
			Item.UseSound = SoundID.Item15;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 48;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.scale = 1f;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 1);
		}
		
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			Lighting.AddLight(player.itemLocation + new Vector2(6f + player.velocity.X, 14f), 0.058f, 0.061f, 0.06f);
		}
	    
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalShard, 25);
			recipe.AddIngredient(ModContent.ItemType<BlackPhaseblade>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SortAfterFirstRecipesOf(ItemID.OrangePhasesaber);
			recipe.Register();
		}
	}
}
