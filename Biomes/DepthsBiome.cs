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

    public override ModWaterStyle WaterStyle => ModContent.GetInstance<FreezingWaterStyle>();

    public override string MapBackground => BackgroundPath;

    public override string BackgroundPath => "TheDepths/Biomes/DepthsMapBackground";

    public override string BestiaryIcon => "TheDepths/Biomes/DepthsBestiaryIcon";

    public override bool IsBiomeActive(Player player)
    {
        if (Main.drunkWorld && TheDepthsWorldGen.DrunkDepthsLeft)
        {
            return Math.Abs(player.position.ToTileCoordinates().X) < Main.maxTilesX / 2 && Math.Abs(player.position.ToTileCoordinates().Y) >= Main.maxTilesY - 210;
        }
        else if (Main.drunkWorld && TheDepthsWorldGen.DrunkDepthsRight)
        {
            return Math.Abs(player.position.ToTileCoordinates().X) > Main.maxTilesX / 2 && Math.Abs(player.position.ToTileCoordinates().Y) >= Main.maxTilesY - 210;
        }

        return Math.Abs(player.position.ToTileCoordinates().Y) >= Main.maxTilesY - 210 && TheDepthsWorldGen.depthsorHell;
    }
}
