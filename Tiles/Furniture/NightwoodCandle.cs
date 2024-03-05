using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;

namespace TheDepths.Tiles.Furniture
{
    public class NightwoodCandle : ModTile
    {

        public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.newTile.CoordinateHeights = new int[] { 20 };
			TileObjectData.newTile.DrawYOffset = -4;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(Type);
			Main.tileLighted[Type] = true;
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AddMapEntry(new Color(30, 32, 36), CreateMapEntryName());
            TileID.Sets.DisableSmartCursor[Type] = true;
            DustType = ModContent.DustType<NightWoodDust>();
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Furniture.NightwoodCandle>());
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.NightwoodCandle>();
		}
		public override bool RightClick(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int topY = j - tile.TileFrameY / 18;
			short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);
			Main.tile[i, topY].TileFrameX += frameAdjustment;
			NetMessage.SendTileSquare(-1, i, topY, 1);
			return true;
		}
		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int topY = j - tile.TileFrameY / 18;
			short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);
			Main.tile[i, topY].TileFrameX += frameAdjustment;
			NetMessage.SendTileSquare(-1, i, topY, 1);
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 18)
            {
                r = 0.55f;
                g = 0.31f;
                b = 1.2f;
            }
        }
    }
}