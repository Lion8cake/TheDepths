using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class NightwoodGreaves : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 18;
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