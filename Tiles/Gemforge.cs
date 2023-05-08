using Terraria.Localization;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using TheDepths.Items;
using TheDepths.Projectiles.Chasme;
using Terraria.GameContent.ObjectInteractions;
using TheDepths.Buffs;

namespace TheDepths.Tiles
{
	public class Gemforge : ModTile
	{
		public static int RubyRelicIsOnForge;

		public override void SetStaticDefaults() {
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = false;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(140, 17, 206), name);
			DustType = ModContent.DustType<ArqueriteDust>();
            TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.Furnaces };
			Main.tileLighted[Type] = true;
			MinPick = 65;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			Player player = Main.LocalPlayer;
			if (player.HeldItem.type == ModContent.ItemType<RubyRelic>() && Main.LocalPlayer.ZoneUnderworldHeight)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			if (Main.LocalPlayer.ZoneUnderworldHeight)
			{
				player.noThrow = 2;
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = ModContent.ItemType<Items.RubyRelic>();
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.021f;
			g = 0.17f;
			b = 0.107f;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Placeable.Gemforge>());
		}

		public override bool CanExplode(int i, int j)
		{
			return false;
		}

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
			int x = i - Main.tile[i, j].TileFrameX / 18 % 6;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 4;
			for (int m = x; m < x + 6; m++)
			{
				for (int n = y; n < y + 4; n++)
				{
					if (Main.tile[m, n].HasTile && Main.tile[m, n].TileType == Type)
					{
						if (RubyRelicIsOnForge == 1 && Main.tile[m, n].TileFrameX > 18 * 3)
                        {
                            Main.tile[m, n].TileFrameX -= (short)(18 * 3);
                            Main.tile[m + 1, n].TileFrameX -= (short)(18 * 3);
                            Main.tile[m - 1, n].TileFrameX -= (short)(18 * 3);
                            Main.tile[m, n + 1].TileFrameX -= (short)(18 * 3);
							Main.tile[m + 1, n + 1].TileFrameX -= (short)(18 * 3);
							Main.tile[m - 1, n + 1].TileFrameX -= (short)(18 * 3);
							RubyRelicIsOnForge = 0;
						}
					}
				}
			}
		}

        public override bool RightClick(int i, int j)
		{
			int x = i - Main.tile[i, j].TileFrameX / 18 % 3;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 2;
			Player player = Main.LocalPlayer;
			for (int m = x; m < x + 3; m++)
			{
				for (int n = y; n < y + 2; n++)
				{
					if (Main.tile[m, n].HasTile && Main.tile[m, n].TileType == Type)
					{
						if (Main.tile[m, n].TileFrameX < 18 * 3 && player.HeldItem.type == ModContent.ItemType<RubyRelic>() && Main.LocalPlayer.ZoneUnderworldHeight && !player.HasBuff(BuffID.Horrified) && !player.HasBuff(ModContent.BuffType<RelicsCurse>()))
						{
							Main.tile[m, n].TileFrameX += (short)(18 * 3);
							Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(x, y).ToWorldCoordinates() + new Vector2(17, 24), Vector2.Zero, ModContent.ProjectileType<GemforgeExtraDusts>(), 0, 0f, Main.myPlayer);
						}
                    }
                }
            }
            if (player.HeldItem.type == ModContent.ItemType<RubyRelic>() && Main.LocalPlayer.ZoneUnderworldHeight && !player.HasBuff(BuffID.Horrified) && !player.HasBuff(ModContent.BuffType<RelicsCurse>()))
            {
                player.HeldItem.stack -= 1;
				Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(x, y).ToWorldCoordinates() + new Vector2(17, 24), Vector2.Zero, ModContent.ProjectileType<GemforgeCutscene>(), 0, 0f, Main.myPlayer);
				//player.AddBuff(ModContent.BuffType<RelicsCurse>(), 301);
			}
			else if (Main.maxTilesY >= 210 && player.HeldItem.type == ModContent.ItemType<RubyRelic>())
			{
				player.Hurt(PlayerDeathReason.ByCustomReason("Gemforge"), 99999, 0);
			}
            return true;
		}
    }
}