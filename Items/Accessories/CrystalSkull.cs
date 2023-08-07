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
	[AutoloadEquip(new EquipType[] { EquipType.Face })]
	public class CrystalSkull : ModItem
	{
		public override void SetStaticDefaults() {
            ArmorIDs.Face.Sets.DrawInFaceHeadLayer[Item.faceSlot] = true;
			Tooltip.SetDefault("Grants immunity to Mercury Radiation");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 27000;
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
			Item.defense = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
            player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
		}

		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Quartz>(), 20);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();
		}
	}
}