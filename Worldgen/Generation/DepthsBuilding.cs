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
using Terraria.GameContent.Generation;
using System.Net.Http.Headers;
using Steamworks;

namespace TheDepths.Worldgen.Generation;

/// <summary>
/// Contains everything needed to build a Depths house/building.
/// </summary>
internal class DepthsBuilding
{
    /// <summary>
    /// All floor furnitures.
    /// </summary>
    private static int[] Furnitures => new int[] { ModContent.TileType<QuartzWorkbench>(), ModContent.TileType<QuartzDresser>(), ModContent.TileType<QuartzTable>(), ModContent.TileType<QuartzChair>(), ModContent.TileType<QuartzBookcase>(), 
        ModContent.TileType<QuartzLamp>(), ModContent.TileType<QuartzBath>(), ModContent.TileType<QuartzBed>(), ModContent.TileType<QuartzClock>(), ModContent.TileType<QuartzPiano>(), 
        ModContent.TileType<QuartzSofa>(), ModContent.TileType<QuartzVase>(), ModContent.TileType<QuartzCandelabra>() };

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

        HashSet<Point> placedBuildings = new(); //tmod doesnt recognise [] so we use new() instead
        HashSet<Point16> allTiles = new();
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

        if (position.X >= Main.maxTilesX)
            return;

        int placeWalls = Main.tile[position.X + dir, position.Y].WallType;

        for (int i = 0; i < 3; ++i)
        {
            Main.tile[position.X, position.Y - i].ClearTile();

            if (placeWalls != WallID.None) // Place appropriate walls in the door's space
                WorldGen.PlaceWall(position.X, position.Y - i, placeWalls);
        }
        WorldGen.PlaceTile(position.X, position.Y - 2, ModContent.TileType<QuartzDoorClosed>(), true);
        Tile tile = Main.tile[position.X, position.Y + 1];
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
        if (center.X >= Main.maxTilesX)
            return;

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
                    WorldGen.PlaceTile(i, y, ModContent.TileType<QuartzPlatform>(), true);
                }
                else
                    tile.ClearTile();

                if (above.WallType != WallID.None)
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

        if ((x + size.X) < Main.maxTilesX)
        {
            WorldUtils.Gen(new Point(x, y), new Shapes.Rectangle(size.X, size.Y), Actions.Chain( // Clear & add walls
                new Actions.Clear().Output(shapeData),
                new Actions.PlaceWall((ushort)(tileType == ModContent.TileType<ArqueriteBricks>() ? ModContent.WallType<ArqueriteBrickWallUnsafe>() : ModContent.WallType<QuartzBrickWallUnsafe>()))
            ));
        }

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

			bool WallBehindTop = Main.tile[pos.X, pos.Y - 1].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[pos.X, pos.Y - 1].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>();
			bool WallBehindBelow = Main.tile[pos.X, pos.Y + 1].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[pos.X, pos.Y + 1].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>();
			bool WallBehind = Main.tile[pos].WallType == ModContent.WallType<QuartzBrickWallUnsafe>() || Main.tile[pos].WallType == ModContent.WallType<ArqueriteBrickWallUnsafe>();

			if (WallBehindTop && WorldGen.SolidTile(pos.ToPoint()) && noWalls && TryPlaceFurnitureOnHouseFloor(pos) && !Main.tile[pos].HasTile) // Ground furniture
                continue;

			noWalls = !WorldGen.SolidTile(pos.X - 1, pos.Y + 1) && !WorldGen.SolidTile(pos.X + 1, pos.Y + 1);

            if (WallBehindBelow && WorldGen.SolidTile(pos.ToPoint()) && noWalls && TryPlaceFurnitureOnCeiling(pos) && !Main.tile[pos].HasTile)
                continue;

            noWalls = !WorldGen.SolidTile(pos.X - 1, pos.Y) && !WorldGen.SolidTile(pos.X + 1, pos.Y);

            if (!noWalls && WallBehind && GoodPlaceForTorch(pos) && WorldGen.genRand.NextBool(60) && !Main.tile[pos].HasTile)
                WorldGen.PlaceTile(pos.X, pos.Y, ModContent.TileType<GeoTorch>(), true);

            if (!HasSolidAdjacent(pos.X, pos.Y) && WorldGen.genRand.NextBool(200) && !Main.tile[pos].HasTile)
            {
				bool canPlacePainting = true;
				if (nearDepthsPainting(pos.X, pos.Y))
					canPlacePainting = false;
				if (canPlacePainting)
				{
					PaintingEntry paintingEntry = RandDarknessPicture();
					if (!WorldGen.nearPicture(pos.X, pos.Y))
					{
						WorldGen.PlaceTile(pos.X, pos.Y, paintingEntry.tileType, mute: true, forced: false, -1, paintingEntry.style);
					}
				}
			}
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
			int HangingTile = WorldGen.genRand.Next(3);
			switch (HangingTile)
			{
				case 0:
					WorldGen.PlaceObject(pos.X, pos.Y + 1, ModContent.TileType<DepthsVanityBanners>(), style: WorldGen.genRand.Next(6), direction: WorldGen.genRand.NextBool() ? -1 : 1);
					break;
				case 1:
					WorldGen.PlaceObject(pos.X, pos.Y + 1, ModContent.TileType<QuartzChandelier>(), direction: WorldGen.genRand.NextBool() ? -1 : 1);
					break;
				case 2:
					WorldGen.PlaceObject(pos.X, pos.Y + 1, ModContent.TileType<QuartzLantern>(), direction: WorldGen.genRand.NextBool() ? -1 : 1);
					break;
			}
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tries to put down furniture (from <see cref="Furnitures"/>)
    /// </summary>
    /// <param name="pos">Un-adjusted position of the furniture-to-be-placed.</param>
    /// <returns>Whether any furniture was placed or not.</returns>
    private static bool TryPlaceFurnitureOnHouseFloor(Point16 pos)
    {
        if (WorldGen.genRand.NextBool(14))
        {
            int type = WorldGen.genRand.Next(Furnitures);
            int yOff = 1;

            if (type == ModContent.TileType<QuartzLamp>()) // Origin is dumb in worldgen, hardcoded adjustment for it
                yOff = 2;

            WorldGen.PlaceObject(pos.X, pos.Y - yOff, type, direction: WorldGen.genRand.NextBool() ? -1 : 1);

            if (type == ModContent.TileType<QuartzWorkbench>())
            {
                if (Main.rand.NextBool(3))
                    WorldGen.PlaceObject(pos.X, pos.Y - yOff - 1, ModContent.TileType<QuartzCandle>(), direction: WorldGen.genRand.NextBool() ? -1 : 1);
                bool chairDir = WorldGen.genRand.NextBool();
                WorldGen.PlaceObject(pos.X - (chairDir ? -2 : 1), pos.Y - yOff, ModContent.TileType<QuartzChair>(), direction: chairDir ? -1 : 1);
            }
			return true;
        }

        return false;
    }

	/// <summary>
	///  Checks for nearby Depths Paintings. 
	///  Mainly used for depths houses to not place paintings both too close each other and make sure paintings dont overlap each other as well
	/// </summary>
	/// <param name="x">The tied x position of the search</param>
	/// <param name="y">The tied y position of the search</param>
	/// <returns>False if no paintings are found</returns>
	public static bool nearDepthsPainting(int x, int y)
	{
		for (int i = x - 8; i <= x + 8; i++)
		{
			for (int j = y - 5; j <= y + 5; j++)
			{
				if (Main.tile[i, j].HasTile && (Main.tile[i, j].TileType == ModContent.TileType<FlowingQuicksilver>() || Main.tile[i, j].TileType == ModContent.TileType<ForTheSakeOfMakingGadgets>() || Main.tile[i, j].TileType == ModContent.TileType<AltarOfGems>() || Main.tile[i, j].TileType == ModContent.TileType<DONOTDRINK>() || Main.tile[i, j].TileType == ModContent.TileType<OtherPortal>() || Main.tile[i, j].TileType == ModContent.TileType<TheUnknownDepthsBelow>() || Main.tile[i, j].TileType == ModContent.TileType<ImPurrSonation>() || Main.tile[i, j].TileType == ModContent.TileType<MusiciansBestFriend>() || Main.tile[i, j].TileType == ModContent.TileType<Mercury>())) //3x3, 4x3, 3x4, 4x6
				{
					return true;
				}
			}
		}
		return false;
	}

	public static PaintingEntry RandDarknessPicture()
	{
		int num = WorldGen.genRand.Next(8);
		int num2 = 0;
		switch (num)
		{
			case 0:
				num = ModContent.TileType<OtherPortal>();
				break;
			case 1:
				num = ModContent.TileType<MusiciansBestFriend>();
				break;
			case 2:
				num = ModContent.TileType<ImPurrSonation>();
				break;
			case 3:
				num = ModContent.TileType<AltarOfGems>();
				break;
			case 4:
				num = ModContent.TileType<ForTheSakeOfMakingGadgets>();
				break;
			case 5:
				num = ModContent.TileType<Mercury>();
				break;
			case 6:
				num = ModContent.TileType<ChaosCat>();
				break;
			case 7:
				num = ModContent.TileType<TheUnknownDepthsBelow>();
				break;
			case 8:
				num = ModContent.TileType<FlowingQuicksilver>();
				break;
			default:
				num = ModContent.TileType<DONOTDRINK>();
				break;
		}
		PaintingEntry result = default(PaintingEntry);
		result.tileType = num;
		result.style = num2;
		return result;
	}
}
