using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles.Chasme
{
    public class EmeraldEnergy : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            //Projectile.light = 1f;
        }

        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 10, 10, DustID.GemEmerald, Projectile.velocity.X, Projectile.velocity.Y, 150, default(Color), 1.25f);
                Dust dust = Main.dust[num];
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }
    }
}

