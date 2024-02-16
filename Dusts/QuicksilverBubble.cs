using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Dusts
{
    public class QuicksilverBubble : ModDust
    {
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.1f;
			dust.velocity.Y = -0.5f;
		}

		public override bool Update(Dust dust)
		{
			Dust.lavaBubbles++;
			if (dust.velocity.Y > 0f)
				dust.velocity.Y -= 0.2f;

			if (dust.noGravity)
			{
				var noGravityLightStrength = dust.scale * 0.6f;
				if (noGravityLightStrength > 1f)
					noGravityLightStrength = 1f;
			}

			var lightStrength = dust.scale * 0.3f + 0.4f;
			if (lightStrength > 1f)
				lightStrength = 1f;

			return true;
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
