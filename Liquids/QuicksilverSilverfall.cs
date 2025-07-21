using ModLiquidLib.ModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace TheDepths.Liquids
{
	public class QuicksilverSilverfall : ModLiquidFall
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
