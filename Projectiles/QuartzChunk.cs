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
	public class QuartzChunk : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
		}

		public override void AI()
		{
			Projectile.ai[0] += 1f;
			Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
			if (Projectile.velocity.X <= -1)
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + 45;
			}
			else
            {
				Projectile.rotation = Projectile.velocity.ToRotation() + -45;
			}
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation += MathHelper.Pi;
			}
		}

        public override void Kill(int timeLeft)
        {
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, ModContent.DustType<Dusts.QuartzCrystals>());
			}
		}
    }
}