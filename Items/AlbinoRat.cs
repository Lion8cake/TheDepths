using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
    public class AlbinoRat : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsLavaBait[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 20;
            Item.height = 20;
            Item.noUseGraphic = true;
            Item.bait = 15;
            Item.rare = ItemRarityID.Blue;

            Item.makeNPC = (short)ModContent.NPCType<NPCs.AlbinoRat>();
        }
    }
}
