using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Biomes;

public class DepthsBiome : ModBiome
{
    public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

    //public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<DepthsUnderworldBackground?>();

    public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Depths");

    public override string MapBackground => "Biomes/DepthsMapBackground";

    public override string BestiaryIcon => "Biomes/DepthsBestiaryIcon";

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("The Depths");
    }

    public override bool IsBiomeActive(Player player)
    {
        return player.ZoneUnderworldHeight && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome";
    }
}
