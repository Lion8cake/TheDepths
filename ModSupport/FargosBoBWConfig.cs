using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace TheDepths.ModSupport
{
	//[ExtendsFromMod("FargoSeeds")]
	public class FargosBoBWConfig : ModConfig {

		public override bool Autoload(ref string name)
		{
			return ModLoader.HasMod("FargoSeeds");
		}

		public override ConfigScope Mode => ConfigScope.ServerSide;

		public static FargosBoBWConfig Instance => ModContent.GetInstance<FargosBoBWConfig>();

		[Header("DrunkSeedRequiresFargosbestofbothworldsmod")]
		[DefaultValue(true)]
		public bool BothCores;
	}
}
