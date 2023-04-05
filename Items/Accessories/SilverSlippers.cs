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
            player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
			player.waterWalk = true;
			player.buffImmune[ModContent.BuffType<MercuryBoiling>()] = true;
			player.fireWalk = true;
			player.GetModPlayer<TheDepthsPlayer>().stoneRose = true;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Accessories.StoneRose>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Accessories.AmalgamAmulet>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Accessories.CrystalWaterWalkingBoots>(), 1);
			recipe.AddTile(114);
			recipe.Register();
		}
	}
}