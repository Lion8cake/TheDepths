﻿using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Tiles;

namespace TheDepths.Items.Placeable
{
    public class DONOTDRINK : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.DarkSoulReaper);
            Item.placeStyle = 0;
            Item.createTile = ModContent.TileType<Tiles.DONOTDRINK>();
        }
    }
}
