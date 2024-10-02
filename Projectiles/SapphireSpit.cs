using Terraria.Audio;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles
{
	public class SapphireSpit : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.alpha = 255;
		}

		public override void AI() 
		{
			Projectile.velocity.Y += Projectile.ai[0];
			Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.01f / 255f, (float)(255 - Projectile.alpha) * 0.3f / 255f, (float)(255 - Projectile.alpha) * 0.45f / 255f);
			for (int num103 = 0; num103 < 2; num103++)
			{
				float num104 = Projectile.velocity.X / 3f * (float)num103;
				float num100 = Projectile.velocity.Y / 3f * (float)num103;
				int num101 = 4;
				int waterDust = Dust.NewDust(new Vector2(Projectile.position.X + (float)num101, Projectile.position.Y + (float)num101), Projectile.width - num101 * 2, Projectile.height - num101 * 2, DustID.WaterCandle, 0f, 0f, 100, default(Color), 2.4f);
				Dust obj = Main.dust[waterDust];
				obj.noGravity = true;
				obj.velocity *= 0.1f;
				obj.velocity += Projectile.velocity * 0.1f;
				obj.position.X -= num104;
				obj.position.Y -= num100;
			}
			if (Main.rand.NextBool(10))
			{
				int num102 = 4;
				int waterDustSmol = Dust.NewDust(new Vector2(Projectile.position.X + (float)num102, Projectile.position.Y + (float)num102), Projectile.width - num102 * 2, Projectile.height - num102 * 2, DustID.WaterCandle, 0f, 0f, 100, default(Color), 1.2f);
				Dust obj2 = Main.dust[waterDustSmol];
				obj2.velocity *= 0.25f;
				Dust obj3 = Main.dust[waterDustSmol];
				obj3.velocity += Projectile.velocity * 0.5f;
			}
			Projectile.rotation += 0.3f * (float)Projectile.direction;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) {
				Projectile.Kill();
			}
			else {
				Projectile.ai[0] += 0.1f;
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				Projectile.velocity *= 0.75f;
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			}
			return false;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info) 
		{
			target.AddBuff(ModContent.BuffType<Buffs.FreezingWater>(), 150, false);
		}
	}
}