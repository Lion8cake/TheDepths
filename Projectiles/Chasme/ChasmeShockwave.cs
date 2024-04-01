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
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Type] = 1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
		}

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
			Projectile.timeLeft = 300;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		protected virtual Vector2 Collide()
		{
			return Collision.noSlopeCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height, true, true);
		}

		public Vector2 moveDirection;
		public Vector2 newVelocity = Vector2.Zero;

		bool collideX = false;
		bool collideY = false;
		public float Speed = 0.1f;
		float speedTimer = 0f;

		public override void AI()
		{
			//Some ai taken from SLR made by SkippZz
			if (Projectile.tileCollide) //the projectile just collided with the tile, give the moveDirection the opposite of the owners direction
				moveDirection = new Vector2(Projectile.ai[2], 1);

			Projectile.tileCollide = false;

			newVelocity = Collide();
			if (Math.Abs(newVelocity.X) < 0.5f)
				collideX = true;
			else
				collideX = false;

			if (Math.Abs(newVelocity.Y) < 0.5f)
				collideY = true;
			else
				collideY = false;

			if (Projectile.ai[1] == 0f)
			{
				Projectile.rotation += (float)(moveDirection.X * moveDirection.Y) * 0.75f;
				if (collideY)
					Projectile.ai[0] = 2f;

				if (!collideY && Projectile.ai[0] == 2f)
				{
					moveDirection.X = -moveDirection.X;
					Projectile.ai[1] = 1f;
					Projectile.ai[0] = 1f;
				}

				if (collideX)
				{
					moveDirection.Y = -moveDirection.Y;
					Projectile.ai[1] = 1f;
				}
			}
			else
			{
				Projectile.rotation -= (float)(moveDirection.X * moveDirection.Y) * 0.45f;
				if (collideX)
					Projectile.ai[0] = 2f;

				if (!collideX && Projectile.ai[0] == 2f)
				{
					moveDirection.Y = -moveDirection.Y;
					Projectile.ai[1] = 0f;
					Projectile.ai[0] = 1f;
				}

				if (collideY)
				{
					moveDirection.X = -moveDirection.X;
					Projectile.ai[1] = 0f;
				}
			}

			speedTimer += 1f;
			if (!(speedTimer < 30f))
			{
				if (speedTimer < 100f)
				{
					Speed += 0.106f;
				}
				else
				{
					speedTimer = 200f;
				}
			}
			Projectile.velocity = Speed * moveDirection;
			Projectile.velocity = Collide(); // set the velo based of Collision.NoSlopeCollision() and speed + movedirection.

			for (int num159 = 0; num159 < 2; num159++)
			{
				int num160 = Dust.NewDust(new Vector2(Projectile.position.X + 4, Projectile.position.Y + 4), Projectile.width - 4, Projectile.height - 4, DustID.GemRuby, 0f, 0f, 100);
				Main.dust[num160].noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float rotationMultiplier = 0.7f;

			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			int frameHeight = texture.Height;
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
			int length = ProjectileID.Sets.TrailCacheLength[Projectile.type];
			for (int i = 0; i < length; i++)
			{
				//float multiply = ((float)(length - i) / length) * projectile.Opacity * 0.2f;
				float multiply = (float)(length - i) / length * 0.5f;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] - Main.screenPosition + (Projectile.Size / 2f), frame, new Color(255, 128, 128, 128) * multiply, Projectile.oldRot[i] * rotationMultiplier, new Vector2(texture.Width, frameHeight) / 2, Projectile.scale, SpriteEffects.None, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position - Main.screenPosition + (Projectile.Size / 2f), frame, new Color(255, 255, 255, 175) * 0.7f, Projectile.rotation * rotationMultiplier, new Vector2(texture.Width, frameHeight) / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(in SoundID.Item10, Projectile.position);
			for (int num728 = 0; num728 < 30; num728++)
			{
				int num730 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemRuby, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 1.7f);
				Main.dust[num730].noGravity = true;
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemRuby, Projectile.velocity.X, Projectile.velocity.Y, 100);
			}
		}
	}
}

