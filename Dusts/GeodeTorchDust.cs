using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Dusts
{
	public class GeodeTorchDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity *= 0.4f;
			dust.noGravity = false;
			dust.noLight = true;
			dust.scale *= 1f;
		}
	}
}