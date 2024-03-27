using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Audio;

namespace TheDepths.Projectiles.Chasme
{
	public class ChasmeShockwave : ModProjectile
	{
        public override void SetDefaults()
        {
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.alpha = 100;
			Projectile.light = 0.2f;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.scale = 0.9f;
			Projectile.ignoreWater = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			Vector2 vector = new(Projectile.position.X + (float)(width / 2), Projectile.position.Y + (float)(height / 2));
			int num = 12;
			int num2 = 12;
			vector.X -= num / 2;
			vector.Y -= num2 / 2;
			Projectile.velocity = Collision.noSlopeCollision(vector, Projectile.velocity, num, num2, fallThrough: true, fall2: true);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0f)
			{
				Projectile.direction = 1;
				Projectile.ai[0] = 1f;
			}
			int num825 = 6;
			if (Projectile.ai[1] == 0f)
			{
				Projectile.rotation += (float)(Projectile.direction * Projectile.directionY) * 0.13f;
				if (Projectile.collideY)
				{
					Projectile.ai[0] = 2f;
				}
				if (!Projectile.collideY && Projectile.ai[0] == 2f)
				{
					Projectile.direction = -Projectile.direction;
					Projectile.ai[1] = 1f;
					Projectile.ai[0] = 1f;
				}
				if (Projectile.collideX)
				{
					Projectile.directionY = -Projectile.directionY;
					Projectile.ai[1] = 1f;
				}
				Projectile.Colliding(new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height), new Rectangle((int)Projectile.position.X - 1, (int)Projectile.position.Y - 1, Projectile.width + 2, Projectile.height + 2));
			}
			else
			{
				Projectile.rotation -= (float)(Projectile.direction * Projectile.directionY) * 0.13f;
				if (Projectile.collideX)
				{
					Projectile.ai[0] = 2f;
				}
				if (!Projectile.collideX && this.ai[0] == 2f)
				{
					Projectile.directionY = -Projectile.directionY;
					Projectile.ai[1] = 0f;
					Projectile.ai[0] = 1f;
				}
				if (Projectile.collideY)
				{
					Projectile.direction = -Projectile.direction;
					Projectile.ai[1] = 0f;
				}
			}
			Projectile.velocity.X = num825 * Projectile.direction;
			Projectile.velocity.Y = num825 * Projectile.directionY;
			float num826 = (float)(270 - Main.mouseTextColor) / 400f;
			Lighting.AddLight((int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16, (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16, 0.9f, 0.3f + num826, 0.2f);
			/*if (Projectile.ai[1] == 0f)
			{
				Projectile.ai[1] = 1f;
				SoundEngine.PlaySound(in SoundID.Item8, Projectile.position) ;
			}
			Projectile.rotation += (float)Projectile.direction * 0.8f;
			Projectile.ai[0] += 1f;
			if (!(Projectile.ai[0] < 30f))
			{
				if (Projectile.ai[0] < 100f)
				{
					Projectile.velocity *= 1.06f;
				}
				else
				{
					Projectile.ai[0] = 200f;
				}
			}
			for (int num159 = 0; num159 < 2; num159++)
			{
				int num160 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 27, 0f, 0f, 100);
				Main.dust[num160].noGravity = true;
			}*/
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(in SoundID.Item10, Projectile.position);
			for (int num728 = 0; num728 < 30; num728++)
			{
				int num730 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 27, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 1.7f);
				Main.dust[num730].noGravity = true;
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 27, Projectile.velocity.X, Projectile.velocity.Y, 100);
			}
		}
	}
}

