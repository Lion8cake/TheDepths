using CalamityMod.NPCs.VanillaNPCAIOverrides.RegularEnemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Diagnostics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;

namespace TheDepths.Biomes;

public class DepthsBiome : ModBiome
{
    public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

    //public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<DepthsUnderworldBackground?>();

    public override int Music
    {
        get
        {
			bool inTown = Main.LocalPlayer.townNPCs > 2f;
			bool slimeRain = Main.slimeRain;
			if (Main.SceneMetrics.ShadowCandleCount > 0 || Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == ItemID.ShadowCandle)
			{
				inTown = false;
			}
			int newMusic = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Depths");
			if (Main.eclipse && !Main.remixWorld && (double)Main.player[Main.myPlayer].position.Y < Main.worldSurface * 16.0 + (double)(Main.screenHeight / 2))
			{
				newMusic = MusicID.Eclipse;
			}
			else if (Main.eclipse && Main.remixWorld && (double)Main.player[Main.myPlayer].position.Y > Main.rockLayer * 16.0)
			{
				newMusic = MusicID.Eclipse;
			}
			else if (slimeRain && !Main.player[Main.myPlayer].ZoneGraveyard && (!Main.bloodMoon || Main.dayTime) && (double)Main.player[Main.myPlayer].position.Y < Main.worldSurface * 16.0 + (double)(Main.screenHeight / 2))
			{
				newMusic = MusicID.SlimeRain;
			}
			else if (Main.remixWorld && Main.bloodMoon && !Main.player[Main.myPlayer].ZoneCrimson && !Main.player[Main.myPlayer].ZoneCorrupt && (double)Main.player[Main.myPlayer].position.Y > Main.rockLayer * 16.0 && Main.player[Main.myPlayer].position.Y <= (float)(Main.UnderworldLayer * 16))
			{
				newMusic = MusicID.Eerie;
			}
			else if (Main.remixWorld && Main.bloodMoon && Main.player[Main.myPlayer].position.Y > (float)(Main.UnderworldLayer * 16) && (double)(Main.player[Main.myPlayer].Center.X / 16f) > (double)Main.maxTilesX * 0.37 + 50.0 && (double)(Main.player[Main.myPlayer].Center.X / 16f) < (double)Main.maxTilesX * 0.63)
			{
				newMusic = MusicID.Eerie;
			}
			else if (Main.player[Main.myPlayer].ZoneShimmer)
			{
				newMusic = MusicID.Shimmer;
			}
			else if (inTown && Main.dayTime && ((Main.cloudAlpha == 0f && !Main._shouldUseWindyDayMusic) || (double)Main.player[Main.myPlayer].position.Y >= Main.worldSurface * 16.0 + (double)(Main.screenHeight / 2)) && !Main.player[Main.myPlayer].ZoneGraveyard)
			{
				newMusic = MusicID.TownDay;
			}
			else if (inTown && !Main.dayTime && ((!Main.bloodMoon && Main.cloudAlpha == 0f) || (double)Main.player[Main.myPlayer].position.Y >= Main.worldSurface * 16.0 + (double)(Main.screenHeight / 2)) && !Main.player[Main.myPlayer].ZoneGraveyard)
			{
				newMusic = MusicID.TownNight;
			}
			else if (Main.player[Main.myPlayer].ZoneSandstorm)
			{
				newMusic = MusicID.Sandstorm;
			}
			return newMusic;
		}
    }

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
        return InModBiome(player);
    }

    public static bool InModBiome(Player player)
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
        if (CheckIfTileCountisNull() >= 300)
        {
            flag2 = false;
        }
		return (flag || CheckIfTileCountisNull() >= 300) && !(!flag2 && flag) && Math.Abs(player.position.ToTileCoordinates().Y) >= Main.maxTilesY - 210;
	}

	private static int CheckIfTileCountisNull()
	{
		if (ModContent.GetInstance<TheDepthsModSystem>() != null)
		{
			return ModContent.GetInstance<TheDepthsModSystem>().artificialUnderworldBlockCount;
		}
		return 0;
	}
}
