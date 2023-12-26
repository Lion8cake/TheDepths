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
	[AutoloadEquip(new EquipType[] { EquipType.Shoes })]
	public class SilverSlippers : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 500000;
			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.waterWalk2 = true;
			player.fireWalk = true;
			player.waterWalk = true;
			player.GetModPlayer<TheDepthsPlayer>().aAmulet2 = true;
			player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
			player.GetModPlayer<TheDepthsPlayer>().stoneRose = true;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<StoneRose>());
			recipe.AddIngredient(ModContent.ItemType<AmalgamAmulet>());
			recipe.AddIngredient(ModContent.ItemType<CrystalWaterWalkingBoots>());
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ModContent.ItemType<StoneRose>());
			recipe2.AddIngredient(ModContent.ItemType<SilverCharm>());
			recipe2.AddIngredient(ModContent.ItemType<CrystalWaterWalkingBoots>());
			recipe2.AddTile(TileID.TinkerersWorkbench);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ModContent.ItemType<StoneRose>());
			recipe3.AddIngredient(ModContent.ItemType<SilverCharm>());
			recipe3.AddIngredient(ItemID.WaterWalkingBoots);
			recipe3.AddTile(TileID.TinkerersWorkbench);
			recipe3.Register();
		}
	}
}