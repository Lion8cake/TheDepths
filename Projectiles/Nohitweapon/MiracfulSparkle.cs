using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Projectiles.Nohitweapon
{
    public class MiracfulSparkle : ModProjectile
    {
        int timer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 72;
            Projectile.height = 72;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 10;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 800;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                Dust.NewDust(Projectile.Center, 8, 8, 91);
            }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3());
            Projectile.rotation += 0.4f;
            if (++timer > 40)
                Projectile.velocity *= 0.97f;

            if (Projectile.timeLeft < 100)
            {
                Projectile.scale *= 0.98f;
                Projectile.rotation *= 0.98f;
            }

            if (Main.rand.NextBool(15))
            {
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    Dust.NewDust(Projectile.Center, 8, 8, 91);
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate > 0)
                Projectile.Kill();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
