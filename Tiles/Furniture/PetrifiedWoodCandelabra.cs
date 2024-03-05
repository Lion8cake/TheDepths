using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;

namespace TheDepths.Tiles.Furniture
{
    public class PetrifiedWoodCandelabra : ModTile
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
            DustType = ModContent.DustType<PetrifiedWoodDust>();
            AdjTiles = new int[] { TileID.Torches };
			AddMapEntry(new Color(70, 65, 55), CreateMapEntryName());
			HitSound = SoundID.Tink;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Furniture.PetrifiedWoodCandelabra>());
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

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX == 0)
			{
				r = 1f / 1.5f;
				g = 0.95f / 1.75f;
				b = 0.65f / 1.75f;
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color = new Color(60, 60, 60, 0);
			int frameX = Main.tile[i, j].TileFrameX;
			int frameY = Main.tile[i, j].TileFrameY;
			int width = 18;
			int offsetY = 2;
			int height = 18;
			int offsetX = 1;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 7; k++)
			{
				float x = (float)Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
				float y = (float)Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Flame").Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X + offsetX) - (width - 16f) / 2f + x, (float)(j * 16 - (int)Main.screenPosition.Y + offsetY) + y) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
		}
	}
}