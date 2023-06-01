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
    public class QuartzCandle : ModTile
    {

        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 18 };
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            AdjTiles = new int[] { TileID.Torches };
            AddMapEntry(new Color(255, 255, 255), CreateMapEntryName());
            TileID.Sets.DisableSmartCursor[Type] = true;
            DustType = ModContent.DustType<QuartzCrystals>();
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 18)
            {
                r = 0.18f;
                g = 0.54f;
                b = 0.65f;
            }
        }
    }
}