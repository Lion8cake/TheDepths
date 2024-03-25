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
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 1;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 960;

			Main.projFrames[Type] = 6;
            Projectile.width = 32;
			Projectile.height = 32;
            Projectile.aiStyle = -1;//9;
			Projectile.friendly = true;
			Projectile.light = 0.8f;
			Projectile.penetrate = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 12;
		}

		//Inflict Shadowflame?

		//Add purple light

		public override bool PreDraw(ref Color lightColor)
		{
			default(ShadowLashDrawer).Draw(Projectile);

			SpriteEffects dir = (SpriteEffects)0;
			if (Projectile.spriteDirection == -1)
			{
				dir = (SpriteEffects)1;
			}
			Vector2 vector161 = Projectile.position + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D value179 = TextureAssets.Projectile[Projectile.type].Value;
			Color color155 = Projectile.GetAlpha(Color.White);
			Vector2 origin21 = new Vector2((float)value179.Width, (float)value179.Height) / 2f;
			float num314 = Projectile.rotation;
			Vector2 vector162 = Vector2.One * Projectile.scale;
			Rectangle? sourceRectangle5 = null;

			float lerpValue8 = Utils.GetLerpValue(0f, 8f, Projectile.velocity.Length(), clamped: true);
			num314 *= lerpValue8;
			vector162.X *= MathHelper.Lerp(1f, 0.8f, lerpValue8);
			num314 += -(float)Math.PI / 2f * lerpValue8;
			sourceRectangle5 = value179.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			origin21 = sourceRectangle5.Value.Size() / 2f;
			vector161 -= Projectile.velocity * 1f;
			vector161 = Projectile.oldPos[0] + Projectile.Size / 2f - Main.screenPosition - Projectile.velocity / 2f;

			float lerpValue9 = Utils.GetLerpValue(0f, 6f, Projectile.localAI[0], clamped: true);
			Color color167 = new Color(255, 255, 255, 127) * 0.75f;
			Vector2 scale13 = new(lerpValue9);
			Vector2 spinningpoint31 = new Vector2(4f * scale13.X, 0f);
			double radians31 = num314;
			Vector2 spinningpoint3 = Utils.RotatedBy(spinningpoint31, radians31);
			for (float num333 = 0f; num333 < 1f; num333 += 0.25f)
			{
				Texture2D texture10 = value179;
				Vector2 val25 = vector161;
				double radians32 = num333 * ((float)Math.PI * 2f);
				Main.EntitySpriteDraw(texture10, val25 + spinningpoint3.RotatedBy(radians32), sourceRectangle5, color167, num314, origin21, scale13, dir);
			}
			return true;
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
			if (Main.rand.Next(6) == 0)
			{
				Dust dust6 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Shadowflame, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, newColor, 2.5f);
				dust6.noGravity = true;
				dust6.velocity *= 1.4f;
				dust6.velocity += Main.rand.NextVector2Circular(1f, 1f);
				dust6.velocity += Projectile.velocity * 0.15f;
			}
			if (Main.rand.Next(12) == 0)
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
	}

	public struct ShadowLashDrawer
	{
		private static VertexStrip _vertexStrip = new VertexStrip();

		private float transitToDark;

		public void Draw(Projectile proj)
		{
			transitToDark = Utils.GetLerpValue(0f, 6f, proj.localAI[0], clamped: true);
			MiscShaderData miscShaderData = GameShaders.Misc["FlameLash"];
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

