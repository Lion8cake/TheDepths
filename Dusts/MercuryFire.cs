using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Dusts
{
	public class MercuryFire : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity.Y = Main.rand.Next(-10, 6) * 0.1f;
			dust.velocity.X *= 0.3f;
			dust.scale *= 0.7f;
		}

		public override bool MidUpdate(Dust dust) {
			if (!dust.noGravity) {
				dust.velocity.Y += 0.05f;
			}
			return false;
		}
	}
}