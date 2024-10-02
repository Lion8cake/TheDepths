using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using TheDepths.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;
using System;
using Terraria.DataStructures;
using TheDepths.Worldgen;

namespace TheDepths.Tiles
{
	public class ArqueriteOre : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			TileID.Sets.DoesntGetReplacedWithTileReplacement[Type] = true;
			Main.tileOreFinderPriority[Type] = 500;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 975;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			DustType = ModContent.DustType<ArqueriteDust>();
			Main.tileMerge[Type][ModContent.TileType<Shalestone>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Quartz>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShaleBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneAmethyst>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneDiamond>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneEmerald>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneRuby>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneSapphire>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneTopaz>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OnyxShalestone>()] = true;
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
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (j > Main.UnderworldLayer && TheDepthsWorldGen.TileInDepths(x))
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