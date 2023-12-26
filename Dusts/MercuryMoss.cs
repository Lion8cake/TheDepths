using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheDepths.Dusts
{
	public class MercuryMoss : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true;
			dust.scale += 0.015f;
		}

		public override bool Update(Dust dust)
		{
			Lighting.AddLight(dust.position, 1.19f, 1.35f, 1.62f);
			return true;
		}
	}
}