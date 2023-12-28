using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;

namespace TheDepths.Projectiles
{
	public class OnyxExplosion : ModProjectile
	{
		public override string Texture => "TheDepths/Projectiles/CrystalBall";

		public override void SetDefaults()
		{
			Projectile.width = 70;
			Projectile.height = 70;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(in SoundID.Item14, Projectile.position);
			for (int num980 = 0; num980 < 10; num980++)
			{
				int num981 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.BlackGemsparkDust>(), 0f, 0f, 200, default(Color), 1.7f);
				Main.dust[num981].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)Projectile.width / 2f;
				Main.dust[num981].noGravity = true;
				Dust dust36 = Main.dust[num981];
				Dust dust212 = dust36;
				dust212.velocity *= 3f;
				num981 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.BlackGemsparkDust>(), 0f, 0f, 100, default(Color), 0.5f);
				Main.dust[num981].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)Projectile.width / 2f;
				dust36 = Main.dust[num981];
				dust212 = dust36;
				dust212.velocity *= 2f;
				Main.dust[num981].noGravity = true;
				Main.dust[num981].fadeIn = 2.5f;
			}
			for (int num982 = 0; num982 < 5; num982++)
			{
				int num983 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.BlackGemsparkDust>(), 0f, 0f, 0, default(Color), 1.7f);
				Dust obj5 = Main.dust[num983];
				Vector2 center24 = Projectile.Center;
				Vector2 spinningpoint67 = Vector2.UnitX.RotatedByRandom(3.1415927410125732);
				double radians56 = Projectile.velocity.ToRotation();
				obj5.position = center24 + spinningpoint67.RotatedBy(radians56) * (float)Projectile.width / 2f;
				Main.dust[num983].noGravity = true;
				Dust dust37 = Main.dust[num983];
				Dust dust212 = dust37;
				dust212.velocity *= 3f;
			}
			if (Main.myPlayer != Projectile.owner)
			{
				return;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.ArmorPenetration += 0.1f;
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			modifiers.ArmorPenetration += 0.1f;
		}
	}
}