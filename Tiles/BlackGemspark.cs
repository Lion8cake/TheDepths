using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TheDepths.Tiles
{
	public class BlackGemspark : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileSolid[Type] = true;
			DustType = Mod.Find<ModDust>("BlackGemsparkDust").Type;
			AddMapEntry(new Color(22, 19, 28));
			ItemDrop = ModContent.ItemType<Items.Placeable.BlackGemspark>();
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.65f;
			g = 0.71f;
			b = 0.92f;
		}

		public override void ChangeWaterfallStyle(ref int style)
		{
			style = ModContent.Find<ModWaterfallStyle>("TheDepths/BlackWaterfallStyle").Slot;
		}

		public override void HitWire(int i, int j)
		{
			Tile tileSafely = Framing.GetTileSafely(i, j);
			if (!tileSafely.HasActuator)
			{
				tileSafely.TileType = (ushort)ModContent.TileType<BlackGemsparkOff>();
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j, 1);
			}
		}
	
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
        }
	}
}