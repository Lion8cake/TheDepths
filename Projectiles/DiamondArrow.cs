using Terraria.Audio;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TheDepths.Projectiles
{
	public class DiamondArrow : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
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
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
			if (Main.myPlayer != Projectile.owner)
			{
				return;
			}
			Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, -8, -8, ModContent.ProjectileType<CrystalBallPassive>(), Projectile.damage % 2, Projectile.knockBack, Main.myPlayer, 0f, 0f);
			Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X - 2, Projectile.Center.Y - 2, 10, 10, ModContent.ProjectileType<CrystalBallPassive>(), Projectile.damage % 2, Projectile.knockBack, Main.myPlayer, 0f, 0f);
			Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X - 2, Projectile.Center.Y - 2, 0, 10, ModContent.ProjectileType<CrystalBallPassive>(), Projectile.damage % 2, Projectile.knockBack, Main.myPlayer, 0f, 0f);
		}
	}
}