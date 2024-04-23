using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using static Terraria.GameContent.Drawing.TileDrawing;

namespace TheDepths.Tiles.Furniture
{
    public class PetrifiedWoodChandelier : ModTile
    {
		public virtual Color FlameColor => new Color(60, 60, 60, 0);
		public virtual float FlameJitterMultX => 0.15f;
		public virtual float FlameJitterMultY => 0.35f;
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
			AddMapEntry(new Color(70, 65, 55), CreateMapEntryName());
			HitSound = SoundID.Tink;
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = ModContent.DustType<Dusts.PetrifiedWoodDust>();
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Furniture.PetrifiedWoodChandelier>());
			Coordinates = new();
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
				//Wiring.SkipWire(x, y);
				//Wiring.SkipWire(x, y + 1);
				//Wiring.SkipWire(x, y + 2);
				//Wiring.SkipWire(x + 1, y);
				//Wiring.SkipWire(x + 1, y + 1);
				//Wiring.SkipWire(x + 1, y + 2);
				//Wiring.SkipWire(x + 2, y);
				//Wiring.SkipWire(x + 2, y + 1);
				//Wiring.SkipWire(x + 2, y + 2);
			}
			for (int q = 0; q < 3; q++)
			{
				for (int z = 0; z < 3; z++)
				{
					NetMessage.SendTileSquare(-1, x + q, y + z, 3);
				}
			}
			//NetMessage.SendTileSquare(-1, x, y + 1, 3);
			//NetMessage.SendTileSquare(-1, x, y + 2, 3);
			//NetMessage.SendTileSquare(-1, x + 1, y, 3);
			//NetMessage.SendTileSquare(-1, x + 1, y + 1, 3);
			//NetMessage.SendTileSquare(-1, x + 1, y + 2, 3);
			//NetMessage.SendTileSquare(-1, x + 2, y, 3);
			//NetMessage.SendTileSquare(-1, x + 2, y + 1, 3);
			//NetMessage.SendTileSquare(-1, x + 2, y + 2, 3);
		}

		public List<Point> Coordinates = new List<Point>();

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Point p = new(i, j);
			if (Coordinates.Contains(p)) Coordinates.Remove(p);
		}
		public void DrawMultiTileVines(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			Tile tile = Main.tile[i, j];
			if (tile != null && tile.HasTile)
			{
				int sizeX = 3;
				int sizeY = 3;
				DrawMultiTileVinesInWind(unscaledPosition, zero, i, j, sizeX, sizeY, spriteBatch);
			}
		}

		private void DrawMultiTileVinesInWind(Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY, SpriteBatch spriteBatch)
		{
			double _sunflowerWindCounter = (double)typeof(TileDrawing).GetField("_sunflowerWindCounter", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			UnifiedRandom _rand = (UnifiedRandom)typeof(TileDrawing).GetField("_rand", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			bool _isActiveAndNotPaused = (bool)typeof(TileDrawing).GetField("_isActiveAndNotPaused", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);

			long seedX = (long)topLeftY << 32;
			long seedY = (long)(ulong)topLeftX;
			ulong randSeed = Main.TileFrameSeed ^ (ulong)(seedX | seedY);

			float windCycle = (float)typeof(TileDrawing).GetMethod("GetWindCycle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Main.instance.TilesRenderer, new object[] { topLeftX, topLeftY, _sunflowerWindCounter });
			float num = windCycle;
			int totalPushTime = 60;
			float pushForcePerFrame = 1.26f;
			float highestWindGridPushComplex = (float)typeof(TileDrawing).GetMethod("GetHighestWindGridPushComplex", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Main.instance.TilesRenderer, new object[] { topLeftX, topLeftY, sizeX, sizeY, totalPushTime, pushForcePerFrame, 3, true });
			windCycle += highestWindGridPushComplex;
			new Vector2(sizeX * 16 * 0.5f, 0f);
			Vector2 vector = new Vector2(topLeftX * 16 - (int)screenPosition.X + sizeX * 16f * 0.5f, topLeftY * 16 - (int)screenPosition.Y) + offSet;
			Tile tile = Main.tile[topLeftX, topLeftY];
			int type = tile.TileType;
			Vector2 vector2 = new(0f, -2f);
			vector += vector2;
			float num4 = 1f;
			bool flag2 = false;
			float num2 = 0.15f;
			float? num3 = 1f;
			float num5 = 0f;
			if (flag2)
			{
				vector += new Vector2(0f, 16f);
			}
			num2 *= -1f;
			if (!WorldGen.InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY))
			{
				windCycle -= num;
			}
			for (int i = topLeftX; i < topLeftX + sizeX; i++)
			{
				for (int j = topLeftY; j < topLeftY + sizeY; j++)
				{
					Tile tile2 = Main.tile[i, j];
					ushort type2 = tile2.TileType;
					if (type2 != type || !IsVisible(tile2))
					{
						return;
					}
					Math.Abs((i - topLeftX + 0.5f) / sizeX - 0.5f);
					short tileFrameX = tile2.TileFrameX;
					short tileFrameY = tile2.TileFrameY;
					float num7 = (j - topLeftY + 1) / (float)sizeY;
					if (num7 == 0f)
					{
						num7 = 0.1f;
					}
					if (num3.HasValue)
					{
						num7 = num3.Value;
					}
					if (flag2 && j == topLeftY)
					{
						num7 = 0f;
					}
					Main.instance.TilesRenderer.GetTileDrawData(i, j, tile2, type2, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out _, out _, out _);
					bool flag3 = _rand.NextBool(4);
					Color tileLight = Lighting.GetColor(i, j);
					typeof(TileDrawing).GetMethod("DrawAnimatedTile_AdjustForVisionChangers", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Main.instance.TilesRenderer, new object[] { i, j, tile2, type2, tileFrameX, tileFrameY, tileLight, flag3 });
					tileLight = (Color)typeof(TileDrawing).GetMethod("DrawTiles_GetLightOverride", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Main.instance.TilesRenderer, new object[] { j, i, tile2, type2, tileFrameX, tileFrameY, tileLight });
					if (_isActiveAndNotPaused && flag3)
					{
						typeof(TileDrawing).GetMethod("DrawTiles_EmitParticles", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Main.instance.TilesRenderer, new object[] { j, i, tile2, type2, tileFrameX, tileFrameY, tileLight });
					}
					Vector2 vector3 = new Vector2(i * 16 - (int)screenPosition.X, j * 16 - (int)screenPosition.Y + tileTop) + offSet;
					vector3 += vector2;
					Vector2 vector4 = new(windCycle * num4, Math.Abs(windCycle) * num5 * num7);
					Vector2 vector5 = vector - vector3;
					Texture2D tileDrawTexture = Main.instance.TilesRenderer.GetTileDrawTexture(tile2, i, j);
					if (tileDrawTexture != null)
					{
						Vector2 vector6 = vector + new Vector2(0f, vector4.Y);
						Rectangle rectangle = new(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight);
						float rotation = windCycle * num2 * num7;
						Main.spriteBatch.Draw(tileDrawTexture, vector6, (Rectangle?)rectangle, tileLight, rotation, vector5, 1f, tileSpriteEffect, 0f);

						for (int q = 0; q < 7; q++)
						{
							float x = Utils.RandomInt(ref randSeed, -10, 11) * FlameJitterMultX;
							float y = Utils.RandomInt(ref randSeed, -10, 1) * FlameJitterMultY;
							spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Flame").Value, vector6 + new Vector2(x, y), (Rectangle?)rectangle, FlameColor, rotation, vector5, 1f, tileSpriteEffect, 0f);
						}
					}
				}
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.tile[i, j].TileFrameX % 54 == 0 && Main.tile[i, j].TileFrameY % 54 == 0)
			{
				Point p = new(i, j);
				if (!Coordinates.Contains(p)) Coordinates.Add(p);
			}
			return false;
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
	}
}