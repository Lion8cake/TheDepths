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
	public class ShadowflameEmberedTreads : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.vanity = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!hideVisual)
			{
				player.GetModPlayer<TheDepthsPlayer>().sEmbers = true;
			}
		}

		public override void UpdateVanity(Player player)
        {
			player.GetModPlayer<TheDepthsPlayer>().sEmbers = true;
		}
    }
}