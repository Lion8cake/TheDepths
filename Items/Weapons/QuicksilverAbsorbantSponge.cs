using ModLiquidLib.ID;
using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Liquids;

namespace TheDepths.Items.Weapons
{
	public class QuicksilverAbsorbantSponge : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.AlsoABuildingItem[Type] = true; //Unused, but useful to have here for both other mods and future game updates
			ItemID.Sets.DuplicationMenuToolsFilter[Type] = true;

			//Unlike buckets, sponges have extra functionality to allow the removing and adding of sponge items to liquids
			LiquidID_TLmod.Sets.CanBeAbsorbedBy[LiquidLoader.LiquidType<Quicksilver>()].Add(Type);

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 12;
			Item.useTime = 5;
			Item.width = 20;
			Item.height = 20;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 10);
			Item.tileBoost += 2;
		}
	}
}

