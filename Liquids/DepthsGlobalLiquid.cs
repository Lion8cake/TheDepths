using ModLiquidLib.ModLoader;
using Terraria.ID;
using TheDepths.Worldgen;

namespace TheDepths.Liquids
{
	public class DepthsGlobalLiquid : GlobalLiquid
	{
		public override bool? EvaporatesInHell(int i, int j, int type)
		{
			if (type == LiquidID.Water && TheDepthsWorldGen.TileInDepths(i))
			{
				return false;
			}
			return null;
		}
	}
}
