using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace TheDepths.Projectiles.Chasme
{
	public class ShadowLash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 30;
			ProjectileID.Sets.TrailingMode[Type] = 3;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 960;
			TheDepthsIDs.Sets.UnreflectiveProjectiles[Type] = true;

			Main.projFrames[Type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 2;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Main.rand.NextBool(3))
				target.AddBuff(BuffID.ShadowFlame, 160);
		}
		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			if (Main.rand.NextBool(3))
				target.AddBuff(BuffID.ShadowFlame, 160);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			default(ShadowLashDrawer).Draw(Projectile);

			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			int frameHeight = texture.Height / Main.projFrames[Type];
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;

			float Rot = Projectile.rotation;

			Main.EntitySpriteDraw(texture, drawPos, frame, Color.White, Rot, new Vector2(texture.Width, frameHeight) / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * Projectile.Opacity;
		}

		public override void AI()
		{
			int num4 = Main.maxTilesY * 16;
			int num5 = 0;
			if (Projectile.ai[0] >= 0f)
			{
				num5 = (int)(Projectile.ai[1] / (float)num4);
			}

			if (Projectile.frameCounter++ >= 4)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
			if (Projectile.penetrate == 1 && Projectile.ai[0] >= 0f && num5 == 0)
			{
				Projectile.ai[1] += num4;
				num5 = 1;
				Projectile.netUpdate = true;
			}
			if (Projectile.penetrate == 1 && Projectile.ai[0] == -1f)
			{
				Projectile.ai[0] = -2f;
				Projectile.netUpdate = true;
			}
			if (num5 > 0 || Projectile.ai[0] == -2f)
			{
				Projectile.localAI[0] += 1f;
			}

			if (Projectile.velocity != Vector2.Zero)
			{
				Projectile.rotation = Projectile.rotation.AngleTowards(Projectile.velocity.ToRotation() - MathHelper.PiOver2, (float)Math.PI / 4f);
			}
			else
			{
				Projectile.rotation = Projectile.rotation.AngleLerp(0f, 0.2f);
			}

			bool flag3 = Projectile.velocity.Length() > 0.1f && Vector2.Dot(Projectile.oldVelocity.SafeNormalize(Vector2.Zero), Projectile.velocity.SafeNormalize(Vector2.Zero)) < 0.2f;

			float lerpValue = Utils.GetLerpValue(0f, 10f, Projectile.localAI[0], clamped: true);
			Color newColor = Color.Lerp(Color.Transparent, Color.Crimson, lerpValue);
			if (Main.rand.NextBool(6))
			{
				Dust dust6 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Shadowflame, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, newColor, 2.5f);
				dust6.noGravity = true;
				dust6.velocity *= 1.4f;
				dust6.velocity += Main.rand.NextVector2Circular(1f, 1f);
				dust6.velocity += Projectile.velocity * 0.15f;
			}
			if (Main.rand.NextBool(12))
			{
				Dust dust7 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Shadowflame, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, newColor, 1f);
				dust7.velocity += Main.rand.NextVector2Circular(1f, 1f);
				dust7.velocity += Projectile.velocity * 0.15f;
			}
			if (flag3)
			{
				int num3 = Main.rand.Next(2, 5 + (int)(lerpValue * 4f));
				for (int j = 0; j < num3; j++)
				{
					Dust dust4 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 100, newColor, 1f);
					dust4.velocity *= 0.3f;
					dust4.position = Projectile.Center;
					dust4.noGravity = true;
					dust4.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
					dust4.fadeIn = 2.2f;
					dust4.position += (dust4.position - Projectile.Center) * lerpValue * 10f;
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			float decreaseBy = 0.05f;
			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type] / 2; i++)
			{
				if (Projectile.oldPos[i].DirectionFrom(Projectile.oldPos[i + 1]).Length() > 0.6f)
				{
					for (int i2 = 0; i2 < Main.rand.Next(2, 4); i2++)
					{
						Dust d = Dust.NewDustPerfect(Projectile.oldPos[i], DustID.Shadowflame, Projectile.oldPos[i].DirectionFrom(Projectile.oldPos[i + 1]).RotateRandom(0.4f) * Main.rand.NextFloat(7, 9));
						d.noGravity = !Main.rand.NextBool(3);
						d.scale = (Main.rand.NextFloat(0.25f, 0.5f) * i2) - (decreaseBy * i);
						d.fadeIn = (Main.rand.NextFloat(0.75f, 1f) * i2) - (decreaseBy * i * 2);
						if (!d.noGravity)
						{
							d.scale *= 0.5f;
						}
					}
				}
			}
		}
	}

	public struct ShadowLashDrawer
	{
		private static VertexStrip _vertexStrip = new VertexStrip();

		private float transitToDark;

		public void Draw(Projectile proj)
		{
			transitToDark = Utils.GetLerpValue(0f, 6f, proj.localAI[0], clamped: true);
			MiscShaderData miscShaderData = GameShaders.Misc["TheDepths:ShadowLash"];
			miscShaderData.UseSaturation(-2f);
			miscShaderData.UseOpacity(MathHelper.Lerp(4f, 8f, transitToDark));
			miscShaderData.Apply();
			_vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
			_vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		private Color StripColors(float progressOnStrip)
		{
			float lerpValue = Utils.GetLerpValue(0f - 0.1f * transitToDark, 0.7f - 0.2f * transitToDark, progressOnStrip, clamped: true);
			Color result = Color.Lerp(Color.Lerp(Color.White, Color.Purple, transitToDark * 0.5f), Color.MediumPurple, lerpValue) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = (byte)(result.A / 8);
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float lerpValue = Utils.GetLerpValue(0f, 0.06f + transitToDark * 0.01f, progressOnStrip, clamped: true);
			lerpValue = 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(24f + transitToDark * 16f, 8f, Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true)) * lerpValue;
		}
	}
}
