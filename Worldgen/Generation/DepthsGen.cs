using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using System.Numerics;
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
    public static int[] Gems => new int[] { ModContent.TileType<ShalestoneAmethyst>(), ModContent.TileType<ShalestoneTopaz>(), ModContent.TileType<ShalestoneDiamond>(),
        ModContent.TileType<ShalestoneSapphire>(),  ModContent.TileType<ShalestoneEmerald>(), ModContent.TileType<ShalestoneRuby>() };

    /// <summary>
    /// The generation for the main Depths. This does not include the pots (as that's a different step to override), but it does include trees and nightmare grove.<br/>
    /// Building code is in <see cref="DepthsBuilding"/> as well, for nicer organization.
    /// </summary>
    internal static void Generate(GenerationProgress progress, GameConfiguration configuration) // configuration is for it to be used as a genpass, even if your IDE says it's useless
    {
        int biomeWidth = Main.maxTilesX; // Change this and the biome will adjust in size accordingly.
        
        progress.Message = "Digging the Depths...";
		progress.Set(0.0);

		int centerSizeHalved = Main.maxTilesX / 6; // Middle area with the buildings takes up 1/3rd of the world
        AddBaseTiles(0, Main.maxTilesY - 220, biomeWidth, 220, 20, centerSizeHalved, out var baseProgress); // Adds base tiles - Shale, Arquerite, Shalestone
		progress.Set(baseProgress / 2.0 + 0.5);

		for (int i = 0; i < 2; ++i) // Adds the two chasms, and their stalactites
        {
            ClearTunnel(0, Main.maxTilesY - 160, biomeWidth, 80, 0.32f, true); // Top one is bigger (as per the widenFactor of .32)
            AddStalactites(0, Main.maxTilesY - 160, biomeWidth, 15, 20, 30, 60);
            ClearTunnel(0, Main.maxTilesY - 80, biomeWidth, 80, 0.06f, false); // and the bottom one is smaller (widenFactor of .06)
            AddStalactites(0, Main.maxTilesY - 80, biomeWidth, 4, 8, 20, 50);
        }

        AddHolesBetweenTunnels(0, Main.maxTilesY - 160, biomeWidth, 120, 240); // Digs pits between the two chasms
        AddLiquidHoles(0, Main.maxTilesY - 200, biomeWidth, 160);

        int nightmareGroveSize = Main.maxTilesX / 6; // Each nightmare grove takes up 1/6th of the world, and non-grove is the rest
        AddNightmareGrove(nightmareGroveSize, 0);
        AddBuildings(Main.maxTilesX / 2 - centerSizeHalved, Main.maxTilesX / 2 + centerSizeHalved, Main.maxTilesY - 160); // Adds all of the buildings
		AddDepthsDecor(nightmareGroveSize);
	}

    internal static void SpecialGenerate(GenerationProgress progress, GameConfiguration configuration)
    {
        int biomeWidth = (WorldGen.drunkWorldGen || ModSupport.DepthsModCalling.FargoBoBW) ? (int)(Main.maxTilesX / 2) : Main.maxTilesX; // Change this and the biome will adjust in size accordingly.
        int x = 0;
        int side = 0;

		if (WorldGen.drunkWorldGen || ModSupport.DepthsModCalling.FargoBoBW)
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
                x = Main.maxTilesX - (int)(Main.maxTilesX / 2);
                TheDepthsWorldGen.DrunkDepthsLeft = false;
                TheDepthsWorldGen.DrunkDepthsRight = true;
            }
        }

        progress.Message = "Digging the depths...";
		progress.Set(0.0);

		int buildingArea = Main.maxTilesX / 6; // Middle area with the buildings takes up 1/3rd of the world
        AddBaseTiles(x, Main.maxTilesY - 220, biomeWidth, 220, 20, buildingArea, out var baseProgress); // Adds base tiles - Shale, Arquerite, Shalestone
		progress.Set(baseProgress / 2.0 + 0.5);

		for (int i = 0; i < 2; ++i) // Adds the two chasms, and their stalactites
        {
            ClearTunnel(x, Main.maxTilesY - 160, biomeWidth, 80, 0.32f, true); // Top one is bigger (as per the widenFactor of .32)
            AddStalactites(x, Main.maxTilesY - 160, biomeWidth, 15, 20, 30, 60);
            ClearTunnel(x, Main.maxTilesY - 80, biomeWidth, 80, 0.06f, false); // and the bottom one is smaller (widenFactor of .06)
            AddStalactites(x, Main.maxTilesY - 80, biomeWidth, 4, 8, 20, 50);
        }

        AddHolesBetweenTunnels(x, Main.maxTilesY - 160, biomeWidth, 120, 240); // Digs pits between the two chasms
        AddLiquidHoles(x, Main.maxTilesY - 200, biomeWidth, 160);
        SpecialSeedGen();

        progress.Message = "Growing bioluminecent plants in very dark areas";

        int nightmareGroveSize = WorldGen.drunkWorldGen ? Main.maxTilesX / 2 :  Main.maxTilesX / 6; // Each nightmare grove takes up 1/6th of the world, and non-grove is the rest
        AddNightmareGrove(nightmareGroveSize, side);
        AddDepthsDecor(nightmareGroveSize);
        PostSpecialSeedGen();

        progress.Message = "Building ruined homes...";

        if (Main.drunkWorld)
        {
            if (side is 0 or (-1))
                AddBuildings(60, buildingArea, Main.maxTilesY - 160); // Adds all of the buildings

            if (side is 0 or 1)
                AddBuildings(Main.maxTilesX - buildingArea, Main.maxTilesX - 60, Main.maxTilesY - 160);
        }
        else
        {
			AddBuildings(side == -1 ? Main.maxTilesX / 2 - buildingArea : Main.maxTilesX / 2, side == -1 ? Main.maxTilesX / 2 : Main.maxTilesX / 2 + buildingArea, Main.maxTilesY - 160); // Adds all of the buildings
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
    /// Spams holes full of liquid (water or quicksilver) throughout the biome.
    /// </summary>
    /// <param name="x">X position (left).</param>
    /// <param name="y">Y position (top).</param>
    /// <param name="width">Width of the area to add water holes to.</param>
    /// <param name="height">Height of the area to add water holes to.</param>
    private static void AddLiquidHoles(int x, int y, int width, int height)
    {
		for (int i = 0; i < Main.maxTilesX / 4200f * 300; ++i)
        {
			int placeX = x + WorldGen.genRand.Next(width);
			int placeY = y + WorldGen.genRand.Next(height);
			Tile tile = Main.tile[placeX, placeY];
			if (tile.HasTile)
                digTunnel(placeX, placeY, WorldGen.genRand.NextFloat(-2, 2f), WorldGen.genRand.NextFloat(-2f, 2f), WorldGen.genRand.Next(2, 15), 2, LiquidID.Water);
        }

		for (int i = 0; i < Main.maxTilesX / 2100f * 150; ++i)
		{
			int placeX = x + WorldGen.genRand.Next(width);
			int placeY = y + WorldGen.genRand.Next(height);
			Tile tile = Main.tile[placeX, placeY];
			if (tile.HasTile)
				digTunnel(placeX, placeY, WorldGen.genRand.NextFloat(-2, 2f), WorldGen.genRand.NextFloat(-2f, 2f), WorldGen.genRand.Next(2, 30), 4, LiquidID.Lava);
		}
	}

    private static void SpecialSeedGen() //Lion8cake's dinkie ass code
    {
		//Drunkseed remnant remover
		for (int x = (TheDepthsWorldGen.DrunkDepthsRight ? Main.maxTilesX / 2 : 0); x < (TheDepthsWorldGen.DrunkDepthsLeft ? Main.maxTilesX / 2 : Main.maxTilesX); x++)
		{
			for (int y = Main.maxTilesY - 300; y < Main.maxTilesY; y++)
			{
				if (Main.tile[x, y].TileType == TileID.ObsidianBrick || Main.tile[x, y].TileType == TileID.HellstoneBrick || Main.tile[x, y].TileType == TileID.Beds || Main.tile[x, y].TileType == TileID.Bathtubs || Main.tile[x, y].TileType == TileID.Tables || Main.tile[x, y].TileType == TileID.WorkBenches 
                    || Main.tile[x, y].TileType == TileID.Chairs || Main.tile[x, y].TileType == TileID.Platforms || Main.tile[x, y].TileType == TileID.Candelabras || Main.tile[x, y].TileType == TileID.GrandfatherClocks || Main.tile[x, y].TileType == TileID.Pianos || Main.tile[x, y].TileType == TileID.Bookcases 
                    || Main.tile[x, y].TileType == TileID.Hellforge || Main.tile[x, y].TileType == TileID.Chandeliers || Main.tile[x, y].TileType == TileID.Torches || Main.tile[x, y].TileType == 89 || Main.tile[x, y].TileType == TileID.Dressers || Main.tile[x, y].TileType == TileID.Candles 
                    || Main.tile[x, y].TileType == TileID.Statues || Main.tile[x, y].TileType == TileID.ClosedDoor || Main.tile[x, y].TileType == TileID.OpenDoor || Main.tile[x, y].TileType == TileID.TreeAsh || Main.tile[x, y].TileType == TileID.AshVines || Main.tile[x, y].TileType == TileID.HangingLanterns 
                    || Main.tile[x, y].TileType == TileID.Lamps || Main.tile[x, y].TileType == TileID.Banners || Main.tile[x, y].TileType == TileID.Painting2X3 || Main.tile[x, y].TileType == TileID.Painting3X2 || Main.tile[x, y].TileType == TileID.Painting3X3 || Main.tile[x, y].TileType == TileID.Painting4X3 
                    || Main.tile[x, y].TileType == TileID.Painting6X4)
				{
					Tile tile = Main.tile[x, y];
					tile.HasTile = false;
				}
				if (Main.tile[x, y].WallType == WallID.ObsidianBrickUnsafe || Main.tile[x, y].WallType == WallID.HellstoneBrickUnsafe)
				{
					Tile tile = Main.tile[x, y];
					tile.WallType = 0;
				}
			}
		}

		//Quicksilver Ocean
		if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
		{
			for (int n = 0; n < Main.maxTilesX * 2; ++n)
			{
				digTunnel(WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.35), (int)((double)Main.maxTilesX * 0.65)), WorldGen.genRand.Next(Main.maxTilesY - 150, Main.maxTilesY - 10), 1, 1, WorldGen.genRand.Next(5, 20), WorldGen.genRand.Next(5, 10), LiquidID.Lava);
				WorldGen.TileRunner(WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.35), (int)((double)Main.maxTilesX * 0.65)), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(5, 20), WorldGen.genRand.Next(5, 10), -2);
			}
			for (int k = Main.maxTilesY; k > Main.maxTilesY - 130; --k) //Barrier
			{
                if (!TheDepthsWorldGen.DrunkDepthsRight)
				    WorldGen.TileRunner((int)(Main.maxTilesX * 0.35), k, 20, 12, ModContent.TileType<ShaleBlock>(), addTile: true, overRide: true);
				if (!TheDepthsWorldGen.DrunkDepthsLeft)
					WorldGen.TileRunner((int)(Main.maxTilesX * 0.65), k, 20, 12, ModContent.TileType<ShaleBlock>(), addTile: true, overRide: true);
			}
			for (int i = (int)(Main.maxTilesX * 0.35); i < Main.maxTilesX * 0.65; ++i)
			{
				for (int j = Main.maxTilesY; j > Main.maxTilesY - 220; --j)
				{
					Main.tile[i, j].WallType = 0;
				}
			}
		}

		//Island, taken from vanilla
		if (WorldGen.remixWorldGen)
		{
			int num854 = (int)((double)Main.maxTilesX * 0.38);
			int num855 = (int)((double)Main.maxTilesX * 0.62);
			int num856 = num854;
			int num857 = Main.maxTilesY - 1;
			int num858 = Main.maxTilesY - 135;
			int num859 = Main.maxTilesY - 160;
			bool flag55 = false;
			for (; num857 < Main.maxTilesY - 1 || num856 < num855; num856++)
			{
				if (!flag55)
				{
					num857 -= WorldGen.genRand.Next(1, 4);
					if (num857 < num858)
					{
						flag55 = true;
					}
				}
				else if (num856 >= num855)
				{
					num857 += WorldGen.genRand.Next(1, 4);
					if (num857 > Main.maxTilesY - 1)
					{
						num857 = Main.maxTilesY - 1;
					}
				}
				else
				{
					if ((num856 <= Main.maxTilesX / 2 - 5 || num856 >= Main.maxTilesX / 2 + 5) && WorldGen.genRand.NextBool(4))
					{
						if (WorldGen.genRand.NextBool(3))
						{
							num857 += WorldGen.genRand.Next(-1, 2);
						}
						else if (WorldGen.genRand.NextBool(6))
						{
							num857 += WorldGen.genRand.Next(-2, 3);
						}
						else if (WorldGen.genRand.NextBool(8))
						{
							num857 += WorldGen.genRand.Next(-4, 5);
						}
					}
					if (num857 < num859)
					{
						num857 = num859;
					}
					if (num857 > num858)
					{
						num857 = num858;
					}
				}
				for (int num860 = num857; num860 > num857 - 20; num860--)
				{
					Main.tile[num856, num860].LiquidAmount = 0;
				}
				for (int num861 = num857; num861 < Main.maxTilesY; num861++)
                {
					Tile tile = Main.tile[num856, num861];
					tile.HasTile = true;
					Main.tile[num856, num861].TileType = (ushort)ModContent.TileType<ShaleBlock>();
				}
			}
		}
	}

    private static void PostSpecialSeedGen()
    {
		//Converting the tiles from nightmare to underworld is so much easier than the other way around due to how the quicksilver ocean and the depths roof works
		//Replaces shale and nightmare grass, replaces trees
		if (WorldGen.remixWorldGen && (WorldGen.drunkWorldGen || ModSupport.DepthsModCalling.FargoBoBW))
		{
			for (int k = (TheDepthsWorldGen.DrunkDepthsLeft ? Main.maxTilesX / 2 : 0); k < (TheDepthsWorldGen.DrunkDepthsRight ? Main.maxTilesX / 2 : Main.maxTilesX); k++)
			{
				for (int l = Main.maxTilesY - 300; l < Main.maxTilesY; l++)
				{
					if (Main.tile[k, l].TileType == ModContent.TileType<ShaleBlock>())
					{
						WorldGen.KillTile(k, l, false, false, false);
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
                        Main.tile[k, l].TileType = (ushort)TileID.Ash;
					}
					else if (Main.tile[k, l].TileType == ModContent.TileType<NightmareGrass>())
					{
						WorldGen.KillTile(k, l, false, false, false);
						Tile tile = Main.tile[k, l];
						tile.HasTile = true;
                        Main.tile[k, l].TileType = (ushort)TileID.AshGrass;
					}
				}
			}

			int num854 = (int)((double)Main.maxTilesX * 0.38);
			int num855 = (int)((double)Main.maxTilesX * 0.62);
			int num856 = num854;
			int num857 = Main.maxTilesY - 1;
			int num858 = Main.maxTilesY - 135;
			int num859 = Main.maxTilesY - 160;
			for (int num840 = num854; num840 < num855 + 15; num840++)
			{
				for (int num841 = Main.maxTilesY - 300; num841 < num858 + 20; num841++)
				{
					Main.tile[num840, num841].LiquidAmount = 0;
					if (Main.tile[num840, num841].TileType == 57 && Main.tile[num840, num841].HasTile && (!Main.tile[num840 - 1, num841 - 1].HasTile || !Main.tile[num840, num841 - 1].HasTile || !Main.tile[num840 + 1, num841 - 1].HasTile || !Main.tile[num840 - 1, num841].HasTile || !Main.tile[num840 + 1, num841].HasTile || !Main.tile[num840 - 1, num841 + 1].HasTile || !Main.tile[num840, num841 + 1].HasTile || !Main.tile[num840 + 1, num841 + 1].HasTile))
					{
						Main.tile[num840, num841].TileType = 633;
					}
				}
			}
			for (int num842 = num854; num842 < num855 + 15; num842++)
			{
				for (int num843 = Main.maxTilesY - 200; num843 < num858 + 20; num843++)
				{
					if (Main.tile[num842, num843].TileType == 633 && Main.tile[num842, num843].HasTile && !Main.tile[num842, num843 - 1].HasTile && WorldGen.genRand.NextBool(3))
					{
						WorldGen.TryGrowingTreeByType(634, num842, num843);
					}
				}
			}
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

            digTunnel(x, j, WorldGen.genRand.NextFloat(-2f, 2f), WorldGen.genRand.NextFloat(3, 9), WorldGen.genRand.Next(14, 18), 10);
        }
    }

    /// <summary>
    /// Calls <see cref="PlaceDepthsDecor(int, int)"/> on every Shale tile outside of the Nightmare Grove, per <paramref name="nightmareGroveSize"/>.
    /// </summary>
    /// <param name="nightmareGroveSize">Size of the nightmare grove, in tiles.</param>
    private static void AddDepthsDecor(int nightmareGroveSize)
    {
        if (WorldGen.remixWorldGen)
            nightmareGroveSize = Main.maxTilesX;
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

        Tile tile = Main.tile[i, j];
        if (!WorldGen.SolidTile(i, j + 1) && (tile.TileType != ModContent.TileType<QuartzBricks>() || tile.TileType != ModContent.TileType<ArqueriteBricks>())) // If no tile below and is not any depths' bricks...
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
    public static void AddBaseTiles(int x, int y, int width, int height, int fadeHeight, int middleAreaSizeHalved, out double progress)
    {
        FastNoiseLite noise = new(WorldGen._genRandSeed + _seedNumber++);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFractalType(FastNoiseLite.FractalType.PingPong);
        noise.SetFrequency(0.03f);
        noise.SetFractalGain(0.13f);
        noise.SetFractalLacunarity(3.95f);
        noise.SetFractalPingPongStrength(0.39f);
        progress = 0;
        for (int i = x; i < x + (width - 1); ++i)
        {
            progress = (double)i / (double)(Main.maxTilesX - 1);
            float modFadeHeight = fadeHeight * (noise.GetNoise(i, -100) + 1); // Increases or decreases the depth of the gradient
            int baseY = y - (int)modFadeHeight;

			//ModContent.GetInstance<TheDepths>().Logger.Debug(Main.maxTilesX);
			//ModContent.GetInstance<TheDepths>().Logger.Debug(i);

			for (int j = baseY; j < baseY + height + modFadeHeight; ++j)
            {
				//ModContent.GetInstance<TheDepths>().Logger.Debug(j);

                if (j >= Main.maxTilesY)
                    continue;

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

            for (int l = Main.maxTilesY; l > y - 60; l--)
            {
				if (l >= Main.maxTilesY)
					break;

				Tile tile = Main.tile[i, l];
                if (tile.LiquidType > -1)
                {
                    tile.LiquidType = -1;
                    tile.LiquidAmount = 0;
                }
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
						Main.tile[realX, y - j + 1].Clear(Terraria.DataStructures.TileDataType.Wall);
                        Main.tile[realX, y - j - 1].Clear(Terraria.DataStructures.TileDataType.Wall);
                    }
                    else
                    {
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
						Main.tile[realX, y + j + 1].Clear(Terraria.DataStructures.TileDataType.Wall);
                        Main.tile[realX, y + j - 1].Clear(Terraria.DataStructures.TileDataType.Wall);
					}
                    else
                    {
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
		if (WorldGen.remixWorldGen && !WorldGen.drunkWorldGen)
		{
			for (int i = (int)((double)Main.maxTilesX * 0.38); i < (int)((double)Main.maxTilesX * 0.62) + 15; ++i)
			{
                for (int j = Main.maxTilesY - 400; j < Main.maxTilesY; ++j)
                {
                    TryGivingShaleNightmare(i, j);
                }
            }
		}
		else
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
    }

    /// <summary>
    /// Grows Nightmare Grass on given shale, if any. Also grows trees and (tbd) foliage on successful placement.
    /// </summary>
    /// <param name="i">X position of the tile.</param>
    /// <param name="j">Y position of the tile.</param>
    private static void TryGivingShaleNightmare(int i, int j)
    {
        if (i >= Main.maxTilesX || j >= Main.maxTilesY)
            return;

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
            if (WorldGen.remixWorldGen ? WorldGen.genRand.NextBool() : WorldGen.genRand.NextBool(5)) // Try to place a tree, or...
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

            for (int y = j + 1; y < j + (height); y++)
            {
                if (WorldGen.SolidTile(i, y) || (j + height) >= Main.maxTilesY)
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

    /// <summary>
    /// A clone of WorldGen.digTunnel, Creates a simple cave that can either be filled with a liquid or set with air (return liquid as -1)
    /// Added by Lion8cake, the decompiled terraria junkie
    /// </summary>
    /// <param name="X">X position of the tunnel</param>
    /// <param name="Y">Y position of the tunnel</param>
    /// <param name="xDir">X direction of the tunnel</param>
    /// <param name="yDir">Y direction of the tunnel</param>
    /// <param name="Steps">How mant steps the tunnel has to generate</param>
    /// <param name="Size">How bug the tunnel is</param>
    /// <param name="liquid">the liquid ID of what the cave is filled with</param>
    /// <returns></returns>
	public static Vector2D digTunnel(double X, double Y, double xDir, double yDir, int Steps, int Size, int liquid = -1)
	{
		double num = X;
		double num2 = Y;
		try
		{
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = Size;
			num = Utils.Clamp(num, num5 + 1.0, (double)Main.maxTilesX - num5 - 1.0);
			num2 = Utils.Clamp(num2, num5 + 1.0, (double)Main.maxTilesY - num5 - 1.0);
			for (int i = 0; i < Steps; i++)
			{
				for (int j = (int)(num - num5); (double)j <= num + num5; j++)
				{
					for (int k = (int)(num2 - num5); (double)k <= num2 + num5; k++)
					{
						if (Math.Abs((double)j - num) + Math.Abs((double)k - num2) < num5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.005) && j >= 0 && j < Main.maxTilesX && k >= 0 && k < Main.maxTilesY)
						{
                            Tile tile = Main.tile[j, k];
                            tile.HasTile = false;
							if (liquid > -1)
							{
								tile.LiquidAmount = byte.MaxValue;
                                tile.LiquidType = liquid;
							}
						}
					}
				}
				num5 += (double)WorldGen.genRand.Next(-50, 51) * 0.03;
				if (num5 < (double)Size * 0.6)
				{
					num5 = (double)Size * 0.6;
				}
				if (num5 > (double)(Size * 2))
				{
					num5 = Size * 2;
				}
				num3 += (double)WorldGen.genRand.Next(-20, 21) * 0.01;
				num4 += (double)WorldGen.genRand.Next(-20, 21) * 0.01;
				if (num3 < -1.0)
				{
					num3 = -1.0;
				}
				if (num3 > 1.0)
				{
					num3 = 1.0;
				}
				if (num4 < -1.0)
				{
					num4 = -1.0;
				}
				if (num4 > 1.0)
				{
					num4 = 1.0;
				}
				num += (xDir + num3) * 0.6;
				num2 += (yDir + num4) * 0.6;
			}
		}
		catch
		{
		}
		return new Vector2D(num, num2);
	}
}
