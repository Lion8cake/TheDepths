using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Dusts
{
	public class BlackGemsparkDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.scale *= 1.3f;
		}

		public override bool MidUpdate(Dust dust)
		{
			float num = dust.scale * 1.25f;
			if (num > 1f)
			{
				num = 1f;
			}
			Lighting.AddLight(dust.position, 0.058f * num, 0.061f * num, 0.06f * num);
			return false;
		}
	}
}