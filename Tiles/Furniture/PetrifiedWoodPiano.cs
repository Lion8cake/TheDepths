using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;

namespace TheDepths.Tiles.Furniture
{
    public class PetrifiedWoodPiano : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.addTile(Type);
			AddMapEntry(new Color(70, 65, 55), CreateMapEntryName());
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			HitSound = SoundID.Tink;
			TileID.Sets.DisableSmartCursor[Type] = true;
            DustType = ModContent.DustType<PetrifiedWoodDust>();
        }
    }
}
