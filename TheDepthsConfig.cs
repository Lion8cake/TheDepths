using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria.ModLoader.Config;

namespace TheDepths
{
	public class TheDepthsClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("DepthsClientsideConfig")]
		[Label("Enable Depths Fog")]
		[Tooltip("Enables the intense fog seen within the depths, disable to turn off the fog.")]
		[DefaultValue(true)]
		public bool EnableFog;

		[Label("Renames Shale to Slate")]
		[Tooltip("Renames Shale to Slate, Requires the mod to reload for the config to work")]
		[DefaultValue(false)]
        [ReloadRequired]
		public bool SlateConfig;
	}

	/*public class TheDepthsServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("Depths Serverside Config")]
		[Label("EXPERIMENTAL!: No Water Evaporation")]
		[Tooltip("Warning: is known to disable worlds when trying to enter with this config on, turn off before entering a world")]
		[DefaultValue(false)]
		public bool DepthsNoWaterVapor;
	}*/
}
