using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using TheDepths.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;
using Terraria.DataStructures;

namespace TheDepths.Tiles
{
	public class OnyxStone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(22, 26, 25), name);
			DustType = DustID.Stone;
			Main.tileMerge[Type][TileID.Stone] = true;
			Main.tileMerge[TileID.Stone][Type] = true;

			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 110;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Onyx>());
			Main.tileSpelunker[Type] = true;
		}

		public override bool CanDrop(int i, int j)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Placeable.Onyx>());
			return false;
		}

		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}