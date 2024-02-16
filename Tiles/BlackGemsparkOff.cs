using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
	public class BlackGemsparkOff : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[Type] = true;
			TileID.Sets.ForcedDirtMerging[Type] = true;
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
			DustType = ModContent.DustType<GemOnyxDust>();
			AddMapEntry(new Color(3, 0, 9));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.BlackGemspark>());
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

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
			return false;
		}
	}
}