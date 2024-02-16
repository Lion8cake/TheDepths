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

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			TheDepthsModSystem.DrawGlowmask(i, j);
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