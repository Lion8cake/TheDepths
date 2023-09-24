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
			Main.tileMerge[Type][Mod.Find<ModTile>("ArqueriteOre").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("Quartz").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShaleBlock").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("Shalestone").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneAmethyst").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneDiamond").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneEmerald").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneRuby").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneSapphire").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneTopaz").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("OnyxShalestone").Type] = true;
            DustType = ModContent.DustType<ShaleDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile up = Main.tile[i, j - 1];
            Tile down = Main.tile[i, j + 1];
            Tile left = Main.tile[i - 1, j];
            Tile right = Main.tile[i + 1, j];
            if (WorldGen.genRand.Next(3) == 0 && (up.TileType == ModContent.TileType<NightmareGrass>() || down.TileType == ModContent.TileType<NightmareGrass>() || left.TileType == ModContent.TileType<NightmareGrass>() || right.TileType == ModContent.TileType<NightmareGrass>()))
            {
                WorldGen.SpreadGrass(i, j, Type, ModContent.TileType<NightmareGrass>(), repeat: false);
            }
        }
    }
}