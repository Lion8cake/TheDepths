using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using TheDepths.Dusts;

namespace TheDepths.Tiles.Furniture
{
	public class QuartzTable : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 18 };
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(255, 255, 255), name);
			DustType = 0;
            TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[]{ TileID.Tables };
			DustType = ModContent.DustType<QuartzCrystals>();
		}
	}
}