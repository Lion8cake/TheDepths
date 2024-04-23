using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;
using Terraria.DataStructures;
using ReLogic.Content;

namespace TheDepths.Tiles
{
    public class MercuryMoss_Foliage : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileCut[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoFail[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;
            DustType = ModContent.DustType<Dusts.MercuryMoss>();
            HitSound = SoundID.Grass;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(73, 89, 118));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
			Tile tile = Main.tile[i, j];
			Tile tile9 = Main.tile[i, j - 1];
			Tile tile17 = Main.tile[i, j + 1];
			Tile tile24 = Main.tile[i - 1, j];
			Tile tile31 = Main.tile[i + 1, j];
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			int num5 = -1;
			if (tile9 != null && tile9.HasTile && !tile9.BottomSlope)
			{
				num3 = tile9.TileType;
			}
			if (tile17 != null && tile17.HasTile && !tile17.IsHalfBlock && !tile17.TopSlope)
			{
				num2 = tile17.TileType;
			}
			if (tile24 != null && tile24.HasTile)
			{
				num4 = tile24.TileType;
			}
			if (tile31 != null && tile31.HasTile)
			{
				num5 = tile31.TileType;
			}
			short num6 = (short)(WorldGen.genRand.Next(3) * 18);
			if (num2 >= 0)
			{
				if (tile.TileFrameY < 0 || tile.TileFrameY > 36)
				{
					tile.TileFrameY = num6;
				}
			}
			else if (num3 >= 0)
			{
				if (tile.TileFrameY < 54 || tile.TileFrameY > 90)
				{
					tile.TileFrameY = (short)(54 + num6);
				}
			}
			else if (num4 >= 0)
			{
				if (tile.TileFrameY < 108 || tile.TileFrameY > 144)
				{
					tile.TileFrameY = (short)(108 + num6);
				}
			}
			else if (num5 >= 0)
			{
				if (tile.TileFrameY < 162 || tile.TileFrameY > 198)
				{
					tile.TileFrameY = (short)(162 + num6);
				}
			}
			else
			{
				WorldGen.KillTile(i, j);
			}
			return false;
        }

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (Main.rand.NextFloat() < 0.01f)
			{
				Vector2 position = new Vector2(i * 16, j * 16);
				Dust dust = Main.dust[Dust.NewDust(position, 4, 4, DustID.AncientLight, 0f, 0f, 0, new Color(119, 135, 162), 1f)];
				dust.noGravity = true;
			}
		}
	}
}
