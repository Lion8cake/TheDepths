using Terraria.Localization;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace TheDepths.Tiles
{
	public class CoreBuilderTile : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = false;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			LocalizedText name = CreateMapEntryName();
			AnimationFrameHeight = 40;
			AddMapEntry(new Color(140, 17, 206), name);
			DustType = ModContent.DustType<ArqueriteDust>();
            TileID.Sets.DisableSmartCursor[Type] = true;
			TileObjectData.newTile.DrawYOffset = -2;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1.04f;
            g = 2.53f;
            b = 1.64f;
        }

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 6)
			{
				frameCounter = 0;
				frame++;
				frame %= 3;
			}
		}
	}
}