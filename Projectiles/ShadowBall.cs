using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TheDepths.Projectiles
{
	public class ShadowBall : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 250;
			Projectile.aiStyle = 8;
			Projectile.alpha = 255; 
		}

		public override void AI()
	{
		if (Projectile.alpha > 0)
		{
			Projectile.alpha -= 25;
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
		}
		Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.01f / 255f, (float)(255 - Projectile.alpha) * 0.3f / 255f, (float)(255 - Projectile.alpha) * 0.45f / 255f);
		for (int num103 = 0; num103 < 2; num103++)
		{
			float num104 = Projectile.velocity.X / 3f * (float)num103;
			float num100 = Projectile.velocity.Y / 3f * (float)num103;
			int num101 = 4;
			int shadowDust = Dust.NewDust(new Vector2(Projectile.position.X + (float)num101, Projectile.position.Y + (float)num101), Projectile.width - num101 * 2, Projectile.height - num101 * 2, 27, 0f, 0f, 100, default(Color), 1.2f);
			Dust obj = Main.dust[shadowDust];
			obj.noGravity = true;
			obj.velocity *= 0.1f;
			obj.velocity += Projectile.velocity * 0.1f;
			obj.position.X -= num104;
			obj.position.Y -= num100;
		}
		if (Main.rand.NextBool(10))
		{
			int num102 = 4;
			int shadowDustSmol = Dust.NewDust(new Vector2(Projectile.position.X + (float)num102, Projectile.position.Y + (float)num102), Projectile.width - num102 * 2, Projectile.height - num102 * 2, 27, 0f, 0f, 100, default(Color), 0.6f);
			Dust obj2 = Main.dust[shadowDustSmol];
			obj2.velocity *= 0.25f;
			Dust obj3 = Main.dust[shadowDustSmol];
			obj3.velocity += Projectile.velocity * 0.5f;
		}
		Projectile.rotation += 0.3f * (float)Projectile.direction;
	}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) {
				Projectile.Kill();
			}
			return false;
		}

		public override void Kill(int timeLeft)
	{
		if (Main.myPlayer != Projectile.owner)
		{
			return;
		}
		int choice = Main.rand.Next(1);
		if (choice == 0)
		{
			Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<ShadowBall2>(), 24, 1f, Main.myPlayer, 0f, 0f);
		}
		
		int num = Main.rand.Next(1);
		if (num == 0)
		{
			Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<ShadowBall2>(), 24, 1f, Main.myPlayer, 0f, 0f);
		}
	}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			Projectile.ai[0] += 0.1f;
			Projectile.velocity *= 0.75f;
		}
	}
}