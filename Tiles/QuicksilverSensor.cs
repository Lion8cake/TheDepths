using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TheDepths.Tiles
{
	public class QuicksilverSensor : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(85, 96, 102), name);
		}

		public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}

        public override bool Slope(int i, int j)
        {
			return false;
        }

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			int x = i - Main.tile[i, j].TileFrameX / 18 % 1;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 1;
			if (Worldgen.TheDepthsWorldGen.isWorldDepths && !Main.drunkWorld || (Worldgen.TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(x) < Main.maxTilesX / 2 || Worldgen.TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(x) > Main.maxTilesX / 2) && Main.drunkWorld)
			{
				if ((Main.tile[x, y].LiquidType == LiquidID.Lava) && Main.tile[x, y].TileFrameX == 0)
				{
					Wiring.TripWire(x, y, 1, 1);
				}
				if (!(Main.tile[x, y].LiquidType == LiquidID.Lava) && Main.tile[x, y].TileFrameX == 18)
				{
					Wiring.TripWire(x, y, 1, 1);
				}

				if (Main.tile[x, y].LiquidType == LiquidID.Lava)
				{
					Main.tile[x, y].TileFrameX = 18;
				}
				if (Main.tile[x, y].LiquidType != LiquidID.Lava)
				{
					Main.tile[x, y].TileFrameX = 0;
				}
			}
			else
            {
				if (Main.tile[x, y].TileFrameX == 18)
				{
					Wiring.TripWire(x, y, 1, 1);
				}
				Main.tile[x, y].TileFrameX = 0;
            }
        }
    }
}