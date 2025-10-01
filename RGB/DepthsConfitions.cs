using ModLiquidLib.ModLoader;
using ModLiquidLib.Utils;
using ReLogic.Peripherals.RGB;
using System;
using Terraria;
using Terraria.ModLoader;
using TheDepths.Biomes;
using TheDepths.Liquids;
using TheDepths.NPCs.Chasme;
using TheDepths.Worldgen;
using static Terraria.GameContent.RGB.CommonConditions;

namespace TheDepths.RGB
{
	public static class DepthsConfitions
	{
		private class SimpleCondition : ConditionBase
		{
			private Func<Player, bool> _condition;

			public SimpleCondition(Func<Player, bool> condition)
			{
				_condition = condition;
			}

			public override bool IsActive()
			{
				return _condition(base.CurrentPlayer);
			}
		}

		public static class Boss
		{
			private enum ChasmeActiveState
			{
				NoBoss,
				SoulActive,
				BeastActive
			}

			public static readonly ChromaCondition Chasme = new SimpleCondition((Player player) => !Main.gameMenu && ModLoader.HasMod(nameof(TheDepths)) && IsChasmeSoulActive(player) == ChasmeActiveState.SoulActive);

			public static readonly ChromaCondition ShalestoneBeast = new SimpleCondition((Player player) => !Main.gameMenu && ModLoader.HasMod(nameof(TheDepths)) && IsChasmeSoulActive(player) == ChasmeActiveState.BeastActive);

			private static ChasmeActiveState IsChasmeSoulActive(Player player)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<ChasmeHeart>())
					{
						if (Main.npc[i].ai[0] > 0f)
						{
							return ChasmeActiveState.SoulActive;
						}
						else
						{
							return ChasmeActiveState.BeastActive;
						}
					}
				}
				return ChasmeActiveState.NoBoss;
			}
		}

		public static class Depth
		{
			public static readonly ChromaCondition Mercury = new SimpleCondition((Player player) => !Main.gameMenu && ModLoader.HasMod(nameof(TheDepths)) && TheDepthsWorldGen.InDepths(player) && player.ZoneRockLayerHeight && player.position.ToTileCoordinates().Y > Main.maxTilesY - 400);

			public static readonly ChromaCondition Depths = new SimpleCondition((Player player) => !Main.gameMenu && ModLoader.HasMod(nameof(TheDepths)) && DepthsBiome.InModBiome(player) && player.ZoneUnderworldHeight);
		}

		public static class Alert
		{
			public static readonly ChromaCondition QuicksilverIndicator = new SimpleCondition((Player player) => !Main.gameMenu && ModLoader.HasMod(nameof(TheDepths)) && player.GetWet(LiquidLoader.LiquidType<Quicksilver>()) && !player.GetModPlayer<TheDepthsPlayer>().cSkin);
		}

		public static readonly ChromaCondition InDepthsMenu = new SimpleCondition((Player player) => Main.gameMenu && !TheDepthsReflectionUtils.GetIsLoading() && ModLoader.HasMod(nameof(TheDepths)) && (ModContent.GetInstance<TheDepthsMenuTheme>().IsSelected || ModContent.GetInstance<TheDepthsOtherworldlyMenuTheme>().IsSelected) && !Main.drunkWorld);
	}
}
