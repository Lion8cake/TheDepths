using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TheDepths
{
	public class TheDepthsClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("DepthsClientsideConfig")]
		[Label("Enables The Depths tile lighting")]
		[Tooltip("Enables the Depths tile lighting making the depths require torches to see where you are going")]
		[DefaultValue(true)]
		public bool DepthsLightingConfig;

		[Label("Renames Shale to Slate")]
		[Tooltip("Renames Shale to Slate, Requires the mod to reload for the config to work")]
		[DefaultValue(false)]
        [ReloadRequired]
		public bool SlateConfig;

		[Label("[EXPERIMENTAL], Depths world icon")]
		[Tooltip("Enables the depths and underworld icons, NOTE: due to current limitations they are based off the world evil type not core type")]
		[DefaultValue(false)]
		public bool DepthsIconsConfig;
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
