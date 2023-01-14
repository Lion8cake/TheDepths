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

    public override string MapBackground => BackgroundPath;

    public override string BackgroundPath => "TheDepths/Biomes/DepthsMapBackground";

    public override string BestiaryIcon => "TheDepths/Biomes/DepthsBestiaryIcon";

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("The Depths");
    }

    public override bool IsBiomeActive(Player player)
    {
        return Math.Abs(player.position.ToTileCoordinates().Y) >= Main.maxTilesY - 210 && WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome";
    }
}
