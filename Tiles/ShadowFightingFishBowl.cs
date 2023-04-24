using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;

namespace TheDepths.Tiles
{
    public class ShadowFightingFishBowl : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            AddMapEntry(new Color(112, 217, 232));
            DustType = DustID.Glass;
            AnimationFrameHeight = 36;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset) //Credit to Rijam
        {
            int x = i - Main.tile[i, j].TileFrameX / 18;
            int y = j - Main.tile[i, j].TileFrameY / 18;
            int smallAnimalCageFrame = x / 2 * (y / 2) % Main.cageFrames;

            frameYOffset = Main.fishBowlFrame[smallAnimalCageFrame] * AnimationFrameHeight;
        }
    }
}
