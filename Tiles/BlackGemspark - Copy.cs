using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
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
	
		public override void HitWire(int i, int j)
		{
			ModContent.TileType<BlackGemsparkOff>();
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j, 1);
		}
	}
}