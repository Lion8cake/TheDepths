using Microsoft.Xna.Framework;
using TheDepths.Items.Placeable;
using TheDepths.Dusts;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace TheDepths.Walls
{
	public class MercuryMossBrickWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<Dusts.MercuryMoss>();
			AddMapEntry(new Color(46, 54, 64));
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.rand.NextFloat() < 0.005f)
			{
				Vector2 position = new Vector2(i * 16, j * 16);
				Dust dust = Main.dust[Dust.NewDust(position, 4, 4, 261, 0f, 0f, 0, new Color(119, 135, 162), 1f)];
				dust.noGravity = true;
			}

			Tile tile = Main.tile[i, j];
            Rectangle frame = new Rectangle(tile.WallFrameX, tile.WallFrameY, 32, 32);
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 pos = new Vector2(i * 16, j * 16) + zero - Main.screenPosition;
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("TheDepths/Walls/MercuryMossBrickWall_Glow").Value, pos + new Vector2(-8, -8), frame, Color.White);
        }

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.01f;
			g = 0.01f;
			b = 0.01f;
		}
	}
}