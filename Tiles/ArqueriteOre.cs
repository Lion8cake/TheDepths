using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using TheDepths.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;
using System;
using Terraria.DataStructures;

namespace TheDepths.Tiles
{
	public class ArqueriteOre : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileOreFinderPriority[Type] = 500;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 975;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			DustType = ModContent.DustType<ArqueriteDust>();
			Main.tileMerge[Type][Mod.Find<ModTile>("Shalestone").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("Quartz").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShaleBlock").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneAmethyst").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneDiamond").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneEmerald").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneRuby").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneSapphire").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("ShalestoneTopaz").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("OnyxShalestone").Type] = true;

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(119, 134, 162), name);

			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 65;
		}
		
		public override void NearbyEffects(int i, int j, bool closer)
		{
			Player player = Main.LocalPlayer;
			if ((int)Vector2.Distance(player.Center / 16f, new Vector2((float)i, (float)j)) <= 1 && !player.dead)
			{
				player.AddBuff(ModContent.BuffType<MercuryPoisoning>(), Main.rand.Next(10, 20));
			}
		}
		
		public override bool CanExplode(int i, int j)
		{
			if (Main.hardMode)
			{
				return true;
			}
			return false;
		}

        public override bool IsTileSpelunkable(int i, int j)
        {
			return true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
			Tile tile = Main.tile[i, j];
			int x = i - Main.tile[i, j].TileFrameX / 18 % 1;
			if (Main.netMode != 1)
			{
				if (j > Main.UnderworldLayer && (Worldgen.TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (Worldgen.TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(x) < Main.maxTilesX / 2 || Worldgen.TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(x) > Main.maxTilesX / 2) && Main.drunkWorld))
				{
					tile.LiquidType = LiquidID.Lava;
					tile.LiquidAmount = 128;
				}
			}
		}
    }

	//For future removal of lava dropping quicksilver
	public class Hellstone : GlobalTile
    {	

        /*public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
			Tile tile = Main.tile[i, j];
			int x = i - Main.tile[i, j].TileFrameX / 18 % 1;
			if (type == TileID.Hellstone && fail == false && (Worldgen.TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (Worldgen.TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(x) < Main.maxTilesX / 2 || Worldgen.TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(x) > Main.maxTilesX / 2) && Main.drunkWorld))
            {
				noItem = true;
				tile.HasTile = false;
				tile.ClearEverything();
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Hellstone);
			}
        }*/
    }
}