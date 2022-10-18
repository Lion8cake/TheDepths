using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Items.Placeable;

namespace TheDepths.Items
{
    class LargeOnyx : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Large Onyx");
            Tooltip.SetDefault("For Capture the Gem. It drops when you die");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LargeAmber);
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateInventory(Player player) => player.GetModPlayer<TheDepthsPlayer>().largeGems[0] = true;


        public override bool PreDrawInWorld(
          SpriteBatch spriteBatch,
          Color lightColor,
          Color alphaColor,
          ref float rotation,
          ref float scale,
          int whoAmI)
        {
            Texture2D texture2D = ModContent.Request<Texture2D>("TheDepths/Items/LargeOnyx").Value;
            Vector2 vector2_1 = Vector2.Divide(Utils.Size(texture2D), 2f);
            Vector2 vector2_2 = new Vector2(Item.width / 2 - texture2D.Width / 2, Item.height - texture2D.Height + 2f);
            Vector2 vector2_3 = Vector2.Add(Vector2.Add(Vector2.Subtract(Item.position, Main.screenPosition), vector2_1), vector2_2);
            spriteBatch.Draw(texture2D, vector2_3, new Rectangle?(), new Color(250, 250, 250, Main.mouseTextColor / 2), rotation, vector2_1, (float)(Main.mouseTextColor / 1000.0 + 0.800000011920929), 0, 0.0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Onyx>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
