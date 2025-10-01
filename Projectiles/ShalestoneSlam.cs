using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;

namespace TheDepths.Projectiles
{
	public class ShalestoneSlam : ModProjectile
	{
	    public override string Texture => "TheDepths/Projectiles/CrystalBall";
	
		public override void SetDefaults() {
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 120;
			Projectile.penetrate = -1;
			Projectile.damage = 0;
		}

		public override void AI()
		{
			float num = 12f; //Size/length of the stomp
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > 9f)
			{
				Projectile.Kill();
				return;
			}
			Projectile.velocity = Vector2.Zero;
			Projectile.position = Projectile.Center;
			Projectile.Size = new Vector2(16f, 16f) * MathHelper.Lerp(5f, num, Utils.GetLerpValue(0f, 9f, Projectile.ai[0]));
			Projectile.Center = Projectile.position;
			Point point = Projectile.TopLeft.ToTileCoordinates();
			Point point2 = Projectile.BottomRight.ToTileCoordinates();
			int num2 = point.X / 2 + point2.X / 2;
			int num3 = Projectile.width / 2;
			if ((int)Projectile.ai[0] % 3 != 0)
			{
				return;
			}
			int num4 = (int)Projectile.ai[0] / 3;

			for (int i = point.X; i <= point2.X; i++)
			{
				for (int j = point.Y; j <= point2.Y; j++)
				{

					if (Vector2.Distance(Projectile.Center, new Vector2((float)(i * 16), (float)(j * 16))) > (float)num3)
					{
						continue;
					}
					Tile tileSafely = Framing.GetTileSafely(i, Main.player[Projectile.owner].gravDir == -1 ? j - 1 : j);
					if (!tileSafely.HasTile || !Main.tileSolid[tileSafely.TileType] || Main.tileSolidTop[tileSafely.TileType] || Main.tileFrameImportant[tileSafely.TileType])
					{
						continue;
					}
					Tile tileSafely2 = Framing.GetTileSafely(i, Main.player[Projectile.owner].gravDir == -1 ? j : j - 1);
					if (tileSafely2.HasTile && Main.tileSolid[tileSafely2.TileType] && !Main.tileSolidTop[tileSafely2.TileType])
					{
						continue;
					}
					int num5 = WorldGen.KillTile_GetTileDustAmount(fail: true, tileSafely, i, Main.player[Projectile.owner].gravDir == -1 ? j - 1 : j);
					for (int k = 0; k < num5; k++)
					{
						Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, Main.player[Projectile.owner].gravDir == -1 ? j - 1 : j, tileSafely)];
						obj.velocity.Y -= (3f + (float)num4 * 1.5f) * Main.player[Projectile.owner].gravDir;
						obj.velocity.Y *= Main.rand.NextFloat();
						obj.velocity.Y *= 0.75f;
						obj.scale += (float)num4 * 0.03f;
					}
					if (num4 >= 2)
					{
						for (int m = 0; m < num5 - 1; m++)
						{
							Dust obj3 = Main.dust[WorldGen.KillTile_MakeTileDust(i, Main.player[Projectile.owner].gravDir == -1 ? j - 1 : j, tileSafely)];
							obj3.velocity.Y -= (1f + (float)num4) * Main.player[Projectile.owner].gravDir;
							obj3.velocity.Y *= Main.rand.NextFloat();
							obj3.velocity.Y *= 0.75f;
						}
					}
					if (num5 <= 0 || Main.rand.NextBool(3))
					{
						continue;
					}
					float num7 = (float)Math.Abs(num2 - i) / (num / 2f);
					if (Main.rand.NextBool(2))
					{
						Gore gore = Gore.NewGoreDirect(new EntitySource_Misc(""), Projectile.position, Vector2.Zero, 61 + Main.rand.Next(3), 1f - (float)num4 * 0.15f + num7 * 0.2f);
						gore.velocity.Y -= (0.1f + (float)num4 * 0.5f + num7 * (float)num4 * 1f) * Main.player[Projectile.owner].gravDir;
						gore.velocity.Y *= Main.rand.NextFloat();
						gore.position = new Vector2((float)(i * 16 + 20), (float)((Main.player[Projectile.owner].gravDir == -1 ? j - 1 : j) * 16 + 20));
					}
				}
			}
		}
	}
}