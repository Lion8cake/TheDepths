using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
    public class EnchantedNightmareWorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsLavaBait[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 12;
            Item.height = 12;
            Item.noUseGraphic = true;
            Item.bait = 25;
            Item.rare = ItemRarityID.Blue;

            Item.makeNPC = (short)ModContent.NPCType<NPCs.EnchantedNightmareWorm>();
        }
    }
}
