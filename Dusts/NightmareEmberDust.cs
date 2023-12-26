using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheDepths.Dusts
{
	public class NightmareEmberDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = false;
			dust.scale *= 0.5f;
			dust.noGravity = true;
		}

		public override bool MidUpdate(Dust dust)
		{
			if (dust.noLight)
			{
				return false;
			}

			float strength = dust.scale * 1.4f;
			if (strength > 1f)
			{
				strength = 1f;
			}
			Lighting.AddLight(dust.position, 1.9f * strength, 0.72f * strength, 2.15f * strength);
			dust.alpha = 0;
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
			=> new Color(lightColor.R, lightColor.G, lightColor.B, 0);
	}
}