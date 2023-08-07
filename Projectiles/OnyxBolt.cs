using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles
{
	public class OnyxBolt : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.alpha = 255;
		Projectile.penetrate = 2;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Magic;
	}

	public override void AI()
	{
		for (int i = 0; i < 2; i++)
		{
			int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("BlackGemsparkDust").Type, Projectile.velocity.X, Projectile.velocity.Y, 150, default(Color), 1.25f);
			Main.dust[num].noGravity = true;
			Dust obj = Main.dust[num];
			obj.velocity *= 0.3f;
		}
	}

	public override void Kill(int timeLeft)
	{
		for (int i = 0; i < 8; i++)
		{
			int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("BlackGemsparkDust").Type, Projectile.velocity.X * 0.75f, Projectile.velocity.Y * 0.75f, 150);
			Main.dust[num].noGravity = true;
		}
	}
}
}