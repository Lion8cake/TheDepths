using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Placeable
{
	public class BlackTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults() {
			Item.width = 10;
			Item.height = 12;
			Item.maxStack = 99;
			Item.holdStyle = 1;
			Item.noWet = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.rare = ItemRarityID.White;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.BlackTorch>();
			Item.flame = true;
			Item.value = 200;
		}

		public override void HoldItem(Player player) {
			if (Main.rand.Next(player.itemAnimation > 0 ? 40 : 80) == 0) {
				Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, ModContent.DustType<BlackGemsparkDust>());
			}
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, 2f, 1f, 1f);
		}

		public override void PostUpdate() {
			if (!Item.wet) {
				Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 1f, 1f, 1f);
			}
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick) {
			dryTorch = true;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(10);
			recipe.AddIngredient(ItemID.Torch, 10);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Onyx>(), 1);
			recipe.Register();
		}
	}
}