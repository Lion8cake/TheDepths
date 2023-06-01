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
    public class QuartzLantern : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36; ;
            TileObjectData.addTile(Type);
            TileID.Sets.DisableSmartCursor[Type] = true;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            AddMapEntry(new Color(255, 255, 255), CreateMapEntryName());
            DustType = ModContent.DustType<QuartzCrystals>();
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX < 88)
            {
                r = 0.70f;
                g = 0.43f;
                b = 0.21f;
            }
        }
    }
}