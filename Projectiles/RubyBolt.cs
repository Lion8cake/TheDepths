using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace TheDepths.Projectiles
{
	public class RubyBolt : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.alpha = 255;
			Projectile.penetrate = 15;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 900;
		}

		public override void AI()
		{
			for (int i = 0; i < 2; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemRuby, Projectile.velocity.X, Projectile.velocity.Y, 150, default(Color), 1.25f);
				Main.dust[num].noGravity = true;
				Dust obj = Main.dust[num];
				obj.velocity *= 0.3f;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire, 240, false);
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.OnFire, 240, false);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
			{
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Projectile.velocity.X * 0.75f, Projectile.velocity.Y * 0.75f, 150);
				Main.dust[num].noGravity = true;
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
	}
}