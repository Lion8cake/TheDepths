using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;

namespace TheDepths.Tiles.Furniture
{
    public class NightwoodCandelabra : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = ModContent.DustType<NightWoodDust>();
            AdjTiles = new int[] { TileID.Torches };
            AddMapEntry(new Color(30, 32, 36), CreateMapEntryName());
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Furniture.NightwoodCandelabra>());
		}

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0)
            {
                r = 0.55f;
                g = 0.31f;
                b = 1.2f;
            }
        }

		public override void HitWire(int i, int j)
		{
			int x = i - Main.tile[i, j].TileFrameX / 18 % 2;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 2;
			for (int l = x; l < x + 2; l++)
			{
				for (int m = y; m < y + 2; m++)
				{
					if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type)
					{
						if (Main.tile[l, m].TileFrameX < 36)
						{
							Main.tile[l, m].TileFrameX += 36;
						}
						else
						{
							Main.tile[l, m].TileFrameX -= 36;
						}
					}
				}
			}
			if (Wiring.running)
			{
				Wiring.SkipWire(x, y);
				Wiring.SkipWire(x, y + 1);
				Wiring.SkipWire(x + 1, y);
				Wiring.SkipWire(x + 1, y + 1);
			}
			NetMessage.SendTileSquare(-1, x, y, 2);
			NetMessage.SendTileSquare(-1, x, y + 1, 2);
			NetMessage.SendTileSquare(-1, x + 1, y, 2);
			NetMessage.SendTileSquare(-1, x + 1, y + 1, 2);
		}
	}
}