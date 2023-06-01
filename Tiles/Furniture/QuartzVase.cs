using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;

namespace TheDepths.Tiles.Furniture
{
    public class QuartzVase : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleWrapLimit = 55;
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(165);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(255, 255, 255), Language.GetText("MapObject.Vase"));
            DustType = ModContent.DustType<Dusts.QuartzCrystals>();
            TileID.Sets.DisableSmartCursor[Type] = true;
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Placeable.Furniture.QuartzVase>());
        }
    }
}