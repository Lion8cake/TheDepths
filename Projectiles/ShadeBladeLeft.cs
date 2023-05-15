using Terraria.Audio;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TheDepths.Items.Weapons;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace TheDepths.Projectiles
{
	public class ShadeBladeLeft : ModProjectile
	{
		public static bool ComingBack;
		public static bool LaunchReady;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
		public override void SetDefaults() {
			Projectile.width = 70;
			Projectile.height = 60;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 100000;
			Projectile.scale = 0.75f;
			Projectile.tileCollide = false;
		}

        public override void OnSpawn(IEntitySource source)
        {
			LaunchReady = true;
			ShadeBlade.handsReturning = false;
		}

        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ShadeBladeCalculator>()] > 0)
            {
				LaunchReady = false;
            }
			else if (LaunchReady == false && player.ownedProjectileCounts[ModContent.ProjectileType<ShadeBladeCalculator>()] == 0)
			{
				ComingBack = true;
				ShadeBlade.handsReturning = true;
			}
			if (ComingBack == true)
			{
				Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 20;
				Projectile.rotation = Projectile.velocity.ToRotation();
				if (Projectile.Hitbox.Intersects(player.Hitbox))
				{
					Projectile.Kill();
				}
				Projectile.penetrate = -1;
			}
			if (LaunchReady == true)
			{
				if (player.direction == 1)
				{
					Projectile.position = player.position + new Vector2(5, 10);
					Projectile.spriteDirection = -1;
					Projectile.rotation = 0;
				}
				else
				{
					Projectile.position = player.position + new Vector2(-58, 10);
					Projectile.spriteDirection = 1;
					Projectile.rotation = 0;
				}
				Projectile.penetrate = 1;
			}
			else
            {
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile secProj = Main.projectile[i];
					if (secProj.type == ModContent.ProjectileType<ShadeBladeCalculator>() && secProj.owner == Projectile.owner && secProj.active == true)
					{
						Projectile.velocity = secProj.velocity + new Vector2(0, -2);
					}
				}
				if (Projectile.velocity.X <= -1)
				{
					Projectile.rotation = Projectile.velocity.ToRotation() + 135;
				}
				else
				{
					Projectile.rotation = Projectile.velocity.ToRotation();
				}
				Projectile.penetrate = -1;
			}
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ShadeBladeRight>()] == 0 && ComingBack == false)
            {
				Projectile.Kill();
            }
			if (player.HeldItem.type != ModContent.ItemType<ShadeBlade>())
            {
				Projectile.Kill();
            }
		}
		public override bool? CanCutTiles()
		{
			return false;
		}

        public override bool PreKill(int timeLeft)
        {
			ComingBack = false;
			LaunchReady = false;
            return true;
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(projectileTexture.Width * 0.5f, Projectile.height * 0.5f);
			SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(projectileTexture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length / 3, spriteEffects, 0f);
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.ShadowDust>(), 0f, -2f, 0, default(Color), 0.75f);
			}
		}
	}
}