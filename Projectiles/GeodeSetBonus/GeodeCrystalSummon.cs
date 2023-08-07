using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles.GeodeSetBonus
{
	public class GeodeCrystalSummon : ModProjectile
	{
	public bool shift;

	public bool shift2;

	public int timer;

	public float rot;
	
	public float fadeOut = 0.75f;

	public override void SetDefaults()
	{
		Projectile.width = 18;
		Projectile.height = 26;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 60;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
	}

	public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
	{
		Player player = Main.player[Projectile.owner];
		hitDirection = ((!(target.Center.X < player.Center.X)) ? 1 : (-1));
	}
	
	public override Color? GetAlpha(Color lightColor)
	{
		return new Color(255, 255, 255, 100) * fadeOut;
	}

	public static Vector2 RotateVector(Vector2 origin, Vector2 vecToRot, float rot)
	{
		float num = (float)(Math.Cos(rot) * (double)(vecToRot.X - origin.X) - Math.Sin(rot) * (double)(vecToRot.Y - origin.Y) + (double)origin.X);
		float num2 = (float)(Math.Sin(rot) * (double)(vecToRot.X - origin.X) + Math.Cos(rot) * (double)(vecToRot.Y - origin.Y) + (double)origin.Y);
		return new Vector2(num, num2);
	}

	public override void AI()
    {
		Player player = Main.player[Projectile.owner];
		rot += 0.03f;
		Projectile.Center = player.Center + RotateVector(default(Vector2), new Vector2(0f, (float)(90 + Projectile.frameCounter)), rot + Projectile.ai[0] * 1.04666674f);
		Projectile.velocity.X = ((Projectile.position.X > player.position.X) ? 1f : (-1f));
		if (Projectile.ai[1] == 0f)
		{
			Projectile.friendly = true;
			fadeOut = 0.15f;
			Projectile.alpha = 0;
			timer = 0;
		}
		else
		{
			Projectile.friendly = false;
			fadeOut = 0.15f;
			Projectile.alpha += ((!shift2) ? 5 : (-5));
			if (Projectile.alpha > 225 && !shift2)
			{
				shift2 = true;
			}
			if (Projectile.alpha <= 125)
			{
				shift2 = false;
			}
			timer++;
			if (timer > 180)
			{
				Projectile.ai[1] = 0f;
				timer = 0;
			}
		}
	}
	}
}