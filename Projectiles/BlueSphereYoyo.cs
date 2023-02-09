using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheDepths.Projectiles
{
	public class BlueSphereYoyo : ModProjectile
	{
		public int TheGlittening;

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

		public override void AI()
		{
			TheGlittening++;
			if (TheGlittening == 300)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), Main.LocalPlayer.Center.X, Main.LocalPlayer.Center.Y, 0, 0, ModContent.ProjectileType<GlitteringGoldenLine>(), 0, 0f, Main.myPlayer, 0f, 0f);
				TheGlittening = 0;
			}

			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile other = Main.projectile[i];
				if (Main.projectile[i].type == ModContent.ProjectileType<GlitteringGoldenLine>() && Main.projectile[i].Hitbox.Intersects(Projectile.Hitbox) && other.Center.DistanceSQ(Projectile.Center) < 10 * 10)
				{
					Main.projectile[i].Kill();
					Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<BlueExplosion>(), Projectile.damage * 4, Projectile.knockBack, Main.myPlayer, 0f, 0f);
					Gore.NewGore(new EntitySource_Misc(""), Projectile.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), GoreID.Smoke3);
					Gore.NewGore(new EntitySource_Misc(""), Projectile.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), GoreID.Smoke2);
					Gore.NewGore(new EntitySource_Misc(""), Projectile.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), GoreID.Smoke1);
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			Projectile.ai[0] += 0.1f;
			Projectile.velocity *= 0.75f;
		}
	}
}
