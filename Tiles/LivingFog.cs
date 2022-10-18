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
            HitSound = SoundID.Dig;
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
            //TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(new Func<int, int, int, int, int, int>(CanPlaceAlter), -1, 0, true);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            //TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(AfterPlacement),
                //-1, 0, false);
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

        public static int AfterPlacement(int i, int j, int type, int style, int direction)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 1, 1, TileChangeType.None);
            return 1;
        }

        public override bool CanPlace(int i, int j)
        {
            List<List<int>> intListList = new List<List<int>>()
            {
                new List<int>() { i, j - 1 },
                new List<int>() { i - 1, j },
                new List<int>() { i + 1, j },
                new List<int>() { i, j + 1 }
            };
            if (Main.tile[i, j].WallType != 0)
                return true;
            for (int index = 0; index < intListList.Count; ++index)
            {
                Tile tile = Main.tile[intListList[index][0], intListList[index][1]];
                if (tile.HasTile && (Main.tileSolid[tile.TileType] || TheDepths.livingFireBlockList.Contains(tile.TileType)))
                    return true;
            }
            return false;
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
