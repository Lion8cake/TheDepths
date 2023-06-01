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
    public class QuartzCandelabra : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                18
            };
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            TileID.Sets.DisableSmartCursor[Type] = true;
            DustType = ModContent.DustType<QuartzCrystals>();
            AdjTiles = new int[] { TileID.Torches };
            AddMapEntry(new Color(255, 255, 255), CreateMapEntryName());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX < 88)
            {
                r = 0.55f;
                g = 0.31f;
                b = 1.2f;
            }
        }

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int topY = j - tile.TileFrameY / 18 % 2;
            int topX = i - tile.TileFrameX / 18 % 2;
            short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);

            Main.tile[i, topY].TileFrameX += frameAdjustment;
            Main.tile[i, topY + 1].TileFrameX += frameAdjustment;
            Main.tile[j, topX].TileFrameY += frameAdjustment;
            Main.tile[j, topX + 1].TileFrameY += frameAdjustment;

            Wiring.SkipWire(i, topY);
            Wiring.SkipWire(i, topY + 1);
            Wiring.SkipWire(j, topX);
            Wiring.SkipWire(j, topX + 1);

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendTileSquare(-1, topX, topY + 2, 2, TileChangeType.None);
            }
        }
    }
}