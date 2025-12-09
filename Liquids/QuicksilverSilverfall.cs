using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Liquids
{
	public class QuicksilverSilverfall : ModWaterfallStyle
	{
		public override float? Alpha(int x, int y, float Alpha, int maxSteps, int s, Tile tileCache)
		{
			float num = 1f;
			if (s > maxSteps - 10)
			{
				num *= (float)(maxSteps - s) / 10f;
			}
			return num;
		}
	}
}
