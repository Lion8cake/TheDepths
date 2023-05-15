using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

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
            var DepthsItems1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Accessories.AmalgamAmulet>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Accessories.ShadowflameEmberedTreads>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<PurpleflameNecklace>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<ShadowFightingFishBowl>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Accessories.QuicksilverproofFishingHook>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<FluorescentLightBulb>(), 1)
            };
            IItemDropRule bc_hangingPot = ItemDropRule.NotScalingWithLuck(ItemID.PotSuspended, 4, 1, 2);
            IItemDropRule bc_locketBox = ItemDropRule.NotScalingWithLuck(ModContent.ItemType<QuartzLockBox>());
            IItemDropRule bc_waterbomb = ItemDropRule.NotScalingWithLuck(ItemID.WetBomb, 3, 7, 10);
            var DepthsItems2 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<PottedPurplestarBrambles>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<PottedShadowflameBulb>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<PottedNightterrorBush>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<PottedNightmarePalm>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<PottedMercuryTendrils>(), 1)
            };
            IItemDropRule bc_goldCoin = ItemDropRule.NotScalingWithLuck(ItemID.GoldCoin, 4, 5, 12);
            var oresTier1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.CopperOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.TinOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.IronOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.LeadOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.SilverOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.TungstenOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.GoldOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.PlatinumOre, 1, 30, 49)
            };
            var hardmodeOresTier1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.CobaltOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.PalladiumOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.MythrilOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.OrichalcumOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.AdamantiteOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.TitaniumOre, 1, 30, 49)
            };
            var barsTier1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.IronBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.LeadBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.SilverBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.TungstenBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.GoldBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.PlatinumBar, 1, 10, 20)
            };
            var hardmodeBarsTier1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.CobaltBar, 1, 8, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.PalladiumBar, 1, 8, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.MythrilBar, 1, 8, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.OrichalcumBar, 1, 8, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.AdamantiteBar, 1, 8, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.TitaniumBar, 1, 8, 20)
            };
            var potions = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<CrystalSkinPotion>(), 1, 2, 4),
                ItemDropRule.NotScalingWithLuck(ItemID.SpelunkerPotion, 1, 2, 4),
                ItemDropRule.NotScalingWithLuck(ItemID.HunterPotion, 1, 2, 4),
                ItemDropRule.NotScalingWithLuck(ItemID.GravitationPotion, 1, 2, 4),
                ItemDropRule.NotScalingWithLuck(ItemID.MiningPotion, 1, 2, 4),
                ItemDropRule.NotScalingWithLuck(ItemID.HeartreachPotion, 1, 2, 4)
            };
            var extraPotions = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.HealingPotion, 1, 5, 17),
                ItemDropRule.NotScalingWithLuck(ItemID.ManaPotion, 1, 5, 17)
            };
            var extraBait = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.MasterBait, 1, 2, 6),
                ItemDropRule.NotScalingWithLuck(ItemID.JourneymanBait, 1, 2, 6)
            };
            IItemDropRule bc_pet1 = ItemDropRule.NotScalingWithLuck(ItemID.HellCake, 20);
            IItemDropRule bc_pet2 = ItemDropRule.NotScalingWithLuck(ItemID.WetBomb, 20);

            var oresList = new List<IItemDropRule>();
            var barsList = new List<IItemDropRule>();
            oresList.AddRange(oresTier1);
            oresList.AddRange(hardmodeOresTier1);
            barsList.AddRange(barsTier1);
            barsList.AddRange(hardmodeBarsTier1);

            IItemDropRule[] hell = new IItemDropRule[] {
                bc_goldCoin,
                ItemDropRule.SequentialRulesNotScalingWithLuck(1, new OneFromRulesRule(5, oresList.ToArray()), new OneFromRulesRule(3, 2, barsList.ToArray())),
                new OneFromRulesRule(3, potions),
                bc_waterbomb,
                bc_locketBox,
                bc_hangingPot,
                new OneFromRulesRule(1, DepthsItems1),
                bc_pet1,
                bc_pet2,
            };
            
            itemLoot.Add(ItemDropRule.AlwaysAtleastOneSuccess(hell));
            itemLoot.Add(new OneFromRulesRule(2, extraPotions));
            itemLoot.Add(new OneFromRulesRule(2, DepthsItems2));
            itemLoot.Add(ItemDropRule.SequentialRulesNotScalingWithLuck(2, extraBait));
        }
    }
}