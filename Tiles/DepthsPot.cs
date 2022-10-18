using Microsoft.Xna.Framework;
using Terraria.Audio;
using TheDepths.Buffs;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Items;
using TheDepths.Items.Weapons;
using Terraria.DataStructures;

namespace TheDepths.Tiles
{
	public class DepthsPot : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileOreFinderPriority[Type] = 100;
			Main.tileSpelunker[Type] = true;
			Main.tileCut[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Pot");
			AddMapEntry(new Color(33, 38, 97), name);
			DustType = 29;
			HitSound = SoundID.Item13;
		}
	
		public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}
	
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			_ = j - Main.tile[i, j].TileFrameY / 18;
			_ = i - Main.tile[i, j].TileFrameX / 18;
			SoundEngine.PlaySound(SoundID.Shatter, new Vector2(i * 16, j * 16));
			//Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), Mod.Find<ModGore>("DepthsPotGore1").Type);
			//Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), Mod.Find<ModGore>("DepthsPotGore2").Type);
			//Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), Mod.Find<ModGore>("DepthsPotGore3").Type);
			if (!WorldGen.gen && Main.netMode != 1)
			{
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 72, Main.rand.Next(15, 18));
				int num = Main.rand.Next(18);
				if (num == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 8, Main.rand.Next(2, 5)); //Torch
				}
				if (num == 1)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<DiamondArrow>(), Main.rand.Next(20, 40)); //Arrow
				}
				if (num == 2)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 166, Main.rand.Next(1, 4)); //Bomb
				}
				if (num == 3)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 72, Main.rand.Next(13, 18));
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 71, Main.rand.Next(0, 100)); //Coins
				}
				if (num == 4)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 188, Main.rand.Next(1, 1)); //Healing Potion 
				}
				if (num == 5)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 296, Main.rand.Next(1, 1)); //Splunker Potion
				}
				if (num == 6)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 295, Main.rand.Next(1, 1)); //Feather Fall Potion
				}
				if (num == 7)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 293, Main.rand.Next(1, 1)); //Mana Regen Potion
				}
				if (num == 8)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<CrystalSkinPotion>(), Main.rand.Next(1, 1)); //Crystal Skin Potion
				}
				if (num == 9)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 294, Main.rand.Next(1, 1)); //Mana Power Potion
				}
				if (num == 10)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 297, Main.rand.Next(1, 1)); //Invis Potion
				}
				if (num == 11)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 304, Main.rand.Next(1, 1)); //Hunter Potion
				}
				if (num == 12)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 305, Main.rand.Next(1, 1)); //Gravity Potion
				}
				if (num == 13)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 301, Main.rand.Next(1, 1)); //Thorns Potion
				}
				if (num == 14)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 302, Main.rand.Next(1, 1)); //Water Walking Potion
				}
				if (num == 15)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 300, Main.rand.Next(1, 1)); //Battle Potion
				}
				if (num == 16)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 2323, Main.rand.Next(1, 1)); //Heart REACH Potion
				}
				if (num == 17)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 2326, Main.rand.Next(1, 1)); //Titan Potion
				}
			}
		}
	}
}