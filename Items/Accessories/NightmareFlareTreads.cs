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
	public class NightmareFlareTreads : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 36;
			Item.height = 30;
			Item.value = 10000;
			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
			Item.vanity = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.accRunSpeed = 6f;
			player.rocketBoots = 3;
			if (!hideVisual)
            {
				player.GetModPlayer<TheDepthsPlayer>().sEmbers = true;
				player.GetModPlayer<TheDepthsPlayer>().nFlare = true;
			}
		}

        public override void UpdateVanity(Player player)
        {
			player.GetModPlayer<TheDepthsPlayer>().sEmbers = true;
			player.GetModPlayer<TheDepthsPlayer>().nFlare = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpectreBoots, 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Accessories.ShadowflameEmberedTreads>(), 1);
			recipe.AddTile(114);
			recipe.Register();
		}
	}
}