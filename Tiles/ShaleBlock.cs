using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
    public class ShaleBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            AddMapEntry(new Color(27, 29, 33));
            TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<ArqueriteOre>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Quartz>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShaleBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Shalestone>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneAmethyst>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneDiamond>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneEmerald>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneRuby>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneSapphire>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneTopaz>()] = true;
            Main.tileMerge[Type][ModContent.TileType<OnyxShalestone>()] = true;
			Main.tileMerge[Type][ModContent.TileType<NightWood>()] = true;
			Main.tileMerge[Type][TileID.Stone] = true;
			DustType = ModContent.DustType<ShaleDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}