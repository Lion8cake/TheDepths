using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
    public class NightmareGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.Grass[Type] = true;
            TileID.Sets.ChecksForMerge[Type] = true;
            TileID.Sets.ForcedDirtMerging[Type] = true;
            TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
            DustType = ModContent.DustType<NightDust>();
            AddMapEntry(new Color(43, 28, 83));
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.ShaleBlock>());
        }

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (drawData.tileFrameX >= 0 && drawData.tileFrameY >= 0)
			{
				Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
			}
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}

			Vector2 pos = new Vector2(i * 16, j * 16) + zero - Main.screenPosition;
			var frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			var halfFrame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 8);
			Texture2D tex = Mod.Assets.Request<Texture2D>("Tiles/NightmareGrass_Glow").Value;
			if (tile.Slope == SlopeType.Solid && !tile.IsHalfBlock)
			{
				Main.spriteBatch.Draw(tex, pos, frame, Color.White);
			}
			else if (tile.IsHalfBlock)
			{
				pos = new Vector2(i * 16, (j * 16) + 8) + zero - Main.screenPosition;
				Main.spriteBatch.Draw(tex, pos, halfFrame, Color.White);
			}
			else
			{
				Vector2 screenOffset = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen)
				{
					screenOffset = Vector2.Zero;
				}
				Vector2 vector = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + screenOffset;
				int slopeType = (int)tile.Slope;
				int num5 = 2;
				int addFrY = Main.tileFrame[Type] * 90;
				int addFrX = 0;
				for (int q = 0; q < 8; q++)
				{
					int num6 = q * -2;
					int num7 = 16 - q * 2;
					int num8 = 16 - num7;
					int num9;
					switch (slopeType)
					{
						case 1:
							num6 = 0;
							num9 = q * 2;
							num7 = 14 - q * 2;
							num8 = 0;
							break;
						case 2:
							num6 = 0;
							num9 = 16 - q * 2 - 2;
							num7 = 14 - q * 2;
							num8 = 0;
							break;
						case 3:
							num9 = q * 2;
							break;
						default:
							num9 = 16 - q * 2 - 2;
							break;
					}
					Main.spriteBatch.Draw(tex, vector + new Vector2(num9, q * num5 + num6), (Rectangle?)new Rectangle(tile.TileFrameX + addFrX + num9, tile.TileFrameY + addFrY + num8, num5, num7), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
				int num10 = ((slopeType <= 2) ? 14 : 0);
				Main.spriteBatch.Draw(tex, vector + new Vector2(0f, num10), (Rectangle?)new Rectangle(tile.TileFrameX + addFrX, tile.TileFrameY + addFrY + num10, 16, 2), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.43f;
            g = 0.28f;
            b = 0.83f;
        }

        public override void RandomUpdate(int i, int j)
        {
			int minI = i - 1;
			int maxI = i + 2;
			int minJ = j - 1;
			int maxJ = j + 2;

			if (!WorldGen.InWorld(i, j, 10))
			{
				return;
			}
			if (j > Main.rockLayer)
			{
				int type = Main.tile[i, j].TileType;
				int num12 = -1;
				int num19 = type;
				int num20 = -1;
				int num = ModContent.TileType<ShaleBlock>();
				int num18 = ModContent.TileType<NightmareGrass_Foliage>();
				int maxValue = 2;
				bool flag = false;
				if (num18 != -1 && !Main.tile[i, minJ].HasTile && WorldGen.genRand.Next(maxValue) == 0)
				{
					flag = true;
					if (WorldGen.PlaceTile(i, minJ, num18, mute: true))
					{
						Main.tile[i, minJ].CopyPaintAndCoating(Main.tile[i, j]);
					}
					if (Main.netMode == 2 && Main.tile[i, minJ].HasTile)
					{
						NetMessage.SendTileSquare(-1, i, minJ);
					}
				}
				if (num != -1)
				{
					bool flag3 = false;
					TileColorCache color = Main.tile[i, j].BlockColorAndCoating();
					for (int k = minI; k < maxI; k++)
					{
						for (int l = minJ; l < maxJ; l++)
						{
							if (!WorldGen.InWorld(k, l, 10) || (i == k && j == l) || !Main.tile[k, l].HasTile)
							{
								continue;
							}
							if (Main.tile[k, l].TileType == num)
							{
								WorldGen.SpreadGrass(k, l, num, num19, repeat: false, color);
								if (Main.tile[k, l].TileType == num19)
								{
									WorldGen.SquareTileFrame(k, l);
									flag3 = true;
								}
							}
							else if (num12 > -1 && num20 > -1 && Main.tile[k, l].TileType == num12)
							{
								WorldGen.SpreadGrass(k, l, num12, num20, repeat: false, color);
								if (Main.tile[k, l].TileType == num20)
								{
									WorldGen.SquareTileFrame(k, l);
									flag3 = true;
								}
							}
						}
					}
					if (Main.netMode == 2 && flag3)
					{
						NetMessage.SendTileSquare(-1, i, j, 3);
					}
				}
			}
			else
			{
				int num2 = Main.tile[i, j].TileType;

				if (!Main.tile[i, minJ].HasTile && WorldGen.genRand.Next(10) == 0)
				{
					WorldGen.PlaceTile(i, minJ, ModContent.TileType<NightmareGrass_Foliage>(), mute: true);
					if (Main.tile[i, minJ].HasTile)
					{
						Main.tile[i, minJ].CopyPaintAndCoating(Main.tile[i, j]);
					}
					if (Main.netMode == 2 && Main.tile[i, minJ].HasTile)
					{
						NetMessage.SendTileSquare(-1, i, minJ);
					}
				}
				TileColorCache color2 = Main.tile[i, j].BlockColorAndCoating();
				bool flag6 = false;
				for (int num3 = minI; num3 < maxI; num3++)
				{
					for (int num4 = minJ; num4 < maxJ; num4++)
					{
						if ((i != num3 || j != num4) && Main.tile[num3, num4].HasTile && Main.tile[num3, num4].TileType == ModContent.TileType<ShaleBlock>())
						{
							WorldGen.SpreadGrass(num3, num4, ModContent.TileType<ShaleBlock>(), num2, repeat: false, color2);
							if (Main.tile[num3, num4].TileType == num2)
							{
								WorldGen.SquareTileFrame(num3, num4);
								flag6 = true;
							}
						}
					}
				}
				if (Main.netMode == 2 && flag6)
				{
					NetMessage.SendTileSquare(-1, i, j, 3);
				}
			}

			Tile tile = Main.tile[i, j];
			if (Worldgen.TheDepthsWorldGen.GrowMoreVines(i, j))
			{
				int maxValue6 = 70;
				tile = Main.tile[i, j];
				if (tile.TileType == ModContent.TileType<NightmareVines>())
				{
					maxValue6 = 7;
				}
				if (WorldGen.genRand.Next(maxValue6) == 0)
				{
					tile = Main.tile[i, j + 1];
					if (!tile.HasTile)
					{
						tile = Main.tile[i, j + 1];
						if (tile.LiquidType != LiquidID.Lava)
						{
							bool flag3 = false;
							for (int num41 = j; num41 > j - 10; num41--)
							{
								tile = Main.tile[i, num41];
								if (tile.BottomSlope)
								{
									flag3 = false;
									break;
								}
								tile = Main.tile[i, num41];
								if (tile.HasTile)
								{
									tile = Main.tile[i, num41];
									if (tile.TileType == Type)
									{
										tile = Main.tile[i, num41];
										if (!tile.BottomSlope)
										{
											flag3 = true;
											break;
										}
									}
								}
							}
							if (flag3)
							{
								int num42 = j + 1;
								tile = Main.tile[i, num42];
								tile.TileType = (ushort)ModContent.TileType<NightmareVines>();
								tile = Main.tile[i, num42];
								tile.HasTile = true;
								tile = Main.tile[i, num42];
								tile.CopyPaintAndCoating(Main.tile[i, j]);
								WorldGen.SquareTileFrame(i, num42);
								if (Main.netMode == 2)
								{
									NetMessage.SendTileSquare(-1, i, num42);
								}
							}
						}
					}
				}
			}
		}

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<ShaleBlock>();
            }
        }
    }
}