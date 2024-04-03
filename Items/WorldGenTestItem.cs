using Microsoft.Xna.Framework;
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
            Item.useTime = 15;
            Item.maxStack = 9999;
            Item.consumable = false;
            Item.width = 20;
            Item.height = 20;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool? UseItem(Player player)
        {
            Point p = Main.MouseWorld.ToTileCoordinates();
            DepthsBuilding.BuildBuilding(p.X, p.Y);
            //DepthsGen.Generate(new(), new(null));
            return true;
        }
    }
}
