using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;
using static Terraria.WorldGen;

namespace TheDepths.Tiles.Trees
{
    public class NightSapling : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<NightmareGrass>() };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawFlipHorizontal = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleMultiplier = 3;

            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(42, 43, 51), name);

            //TileID.Sets.TreeSapling[Type] = true;
            TileID.Sets.CommonSapling[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;

            DustType = ModContent.DustType<NightDust>();

            AdjTiles = new int[] { TileID.Saplings };
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

		public override void RandomUpdate(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (tile.HasUnactuatedTile)
			{
				if (j > Main.rockLayer)
				{
					if (WorldGen.genRand.NextBool(5))
					{
						AttemptToGrowNightmareFromSapling(i, j);
					}
				}
				else
				{
					if (WorldGen.genRand.NextBool(20))
					{
						AttemptToGrowNightmareFromSapling(i, j);
					}
				}
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects)
        {
            if (i % 2 == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
        }

		public static bool AttemptToGrowNightmareFromSapling(int x, int y)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return false;
			}
			if (!InWorld(x, y, 2))
			{
				return false;
			}
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.HasTile)
			{
				return false;
			}
			bool flag = DepthsModTree.GrowModdedTreeWithSettings(x, y, NightwoodTree.Tree_Nightmare);
			if (flag && PlayerLOS(x, y))
			{
				GrowNightmareTreeFXCheck(x, y);
			}
			return flag;
		}

		public static void GrowNightmareTreeFXCheck(int x, int y)
		{
			int treeHeight = 1;
			for (int num = -1; num > -100; num--)
			{
				Tile tile = Main.tile[x, y + num];
				if (!tile.HasTile || !TileID.Sets.GetsCheckedForLeaves[tile.TileType])
				{
					break;
				}
				treeHeight++;
			}
			for (int i = 1; i < 5; i++)
			{
				Tile tile2 = Main.tile[x, y + i];
				if (tile2.HasTile && TileID.Sets.GetsCheckedForLeaves[tile2.TileType])
				{
					treeHeight++;
					continue;
				}
				break;
			}
			if (treeHeight > 0)
			{
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 1, x, y, treeHeight, ModContent.GoreType<PetrifiedTreeLeaf>());
				}
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					WorldGen.TreeGrowFX(x, y, treeHeight, ModContent.GoreType<PetrifiedTreeLeaf>());
				}
			}
		}
	}
}