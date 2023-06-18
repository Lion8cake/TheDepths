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
		[Label("Enables The Depths tile lighting")]
		[Tooltip("Enables the Depths tile lighting making the depths require torches to see where you are going")]
		[DefaultValue(true)]
		public bool DepthsLightingConfig;

		[Label("Renames Shale to Slate")]
		[Tooltip("Renames Shale to Slate, Requires the mod to reload for the config to work")]
		[DefaultValue(false)]
        [ReloadRequired]
		public bool SlateConfig;

		public struct WorldDataValues
		{
			public bool depths;
			public bool depthsLeft;
			public bool depthsRight;
		}

		// Key value is each twld path
		[DefaultListValue(false)]
		[JsonProperty]
		private Dictionary<string, WorldDataValues> worldData = new Dictionary<string, WorldDataValues>();

		// Methods to avoid public variable getting picked up by serialiser
		public Dictionary<string, WorldDataValues> GetWorldData() { return worldData; }
		public void SetWorldData(Dictionary<string, WorldDataValues> newDict) { worldData = newDict; }
		public static void Save(ModConfig config)
		{
			Directory.CreateDirectory(ConfigManager.ModConfigPath);
			string filename = config.Mod.Name + "_" + config.Name + ".json";
			string path = Path.Combine(ConfigManager.ModConfigPath, filename);
			string json = JsonConvert.SerializeObject((object)config, ConfigManager.serializerSettings);
			File.WriteAllText(path, json);
		}
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
