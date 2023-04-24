using Terraria.Localization;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using ReLogic.Content;

namespace TheDepths.Tiles
{
	public class FluorescentLightBulb : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileWaterDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 18 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);

			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.CoordinateHeights = new int[] { 18 };
			TileObjectData.newAlternate.CoordinateWidth = 16;
			TileObjectData.newAlternate.CoordinatePadding = 0;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(Type);

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AddMapEntry(new Color(140, 165, 170));
			DustType = 0;
			ItemDrop = ModContent.ItemType<Items.Placeable.FluorescentLightBulb>();
			TileID.Sets.DisableSmartCursor[Type] = true;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX >= 18) {
				r = 1.40f;
				g = 1.65f;
				b = 1.70f;
			}
		}

		public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}

		public override void HitWire(int i, int j)
        {
			int x = i - Main.tile[i, j].TileFrameX / 18 % 1;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 1;
			for (int m = x; m < x + 1; m++)
			{
				for (int n = y; n < y + 1; n++)
				{
					if (Main.tile[m, n].HasTile && Main.tile[m, n].TileType == Type)
					{
						if (Main.tile[m, n].TileFrameX < 18)
						{
							Main.tile[m, n].TileFrameX += 18;
						}
						else
                        {
							Main.tile[m, n].TileFrameX -= 18;
						}
					}
				}
			}
		}
    }
}