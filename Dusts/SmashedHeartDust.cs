using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Dusts
{
	public class SmashedHeartDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noLight = true;
			dust.scale *= 1f;
		}
	}
}