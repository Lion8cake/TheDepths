using System.Collections.Generic;
using System;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;
using TheDepths.Walls;
using Microsoft.Xna.Framework;
using TheDepths.Tiles;
using TheDepths.Tiles.Furniture;
using Terraria.ID;

namespace TheDepths.Worldgen.Generation;

internal class DepthsBuilding
{
    internal static void BuildBuilding(int x, int y)
    {
        int count = 0;
        Point size = new(WorldGen.genRand.Next(16, 20), WorldGen.genRand.Next(10, 14));

        HashSet<Point> placedBuildings = [];
        int tileType = !WorldGen.genRand.NextBool(4) ? ModContent.TileType<QuartzBricks>() : ModContent.TileType<ArqueriteBricks>();

        while (WorldGen.genRand.NextBool(Math.Max(1, count / 3)))
        {
            count++;
            BuildSingleBuilding(x, y, size, tileType);
            placedBuildings.Add(new Point(x, y));

            do // Repeat this until we do not overlap an existing building - this can also make fun T shapes occasionally
            {
                switch (WorldGen.genRand.Next(5))
                {
                    case 0:
                        x += size.X - 1;
                        break;
                    case 1:
                        x -= size.X - 1;
                        break;
                    default:
                        y += size.Y - 1;
                        break;
                }
            } while (placedBuildings.Contains(new Point(x, y)));

            if (y >= Main.maxTilesY - 60)
                break;
        }

        foreach (var item in placedBuildings)
        {
            Point center = new(item.X + size.X / 2, item.Y + size.Y / 2);

            CheckAddPlatforms(center, -1, size.X, size.Y / 2);
            CheckAddPlatforms(center, 1, size.X, size.Y / 2);
            CheckAddDoors(new Point(item.X, item.Y + size.Y - 2), -1);
            CheckAddDoors(new Point(item.X + size.X - 1, item.Y + size.Y - 2), 1);
        }
    }

    private static void CheckAddDoors(Point position, int dir)
    {
        if (WorldGen.SolidTile(position.X + dir, position.Y))
            return;

        for (int i = 0; i < 3; ++i)
            Main.tile[position.X, position.Y - i].ClearTile();

        if (WorldGen.genRand.NextBool(6)) // Sometimes, just have a hole in the wall
            return;
        
        WorldGen.PlaceTile(position.X, position.Y - 2, ModContent.TileType<QuartzDoorClosed>(), true);
    }

    private static void CheckAddPlatforms(Point center, int dir, int width, int height)
    {
        int y = center.Y + (height * dir);
        Tile above = Main.tile[center.X, y + dir];

        if (above.WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || above.WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>() || !above.HasTile)
        {
            int useWidth = Math.Max(WorldGen.genRand.Next((int)(width / 3f)), 2);
            int off = WorldGen.genRand.Next(-2, 3);
            bool makeHole = WorldGen.genRand.NextBool(8); // Sometimes, just have a hole in the wall

            for (int i = center.X - useWidth - off;  i < center.X + useWidth - off; ++i)
            {
                Tile tile = Main.tile[i, y];

                if (!makeHole)
                {
                    if (!WorldGen.genRand.NextBool(12)) // Platforms have a chance to fail to give a more weathered look
                        WorldGen.PlaceTile(i, y - dir, ModContent.TileType<QuartzPlatform>(), true);
                }
                else
                    tile.ClearTile();

                tile.WallType = Main.tile[center.X, y - dir].WallType;
            }
        }
    }

    private static void BuildSingleBuilding(int x, int y, Point size, int tileType)
    {
        ShapeData shapeData = new();

        WorldUtils.Gen(new Point(x, y), new Shapes.Rectangle(size.X, size.Y), Actions.Chain( // Clear & add walls
            new Actions.Clear().Output(shapeData),
            new Actions.PlaceWall((ushort)(tileType == ModContent.TileType<ArqueriteBricks>() ? ModContent.WallType<ArqueriteBrickWallUnsafe>() : ModContent.WallType<QuartzBrickWallUnsafe>()))
        ));

        WorldUtils.Gen(new Point(x, y), new ModShapes.InnerOutline(shapeData, true), Actions.Chain( // Add tile walls & clear walls on border
            new Actions.ClearWall(),
            new Actions.SetTile((ushort)tileType, true)
        ));
    }
}
