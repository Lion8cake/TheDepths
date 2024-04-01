using Terraria.Audio;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TheDepths.Projectiles.Chasme
{
	public class QuicksilverTears : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 22;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.aiStyle = 1;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.alpha = 255;
		}

		public override void AI() {
			if (Projectile.alpha > 0)
				Projectile.alpha -= 8;
			if (Projectile.alpha <= 0)
				Projectile.alpha = 0;

			Projectile.ai[0] += 1f;
			Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
			if (Projectile.spriteDirection == -1) {
				Projectile.rotation += MathHelper.Pi;
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<Buffs.MercuryBoiling>(), 125, false);
		}

		public override void OnKill(int timeLeft)
		{
			for (int num105 = 0; num105 < 50; num105++)
			{
				int num106 = Dust.NewDust(new Vector2(Projectile.position.X - 6f, Projectile.position.Y + (float)(Projectile.height / 2) - 8f), Projectile.width + 12, 24, ModContent.DustType<Dusts.QuicksilverBubble>());
				Main.dust[num106].velocity.Y -= 3f;
				Main.dust[num106].velocity.X *= 2.5f;
				Main.dust[num106].scale = 0.8f;
				Main.dust[num106].alpha = 100;
				Main.dust[num106].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.SplashWeak, new Vector2((int)Projectile.position.X, (int)Projectile.position.Y));
		}
	}
}