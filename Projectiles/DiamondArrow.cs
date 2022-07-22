using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace TheDepths.Projectiles
{
	public class DiamondArrow : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
		}

		public override void AI() {
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

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			Projectile.ai[0] += 0.1f;
			Projectile.velocity *= 0.75f;
		}
		
		public override void Kill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
		Vector2 launchVelocity = new Vector2(-4, 0);
		if (Main.myPlayer != Projectile.owner)
		{
			return;
		}
		int choice = Main.rand.Next(1);
		if (choice == 0)
		{
		    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);
			Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<CrystalBallPassive>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
		}
		
		int num = Main.rand.Next(1);
		if (num == 0)
		{ 
		    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);
			Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<CrystalBallPassive>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
		}
		
		int num2 = Main.rand.Next(1);
		if (num2 == 0)
		{ 
		    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);
			Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<CrystalBallPassive>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
		}
	}
	}
}