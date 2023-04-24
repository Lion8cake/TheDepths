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
            var DepthsItems1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.DarkLance, 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<SilverStar>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Skyfall>(), 1),
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<WhiteLightning>(), 1),
                //ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Skyfall>(), 1), //Alt Hellwing bow is yet to be made
                ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Accessories.LodeStone>(), 1)
            };

            IItemDropRule[] hell = new IItemDropRule[] {
                new OneFromRulesRule(1, DepthsItems1),
            };
            itemLoot.Add(ItemDropRule.AlwaysAtleastOneSuccess(hell));
        }
    }
}