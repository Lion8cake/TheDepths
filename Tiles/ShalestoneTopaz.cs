using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
	public class ShalestoneTopaz : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			DustType = ModContent.DustType<ShaleDust>();
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(147, 116, 11), name);
			Main.tileMerge[Type][Mod.Find<ModTile>("ShaleBlock").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("Shalestone").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneDiamond").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneEmerald").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneRuby").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneSapphire").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneAmethyst").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("OnyxShalestone").Type] = true;
			
			HitSound = SoundID.Tink;
			MinPick = 65;
			RegisterItemDrop(ItemID.Topaz);
			Main.tileSpelunker[Type] = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}