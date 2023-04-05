using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles
{
	public class WhiteLightningTo1 : ModProjectile
	{
		public override string Texture => "TheDepths/Projectiles/CrystalBallPassive";

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 99999;
			Projectile.penetrate = 99999;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}
			Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.226f / 255f, (float)(255 - Projectile.alpha) * 0.227f / 255f, (float)(255 - Projectile.alpha) * 0.231f / 255f);
			for (int num103 = 0; num103 < 2; num103++)
			{
				float num104 = Projectile.velocity.X / 3f * (float)num103;
				float num100 = Projectile.velocity.Y / 3f * (float)num103;
				int num101 = 4;
				int LightningDust = Dust.NewDust(new Vector2(Projectile.position.X + (float)num101, Projectile.position.Y + (float)num101), Projectile.width - num101 * 2, Projectile.height - num101 * 2, ModContent.DustType<Dusts.LightningDust>(), 0f, 0f, 100, default(Color), 1.2f);
				Dust obj = Main.dust[LightningDust];
				obj.noGravity = true;
				obj.velocity *= 0.1f;
				obj.velocity += Projectile.velocity * 0.1f;
				obj.position.X -= num104;
				obj.position.Y -= num100;
			}
			Projectile.rotation += 0.3f * (float)Projectile.direction;

			bool foundOrb = false;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<WhiteLightningOrb>())
				{
					foundOrb = true;
					break;
				}
			}
			if (!foundOrb)
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

			float maxDetectRadius = 1000f;

			float projSpeed = 30f;

			Projectile closestFirstOrb = FindFirstOrb(maxDetectRadius);
			if (closestFirstOrb == null)
				return;

			Projectile.velocity = (closestFirstOrb.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public Projectile FindFirstOrb(float maxDetectDistance)
		{
			Projectile ToFirst = null;

			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			for (int k = 0; k < Main.maxProjectiles; k++)
			{
				Projectile target = Main.projectile[k];

				if (target.type == ModContent.ProjectileType<WhiteLightningOrb>())
				{
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					if (sqrDistanceToTarget < sqrMaxDetectDistance)
					{
						sqrMaxDetectDistance = sqrDistanceToTarget;
						ToFirst = target;
					}
				}
			}

			return ToFirst;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
	}

	public class WhiteLightningTo2 : ModProjectile
	{
		public override string Texture => "TheDepths/Projectiles/CrystalBallPassive";

		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 99999;
			Projectile.penetrate = 99999;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}
			Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.226f / 255f, (float)(255 - Projectile.alpha) * 0.227f / 255f, (float)(255 - Projectile.alpha) * 0.231f / 255f);
			for (int num103 = 0; num103 < 2; num103++)
			{
				float num104 = Projectile.velocity.X / 3f * (float)num103;
				float num100 = Projectile.velocity.Y / 3f * (float)num103;
				int num101 = 4;
				int LightningDust = Dust.NewDust(new Vector2(Projectile.position.X + (float)num101, Projectile.position.Y + (float)num101), Projectile.width - num101 * 2, Projectile.height - num101 * 2, ModContent.DustType<Dusts.LightningDust>(), 0f, 0f, 100, default(Color), 1.2f);
				Dust obj = Main.dust[LightningDust];
				obj.noGravity = true;
				obj.velocity *= 0.1f;
				obj.velocity += Projectile.velocity * 0.1f;
				obj.position.X -= num104;
				obj.position.Y -= num100;
			}
			Projectile.rotation += 0.3f * (float)Projectile.direction;

			bool foundOrb = false;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<WhiteLightningOrb2>())
				{
					foundOrb = true;
					break;
				}
			}
			if (!foundOrb)
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

			float maxDetectRadius = 1000f;

			float projSpeed = 30f;

			Projectile closestSecondOrb = FindSecondOrb(maxDetectRadius);
			if (closestSecondOrb == null)
				return;

			Projectile.velocity = (closestSecondOrb.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public Projectile FindSecondOrb(float maxDetectDistance)
		{
			Projectile ToSecond = null;

			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			for (int k = 0; k < Main.maxProjectiles; k++)
			{
				Projectile target = Main.projectile[k];

				if (target.type == ModContent.ProjectileType<WhiteLightningOrb2>())
				{
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					if (sqrDistanceToTarget < sqrMaxDetectDistance)
					{
						sqrMaxDetectDistance = sqrDistanceToTarget;
						ToSecond = target;
					}
				}
			}

			return ToSecond;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			return false;
		}
	}

	public class WhiteLightningTo3 : ModProjectile
	{
		public override string Texture => "TheDepths/Projectiles/CrystalBallPassive";

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 99999;
			Projectile.penetrate = 99999;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}
			Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.226f / 255f, (float)(255 - Projectile.alpha) * 0.227f / 255f, (float)(255 - Projectile.alpha) * 0.231f / 255f);
			for (int num103 = 0; num103 < 2; num103++)
			{
				float num104 = Projectile.velocity.X / 3f * (float)num103;
				float num100 = Projectile.velocity.Y / 3f * (float)num103;
				int num101 = 4;
				int LightningDust = Dust.NewDust(new Vector2(Projectile.position.X + (float)num101, Projectile.position.Y + (float)num101), Projectile.width - num101 * 2, Projectile.height - num101 * 2, ModContent.DustType<Dusts.LightningDust>(), 0f, 0f, 100, default(Color), 1.2f);
				Dust obj = Main.dust[LightningDust];
				obj.noGravity = true;
				obj.velocity *= 0.1f;
				obj.velocity += Projectile.velocity * 0.1f;
				obj.position.X -= num104;
				obj.position.Y -= num100;
			}
			Projectile.rotation += 0.3f * (float)Projectile.direction;

			bool foundOrb = false;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<WhiteLightningOrb3>())
				{
					foundOrb = true;
					break;
				}
			}
			if (!foundOrb)
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

			float maxDetectRadius = 1000f;

			float projSpeed = 30f;

			Projectile closestThirdOrb = FindThirdOrb(maxDetectRadius);
			if (closestThirdOrb == null)
				return;

			Projectile.velocity = (closestThirdOrb.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public Projectile FindThirdOrb(float maxDetectDistance)
		{
			Projectile ToThird = null;

			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			for (int k = 0; k < Main.maxProjectiles; k++)
			{
				Projectile target = Main.projectile[k];

				if (target.type == ModContent.ProjectileType<WhiteLightningOrb3>())
				{
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					if (sqrDistanceToTarget < sqrMaxDetectDistance)
					{
						sqrMaxDetectDistance = sqrDistanceToTarget;
						ToThird = target;
					}
				}
			}

			return ToThird;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
	}
}