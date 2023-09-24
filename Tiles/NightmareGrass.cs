using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
    public class NightmareGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileGlowMask[Type] = 326;
            TileID.Sets.Grass[Type] = true;
            TileID.Sets.ChecksForMerge[Type] = true;
            TileID.Sets.ForcedDirtMerging[Type] = true;
            TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
            DustType = ModContent.DustType<NightDust>();
            AddMapEntry(new Color(43, 28, 83));
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.ShaleBlock>());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.43f;
            g = 0.28f;
            b = 0.83f;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.rand.NextBool(8))
            {
                bool spawned = false;
                if (!spawned)
                {
                    if (Main.tile[i, j - 1].TileType == 0 && Main.rand.NextBool(4))
                    {
                        WorldGen.PlaceTile(i, j - 1, ModContent.TileType<NightmareGrass_Foliage>(), mute: true);
                    }
                }
            }

            Tile tile = Framing.GetTileSafely(i, j);
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            if (WorldGen.genRand.NextBool(15) && !tileBelow.HasTile && !tileBelow.CheckingLiquid && !tile.IsHalfBlock)
            {
                tileBelow.TileType = (ushort)ModContent.TileType<NightmareVines>();
                WorldGen.SquareTileFrame(i, j + 1);
                if (Main.netMode == 2)
                {
                    NetMessage.SendTileSquare(-1, i, j + 1, 3);
                }
            }
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<ShaleBlock>();
            }
        }
    }
}