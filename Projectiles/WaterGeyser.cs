using Terraria.Audio;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TheDepths.Projectiles
{
	public class WaterGeyser : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 120;
			Projectile.friendly = true;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.trap = true;
		}

		public override void AI() {
			int num26 = Math.Sign(Projectile.velocity.Y);
			int num27 = ((num26 != -1) ? 1 : 0);
			if (Projectile.ai[0] == 0f)
			{
				if (!Collision.SolidCollision(Projectile.position + new Vector2(0f, (float)((num26 == -1) ? (Projectile.height - 48) : 0)), Projectile.width, 48) && !Collision.WetCollision(Projectile.position + new Vector2(0f, (float)((num26 == -1) ? (Projectile.height - 20) : 0)), Projectile.width, 20))
				{
					Projectile.velocity = new Vector2(0f, (float)Math.Sign(Projectile.velocity.Y) * 0.001f);
					Projectile.ai[0] = 1f;
					Projectile.ai[1] = 0f;
					Projectile.timeLeft = 60;
				}
				Projectile.ai[1]++;
				if (Projectile.ai[1] >= 60f)
				{
					Projectile.Kill();
				}
				for (int num28 = 0; num28 < 3; num28++)
				{
					int num29 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100);
					Main.dust[num29].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[num29].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[num29].noGravity = true;
					Dust obj8 = Main.dust[num29];
					Vector2 center27 = Projectile.Center;
					Vector2 spinningpoint75 = new Vector2(0f, (float)(-Projectile.height / 2));
					double radians62 = Projectile.rotation;
					obj8.position = center27 + Utils.RotatedBy(spinningpoint75, radians62) * 1.1f;
				}
			}
			if (Projectile.ai[0] != 1f)
			{
				return;
			}
			Projectile.velocity = new Vector2(0f, (float)Math.Sign(Projectile.velocity.Y) * 0.001f);
			if (num26 != 0)
			{
				int num30 = 16;
				int num31 = 640;
				for (; num30 < num31 && !Collision.SolidCollision(Projectile.position + new Vector2(0f, (float)((num26 == -1) ? (Projectile.height - num30 - 16) : 0)), Projectile.width, num30 + 16); num30 += 16)
				{
				}
				if (num26 == -1)
				{
					Projectile.position.Y += Projectile.height;
					Projectile.height = num30;
					Projectile.position.Y -= num30;
				}
				else
				{
					Projectile.height = num30;
				}
			}
			Projectile.ai[1]++;
			if (Projectile.ai[1] >= 60f)
			{
				Projectile.Kill();
			}
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
				for (int num35 = 0; (float)num35 < 60f; num35++)
				{
					//int num37 = Utils.SelectRandom<int>(Main.rand, 6, 259, 158);
					int num38 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, /*num37*/Dust.dustWater(), 0f, -2.5f * (float)(-num26));
					Main.dust[num38].alpha = 200;
					Dust dust18 = Main.dust[num38];
					Dust dust212 = dust18;
					dust212.velocity *= new Vector2(0.3f, 2f);
					Main.dust[num38].velocity.Y += 2 * num26;
					dust18 = Main.dust[num38];
					dust212 = dust18;
					dust212.scale += Main.rand.NextFloat();
					Main.dust[num38].position = new Vector2(Projectile.Center.X, Projectile.Center.Y + (float)Projectile.height * 0.5f * (float)(-num26));
					Main.dust[num38].customData = num27;
					if (num26 == -1 && !Main.rand.NextBool(4))
					{
						Main.dust[num38].velocity.Y -= 0.2f;
					}
				}
				SoundEngine.PlaySound(in SoundID.Item34, Projectile.position);
			}
			if (num26 == 1)
			{
				for (int num39 = 0; (float)num39 < 9f; num39++)
				{
					//int num40 = Utils.SelectRandom<int>(Main.rand, 6, 259, 158);
					int num41 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, /*num40*/ Dust.dustWater(), 0f, -2.5f * (float)(-num26));
					Main.dust[num41].alpha = 200;
					Dust dust17 = Main.dust[num41];
					Dust dust212 = dust17;
					dust212.velocity *= new Vector2(0.3f, 2f);
					Main.dust[num41].velocity.Y += 2 * num26;
					dust17 = Main.dust[num41];
					dust212 = dust17;
					dust212.scale += Main.rand.NextFloat();
					Main.dust[num41].position = new Vector2(Projectile.Center.X, Projectile.Center.Y + (float)Projectile.height * 0.5f * (float)(-num26));
					Main.dust[num41].customData = num27;
					if (num26 == -1 && !Main.rand.NextBool(4))
					{
						Main.dust[num41].velocity.Y -= 0.2f;
					}
				}
			}
			int num42 = (int)(Projectile.ai[1] / 60f * (float)Projectile.height) * 3;
			if (num42 > Projectile.height)
			{
				num42 = Projectile.height;
			}
			Vector2 vector71 = Projectile.position + (Vector2)((num26 == -1) ? new Vector2(0f, (float)(Projectile.height - num42)) : Vector2.Zero);
			Vector2 vector72 = Projectile.position + (Vector2)((num26 == -1) ? new Vector2(0f, (float)Projectile.height) : Vector2.Zero);
			for (int num43 = 0; (float)num43 < 6f; num43++)
			{
				if (Main.rand.Next(6) < 5)
				{
					int num44 = Dust.NewDust(vector71, Projectile.width, num42, /*6*/Dust.dustWater(), 0f, 0f, 90, default(Color), 2.5f);
					Main.dust[num44].noGravity = true;
					Main.dust[num44].fadeIn = 1f;
					if (Main.dust[num44].velocity.Y > 0f)
					{
						Main.dust[num44].velocity.Y *= -1f;
					}
					if (Main.rand.Next(6) < 3)
					{
						Main.dust[num44].position.Y = MathHelper.Lerp(Main.dust[num44].position.Y, vector72.Y, 0.5f);
						Dust dust14 = Main.dust[num44];
						Dust dust212 = dust14;
						dust212.velocity *= 5f;
						Main.dust[num44].velocity.Y -= 3f;
						Main.dust[num44].position.X = Projectile.Center.X;
						Main.dust[num44].noGravity = false;
						Main.dust[num44].noLight = true;
						Main.dust[num44].fadeIn = 0.4f;
						dust14 = Main.dust[num44];
						dust212 = dust14;
						dust212.scale *= 0.3f;
					}
					else
					{
						Main.dust[num44].velocity = Projectile.DirectionFrom(Main.dust[num44].position) * Main.dust[num44].velocity.Length() * 0.25f;
					}
					Main.dust[num44].velocity.Y *= -num26;
					Main.dust[num44].customData = num27;
				}
			}
			for (int num45 = 0; (float)num45 < 6f; num45++)
			{
				if (!(Main.rand.NextFloat() < 0.8f))
				{
					//int num46 = Utils.SelectRandom<int>(Main.rand, 6, 259, 158);
					int num48 = Dust.NewDust(vector71, Projectile.width, num42, /*num46*/Dust.dustWater(), 0f, -2.5f * (float)(-num26));
					Main.dust[num48].alpha = 200;
					Dust dust16 = Main.dust[num48];
					Dust dust212 = dust16;
					dust212.velocity *= new Vector2(0.6f, 1.5f);
					dust16 = Main.dust[num48];
					dust212 = dust16;
					dust212.scale += Main.rand.NextFloat();
					if (num26 == -1 && !Main.rand.NextBool(4))
					{
						Main.dust[num48].velocity.Y -= 0.2f;
					}
					Main.dust[num48].customData = num27;
				}
			}

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player player = Main.player[i];
				if (player.Hitbox.Intersects(Projectile.Hitbox) && player.active && !player.dead)
				{
					if (num26 == -1)
					{
						player.velocity.Y -= 1f;
					}
					else
					{
						player.velocity.Y += 1f;
					}
				}
			}
		}
	}
}