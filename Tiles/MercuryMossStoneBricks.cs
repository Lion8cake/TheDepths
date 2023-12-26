using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
    public class MercuryMossStoneBricks : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
			TileID.Sets.tileMossBrick[Type] = true;
            TileID.Sets.ChecksForMerge[Type] = true;
            DustType = ModContent.DustType<Dusts.MercuryMoss>();
			AddMapEntry(new Color(119, 135, 162));
			RegisterItemDrop(ItemID.GrayBrick);
        }

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
            if (Main.rand.NextFloat() < 0.01f)
            {
                Vector2 position = new Vector2(i * 16, j * 16);
                Dust dust = Main.dust[Dust.NewDust(position, 4, 4, 261, 0f, 0f, 0, new Color(119, 135, 162), 1f)];
                dust.noGravity = true;
            }
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
			Texture2D tex = Mod.Assets.Request<Texture2D>("Tiles/MercuryMossStoneBricks_Glow").Value;
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

		public override void RandomUpdate(int i, int j)
		{
			Tile Blockpos = Main.tile[i, j];
			Tile left = Main.tile[i + 1, j];
			Tile right = Main.tile[i - 1, j];
			Tile down = Main.tile[i, j + 1];
			Tile up = Main.tile[i, j - 1];
			short framing = (short)(WorldGen.genRand.Next(3) * 18);
			if (WorldGen.genRand.NextBool(6) && !Blockpos.IsHalfBlock && !Blockpos.BottomSlope && !Blockpos.LeftSlope && !Blockpos.RightSlope && !Blockpos.TopSlope)
			{
				if (!left.HasTile && left.LiquidAmount == 0)
				{
					WorldGen.PlaceTile(i + 1, j, ModContent.TileType<MercuryMoss_Foliage>(), mute: true);
					left.TileFrameY = (short)(108 + framing);
					left.CopyPaintAndCoating(Blockpos);
				}
				if (!right.HasTile && right.LiquidAmount == 0)
				{
					WorldGen.PlaceTile(i - 1, j, ModContent.TileType<MercuryMoss_Foliage>(), mute: true);
					right.TileFrameY = (short)(162 + framing);
					right.CopyPaintAndCoating(Blockpos);
				}
				if (!down.HasTile && down.LiquidAmount == 0)
				{
					WorldGen.PlaceTile(i, j + 1, ModContent.TileType<MercuryMoss_Foliage>(), mute: true);
					down.TileFrameY = (short)(54 + framing);
					down.CopyPaintAndCoating(Blockpos);
				}
				if (!up.HasTile && up.LiquidAmount == 0)
				{
					WorldGen.PlaceTile(i, j - 1, ModContent.TileType<MercuryMoss_Foliage>(), mute: true);
					up.TileFrameY = framing;
					up.CopyPaintAndCoating(Blockpos);
				}
			}

			Tile tile;
			int num = i - 1;
			int num11 = i + 2;
			int num22 = j - 1;
			int num33 = j + 2;

			tile = Main.tile[i, j];
			int type2 = tile.TileType;
			bool flag = false;
			TileColorCache color = tile.BlockColorAndCoating();
			for (int num8 = num; num8 < num11; num8++)
			{
				for (int num9 = num22; num9 < num33; num9++)
				{
					if (i == num8 && j == num9)
					{
						continue;
					}
					tile = Main.tile[num8, num9];
					if (!tile.HasTile)
					{
						continue;
					}
					tile = Main.tile[num8, num9];
					if (tile.TileType != 1)
					{
						tile = Main.tile[num8, num9];
						if (tile.TileType != 38)
						{
							continue;
						}
					}
					tile = Main.tile[num8, num9];
					int type3 = tile.TileType;
					int? num10 = TheDepthsModSystem.MossConversion(type2, type3);
					if (num10 != null)
					{
						int num100 = (int)num10;
						int i2 = num8;
						int j2 = num9;
						tile = Main.tile[num8, num9];
						WorldGen.SpreadGrass(i2, j2, tile.TileType, num100, repeat: false, color);
						tile = Main.tile[num8, num9];
						if (tile.TileType == num100)
						{
							WorldGen.SquareTileFrame(num8, num9);
							flag = true;
						}
					}
				}
			}
			if (Main.netMode == 2 && flag)
			{
				NetMessage.SendTileSquare(-1, i, j, 3);
			}
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = TileID.GrayBrick;
            }
        }
    }
}