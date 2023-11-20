using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace TheDepths.ModSupport
{
	//[ExtendsFromMod("FargoSeeds")]
	public class FargosBoBWConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;

		public static FargosBoBWConfig Instance => ModContent.GetInstance<FargosBoBWConfig>();

		[Header("DrunkSeedRequiresFargosbestofbothworldsmod")]
		[Label("[i:174][i:TheDepths/ArqueriteOre] Both Cores")]
		[DefaultValue(true)]
		public bool BothCores;
	}
}
