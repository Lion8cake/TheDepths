using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace TheDepths.Tiles
{
    public class PlacedGems : ModTile
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
        }

        public override ushort GetMapOption(int i, int j)
        {
            return (ushort)(Main.tile[i, j].TileFrameX / 18);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            int toDrop = 0;
            switch (Main.tile[i, j].TileFrameX / 18)
            {
                case 0:
                    toDrop = ModContent.ItemType<Items.Placeable.Geode>();
					DustType = Mod.Find<ModDust>("GeodeDust").Type;
                    break;
                case 1:
                    toDrop = ModContent.ItemType<Items.Placeable.Onyx>();
					DustType = Mod.Find<ModDust>("BlackGemsparkDust").Type;
                    break;
            }
            if (toDrop > 0) Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, toDrop);
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
                if (tile.TileFrameY < 0 || tile.TileFrameY > 36)
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
            return WorldGen.SolidTile(i - 1, j) || WorldGen.SolidTile(i + 1, j) || WorldGen.SolidTile(i, j - 1) || WorldGen.SolidTile(i, j + 1);
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            if (Main.tile[i, j].TileFrameY / 18 < 3)
                offsetY = 2;
        }
    }
}