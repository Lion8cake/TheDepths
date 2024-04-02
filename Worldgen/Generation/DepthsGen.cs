using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TheDepths.Tiles;
using TheDepths.Tiles.Trees;

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
    /// The generation for the main Depths. This does not include the pots, but it does include trees and nightmare grove.
    /// </summary>
    internal static void Generate(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = "Digging the Depths...";

        AddBaseTiles(0, Main.maxTilesY - 220, Main.maxTilesX, 220, 20);

        for (int i = 0; i < 2; ++i)
        {
            ClearTunnel(0, Main.maxTilesY - 160, Main.maxTilesX, 80, 0.32f);
            AddStalactites(0, Main.maxTilesY - 160, Main.maxTilesX, 15, 20, 30, 60);
            ClearTunnel(0, Main.maxTilesY - 80, Main.maxTilesX, 80, 0.06f);
            AddStalactites(0, Main.maxTilesY - 80, Main.maxTilesX, 10, 14, 20, 50);
        }

        AddNightmareForest(Main.maxTilesX / 6);
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
            x += WorldGen.genRand.Next(minSpace, maxSpace);

            if (Main.tile[x, y].HasTile || x > Main.maxTilesX) // Stop if there's no space or if out of bounds
                continue;

            int j = y;

            while (!WorldGen.SolidTile(x, j))
                j--;

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
    public static void AddBaseTiles(int x, int y, int width, int height, int fadeHeight)
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

                if (value > 0.42f - (adjustment * 0.15f) && adjustment > 0.25f)
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
            }
        }

        for (int i = 0; i < width * (height / 60f); ++i) // Spam gems
        {
            int gemX = WorldGen.genRand.Next(x, x + width);
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
    public static void ClearTunnel(int x, int y, int width, int height, float? widenFactor)
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
                    Main.tile[realX, y - j].ClearEverything();

                realX = (Math.Abs(i - j)) % Main.maxTilesX;

                if (realX < x)
                    realX += width;

                if (Main.tile[realX, y + j].HasTile && value > NoiseCutoff)
                    Main.tile[realX, y + j].ClearEverything();
            }
        }
    }

    /// <summary>
    /// Replaces the distance from each edge with a Nightmare Grove.
    /// </summary>
    /// <param name="distanceFromBothEdges">Distance to replace from each edge.</param>
    internal static void AddNightmareForest(int distanceFromBothEdges)
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
        OnPlaceNightmareGrass(i, j);
    }

    /// <summary>
    /// Called when placing Nightmare Grass on shale.
    /// </summary>
    /// <param name="i">X position of the tile.</param>
    /// <param name="j">Y position of the tile.</param>
    private static void OnPlaceNightmareGrass(int i, int j)
    {
        if (WorldGen.genRand.NextBool(50))
        {
            if (WorldGen.PlaceObject(i, j - 1, ModContent.TileType<NightSapling>(), true, 0))
            {
                if (!DepthsModTree.GrowModdedTreeWithSettings(i, j - 1, NightwoodTree.Tree_Nightmare))
                    WorldGen.KillTile(i, j - 1, false, false, true);
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
