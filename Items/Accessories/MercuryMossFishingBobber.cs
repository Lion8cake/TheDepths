using TheDepths.Tiles;
using TheDepths.Buffs;
using TheDepths.Items.Placeable;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Accessories
{
	public class MercuryMossFishingBobber : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!hideVisual)
			{
				player.overrideFishingBobber = ModContent.ProjectileType<Projectiles.MercuryMossFishingBobber>();
				player.accFishingBobber = true;
			}
		}

		public override void UpdateVanity(Player player)
        {
			player.overrideFishingBobber = ModContent.ProjectileType<Projectiles.MercuryMossFishingBobber>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.MercuryMoss>(), 5);
			recipe.AddIngredient(ItemID.FishingBobberGlowingStar, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}