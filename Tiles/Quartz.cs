using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using TheDepths.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
	public class Quartz : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 975;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<ArqueriteOre>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Shalestone>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShaleBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneAmethyst>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneDiamond>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneEmerald>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneRuby>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneSapphire>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneTopaz>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OnyxShalestone>()] = true;
			DustType = ModContent.DustType<QuartzCrystals>();

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(255, 255, 255));

			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 55;
		}
	}
}