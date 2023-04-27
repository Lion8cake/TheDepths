using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheDepths.Items.Placeable
{
    public class EnchantedNightmareWormCage : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WormCage);
            Item.createTile = ModContent.TileType<Tiles.EnchantedNightmareWormCage>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ModContent.ItemType<Items.EnchantedNightmareWorm>())
                .AddIngredient(ItemID.Terrarium, 1).Register();
        }
    }
}
