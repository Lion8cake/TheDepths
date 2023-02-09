using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Projectiles
{
	public class GlitteringGoldenLine : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 299;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			bool foundYoyo = false;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<BlueSphereYoyo>())
				{
					foundYoyo = true;
					break;
				}
			}
			if (!foundYoyo)
			{
				Projectile.Kill();
			}
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}
			Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.01f / 255f, (float)(255 - Projectile.alpha) * 0.3f / 255f, (float)(255 - Projectile.alpha) * 0.45f / 255f);
			for (int num103 = 0; num103 < 2; num103++)
			{
				float num104 = Projectile.velocity.X / 3f * (float)num103;
				float num100 = Projectile.velocity.Y / 3f * (float)num103;
				int num101 = 4;
				int frostDust = Dust.NewDust(new Vector2(Projectile.position.X + (float)num101, Projectile.position.Y + (float)num101), Projectile.width - num101 * 2, Projectile.height - num101 * 2, ModContent.DustType<GlitteringGoldenDust>(), 0f, 0f, 100, default(Color), 1.2f);
				Dust obj = Main.dust[frostDust];
				obj.noGravity = true;
				obj.velocity *= 0.1f;
				obj.velocity += Projectile.velocity * 0.1f;
				obj.position.X -= num104;
				obj.position.Y -= num100;
			}
			if (Main.rand.NextBool(10))
			{
				int num102 = 4;
				int frostDustSmol = Dust.NewDust(new Vector2(Projectile.position.X + (float)num102, Projectile.position.Y + (float)num102), Projectile.width - num102 * 2, Projectile.height - num102 * 2, ModContent.DustType<GlitteringGoldenDust>(), 0f, 0f, 100, default(Color), 0.6f);
				Dust obj2 = Main.dust[frostDustSmol];
				obj2.velocity *= 0.25f;
				Dust obj3 = Main.dust[frostDustSmol];
				obj3.velocity += Projectile.velocity * 0.5f;
			}
			Projectile.rotation += 0.3f * (float)Projectile.direction;

			float maxDetectRadius = 400f;

			Projectile BlueSphere = FindClosestBlueSphere(maxDetectRadius);
			float projSpeed = 8f; 

			Projectile closestBlueSphere = FindClosestBlueSphere(maxDetectRadius);
			if (closestBlueSphere == null)
				return;

			Projectile.velocity = (closestBlueSphere.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public Projectile FindClosestBlueSphere(float maxDetectDistance)
		{
			Projectile BlueSphere = null;

			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			for (int k = 0; k < Main.maxProjectiles; k++)
			{
				Projectile target = Main.projectile[k];

				if (target.type == ModContent.ProjectileType<BlueSphereYoyo>())
				{
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					if (sqrDistanceToTarget < sqrMaxDetectDistance)
					{
						sqrMaxDetectDistance = sqrDistanceToTarget;
						BlueSphere = target;
					}
				}
			}

			return BlueSphere;
		}
	}
}