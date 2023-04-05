using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Dusts
{
	public class NightDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = false;
			dust.noLight = true;
			dust.scale *= 0.5f;
		}
	}
}