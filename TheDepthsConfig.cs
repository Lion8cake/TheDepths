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
		[DefaultValue(true)]
		public bool EnableFog;

		[DefaultValue(false)]
        [ReloadRequired]
		public bool SlateConfig;
	}
}
