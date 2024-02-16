using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using TheDepths.Dusts;
using Terraria.ID;

namespace TheDepths.Tiles
{
	public class BlackGemspark : ModTile
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
			AddMapEntry(new Color(22, 19, 28));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.BlackGemspark>());
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.058f;
			g = 0.061f;
			b = 0.06f;
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

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
			return false;
		}
	}
}