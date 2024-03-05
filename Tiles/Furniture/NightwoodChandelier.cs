using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Reflection;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using static Terraria.GameContent.Drawing.TileDrawing;

namespace TheDepths.Tiles.Furniture
{
    public class NightwoodChandelier : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.Origin = new Point16(1, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(30, 32, 36), CreateMapEntryName());
            TileID.Sets.DisableSmartCursor[Type] = true;
            DustType = ModContent.DustType<Dusts.NightWoodDust>();
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Furniture.NightwoodChandelier>());
		}

		public override void HitWire(int i, int j)
		{
			int x = i - Main.tile[i, j].TileFrameX / 18 % 3;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 3;
			for (int l = x; l < x + 3; l++)
			{
				for (int m = y; m < y + 3; m++)
				{
					if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type)
					{
						if (Main.tile[l, m].TileFrameX < 54)
						{
							Main.tile[l, m].TileFrameX += 54;
						}
						else
						{
							Main.tile[l, m].TileFrameX -= 54;
						}
					}
				}
			}
			if (Wiring.running)
			{
				for (int q = 0; q < 3; q++)
				{
					for (int z = 0; z < 3; z++)
					{
						Wiring.SkipWire(x + q, y + z);
					}
				}
			}
			for (int q = 0; q < 3; q++)
			{
				for (int z = 0; z < 3; z++)
				{
					NetMessage.SendTileSquare(-1, x + q, y + z, 3);
				}
			}
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			bool intoRenderTargets = true;
			bool flag = intoRenderTargets || Main.LightingEveryFrame;

			if (Main.tile[i, j].TileFrameX % 54 == 0 && Main.tile[i, j].TileFrameY % 54 == 0 && flag)
			{
				Main.instance.TilesRenderer.AddSpecialPoint(i, j, 5);
			}

			return false;
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
	}
}