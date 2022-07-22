using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles
{
	public class SkyfallYoyo : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 16f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 230f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 10f;
		}

		public override void SetDefaults() {
			Projectile.extraUpdates = 0;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 99;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			Projectile.ai[0] += 0.1f;
			Projectile.velocity *= 0.75f;
			target.AddBuff(ModContent.BuffType<Buffs.MercuryBoiling>(), 250, false);
		}

		public override void OnHitPvp(Player target, int damage, bool crit) {
		target.AddBuff(ModContent.BuffType<Buffs.MercuryBoiling>(), 250, false);
		}
	}
}
