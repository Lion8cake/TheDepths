using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Buffs;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class NightwoodHelmet : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 22;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			if (Main.remixWorld)
			{
				Item.defense = 1;
			}
			else
			{
				Item.defense = 2;
			}
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<NightwoodBreastplate>() && legs.type == ModContent.ItemType<NightwoodGreaves>();
		}

		public override void UpdateArmorSet(Player player) {
			player.setBonus = Language.GetTextValue("Mods.TheDepths.SetBonus.NightwoodHelmet");
			player.GetModPlayer<TheDepthsPlayer>().NightwoodBuff = true;
		}

		public override void UpdateInventory(Player player)
		{
			if (Main.remixWorld)
			{
				Item.defense = 1;
			}
			else
			{
				Item.defense = 2;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.NightWood>(), 20);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}