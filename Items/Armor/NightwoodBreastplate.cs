using TheDepths.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class NightwoodBreastplate : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 20;
			Item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			if (Main.remixWorld)
			{
				Item.defense = 1;
			}
			else
			{
				Item.defense = 3;
			}
		}

		public override void UpdateInventory(Player player)
		{
			if (Main.remixWorld)
			{
				Item.defense = 1;
			}
			else
			{
				Item.defense = 3;
			}
		}
	}
}