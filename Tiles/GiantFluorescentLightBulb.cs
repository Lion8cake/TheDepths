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
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(140, 165, 170));
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

		public override bool CanDrop(int i, int j)
		{
			return false;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<Items.Placeable.GiantFluorescentLightBulb>());
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int x = i - tile.TileFrameX / 18 % 2;
			int y = j - tile.TileFrameY / 18 % 2;
			for (int m = x; m < x + 2; m++)
			{
				for (int n = y; n < y + 2; n++)
				{
					tile = Main.tile[m, n];
					if (!tile.HasTile)
					{
						continue;
					}
					if (tile.TileFrameX < 18 * 2)
					{
						tile = Main.tile[m, n];
						tile.TileFrameX += (short)(18 * 2);
					}
					else
					{
						tile = Main.tile[m, n];
						tile.TileFrameX -= (short)(18 * 2);
					}
				}
			}
			if (!Wiring.running)
			{
				return;
			}
			for (int k = 0; k < 2; k++)
			{
				for (int l = 0; l < 2; l++)
				{
					Wiring.SkipWire(x + k, y + l);
				}
			}
		}
	}
}