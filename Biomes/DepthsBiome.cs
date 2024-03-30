using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace TheDepths.Biomes;

public class DepthsBiome : ModBiome
{
    public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

    //public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<DepthsUnderworldBackground?>();

    public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Depths");

    public override ModWaterStyle WaterStyle => ModContent.GetInstance<FreezingWaterStyle>();

    public override int BiomeTorchItemType => ModContent.ItemType<Items.Placeable.GeoTorch>();

	public override int BiomeCampfireItemType => ModContent.ItemType<Items.Placeable.GeoCampfire>();

	public override string MapBackground => BackgroundPath;

    public override string BackgroundPath => "TheDepths/Biomes/DepthsMapBackground";

    public override string BestiaryIcon => "TheDepths/Biomes/DepthsBestiaryIcon";

    public override void SpecialVisuals(Player player, bool isActive)
    {
        if (ModContent.GetInstance<TheDepthsClientConfig>().EnableFog)
        {
            float FogStrength = (NPC.AnyNPCs(ModContent.NPCType<NPCs.Chasme.ChasmeHeart>()) ? 0.7f : (Main.bloodMoon ? 0.90f : 0.95f));
            Color FogColor = (Main.bloodMoon ? Color.Crimson : Color.White);
            var FogShader = Filters.Scene["TheDepths:FogShader"]?.GetShader().UseIntensity(FogStrength).UseColor(FogColor);
            FogShader.UseImage(ModContent.Request<Texture2D>("TheDepths/Shaders/Perlin", AssetRequestMode.ImmediateLoad).Value, 0);
            FogShader.UseImage(ModContent.Request<Texture2D>("TheDepths/Shaders/Perlin", AssetRequestMode.ImmediateLoad).Value, 1);
            player.ManageSpecialBiomeVisuals("TheDepths:FogShader", isActive);
        }
        else
		{
            player.ManageSpecialBiomeVisuals("TheDepths:FogShader", false);
        }
    }

    public override void OnLeave(Player player)
    {
        player.ManageSpecialBiomeVisuals("TheDepths:FogShader", false);
    }

	public override bool IsBiomeActive(Player player)
    {
        if (Worldgen.TheDepthsWorldGen.DrunkDepthsLeft)
        {
			return Worldgen.TheDepthsWorldGen.IsPlayerInLeftDepths(player) && Math.Abs(player.position.ToTileCoordinates().Y) >= Main.maxTilesY - 210;
		}
        else if (Worldgen.TheDepthsWorldGen.DrunkDepthsRight)
        {
			return Worldgen.TheDepthsWorldGen.IsPlayerInRightDepths(player) && Math.Abs(player.position.ToTileCoordinates().Y) >= Main.maxTilesY - 210;
		}
        else
        {
            return Math.Abs(player.position.ToTileCoordinates().Y) >= Main.maxTilesY - 210 && Worldgen.TheDepthsWorldGen.depthsorHell;
        }
    }
}
