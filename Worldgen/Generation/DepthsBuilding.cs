using System.Collections.Generic;
using System;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;
using TheDepths.Walls;
using Microsoft.Xna.Framework;
using TheDepths.Tiles;
using TheDepths.Tiles.Furniture;
using Terraria.DataStructures;
using Terraria.ID;

namespace TheDepths.Worldgen.Generation;

/// <summary>
/// Contains everything needed to build a Depths house/building.
/// </summary>
internal class DepthsBuilding
{
    /// <summary>
    /// All floor furnitures.
    /// </summary>
    private static int[] Furnitures => [ModContent.TileType<QuartzDresser>(), ModContent.TileType<QuartzTable>(), ModContent.TileType<QuartzChair>(), ModContent.TileType<QuartzBookcase>(),
        ModContent.TileType<QuartzLamp>()];

    /// <summary>
    /// Builds a single building - that is, set of connected rooms.
    /// </summary>
    /// <param name="x">X position (left of first room).</param>
    /// <param name="y">Y position (top of first room).</param>
    internal static void BuildBuilding(int x, int y)
    {
        int count = 0;
        Point size = new(WorldGen.genRand.Next(16, 20), WorldGen.genRand.Next(10, 14)); // Slightly randomize size

        if (WorldGen.drunkWorldGen) // Increase width in drunk worlds
            size.X = WorldGen.genRand.Next(32, 40);

        HashSet<Point> placedBuildings = [];
        HashSet<Point16> allTiles = [];
        int tileType = !WorldGen.genRand.NextBool(4) ? ModContent.TileType<QuartzBricks>() : ModContent.TileType<ArqueriteBricks>(); // Randomize base tile type

        while (WorldGen.genRand.NextBool(Math.Max(1, count / 3))) // Condition makes it possible to make very large buildings, but usually it'll be fairly small (~5 rooms)
        {
            count++;
            BuildSingleBuilding(x, y, size, tileType, out HashSet<Point16> tiles);
            placedBuildings.Add(new Point(x, y));

            foreach (var item in tiles) // Adds all tiles in the new building to the master list for populating later
            {
                var realPos = new Point16(item.X + x, item.Y + y);

                if (!allTiles.Contains(realPos))
                    allTiles.Add(realPos);
            }

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

        foreach (var item in placedBuildings) // Kinda weird way to hack in platforms and doors but it works
        {
            Point center = new(item.X + size.X / 2, item.Y + size.Y / 2);

            CheckAddPlatforms(center, -1, size.X, size.Y / 2);
            CheckAddPlatforms(center, 1, size.X, size.Y / 2);
            CheckAddDoors(new Point(item.X, item.Y + size.Y - 2), -1);
            CheckAddDoors(new Point(item.X + size.X - 1, item.Y + size.Y - 2), 1);
        }

        PopulateInteriors(allTiles);
    }

    /// <summary>
    /// Adds doors to a given house (rectangle).
    /// </summary>
    /// <param name="position">Position adjusted to already be in the wall of the given house.</param>
    /// <param name="dir">-1 when the door is being placed on the left side of the room, 1 otherwise.</param>
    private static void CheckAddDoors(Point position, int dir)
    {
        if (WorldGen.SolidTile(position.X + dir, position.Y))
            return;

        if (WorldGen.genRand.NextBool(8)) // Sometimes, do not add a door even if we can
            return;

        int placeWalls = Main.tile[position.X + dir, position.Y].WallType;

        for (int i = 0; i < 3; ++i)
        {
            Main.tile[position.X, position.Y - i].ClearTile();

            if (placeWalls != WallID.None) // Place appropriate walls in the door's space
                WorldGen.PlaceWall(position.X, position.Y - i, placeWalls);
        }

        if (WorldGen.genRand.NextBool(6)) // Sometimes, just have a hole in the wall
            return;
        
        WorldGen.PlaceTile(position.X, position.Y - 2, ModContent.TileType<QuartzDoorClosed>(), true);
    }

    /// <summary>
    /// Replaces some floor tiles in the given room with platforms (or holes).
    /// </summary>
    /// <param name="center">The center of the room.</param>
    /// <param name="dir">Whether this is being added to the ceiling or floor; -1 if ceiling, 1 otherwise.</param>
    /// <param name="width">Width of the room.</param>
    /// <param name="halfHeight">Half-height of the room, so to get the floor or ceiling of the room.</param>
    private static void CheckAddPlatforms(Point center, int dir, int width, int halfHeight)
    {
        int y = center.Y + (halfHeight * dir);
        Tile above = Main.tile[center.X, y + dir];

        // Must have a wall or be in open air to place the platform
        if (above.WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || above.WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>() || !above.HasTile)
        {
            int useWidth = Math.Max(WorldGen.genRand.Next((int)(width / 3f)), 2);
            int off = WorldGen.genRand.Next(-2, 3);
            bool makeHole = WorldGen.genRand.NextBool(14); // Sometimes, just have a hole in the wall

            for (int i = center.X - useWidth - off;  i < center.X + useWidth - off; ++i)
            {
                Tile tile = Main.tile[i, y];

                if (!tile.HasTile) // Skip if I'm placing over air
                    continue;

                if (!makeHole)
                {
                    WorldGen.KillTile(i, y, false, false, true);

                    if (!WorldGen.genRand.NextBool(12)) // Platforms have a chance to fail to give a more weathered look
                        WorldGen.PlaceTile(i, y, ModContent.TileType<QuartzPlatform>(), true);
                }
                else
                    tile.ClearTile();

                tile.WallType = Main.tile[center.X, y - dir].WallType; // Copy wall to here
            }
        }
    }

    /// <summary>
    /// Builds the base shape of a single building and returns all of the tiles it has.
    /// </summary>
    /// <param name="x">X position (left).</param>
    /// <param name="y">Y position (top).</param>
    /// <param name="size">Size of the building.</param>
    /// <param name="tileType">The tile type to use in the building. Hardcoded wall type is assumed.</param>
    /// <param name="tiles">A hash set of each tile used in the room for later decorations.</param>
    private static void BuildSingleBuilding(int x, int y, Point size, int tileType, out HashSet<Point16> tiles)
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

        tiles = shapeData.GetData();
    }

    /// <summary>
    /// Adds furniture to the given room.
    /// </summary>
    /// <param name="shapeDataPoints">A list of places to add furniture to.</param>
    private static void PopulateInteriors(HashSet<Point16> shapeDataPoints)
    {
        foreach (var pos in shapeDataPoints)
        {
            bool noWalls = !WorldGen.SolidTile(pos.X - 1, pos.Y - 1) && !WorldGen.SolidTile(pos.X + 1, pos.Y - 1);

            if (Main.tile[pos.X, pos.Y - 1].WallType != WallID.None && WorldGen.SolidTile(pos.ToPoint()) && noWalls && TryPlaceFurnitureOnHouseFloor(pos)) // Ground furniture
                continue;

            noWalls = !WorldGen.SolidTile(pos.X - 1, pos.Y + 1) && !WorldGen.SolidTile(pos.X + 1, pos.Y + 1);

            if (Main.tile[pos.X, pos.Y + 1].WallType != WallID.None && WorldGen.SolidTile(pos.ToPoint()) && noWalls && TryPlaceFurnitureOnCeiling(pos))
                continue;

            noWalls = !WorldGen.SolidTile(pos.X - 1, pos.Y) && !WorldGen.SolidTile(pos.X + 1, pos.Y);

            if (!noWalls && Main.tile[pos].WallType != WallID.None && GoodPlaceForTorch(pos) && WorldGen.genRand.NextBool(60))
                WorldGen.PlaceTile(pos.X, pos.Y, ModContent.TileType<GeoTorch>(), true);

            if (!HasSolidAdjacent(pos.X, pos.Y) && WorldGen.genRand.NextBool(2600))
                WorldGen.PlaceObject(pos.X, pos.Y, ModContent.TileType<ForTheSakeOfMakingGadgets>(), true);
        }
    }

    /// <summary>
    /// Determines if a torch should be placed here. Namely, if it's not touching the ceiling and it's far away from the ground.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private static bool GoodPlaceForTorch(Point16 pos)
    {
        if (WorldGen.SolidTile(pos.X, pos.Y)) // This is in a wall
            return false;

        // Check below so it's not close to or touching the ground
        int y = pos.Y;

        while (!WorldGen.SolidTile(pos.X, y))
            y++;

        if (y - pos.Y < 4)
            return false;

        // Check above so it's not close to or touching the ceiling
        y = pos.Y;

        while (!WorldGen.SolidTile(pos.X, y))
            y--;

        if (pos.Y - y < 2)
            return false;

        return true;
    }

    /// <summary>
    /// Checks if the tile has any solid adjacent tile (i.e. any of the 8 surrounding tiles are solid).
    /// </summary>
    /// <param name="i">X position of the tile.</param>
    /// <param name="j">Y position of the tile.</param>
    /// <returns>Whether the tile has a solid adjacent or not.</returns>
    internal static bool HasSolidAdjacent(int i, int j)
    {
        for (int l = -1; l <= 1; ++l)
            for (int k = -1; k <= 1; ++k)
                if (new Point(i + l, j + k) != new Point(i, j) && WorldGen.SolidTile(i, j))
                    return true;
        return false;
    }

    /// <summary>
    /// Simply has a chance to place banners on the ceiling.
    /// </summary>
    /// <param name="pos">Position of the banner-to-be-placed.</param>
    /// <returns>Whether a banner was placed or not.</returns>
    private static bool TryPlaceFurnitureOnCeiling(Point16 pos)
    {
        if (WorldGen.genRand.NextBool(18))
        {
            WorldGen.PlaceObject(pos.X, pos.Y + 1, ModContent.TileType<DepthsVanityBanners>(), style: 3, direction: WorldGen.genRand.NextBool() ? -1 : 1);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tries to put down either furniture (from <see cref="Furnitures"/>) or Gemforges on ground.
    /// </summary>
    /// <param name="pos">Un-adjusted position of the furniture-to-be-placed.</param>
    /// <returns>Whether any furniture was placed or not.</returns>
    private static bool TryPlaceFurnitureOnHouseFloor(Point16 pos)
    {
        if (WorldGen.genRand.NextBool(6))
        {
            int type = WorldGen.genRand.Next(Furnitures);

            if (WorldGen.genRand.NextBool(16)) // Chance to randomize into gem forge
                type = ModContent.TileType<Gemforge>();

            int yOff = 1;

            if (type == ModContent.TileType<QuartzLamp>()) // Origin is dumb in worldgen, hardcoded adjustment for it
                yOff = 2;

            WorldGen.PlaceObject(pos.X, pos.Y - yOff, type, direction: WorldGen.genRand.NextBool() ? -1 : 1);
            return true;
        }

        return false;
    }
}
