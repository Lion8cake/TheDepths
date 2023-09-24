using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace TheDepths.Dusts
{
	public class ShadowflameEmber : ModDust
	{
		protected int scale = 120;

		public override void OnSpawn(Dust dust) {
			dust.velocity.Y = Main.rand.Next(-10, 6) * 0.1f;
			dust.velocity.X *= 0.3f;
			dust.scale *= 0.7f;
		}

		public override bool MidUpdate(Dust dust) {
			if (!dust.noGravity) {
				dust.velocity.Y += 0.005f;
			}

			if (dust.noLight) {
				return false;
			}

			scale--;
			dust.scale = scale / 120f;

			float strength = dust.scale * 1.4f;
			if (strength > 1f) {
				strength = 1f;
			}
			Lighting.AddLight(dust.position, 2.22f / 10 * strength, 1.01f / 10 * strength, 2.48f / 10 * strength);
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor) 
			=> new Color(lightColor.R, lightColor.G, lightColor.B, 25);
	}
}