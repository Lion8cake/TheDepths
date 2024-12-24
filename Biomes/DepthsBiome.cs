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
        bool flag;
        bool flag2 = true;
		if (Worldgen.TheDepthsWorldGen.DrunkDepthsLeft)
        {
			flag = Worldgen.TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(player.position.ToTileCoordinates().X) < Main.maxTilesX / 2;
		}
        else if (Worldgen.TheDepthsWorldGen.DrunkDepthsRight)
        {
			flag = Worldgen.TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(player.position.ToTileCoordinates().X) > Main.maxTilesX / 2;
		}
        else
        {
            flag = Worldgen.TheDepthsWorldGen.isWorldDepths;
        }
        if (ModContent.GetInstance<TheDepthsModSystem>().artificialUnderworldBlockCount >= 300)
        {
            flag2 = false;
        }
        return (flag || ModContent.GetInstance<TheDepthsModSystem>().artificialDepthsBlockCount >= 300) && !(!flag2 && flag) && Math.Abs(player.position.ToTileCoordinates().Y) >= Main.maxTilesY - 210;
	}
}
