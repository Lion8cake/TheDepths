using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Tiles
{
	public class BlackGemsparkOff : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileSolid[Type] = true;
			DustType = Mod.Find<ModDust>("BlackGemsparkDust").Type;
			AddMapEntry(new Color(22, 19, 28));
			ItemDrop = ModContent.ItemType<Items.Placeable.BlackGemspark>();
		}
	
		public override void HitWire(int i, int j)
		{
			Tile tileSafely = Framing.GetTileSafely(i, j);
			if (!tileSafely.HasActuator)
			{
				tileSafely.TileType = (ushort)ModContent.TileType<BlackGemspark>();
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j, 1);
			}
		}
	}
}