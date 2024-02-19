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
	public class HungryLeash : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = 300000;
			Item.expert = true;
			Item.rare = -12;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.statLife < player.statLifeMax2 / 4.8)
			{
				player.moveSpeed += 0.7f;
			}
			if (player.statLife < player.statLifeMax2 / 3)
			{
				player.moveSpeed += 0.5f;
			}
			if (player.statLife < player.statLifeMax2 / 2)
			{
				player.moveSpeed += 0.3f;
			}
			if (player.statLife < player.statLifeMax2 / 1.2)
			{
				player.moveSpeed += 0.2f;
			}
		}
	}
}