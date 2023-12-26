using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace TheDepths.Projectiles
{
	public class DiamondBolt : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.alpha = 255;
			Projectile.penetrate = 20;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 1000;
			//Projectile.ai = 8;
			//AIType = ProjectileID.WaterBolt;
		}

		public override void AI()
		{
			for (int i = 0; i < 2; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, Projectile.velocity.X, Projectile.velocity.Y, 150, default(Color), 1.25f);
				Main.dust[num].noGravity = true;
				Dust obj = Main.dust[num];
				obj.velocity *= 0.3f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
				Projectile.velocity.Y = -oldVelocity.Y;
            }
			if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
			{
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Projectile.velocity.X * 0.75f, Projectile.velocity.Y * 0.75f, 150);
				Main.dust[num].noGravity = true;
			}
		}
	}
}