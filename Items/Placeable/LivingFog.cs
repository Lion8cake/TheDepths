using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TheDepths.Items.Placeable
{
    public class LivingFog : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Fog Block");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.LivingFog>();
        }

        public override void PostUpdate()
        {
            float BASE = 1 / 255;
            float r = BASE * 185;
            float g = BASE * 197;
            float b = BASE * 200;
            Lighting.AddLight((int)(Item.position.X + Item.width / 2 / 16), (int)(Item.position.Y + Item.height / 2 / 16), r, g, b);
        }
    }
}
