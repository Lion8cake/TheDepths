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
	public class ShadowClawMiddle : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 44;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			if (Projectile.ai[0] == 0f)
			{
				Projectile.alpha -= 50;
				if (Projectile.alpha > 0)
				{
					return;
				}
				Projectile.alpha = 0;
				Projectile.ai[0] = 1f;
				if (Projectile.ai[1] == 0f)
				{
					Projectile.ai[1] += 1f;
					Projectile.position += Projectile.velocity * 1f;
				}
				if (Main.myPlayer == Projectile.owner)
				{
					int type = Projectile.type;
					if (Projectile.ai[1] >= 10f)
					{
						type = ModContent.ProjectileType<ShadowClawTip>();
					}
					int num72 = Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, type, Projectile.damage, Projectile.knockBack, Projectile.owner);
					Main.projectile[num72].damage = Projectile.damage;
					Main.projectile[num72].ai[1] = Projectile.ai[1] + 1f;
					NetMessage.SendData(27, -1, -1, null, num72);
				}
				return;
			}
			if (Projectile.alpha < 170 && Projectile.alpha + 5 >= 170)
			{
				for (int num82 = 0; num82 < 3; num82++)
				{
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18, Projectile.velocity.X * 0.025f, Projectile.velocity.Y * 0.025f, 170, default(Color), 1.2f);
				}
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 14, 0f, 0f, 170, default(Color), 1.1f);
			}
			Projectile.alpha += 5;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
	}
		
	public class ShadowClawTip : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			if (Projectile.ai[0] == 0f)
			{
				Projectile.alpha -= 50;
				if (Projectile.alpha > 0)
				{
					return;
				}
				Projectile.alpha = 0;
				Projectile.ai[0] = 1f;
				if (Projectile.ai[1] == 0f)
				{
					Projectile.ai[1] += 1f;
					Projectile.position += Projectile.velocity * 1f;
				}
				return;
			}
			if (Projectile.alpha < 170 && Projectile.alpha + 5 >= 170)
			{
				for (int num82 = 0; num82 < 3; num82++)
				{
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18, Projectile.velocity.X * 0.025f, Projectile.velocity.Y * 0.025f, 170, default(Color), 1.2f);
				}
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 14, 0f, 0f, 170, default(Color), 1.1f);
			}
			Projectile.alpha += 5;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}
	}

	public class ShadowClawBottom : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			if (Projectile.ai[0] == 0f)
			{
				Projectile.alpha -= 50;
				if (Projectile.alpha > 0)
				{
					return;
				}
				Projectile.alpha = 0;
				Projectile.ai[0] = 1f;
				if (Projectile.ai[1] == 0f)
				{
					Projectile.ai[1] += 1f;
					Projectile.position += Projectile.velocity * 1f;
				}
				return;
			}
			if (Projectile.alpha < 170 && Projectile.alpha + 5 >= 170)
			{
				for (int num82 = 0; num82 < 3; num82++)
				{
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18, Projectile.velocity.X * 0.025f, Projectile.velocity.Y * 0.025f, 170, default(Color), 1.2f);
				}
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 14, 0f, 0f, 170, default(Color), 1.1f);
			}
			Projectile.alpha += 5;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}
	}
}