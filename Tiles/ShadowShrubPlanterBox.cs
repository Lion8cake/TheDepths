using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System.Collections.Generic;

namespace TheDepths.Tiles
{
	public class ShadowShrubPlanterBox : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			AddMapEntry(new Color(191, 142, 111));
			ItemDrop = ModContent.ItemType<Items.Placeable.ShadowShrubPlanterBox>();
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.PlanterBox };
		}

		public override bool Slope(int i, int j)
		{
			return false;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile left = Main.tile[i - 1, j];
			Tile right = Main.tile[i + 1, j];
			int x = i - Main.tile[i, j].TileFrameX / 18;
			int y = j - Main.tile[i, j].TileFrameY / 18;
			if (WorldGen.InWorld(x, y))
			{
				if (left.TileType == Type && right.TileType == Type)
				{
					Main.tile[i, j].TileFrameX = 18;
				}
				else if (left.TileType == Type && right.TileType != Type)
				{
					Main.tile[i, j].TileFrameX = 36;
				}
				else if (left.TileType != Type && right.TileType == Type)
				{
					Main.tile[i, j].TileFrameX = 0;
				}
				else
				{
					Main.tile[i, j].TileFrameX = 54;
				}
			}
			return false;
		}
	}
}