using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheDepths.Tiles
{
    public class GiantFluorescentLightBulb : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileWaterDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			AddMapEntry(new Color(140, 165, 170));
			TileObjectData.addTile(Type);
        }

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX > 18 * 2)
			{
				r = 14.0f;
				g = 16.5f;
				b = 17.0f;
			}
		}

		public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}

		public override void HitWire(int i, int j)
		{
			int x = i - Main.tile[i, j].TileFrameX / 18 % 2;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 2;
			for (int m = x; m < x + 2; m++)
			{
				for (int n = y; n < y + 2; n++)
				{
					if (Main.tile[m, n].HasTile && Main.tile[m, n].TileType == Type)
					{
						if (Main.tile[m, n].TileFrameX < 18 * 2)
						{
							Main.tile[m, n].TileFrameX += (short)(18 * 2);
						}
						else if (Main.tile[m, n].TileFrameX > 18 * 2)
						{
							Main.tile[m, n].TileFrameX -= (short)(18 * 2);
						}
					}
				}
			}
		}
	}
}