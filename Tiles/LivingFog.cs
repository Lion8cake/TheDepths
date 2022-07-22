using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace TheDepths.Tiles
{
    public class LivingFog : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            ItemDrop = ItemType<Items.Placeable.LivingFog>();
            AddMapEntry(new Color(185, 197, 200), (LocalizedText)null);
            AnimationFrameHeight = 90;
            Main.tileSolid[Type] = false;
            Main.tileNoAttach[Type] = false;
            Main.tileFrameImportant[Type] = false;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CoordinateHeights = new int[1]
            {
                16
            };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float BASE = 1 / 255;
            r = BASE * 185;
            g = BASE * 197;
            b = BASE * 200;
        }

        public int CanPlaceAlter(int i, int j, int type, int style, int direction)
        {
            return 1;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[336];
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
    }
}
