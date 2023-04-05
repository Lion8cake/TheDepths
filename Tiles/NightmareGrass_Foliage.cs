using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
    public class NightmareGrass_Foliage : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileCut[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoFail[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.SwaysInWindBasic[Type] = true;
            DustType = ModContent.DustType<NightDust>();
            HitSound = SoundID.Grass;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(44, 25, 96));
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 6 < 11)
            {
                spriteEffects = (SpriteEffects)1;
            }
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            frameXOffset = i % 11 * 18;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            int type = -1;
            if (tileBelow.HasTile && !tileBelow.BottomSlope)
            {
                type = tileBelow.TileType;
            }
            if (type == ModContent.TileType<NightmareGrass>() || type == Type)
            {
                return true;
            }
            WorldGen.KillTile(i, j);
            return true;
        }
    }
}
