using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TheDepths.Tiles;

namespace TheDepths.Worldgen.Generation;

internal class DepthsGen
{
    /// <summary>
    /// Increases the seed for the noise used here. This is deterministic and will be consistent for seeds but still be random for generation.
    /// </summary>
    private static int _seedNumber = 0;

    internal static void Generate(GenerationProgress progress, GameConfiguration configuration)
    {
        AddBaseTiles((int)Main.MouseWorld.X / 16, Main.maxTilesY - 200, 800, 198);

        for (int i = 0; i < 2; ++i)
        {
            ClearTunnel((int)Main.MouseWorld.X / 16, Main.maxTilesY - 160, 800, 80, 0.3f);
            ClearTunnel((int)Main.MouseWorld.X / 16, Main.maxTilesY - 80, 800, 80, 0.05f);
        }
    }

    /// <summary>
    /// Clears a rectangle of space, occasionally including Arquerite and Shalestone through noise.
    /// </summary>
    /// <param name="x">X position (left).</param>
    /// <param name="y">Y position (top).</param>
    public static void AddBaseTiles(int x, int y, int width, int height)
    {
        FastNoiseLite noise = new(WorldGen._genRandSeed + _seedNumber++);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFractalType(FastNoiseLite.FractalType.PingPong);
        noise.SetFrequency(0.01f);
        noise.SetFractalGain(0.13f);
        noise.SetFractalLacunarity(3.95f);
        noise.SetFractalPingPongStrength(0.39f);

        for (int i = x; i < x + width; ++i)
        {
            for (int j = y; j < y + height; ++j)
            {
                float value = noise.GetNoise(i, j);
                int type = ModContent.TileType<ShaleBlock>();

                if (value > 0.42f)
                    type = ModContent.TileType<ArqueriteOre>();
                else if (value < -0.79f)
                    type = ModContent.TileType<Shalestone>();

                Tile tile = Main.tile[i, j];
                tile.TileType = (ushort)type;
                tile.HasTile = true;
                tile.Slope = SlopeType.Solid;
            }
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
        int bottomOff = WorldGen.genRand.Next(0, 50);

        for (int i = x; i < x + width; i++)
        {
            float value = (noise.GetNoise(i, 0) + 1) / 2;

            if (widenFactor.HasValue)
                value = MathHelper.Lerp(value, 1, widenFactor.Value);

            for (int j = 0; j < useHeight * value; ++j)
            {
                int realX = (i + j) % Main.maxTilesX;

                if (Main.tile[realX, y - j].HasTile && value > NoiseCutoff)
                    Main.tile[realX, y - j].ClearEverything();

                realX = (Math.Abs(i - j) + bottomOff) % Main.maxTilesX;

                if (Main.tile[realX, y + j].HasTile && value > NoiseCutoff)
                    Main.tile[realX, y + j].ClearEverything();
            }
        }
    }
}
