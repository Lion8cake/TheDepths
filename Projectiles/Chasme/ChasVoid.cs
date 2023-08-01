using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles.Chasme
{
    public class ChasVoid : ModProjectile
    {

        float speed = 2;
        float maxSpeed = 7;
        int target = -1;
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 1;

            Projectile.width = Projectile.height = 60;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.light = 1f;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            Player player = Main.player[0];
            bool chase = Projectile.ai[0] == 1 && Projectile.timeLeft > 40;
            if (target == -1)
            {
                FindPlayerTarget();
                player = Main.player[target];
            }
            if (chase)
            {
                Projectile.velocity = Projectile.DirectionTo(player.Center) * speed;
                speed = Math.Clamp(speed + speed * 0.008f, 0, maxSpeed);

            } else if (Projectile.timeLeft < 40)
            {
                Projectile.velocity *= 0.98f;
                Projectile.scale*= 0.98f;
            }

            Projectile.rotation += Projectile.velocity.Length() * 0.03f;


        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            
            Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            
            SpriteEffects spriteEffects = SpriteEffects.None;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + new Vector2(30, 30);
                float opacity = ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, rectangle, Color.White * opacity, Projectile.rotation - i*0.1f, texture.Size()/2f, Projectile.scale - i / (float)Projectile.oldPos.Length / 3, spriteEffects, 0);
            }
            return true;
        }
        public void FindPlayerTarget()
        {
            int temp = 0;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && Projectile.Distance(Main.player[i].Center) > Projectile.Distance(Main.player[temp].Center) || i == 0)
                {
                    temp = i;
                }
            }
            target = temp;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 dustpos = Projectile.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i/20) * 60;
                Vector2 dustVel = dustpos.DirectionTo(Projectile.Center) * 8;
                var a = Dust.NewDust(dustpos, 3, 3, DustID.PurpleMoss, dustVel.X, dustVel.Y);
                Main.dust[a].noGravity = true;
            }
        }
    }
}

