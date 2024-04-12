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
	[AutoloadEquip(EquipType.Neck)]
	public class HungryLeash : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.expert = true;
			Item.rare = -12;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			double playerlife = player.statLife;
			double playermaxlife = player.statLifeMax2;
			double playerLifePercentage = playerlife / playermaxlife * 100;
			if (playerLifePercentage <= 25)
			{
				player.moveSpeed += 0.75f;
			}
			else if (playerLifePercentage <= 50)
			{
				player.moveSpeed += 0.5f;
			}
			else if (playerLifePercentage <= 75)
			{
				player.moveSpeed += 0.25f;
			}
		}
	}
}