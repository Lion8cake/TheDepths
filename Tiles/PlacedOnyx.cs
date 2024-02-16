using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.Enums;

namespace TheDepths.Tiles
{
    public class PlacedOnyx : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 500;
            Main.tileSpelunker[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(57, 57, 64), name);
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.Onyx>());
            DustType = ModContent.DustType<Dusts.GemOnyxDust>();
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            var tile = Framing.GetTileSafely(i, j);
            var topTile = Framing.GetTileSafely(i, j - 1);
            var bottomTile = Framing.GetTileSafely(i, j + 1);
            var leftTile = Framing.GetTileSafely(i - 1, j);
            var rightTile = Framing.GetTileSafely(i + 1, j);
            var topType = -1;
            var bottomType = -1;
            var leftType = -1;
            var rightType = -1;
            if (tile.TileFrameX >= 18)
			{
                if (WorldGen.SolidTile(i - 1, j) || WorldGen.SolidTile(i + 1, j) || WorldGen.SolidTile(i, j - 1) || WorldGen.SolidTile(i, j + 1))
                {
                    tile.TileFrameX = 0;
                }
                else
				{
                    tile.HasTile = false;
				}
			}
			if (topTile.HasTile && !topTile.BottomSlope)
                bottomType = topTile.TileType;
            if (bottomTile.HasTile && !bottomTile.IsHalfBlock && !bottomTile.TopSlope)
                topType = bottomTile.TileType;
            if (leftTile.HasTile)
                leftType = leftTile.TileType;
            if (rightTile.HasTile)
                rightType = rightTile.TileType;
            var variation = WorldGen.genRand.Next(3) * 18;
            if (topType >= 0 && Main.tileSolid[topType] && !Main.tileSolidTop[topType])
            {
                if (tile.TileFrameY > 0 || tile.TileFrameY < 36)
                    tile.TileFrameY = (short)variation;
            }
            else if (leftType >= 0 && Main.tileSolid[leftType] && !Main.tileSolidTop[leftType])
            {
                if (tile.TileFrameY < 108 || tile.TileFrameY > 54)
                    tile.TileFrameY = (short)(108 + variation);
            }
            else if (rightType >= 0 && Main.tileSolid[rightType] && !Main.tileSolidTop[rightType])
            {
                if (tile.TileFrameY < 162 || tile.TileFrameY > 198)
                    tile.TileFrameY = (short)(162 + variation);
            }
            else if (bottomType >= 0 && Main.tileSolid[bottomType] && !Main.tileSolidTop[bottomType])
            {
                if (tile.TileFrameY < 54 || tile.TileFrameY > 90)
                    tile.TileFrameY = (short)(54 + variation);
            }
            else
                WorldGen.KillTile(i, j);
            return true;
        }

        public override bool CanPlace(int i, int j)
        {
            return WorldGen.SolidTile(i - 1, j) || WorldGen.SolidTile(i + 1, j) || WorldGen.SolidTile(i, j - 1) || WorldGen.SolidTile(i, j + 1) || Main.tile[i, j].WallType <= 0;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            if (Main.tile[i, j].TileFrameY / 18 < 3)
                offsetY = 2;
        }
    }
}
