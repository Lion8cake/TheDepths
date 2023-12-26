using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheDepths.Dusts;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Enums;

namespace TheDepths.Tiles
{
	public class WaterGeyser : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -2;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -2;
			TileObjectData.addAlternate(3);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(144, 148, 144), name);
			DustType = DustID.Stone;
		}

        public override bool Slope(int i, int j)
        {
			return false;
        }

		public override void NearbyEffects(int i, int j, bool closer)
		{
			int x = i * 16;
			int y = j * 16;
			Player player = Main.LocalPlayer;
			if (player.Hitbox.Intersects(new Rectangle(x, y, 16, 16)) && !player.dead)
			{
				HitWire(i, j);
				NetMessage.SendData(MessageID.HitSwitch, -1, -1, null, i, j);
			}
		}

		public override bool IsTileDangerous(int i, int j, Player player)
		{
			return true;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			if (tileFrameX >= 32 * 2)
			{
				offsetY -= 4;
			}
		}

		public override void HitWire(int i, int j)
		{
			if (Main.tile[i - 1, j].TileType == ModContent.TileType<WaterGeyser>())
			{
				GeyserTrap(i, j);
			}
			else
			{
				GeyserTrap(i + 1, j);
			}
		}

		private static void GeyserTrap(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int num = tile.TileFrameX / 36;
			int num2 = i - (tile.TileFrameX - num * 36) / 18;
			if (Wiring.CheckMech(num2, j, 200))
			{
				Vector2 zero = new(i * 16, j * 16);
				Vector2 zero2 = Vector2.Zero;
				int num3 = ModContent.ProjectileType<Projectiles.WaterGeyser>();
				if (num < 2)
				{
					zero2 = new(0f, -8f);
				}
				else
				{
					zero2 = new(0f, 8f);
				}
				Projectile.NewProjectile(new EntitySource_Misc(""), (int)zero.X, (int)zero.Y, zero2.X, zero2.Y, num3, 0, 0f, Main.myPlayer);
			}
		}
	}
}