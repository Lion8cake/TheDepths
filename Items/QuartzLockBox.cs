using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Weapons;

namespace TheDepths.Items
{
    public class QuartzLockBox : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.width = 36;
            Item.height = 26;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 40);
        }

        public override bool CanRightClick()
        {
            if (Main.LocalPlayer.HasItem(ItemID.ShadowKey))
            {
                return true;
            }
            return false;
        }

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
            IItemDropRule skyfallRemixseed = ItemDropRule.ByCondition(new Conditions.NotRemixSeed(), ModContent.ItemType<Skyfall>());
            skyfallRemixseed.OnFailedConditions(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<BlueSphere>()), hideLootReport: true);

            IItemDropRule[] quartzLockBoxList = new IItemDropRule[]
            {
            ItemDropRule.NotScalingWithLuck(ItemID.DarkLance),
            ItemDropRule.NotScalingWithLuck(ModContent.ItemType<SilverStar>()),
            skyfallRemixseed,
            ItemDropRule.NotScalingWithLuck(ModContent.ItemType<WhiteLightning>()),
            ItemDropRule.NotScalingWithLuck(ModContent.ItemType<NightFury>()),
            };

            itemLoot.Add(new OneFromRulesRule(1, quartzLockBoxList));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Accessories.LodeStone>(), 5));
        }
    }
}