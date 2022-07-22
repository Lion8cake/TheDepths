using Microsoft.Xna.Framework;
using Terraria;
using TheDepths.Buffs;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Tiles
{
	public class Shalestone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(27, 29, 33));
			Main.tileMerge[Type][Mod.Find<ModTile>("ArqueriteOre").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("Quartz").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShaleBlock").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneAmethyst").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneDiamond").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneEmerald").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneRuby").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneSapphire").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneTopaz").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("OnyxShalestone").Type] = true;
			DustType = Mod.Find<ModDust>("ShaleDust").Type;

			ItemDrop = ModContent.ItemType<Items.Placeable.Shalestone>();
			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 65;
		}
	}
}