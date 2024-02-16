using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using TheDepths.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
	public class Shalestone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(27, 29, 33));
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<ArqueriteOre>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Quartz>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShaleBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneAmethyst>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneDiamond>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneEmerald>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneRuby>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneSapphire>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneTopaz>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OnyxShalestone>()] = true;
			DustType = ModContent.DustType<ShaleDust>();

			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 65;
		}
	}
}