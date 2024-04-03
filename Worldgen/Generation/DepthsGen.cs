using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
    /// The generation for the main Depths. This does not include the pots (as that's a different step to override), but it does include trees and nightmare grove.
    /// </summary>
    internal static void Generate(GenerationProgress progress, GameConfiguration configuration)
    {
        int biomeWidth = Main.maxTilesX; // Change this and the biome will adjust in size accordingly. It is not currently programmed to start anywhere but the left side, however.
        
        progress.Message = "Digging the Depths...";

        int centerSizeHalved = Main.maxTilesX / 6; // Middle area with the buildings takes up 1/3rd of the world
        AddBaseTiles(0, Main.maxTilesY - 220, biomeWidth, 220, 20, centerSizeHalved);

        for (int i = 0; i < 2; ++i)
        {
            ClearTunnel(0, Main.maxTilesY - 160, biomeWidth, 80, 0.32f, true);
            AddStalactites(0, Main.maxTilesY - 160, biomeWidth, 15, 20, 30, 60);
            ClearTunnel(0, Main.maxTilesY - 80, biomeWidth, 80, 0.06f, false);
            AddStalactites(0, Main.maxTilesY - 80, biomeWidth, 10, 14, 20, 50);
        }

        AddHolesBetweenTunnels(0, Main.maxTilesY - 160, biomeWidth, 120, 240);
        AddWaterHoles(0, Main.maxTilesY - 200, biomeWidth, 160);

        int nightmareGroveSize = Main.maxTilesX / 6; // Each nightmare grove takes up 1/6th of the world, and non-grove is the rest
        AddNightmareGrove(nightmareGroveSize);
        AddDepthsDecor(nightmareGroveSize);
        AddMiddleAreaBuildings(centerSizeHalved, Main.maxTilesY - 160);
    }

    private static void AddMiddleAreaBuildings(int halfWidth, int startY)
    {
        const int MinSpace = 80;
        const int MaxSpace = 160;

        int x = Main.maxTilesX / 2 - halfWidth;
        int width = Main.maxTilesX / 2 + halfWidth;

        while (x < width)
        {
            x += WorldGen.genRand.Next(MinSpace, MaxSpace);

            int y = startY;

            while (!WorldGen.SolidTile(x, y))
            {
                y++;

                if (y > Main.maxTilesY - 20)
                    break;
            }

            if (y > Main.maxTilesY - 20)
                continue;

            y -= WorldGen.genRand.Next(10, 20); // Offset buildings up to give a varied depth

            DepthsBuilding.BuildBuilding(x, y);
        }
    }

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

    private static void AddHolesBetweenTunnels(int x, int y, int width, int minSpace, int maxSpace)
    {
        while (x < width)
        {
            x += WorldGen.genRand.Next(minSpace, maxSpace);

            if (Main.tile[x, y].HasTile || x > Main.maxTilesX) // Stop if there's no space or if out of bounds
                continue;

            int j = y;

            while (!WorldGen.SolidTile(x, j))
                j++;

            WorldGen.digTunnel(x, j, WorldGen.genRand.NextFloat(-2f, 2f), WorldGen.genRand.NextFloat(3, 9), WorldGen.genRand.Next(14, 18), 10);
        }
    }

    private static void AddDepthsDecor(int nightmareGroveSize)
    {
        for (int i = nightmareGroveSize; i < Main.maxTilesX - nightmareGroveSize; ++i)
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

    private static void PlaceDepthsDecor(int i, int j)
    {
        if (!WorldGen.SolidTile(i, j - 1))
        {
            if (WorldGen.genRand.NextBool(50))
            {
                if (WorldGen.PlaceObject(i, j - 1, ModContent.TileType<PetrifiedSapling>(), true, 0))
                {
                    if (!DepthsModTree.GrowModdedTreeWithSettings(i, j - 1, PetrifiedTree.Tree_Petrfied))
                        WorldGen.KillTile(i, j - 1, false, false, true);
                }
            }
            else if (WorldGen.genRand.NextBool(30))
                WorldGen.PlaceObject(i, j - 2, ModContent.TileType<LargeCrystal>(), true);
            else if (WorldGen.genRand.NextBool(20))
                WorldGen.PlaceObject(i, j - 1, ModContent.TileType<ShadowShrub>(), true, 0);
        }

        if (!WorldGen.SolidTile(i, j + 1))
        {
            if (WorldGen.genRand.NextBool(40))
                WorldGen.TileRunner(i, j + 1, WorldGen.genRand.NextFloat(2, 5), 3, ModContent.TileType<Quartz>(), true);
            else if (WorldGen.genRand.NextBool(26))
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
        while (x < width)
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

            if (i != 0 && heightSub < 3)
                heightSub = WorldGen.genRand.Next(2, 4) + 1;

            int baseY = y;

            while (!WorldGen.SolidTile(x + i, baseY))
            {
                baseY--;

                if (baseY < y / 1.5f) // Exit if I'm way too far up
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

            for (int j = baseY; j < baseY + height; ++j)
            {
                float value = noise.GetNoise(i, j);
                int type = ModContent.TileType<ShaleBlock>();
                int currentDepth = j - baseY;
                float adjustment = currentDepth / (float)height;

                if (value > 0.42f - (adjustment * 0.15f) && adjustment > 0.28f)
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
                }
                else if (adjustment < 0.25f) // Opens holes in the ceiling when above the second tunnel
                {
                    if (!WorldGen.SolidTile(i, j))
                        continue;
                }

                Tile tile = Main.tile[i, j];
                tile.TileType = (ushort)type;
                tile.HasTile = true;
                tile.Slope = SlopeType.Solid;
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

        FastNoiseLite noise = new(WorldGen._genRandSeed + _seedNumber++);
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

            if (widenFactor.HasValue)
                value = MathHelper.Lerp(value, 1, widenFactor.Value);

            for (int j = 0; j < useHeight * value; ++j)
            {
                int realX = i + j;

                if (realX >= x + width)
                    realX -= width;

                if (Main.tile[realX, y - j].HasTile && value > NoiseCutoff)
                {
                    if (clearWalls)
                        Main.tile[realX, y - j].ClearEverything();
                    else
                        Main.tile[realX, y - j].Clear(Terraria.DataStructures.TileDataType.Tile);
                }

                realX = (Math.Abs(i - j)) % Main.maxTilesX;

                if (realX < x)
                    realX += width;

                if (Main.tile[realX, y + j].HasTile && value > NoiseCutoff)
                {
                    if (clearWalls)
                        Main.tile[realX, y + j].ClearEverything();
                    else
                        Main.tile[realX, y + j].Clear(Terraria.DataStructures.TileDataType.Tile);
                }
            }
        }
    }

    /// <summary>
    /// Replaces the distance from each edge with a Nightmare Grove.
    /// </summary>
    /// <param name="distanceFromBothEdges">Distance to replace from each edge.</param>
    internal static void AddNightmareGrove(int distanceFromBothEdges)
    {
        for (int i = 0; i < distanceFromBothEdges; ++i)
        {
            for (int j = Main.maxTilesY - 400; j < Main.maxTilesY; ++j)
            {
                TryGivingShaleNightmare(i, j);
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
        if (!WorldGen.SolidTile(i, j - 1))
        {
            if (WorldGen.genRand.NextBool(50))
            {
                if (WorldGen.PlaceObject(i, j - 1, ModContent.TileType<NightSapling>(), true, 0))
                {
                    if (!DepthsModTree.GrowModdedTreeWithSettings(i, j - 1, NightwoodTree.Tree_Nightmare))
                        WorldGen.KillTile(i, j - 1, false, false, true);
                }
            }
            else if (!WorldGen.genRand.NextBool(3))
                WorldGen.PlaceObject(i, j - 1, ModContent.TileType<NightmareGrass_Foliage>(), true, 0);
        }

        if (!WorldGen.SolidTile(i, j + 1) && WorldGen.genRand.NextBool(3))
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
