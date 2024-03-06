using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;

namespace TheDepths.Tiles.Furniture
{
    public class NightwoodBath : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            DustType = ModContent.DustType<NightWoodDust>();
            AdjTiles = new int[] { TileID.Beds };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(30, 32, 36), CreateMapEntryName());
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }
    }
}
