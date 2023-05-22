using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles.Chasme
{
    public class ChasmeRay : ModProjectile
    {

        public ref float type => ref Projectile.ai[1];


        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.light = 1f;

        }
        public override void AI()
        {
            int[] dustTypes = { DustID.GemRuby, DustID.GemEmerald };
            for (int i = 0; i < 2; i++)
            {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 10, 10, dustTypes[(int)Projectile.ai[1]], Projectile.velocity.X, Projectile.velocity.Y, 150, default(Color), 1.25f);
                Dust dust = Main.dust[num];
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
            switch (type)
            {
                case 0:
                    break;
                case 1:
                    break;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 2; i++) { }
                //Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChasmeShockwave>(), Projectile.damage/2, 6, Projectile.owner, 2*i-1, Projectile.ai[1]);
            return base.OnTileCollide(oldVelocity);
        }
    }
}

