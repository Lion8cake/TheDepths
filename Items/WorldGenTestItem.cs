using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Worldgen.Generation;

namespace TheDepths.Items
{
    public class WorldGenTestItem : ModItem
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
            Item.consumable = false;
            Item.width = 20;
            Item.height = 20;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool? UseItem(Player player)
        {
            DepthsGen.AddBaseTiles((int)Main.MouseWorld.X / 16, Main.maxTilesY - 200, 800, 198);

            for (int i = 0; i < 2; ++i)
            {
                DepthsGen.ClearTunnel((int)Main.MouseWorld.X / 16, Main.maxTilesY - 160, 800, 80, 0.3f);
                DepthsGen.ClearTunnel((int)Main.MouseWorld.X / 16, Main.maxTilesY - 80, 800, 80, 0.05f);
            }
            return true;
        }
    }
}
