using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Accessories;

namespace TheDepths.Items.Placeable
{
    public class ArqueriteCrate : ModItem
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsFishingCrate[Type] = true;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<QuartzCrate>();
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 34;
            Item.height = 34;
            Item.rare = ItemRarityID.Green;
            Item.createTile = ModContent.TileType<Tiles.ArqueriteCrate>();
            Item.placeStyle = 0;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.value = Item.sellPrice(gold: 1);
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override bool CanRightClick()
        {
            return true;
        }

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			IItemDropRule[] bc_quicksilver = new IItemDropRule[2]
			{
			ItemDropRule.NotScalingWithLuck(ModContent.ItemType<AmalgamAmulet>(), 20),
			ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<ShadowflameEmberedTreads>(), ModContent.ItemType<PurpleflameNecklace>(), ModContent.ItemType<ShadowFightingFishBowl>(), ModContent.ItemType<QuicksilverproofFishingHook>(), ModContent.ItemType<FluorescentLightBulb>())
			};
			IItemDropRule bc_pot = ItemDropRule.NotScalingWithLuck(ItemID.PotSuspended, 4, 2, 2);
			IItemDropRule bc_quar = ItemDropRule.Common(ModContent.ItemType<QuartzLockBox>());
			IItemDropRule bc_wet = ItemDropRule.NotScalingWithLuck(ItemID.WetBomb, 3, 7, 10);
			IItemDropRule bc_plant = ItemDropRule.OneFromOptionsNotScalingWithLuck(2, ModContent.ItemType<PottedPurplestarBrambles>(), ModContent.ItemType<PottedShadowflameBulb>(), ModContent.ItemType<PottedNightterrorBush>(), ModContent.ItemType<PottedNightmarePalm>(), ModContent.ItemType<PottedMercuryTendrils>());
			IItemDropRule bc_ornate = ItemDropRule.NotScalingWithLuck(ItemID.OrnateShadowKey, 20);
			IItemDropRule bc_geocart = ItemDropRule.NotScalingWithLuck(ModContent.ItemType<PhantomFirecart>(), 20);
			IItemDropRule bc_pointer = ItemDropRule.NotScalingWithLuck(ModContent.ItemType<GeodeLazerPointer>(), 20);
			IItemDropRule bc_goldCoin = ItemDropRule.NotScalingWithLuck(ItemID.GoldCoin, 4, 5, 12);
			IItemDropRule[] ores = new IItemDropRule[8]
			{
			ItemDropRule.NotScalingWithLuck(12, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(699, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(11, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(700, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(14, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(701, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(13, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(702, 1, 20, 35)
			};
			IItemDropRule[] hardmodeOres = new IItemDropRule[6]
			{
			ItemDropRule.NotScalingWithLuck(364, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(1104, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(365, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(1105, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(366, 1, 20, 35),
			ItemDropRule.NotScalingWithLuck(1106, 1, 20, 35)
			};
			IItemDropRule[] bars = new IItemDropRule[6]
			{
			ItemDropRule.NotScalingWithLuck(22, 1, 6, 16),
			ItemDropRule.NotScalingWithLuck(704, 1, 6, 16),
			ItemDropRule.NotScalingWithLuck(21, 1, 6, 16),
			ItemDropRule.NotScalingWithLuck(705, 1, 6, 16),
			ItemDropRule.NotScalingWithLuck(19, 1, 6, 16),
			ItemDropRule.NotScalingWithLuck(706, 1, 6, 16)
			};
			IItemDropRule[] hardmodeBars = new IItemDropRule[6]
			{
			ItemDropRule.NotScalingWithLuck(381, 1, 5, 16),
			ItemDropRule.NotScalingWithLuck(1184, 1, 5, 16),
			ItemDropRule.NotScalingWithLuck(382, 1, 5, 16),
			ItemDropRule.NotScalingWithLuck(1191, 1, 5, 16),
			ItemDropRule.NotScalingWithLuck(391, 1, 5, 16),
			ItemDropRule.NotScalingWithLuck(1198, 1, 5, 16)
			};
			IItemDropRule[] potions = new IItemDropRule[6]
			{
			ItemDropRule.NotScalingWithLuck(ModContent.ItemType<CrystalSkinPotion>(), 1, 2, 4),
			ItemDropRule.NotScalingWithLuck(296, 1, 2, 4),
			ItemDropRule.NotScalingWithLuck(304, 1, 2, 4),
			ItemDropRule.NotScalingWithLuck(305, 1, 2, 4),
			ItemDropRule.NotScalingWithLuck(2322, 1, 2, 4),
			ItemDropRule.NotScalingWithLuck(2323, 1, 2, 4)
			};
			IItemDropRule[] extraPotions = new IItemDropRule[2]
			{
			ItemDropRule.NotScalingWithLuck(188, 1, 5, 17),
			ItemDropRule.NotScalingWithLuck(189, 1, 5, 17)
			};
			IItemDropRule[] extraBait = new IItemDropRule[2]
			{
			ItemDropRule.NotScalingWithLuck(2676, 3, 2, 6),
			ItemDropRule.NotScalingWithLuck(2675, 1, 2, 6)
			};
			IItemDropRule hardmodeBiomeCrateOres = ItemDropRule.SequentialRulesNotScalingWithLuck(7, new OneFromRulesRule(2, hardmodeOres), new OneFromRulesRule(1, ores));
			IItemDropRule hardmodeBiomeCrateBars = ItemDropRule.SequentialRulesNotScalingWithLuck(4, new OneFromRulesRule(3, 2, hardmodeBars), new OneFromRulesRule(1, bars));
			IItemDropRule[] arquerite = new IItemDropRule[12]
			{
				ItemDropRule.SequentialRulesNotScalingWithLuck(1, bc_quicksilver),
				bc_pot,
				bc_quar,
				bc_wet,
				bc_plant,
				bc_geocart,
				bc_goldCoin,
				hardmodeBiomeCrateOres,
				hardmodeBiomeCrateBars,
				new OneFromRulesRule(3, potions),
				bc_ornate,
				bc_pointer
			};
			itemLoot.Add(ItemDropRule.AlwaysAtleastOneSuccess(arquerite));
			itemLoot.Add(new OneFromRulesRule(2, extraPotions));
			itemLoot.Add(ItemDropRule.SequentialRulesNotScalingWithLuck(2, extraBait));
		}
	}
}