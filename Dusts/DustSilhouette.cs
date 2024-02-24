using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace TheDepths.Dusts
{
    public class DustSilhouette : ModDust
    {
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.1f;
			dust.velocity.Y = -0.5f;
		}

		public override bool Update(Dust dust)
		{
			if (dust.velocity.Y > 0f)
				dust.velocity.Y -= 0.2f;
			return true;
		}

		public override bool PreDraw(Dust dust)
		{
			Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), dust.position - Main.screenPosition, new Rectangle?(dust.frame), Color.White, dust.rotation, new Vector2(4f, 4f), dust.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			var lightStrength = (255 - dust.alpha) / 255f;
			lightStrength += 3;
			lightStrength /= 4f;

			var r = (int)(lightColor.R * lightStrength);
			var g = (int)(lightColor.G * lightStrength);
			var b = (int)(lightColor.B * lightStrength);
			var a = Math.Clamp(lightColor.A - dust.alpha, 0, 255);

			return new Color(r, g, b, a);
		}
	}
}
