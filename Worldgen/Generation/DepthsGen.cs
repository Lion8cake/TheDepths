using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TheDepths.Tiles;
using TheDepths.Tiles.Trees;
using TheDepths.Walls;

namespace TheDepths.Worldgen.Generation;

internal class DepthsGen
{
    /// <summary>
    /// Increases the seed for the noise used here. This is deterministic and will be consistent for seeds but still be random for generation.
    /// </summary>
    private static int _seedNumber = 0;

    /// <summary>
    /// A list of each Shale gem. Used for randomization. 
    /// </summary>
    public static int[] Gems => [ModContent.TileType<ShalestoneAmethyst>(), ModContent.TileType<ShalestoneTopaz>(), ModContent.TileType<ShalestoneDiamond>(),
        ModContent.TileType<ShalestoneSapphire>(),  ModContent.TileType<ShalestoneEmerald>(), ModContent.TileType<ShalestoneRuby>()];

    /// <summary>
    /// The generation for the main Depths. This does not include the pots (as that's a different step to override), but it does include trees and nightmare grove.<br/>
    /// Building code is in <see cref="DepthsBuilding"/> as well, for nicer organization.
    /// </summary>
    internal static void Generate(GenerationProgress progress, GameConfiguration configuration) // configuration is for it to be used as a genpass, even if your IDE says it's useless
    {
        int biomeWidth = Main.maxTilesX; // Change this and the biome will adjust in size accordingly.
        
        progress.Message = "Digging the Depths...";

        int centerSizeHalved = Main.maxTilesX / 6; // Middle area with the buildings takes up 1/3rd of the world
        AddBaseTiles(0, Main.maxTilesY - 220, biomeWidth, 220, 20, centerSizeHalved); // Adds base tiles - Shale, Arquerite, Shalestone

        for (int i = 0; i < 2; ++i) // Adds the two chasms, and their stalactites
        {
            ClearTunnel(0, Main.maxTilesY - 160, biomeWidth, 80, 0.32f, true); // Top one is bigger (as per the widenFactor of .32)
            AddStalactites(0, Main.maxTilesY - 160, biomeWidth, 15, 20, 30, 60);
            ClearTunnel(0, Main.maxTilesY - 80, biomeWidth, 80, 0.06f, false); // and the bottom one is smaller (widenFactor of .06)
            AddStalactites(0, Main.maxTilesY - 80, biomeWidth, 4, 8, 20, 50);
        }

        AddHolesBetweenTunnels(0, Main.maxTilesY - 160, biomeWidth, 120, 240); // Digs pits between the two chasms
        AddWaterHoles(0, Main.maxTilesY - 200, biomeWidth, 160);

        int nightmareGroveSize = Main.maxTilesX / 6; // Each nightmare grove takes up 1/6th of the world, and non-grove is the rest
        AddNightmareGrove(nightmareGroveSize, 0);
        AddDepthsDecor(nightmareGroveSize);
        AddBuildings(Main.maxTilesX / 2 - centerSizeHalved, Main.maxTilesX / 2 + centerSizeHalved, Main.maxTilesY - 160); // Adds all of the buildings
    }

    internal static void SpecialGenerate(GenerationProgress progress, GameConfiguration configuration)
    {
        int biomeWidth = WorldGen.drunkWorldGen ? (int)(Main.maxTilesX / 2.6f) : Main.maxTilesX; // Change this and the biome will adjust in size accordingly.
        int x = 0;
        int side = 0;

        if (WorldGen.drunkWorldGen)
        {
            if (WorldGen.genRand.NextBool(2))
            {
                side = -1;
                TheDepthsWorldGen.DrunkDepthsLeft = true;
                TheDepthsWorldGen.DrunkDepthsRight = false;
            }
            else
            {
                side = 1;
                x = Main.maxTilesX - (int)(Main.maxTilesX / 2.6f);
                TheDepthsWorldGen.DrunkDepthsLeft = false;
                TheDepthsWorldGen.DrunkDepthsRight = true;
            }
        }

        progress.Message = "Digging the depths...";

        int buildingArea = Main.maxTilesX / 6; // Middle area with the buildings takes up 1/3rd of the world
        AddBaseTiles(x, Main.maxTilesY - 220, biomeWidth, 220, 20, buildingArea); // Adds base tiles - Shale, Arquerite, Shalestone

        for (int i = 0; i < 2; ++i) // Adds the two chasms, and their stalactites
        {
            ClearTunnel(x, Main.maxTilesY - 160, biomeWidth, 80, 0.32f, true); // Top one is bigger (as per the widenFactor of .32)
            AddStalactites(x, Main.maxTilesY - 160, biomeWidth, 15, 20, 30, 60);
            ClearTunnel(x, Main.maxTilesY - 80, biomeWidth, 80, 0.06f, false); // and the bottom one is smaller (widenFactor of .06)
            AddStalactites(x, Main.maxTilesY - 80, biomeWidth, 4, 8, 20, 50);
        }

        AddHolesBetweenTunnels(x, Main.maxTilesY - 160, biomeWidth, 120, 240); // Digs pits between the two chasms
        AddWaterHoles(x, Main.maxTilesY - 200, biomeWidth, 160);

        progress.Message = "Growing bioluminecent plants in very dark areas";

        int nightmareGroveSize = WorldGen.drunkWorldGen ? Main.maxTilesX / 2 :  Main.maxTilesX / 6; // Each nightmare grove takes up 1/6th of the world, and non-grove is the rest
        AddNightmareGrove(nightmareGroveSize, side);
        //AddDepthsDecor(nightmareGroveSize);

        progress.Message = "Building ruined homes...";

        if (side is 0 or (-1))
            AddBuildings(60, buildingArea, Main.maxTilesY - 160); // Adds all of the buildings

        if (side is 0 or 1)
            AddBuildings(Main.maxTilesX - buildingArea, Main.maxTilesX - 60, Main.maxTilesY - 160);
    }

    /// <summary>
    /// Replaces ceiling tiles in the middle area of a drunk world without me having to add them manually.
    /// </summary>
    /// <param name="startX">Start X position of the generation. Always assumes width is 1/2 of the world.</param>
    internal static void ReplaceHalfCeilingOnDrunkWorldGen(int startX)
    {
        int biomeWidth = Main.maxTilesX / 2;

        for (int i = startX; i < startX + biomeWidth; i++)
        {
            for (int j = Main.maxTilesY - 250; j < Main.maxTilesY - 50; ++j)
            {
                Tile tile = Main.tile[i, j];
                ushort type = tile.TileType;

                if (tile.TileType == TileID.Ash)
                    tile.TileType = (ushort)ModContent.TileType<ShaleBlock>();

                if (tile.TileType == TileID.AshGrass)
                    tile.TileType = (ushort)ModContent.TileType<NightmareGrass>();

                if (type != tile.TileType)
                    WorldGen.SquareTileFrame(i, j, true);
            }
        }
    }

    /// <summary>
    /// Adds buildings to the center of the world, based on <paramref name="halfWidth"/>, and starting at <paramref name="startY"/>.
    /// </summary>
    /// <param name="halfWidth">Half-width of the size of the area to add buildings to.</param>
    /// <param name="startY">Y position to start placing buildings at.</param>
    private static void AddBuildings(int left, int right, int startY)
    {
        const int MinSpace = 80; // Minimum and maximum spacing between buildings.
        const int MaxSpace = 160;

        int x = left;

        while (x < right)
        {
            x += WorldGen.genRand.Next(MinSpace, MaxSpace);

            int y = startY;

            while (!WorldGen.SolidTile(x, y))
            {
                y++;

                if (y > Main.maxTilesY - 20) // Exit & skip later if we're OOB
                    break;
            }

            if (y > Main.maxTilesY - 20)
                continue;

            y -= WorldGen.genRand.Next(10, 20); // Offset buildings up to give a varied depth

            DepthsBuilding.BuildBuilding(x, y);
        }
    }

    /// <summary>
    /// Spams holes full of water throughout the biome.
    /// </summary>
    /// <param name="x">X position (left).</param>
    /// <param name="y">Y position (top).</param>
    /// <param name="width">Width of the area to add water holes to.</param>
    /// <param name="height">Height of the area to add water holes to.</param>
    private static void AddWaterHoles(int x, int y, int width, int height)
    {
        for (int i = 0; i < Main.maxTilesX / 4200f * 300; ++i)
        {
            int placeX = x + WorldGen.genRand.Next(width);
            int placeY = y + WorldGen.genRand.Next(height);
            Tile tile = Main.tile[placeX, placeY];

            if (tile.HasTile)
                WorldGen.digTunnel(placeX, placeY, WorldGen.genRand.NextFloat(-2, 2f), WorldGen.genRand.NextFloat(-2f, 2f), WorldGen.genRand.Next(2, 15), 2, true);
        }
    }

    /// <summary>
    /// Adds holes between the chasms for easier player access.
    /// </summary>
    /// <param name="x">X position (left).</param>
    /// <param name="y">Y position (top).</param>
    /// <param name="width">Width of the area to add holes to.</param>
    /// <param name="minSpace">Minimum distance between holes.</param>
    /// <param name="maxSpace">Maximum distance between holes.</param>
    private static void AddHolesBetweenTunnels(int x, int y, int width, int minSpace, int maxSpace)
    {
        while (x < (width - maxSpace))
        {
            x += WorldGen.genRand.Next(minSpace, maxSpace);

            if (Main.tile[x, y].HasTile || x > Main.maxTilesX) // Stop if there's no space or if out of bounds
                continue;

            int j = y;

            while (!WorldGen.SolidTile(x, j)) // Go to ground
                j++;

            WorldGen.digTunnel(x, j, WorldGen.genRand.NextFloat(-2f, 2f), WorldGen.genRand.NextFloat(3, 9), WorldGen.genRand.Next(14, 18), 10);
        }
    }

    /// <summary>
    /// Calls <see cref="PlaceDepthsDecor(int, int)"/> on every Shale tile outside of the Nightmare Grove, per <paramref name="nightmareGroveSize"/>.
    /// </summary>
    /// <param name="nightmareGroveSize">Size of the nightmare grove, in tiles.</param>
    private static void AddDepthsDecor(int nightmareGroveSize)
    {
        for (int i = nightmareGroveSize; i < Main.maxTilesX - nightmareGroveSize; ++i) // Loops from edge of grove -> other edge of the opposite grove
        {
            for (int j = Main.maxTilesY - 300; j < Main.maxTilesY - 10; ++j)
            {
                Tile tile = Main.tile[i, j];

                if (!tile.HasTile || tile.TileType != ModContent.TileType<ShaleBlock>())
                    continue;

                PlaceDepthsDecor(i, j);
            }
        }
    }

    /// <summary>
    /// Adds trees, crystals and shrubs to Shale.
    /// </summary>
    /// <param name="i">X position.</param>
    /// <param name="j">Y position.</param>
    private static void PlaceDepthsDecor(int i, int j)
    {
        if (!WorldGen.SolidTile(i, j - 1)) // If no tile above...
        {
            if (WorldGen.genRand.NextBool(50)) // Try and place a tree, or
            {
                if (WorldGen.PlaceObject(i, j - 1, ModContent.TileType<PetrifiedSapling>(), true, 0))
                {
                    if (!DepthsModTree.GrowModdedTreeWithSettings(i, j - 1, PetrifiedTree.Tree_Petrfied))
                        WorldGen.KillTile(i, j - 1, false, false, true);
                }
            }
            else if (WorldGen.genRand.NextBool(20)) // Try and place a shadow shrub.
                WorldGen.PlaceObject(i, j - 1, ModContent.TileType<ShadowShrub>(), true, 0);
        }

        if (!WorldGen.SolidTile(i, j + 1)) // If no tile below...
        {
            if (WorldGen.genRand.NextBool(40)) // Try and place a quartz chunk,
                WorldGen.TileRunner(i, j + 1, WorldGen.genRand.NextFloat(1, 4), 3, ModContent.TileType<Quartz>(), true);
            else if (WorldGen.genRand.NextBool(26)) // Or try and place a large crystal.
                WorldGen.PlaceObject(i, j + 1, ModContent.TileType<LargeCrystal>(), true);
        }
    }

    /// <summary>
    /// Adds stalactites to the given area.
    /// </summary>
    /// <param name="x">X position (left).</param>
    /// <param name="y">Y position (center, in open space).</param>
    /// <param name="width">Width of the area to stalactize.</param>
    /// <param name="minHeight">Minimum height of any individual stalactite.</param>
    /// <param name="maxHeight">Maximum height of any individual stalactite.</param>
    /// <param name="minSpace">Minimum spacing between each stalactite.</param>
    /// <param name="maxSpace">Maximum spacing between each stalactite.</param>
    private static void AddStalactites(int x, int y, int width, int minHeight, int maxHeight, int minSpace, int maxSpace)
    {
        while (x < (width - maxSpace))
        {
        retry: // Goto is easier to set up here to escape the while
			x += WorldGen.genRand.Next(minSpace, maxSpace);

			if (Main.tile[x, y].HasTile || x > Main.maxTilesX) // Stop if there's no space or if out of bounds
                continue;

            int j = y;

            while (!WorldGen.SolidTile(x, j))
            {
                j--;

                if (j < y / 2)
                    goto retry;
            }

            PlaceStalactite(x, j, WorldGen.genRand.Next(minHeight, maxHeight));
        }
    }

    /// <summary>
    /// Places a single stalactite.
    /// </summary>
    /// <param name="x">X position (center).</param>
    /// <param name="y">Y position (below ceiling to place on).</param>
    /// <param name="height">Height of the stalactite.</param>
    private static void PlaceStalactite(int x, int y, int height)
    {
        int width = height / 6;
        for (int i = -width; i <= width; ++i)
        {
            int heightSub = (int)MathF.Abs(i * WorldGen.genRand.NextFloat(4, 6)); // Adjusts the shape of the stalactite

            if (i != 0 && heightSub < 3) // Force heightSub to be >= 2 if i != 0 so the shape looks better consistently
                heightSub = WorldGen.genRand.Next(2, 4) + 1;

            int baseY = y;

            while (!WorldGen.SolidTile(x + i, baseY))
            {
                baseY--;

                if (baseY < y / 1.5f) // Exit if it's way too far up
                    return;
            }

            for (int j = baseY; j < baseY + height - heightSub; ++j)
                WorldGen.PlaceTile(x + i, j, ModContent.TileType<ShaleBlock>(), true);
        }
    }

    /// <summary>
    /// Clears a rectangle of space, occasionally including Arquerite and Shalestone through noise.
    /// </summary>
    /// <param name="x">X position (left).</param>
    /// <param name="y">Y position (top).</param>
    public static void AddBaseTiles(int x, int y, int width, int height, int fadeHeight, int middleAreaSizeHalved)
    {
        FastNoiseLite noise = new(WorldGen._genRandSeed + _seedNumber++);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFractalType(FastNoiseLite.FractalType.PingPong);
        noise.SetFrequency(0.03f);
        noise.SetFractalGain(0.13f);
        noise.SetFractalLacunarity(3.95f);
        noise.SetFractalPingPongStrength(0.39f);

        for (int i = x; i < x + width; ++i)
        {
            float modFadeHeight = fadeHeight * (noise.GetNoise(i, -100) + 1); // Increases or decreases the depth of the gradient
            int baseY = y - (int)modFadeHeight;

            for (int j = baseY; j < baseY + height + modFadeHeight; ++j)
            {
                float value = noise.GetNoise(i, j);
                int type = ModContent.TileType<ShaleBlock>();
                int currentDepth = j - baseY;
                float adjustment = currentDepth / (float)height;
                bool skipWalls = false;

                if (value > 0.42f - adjustment * 0.15f && adjustment > 0.28f)
                    type = ModContent.TileType<ArqueriteOre>();
                else if (value < -0.78f)
                    type = ModContent.TileType<Shalestone>();

                if (currentDepth < modFadeHeight) // Fades in stuff through dither
                {
                    float diff = (modFadeHeight - currentDepth) / (float)modFadeHeight;

                    if (!WorldGen.SolidTile(i, j))
                        continue;

                    if (WorldGen.genRand.NextFloat() < diff)
                        continue;

                    skipWalls = HasOpenAdjacent(i, j);
                }
                else if (adjustment < 0.25f) // Opens holes in the ceiling when above the second tunnel
                {
                    if (!WorldGen.SolidTile(i, j))
                        continue;

                    skipWalls = HasOpenAdjacent(i, j);
                }

                Tile tile = Main.tile[i, j];
                tile.TileType = (ushort)type;
                tile.HasTile = true;
                tile.LiquidType = -1;
                tile.LiquidAmount = 0;
                tile.Slope = SlopeType.Solid;

                if (!skipWalls)
                    tile.WallType = (ushort)ModContent.WallType<ShaleWallUnsafe>();
            }
        }

        for (int i = 0; i < width * (height / 60f); ++i) // Spam gems
        {
            int gemX = WorldGen.genRand.Next(x, x + width);

            if (gemX > Main.maxTilesX / 2 - middleAreaSizeHalved && gemX < Main.maxTilesX / 2 + middleAreaSizeHalved) // Do not place gems in the middle area
            {
                i--;
                continue;
            }

            int gemY = WorldGen.genRand.Next(y, y + height);
            float str = WorldGen.genRand.NextFloat(1, 3);
            int steps = WorldGen.genRand.Next(1, 6);
            WorldGen.OreRunner(gemX, gemY, str, steps, (ushort)WorldGen.genRand.Next(Gems));
        }
    }

    /// <summary>
    /// Clears an asymmetrical tunnel, used to create the channels in the Depths.<br/>
    /// This is used twice in <see cref="Generate(GenerationProgress, GameConfiguration)"/> to create the two tunnels.
    /// </summary>
    /// <param name="x">X position (left).</param>
    /// <param name="y">Y position (top).</param>
    /// <param name="widenFactor">How much to bias noise towards being empty space. This is used to make the top channel bigger.</param>
    public static void ClearTunnel(int x, int y, int width, int height, float? widenFactor, bool clearWalls)
    {
        const float NoiseCutoff = -0.2f;

        FastNoiseLite noise = new(WorldGen._genRandSeed + _seedNumber++); // Sets up noise for the caves. This is the same noise used in AddBaseTiles, but lower frequency.
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFractalType(FastNoiseLite.FractalType.PingPong);
        noise.SetFrequency(0.01f);
        noise.SetFractalGain(0.13f);
        noise.SetFractalLacunarity(3.95f);
        noise.SetFractalPingPongStrength(0.39f);

        int useHeight = height / 2;

        for (int i = x; i < x + width; i++)
        {
            float value = (noise.GetNoise(i, 0) + 1) / 2;

            if (widenFactor.HasValue) // Helps expand the space by making the noise closer to empty tiles
                value = MathHelper.Lerp(value, 1, widenFactor.Value);

            for (int j = 0; j < useHeight * value; ++j)
            {
                int realX = i + j;

                if (realX >= x + width)
                    realX -= width;

                Tile tile = Main.tile[realX, y - j];

                if (tile.HasTile && value > NoiseCutoff) // Clear tiles in the top half
                {
                    if (clearWalls)
                    {
                        tile.ClearEverything();
						tile.LiquidAmount = 0;
						tile.LiquidType = -1;
						Main.tile[realX, y - j + 1].Clear(Terraria.DataStructures.TileDataType.Wall);
                        Main.tile[realX, y - j - 1].Clear(Terraria.DataStructures.TileDataType.Wall);
                    }
                    else
                    {
						tile.LiquidAmount = 0;
						tile.LiquidType = -1;
						tile.Clear(Terraria.DataStructures.TileDataType.Tile);
					}
                }

                realX = Math.Abs(i - j) % Main.maxTilesX; // This offsets the tunnel so it looks random, even though it's technically mirrored

                if (realX < x)
                    realX += width;

                if (Main.tile[realX, y + j].HasTile && value > NoiseCutoff) // Clear tiles in the bottom half
                {
                    Tile tile2 = Main.tile[realX, y + j];
                    if (clearWalls)
                    {
                        tile2.ClearEverything();
						tile2.LiquidAmount = 0;
						tile2.LiquidType = -1;
						Main.tile[realX, y + j + 1].Clear(Terraria.DataStructures.TileDataType.Wall);
                        Main.tile[realX, y + j - 1].Clear(Terraria.DataStructures.TileDataType.Wall);
					}
                    else
                    {
                        tile2.LiquidAmount = 0;
                        tile2.LiquidType = -1;
                        tile2.Clear(Terraria.DataStructures.TileDataType.Tile);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Replaces the distance from each edge with a Nightmare Grove.
    /// </summary>
    /// <param name="distanceFromBothEdges">Distance to replace from each edge.</param>
    internal static void AddNightmareGrove(int distanceFromBothEdges, int side)
    {
        for (int i = 0; i < distanceFromBothEdges; ++i)
        {
            for (int j = Main.maxTilesY - 400; j < Main.maxTilesY; ++j)
            {
                if (side is 0 or -1)
                    TryGivingShaleNightmare(i, j);

                if (side is 0 or 1)
                    TryGivingShaleNightmare(Main.maxTilesX - i, j);
            }
        }
    }

    /// <summary>
    /// Grows Nightmare Grass on given shale, if any. Also grows trees and (tbd) foliage on successful placement.
    /// </summary>
    /// <param name="i">X position of the tile.</param>
    /// <param name="j">Y position of the tile.</param>
    private static void TryGivingShaleNightmare(int i, int j)
    {
        Tile tile = Main.tile[i, j];

        if (tile.TileType != ModContent.TileType<ShaleBlock>()) // Must place grass on shale
            return;

        if (!HasOpenAdjacent(i, j)) // Grass needs to be exposed
            return;

        tile.TileType = (ushort)ModContent.TileType<NightmareGrass>();
        PlaceNightmareDecor(i, j);
    }

    /// <summary>
    /// Called when placing Nightmare Grass on shale.<br/>
    /// Currently, grows Trees and foliage.
    /// </summary>
    /// <param name="i">X position of the tile.</param>
    /// <param name="j">Y position of the tile.</param>
    private static void PlaceNightmareDecor(int i, int j)
    {
        if (!WorldGen.SolidTile(i, j - 1)) // If we have no tile above...
        {
            if (WorldGen.genRand.NextBool(50)) // Try to place a tree, or...
            {
                if (WorldGen.PlaceObject(i, j - 1, ModContent.TileType<NightSapling>(), true, 0))
                {
                    if (!DepthsModTree.GrowModdedTreeWithSettings(i, j - 1, NightwoodTree.Tree_Nightmare))
                        WorldGen.KillTile(i, j - 1, false, false, true);
                }
            }
            else if (!WorldGen.genRand.NextBool(3)) // Place foliage
                WorldGen.PlaceObject(i, j - 1, ModContent.TileType<NightmareGrass_Foliage>(), true, 0);
        }

        if (!WorldGen.SolidTile(i, j + 1) && WorldGen.genRand.NextBool(3)) // If we have no tile below, try to spam vines
        {
            int height = WorldGen.genRand.Next(4, 17);

            for (int y = j + 1; y < j + height; y++)
            {
                if (WorldGen.SolidTile(i, y))
                    break;  

                WorldGen.PlaceTile(i, y, ModContent.TileType<NightmareVines>());
            }
        }
    }

    /// <summary>
    /// Checks if the tile has any open adjacent tile (i.e. any of the 8 surrounding tiles are open).
    /// </summary>
    /// <param name="i">X position of the tile.</param>
    /// <param name="j">Y position of the tile.</param>
    /// <returns>Whether the tile has an open adjacent or not.</returns>
    internal static bool HasOpenAdjacent(int i, int j)
    {
        for (int l = -1; l <= 1; ++l)
            for (int k = -1; k <= 1; ++k)
                if (new Point(i + l, j + k) != new Point(i, j) && !Framing.GetTileSafely(i + l, j + k).HasTile)
                    return true;
        return false;
    }
}
