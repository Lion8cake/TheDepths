using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Biomes;
using TheDepths.Items;
using TheDepths.Items.Placeable;
using TheDepths.Items.Weapons;
using TheDepths.Worldgen;

namespace TheDepths.NPCs
{
    public class NpcDrops : GlobalNPC
    {
		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
            if (TheDepthsWorldGen.InDepths(spawnInfo.Player))
            {
                int num58 = Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY - 1].WallType;
                if (Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY - 2].WallType == 244 || Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].WallType == 244)
                {
                    num58 = 244;
                }

                bool flag9 = (double)spawnInfo.SpawnTileY <= Main.rockLayer;
                if (Main.remixWorld)
                {
                    flag9 = (double)spawnInfo.SpawnTileY > Main.rockLayer && spawnInfo.SpawnTileY <= Main.maxTilesY - 190;
                }

                int num56 = Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType;
                bool flag13 = (double)spawnInfo.SpawnTileY <= Main.worldSurface;
                bool flag15 = ((spawnInfo.SpawnTileX < WorldGen.oceanDistance || spawnInfo.SpawnTileX > Main.maxTilesX - WorldGen.oceanDistance) && Main.tileSand[num56] && (double)spawnInfo.SpawnTileY < Main.rockLayer) || (spawnInfo.SpawnTileType == 53 && WorldGen.oceanDepths(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY));
                if (Main.remixWorld)
                {
                    if ((double)spawnInfo.SpawnTileY < Main.worldSurface + 5.0)
                    {
                        Main.raining = false;
                        Main.cloudAlpha = 0f;
                        Main.dayTime = false;
                    }
                    if (spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson)
                    {
                        flag15 = false;
                    }
                    if ((double)spawnInfo.SpawnTileY > Main.rockLayer - 20.0)
                    {
                        if (!(spawnInfo.SpawnTileY <= Main.maxTilesY - 190) && ((Main.bloodMoon || (Main.eclipse && Main.dayTime)) && (double)spawnInfo.SpawnTileX > (double)Main.maxTilesX * 0.38 + 50.0 && (double)spawnInfo.SpawnTileX < (double)Main.maxTilesX * 0.62))
                        {
                            flag13 = true;
                        }
                    }
                }
                num56 = SpawnNPC_TryFindingProperGroundTileType(num56, spawnInfo.SpawnTileX, spawnInfo.SpawnTileY);

                if (!(spawnInfo.Player.ZoneTowerNebula) && !(spawnInfo.Player.ZoneTowerVortex) && !(spawnInfo.Player.ZoneTowerStardust) && !(spawnInfo.Player.ZoneTowerSolar) && !(spawnInfo.Sky) && !(spawnInfo.Invasion)
    && !(num58 == 244 && !Main.remixWorld) && !(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].WallType == 62 || spawnInfo.SpiderCave) && !((NPC.SpawnTileOrAboveHasAnyWallInSet(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY, WallID.Sets.AllowsUndergroundDesertEnemiesToSpawn) || spawnInfo.DesertCave) && WorldGen.checkUnderground(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY))
    && !((!spawnInfo.PlayerInTown || (!NPC.savedAngler && !NPC.AnyNPCs(376))) && spawnInfo.Water && flag15) && !(!spawnInfo.Water && !NPC.savedAngler && !NPC.AnyNPCs(376) && (spawnInfo.SpawnTileX < WorldGen.beachDistance || spawnInfo.SpawnTileX > Main.maxTilesX - WorldGen.beachDistance) && Main.tileSand[num56] && ((double)spawnInfo.SpawnTileY < Main.worldSurface || Main.remixWorld))
    && !(!spawnInfo.PlayerInTown && spawnInfo.Water && (spawnInfo.PlayerInTown || num56 == 60)) && !(spawnInfo.PlayerInTown) && !(spawnInfo.Player.ZoneDungeon)
    && !(spawnInfo.Player.ZoneMeteor) && !(DD2Event.Ongoing && spawnInfo.Player.ZoneOldOneArmy) && !((Main.remixWorld || (double)spawnInfo.SpawnTileY <= Main.worldSurface) && !Main.dayTime && Main.snowMoon)
    && !((Main.remixWorld || (double)spawnInfo.SpawnTileY <= Main.worldSurface) && !Main.dayTime && Main.pumpkinMoon) && !(((double)spawnInfo.SpawnTileY <= Main.worldSurface || (Main.remixWorld && (double)spawnInfo.SpawnTileY > Main.rockLayer)) && Main.dayTime && Main.eclipse)
    && !(SpawnNPC_CheckToSpawnUndergroundFairy(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY, spawnInfo.Player.whoAmI)) && !(Main.hardMode && spawnInfo.SpawnTileType == 70 && spawnInfo.Water)
    && !(((num56 == 226 || num56 == 232) && spawnInfo.Lihzahrd) || (Main.remixWorld && spawnInfo.Lihzahrd)) && !(Sandstorm.Happening && spawnInfo.Player.ZoneSandstorm && TileID.Sets.Conversion.Sand[num56] && NPC.Spawning_SandstoneCheck(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY))
    && !(Main.hardMode && !spawnInfo.Water && flag9 && (num56 == 116 || num56 == 117 || num56 == 109 || num56 == 164))
    && !((num56 == 204 && spawnInfo.Player.ZoneCrimson) || num56 == 199 || num56 == 200 || num56 == 203 || num56 == 234 || num56 == 662) && !((num56 == 22 && spawnInfo.Player.ZoneCorrupt) || num56 == 23 || num56 == 25 || num56 == 112 || num56 == 163 || num56 == 661)
    && !(flag13) && !(flag9) && (spawnInfo.SpawnTileY > Main.maxTilesY - 190))
                {
                    pool[0] = 0f;
                }
            }
        }

		private static int SpawnNPC_TryFindingProperGroundTileType(int spawnTileType, int x, int y)
		{
			if (!NPC.IsValidSpawningGroundTile(x, y))
			{
				for (int i = y + 1; i < y + 30; i++)
				{
					if (NPC.IsValidSpawningGroundTile(x, i))
					{
						return Main.tile[x, i].TileType;
					}
				}
			}
			return spawnTileType;
		}

		private static bool SpawnNPC_CheckToSpawnUndergroundFairy(int spawnTileX, int spawnTileY, int plr)
		{
			if (!NPC.fairyLog)
			{
				return false;
			}
			int num = 500;
			if (Main.tenthAnniversaryWorld && !Main.getGoodWorld)
			{
				num = 250;
			}
			if (Main.hardMode)
			{
				num = (int)((float)num * 1.66f);
			}
			if (Main.player[plr].RollLuck(num) != 0)
			{
				return false;
			}
			if ((double)spawnTileY < (Main.worldSurface + Main.rockLayer) / 2.0 || spawnTileY >= Main.maxTilesY - 300)
			{
				return false;
			}
			if (NPC.AnyHelpfulFairies())
			{
				return false;
			}
			return true;
		}

		//Doesnt work properly, gotta thank tmod for the half baked features
		//Please see NPCSpawningEdit IL edit for removing hell enemies
		#region DepthsBiomeDropRule
		public class DepthsBiomeDropRule : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                if (Conditions.SoulOfWhateverConditionCanDrop(info))
                {
                    return info.player.InModBiome(ModContent.GetInstance<DepthsBiome>());
                }
                return false;
            }

            public bool CanShowItemDropInUI()
            {
                return false;
            }

            public string GetConditionDescription()
            {
                return "Drops in the Depths";
            }
        }

        #endregion

        #region UnderworldDropRule
        public class UnderworldDropRule : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                if (Conditions.SoulOfWhateverConditionCanDrop(info))
                {
                    return info.player.ZoneUnderworldHeight && !info.player.InModBiome(ModContent.GetInstance<DepthsBiome>());
                }
                return false;
            }

            public bool CanShowItemDropInUI()
            {
                return false;
            }

            public string GetConditionDescription()
            {
                return "Drops in the Underworld";
            }
        }
        #endregion

        #region DepthsBiomeHardmodeDropRule
        public class DepthsBiomeHardmodeDropRule : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                if (Conditions.SoulOfWhateverConditionCanDrop(info))
                {
                    return info.player.InModBiome(ModContent.GetInstance<DepthsBiome>()) && Main.hardMode;
                }
                return false;
            }

            public bool CanShowItemDropInUI()
            {
                return false;
            }

            public string GetConditionDescription()
            {
                return "Drops in the Depths in Hardmode";
            }
        }
        #endregion

        #region DepthsBiomeHardmodeDropRuleNoRemix
        public class DepthsBiomeHardmodeDropRuleNoRemix : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                if (Conditions.SoulOfWhateverConditionCanDrop(info))
                {
                    return info.player.InModBiome(ModContent.GetInstance<DepthsBiome>()) && Main.hardMode && !Main.remixWorld;
                }
                return false;
            }

            public bool CanShowItemDropInUI()
            {
                return false;
            }

            public string GetConditionDescription()
            {
                return "Drops in the Depths in Hardmode";
            }
        }
        #endregion

        #region DepthsBiomeHardmodeDropRuleRemix
        public class DepthsBiomeHardmodeDropRuleRemix : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                if (Conditions.SoulOfWhateverConditionCanDrop(info))
                {
                    return info.player.InModBiome(ModContent.GetInstance<DepthsBiome>()) && Main.hardMode && Main.remixWorld;
                }
                return false;
            }

            public bool CanShowItemDropInUI()
            {
                return false;
            }

            public string GetConditionDescription()
            {
                return "Drops in the Depths in Hardmode in the Remix seed";
            }
        }
        #endregion

        #region UnderworldHardmodeDropRule
        public class UnderworldHardmodeDropRule : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                if (Conditions.SoulOfWhateverConditionCanDrop(info))
                {
                    return info.player.ZoneUnderworldHeight && !info.player.InModBiome(ModContent.GetInstance<DepthsBiome>()) && Main.hardMode;
                }
                return false;
            }

            public bool CanShowItemDropInUI()
            {
                return false;
            }

            public string GetConditionDescription()
            {
                return "Drops in the Underworld in Hardmode";
            }
        }
        #endregion

        #region UnderworldPrehardmodeONLYDropRule
        public class UnderworldPrehardmodeONLYDropRule : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                if (Conditions.SoulOfWhateverConditionCanDrop(info))
                {
                    return info.player.ZoneUnderworldHeight && !info.player.InModBiome(ModContent.GetInstance<DepthsBiome>()) && !Main.hardMode;
                }
                return false;
            }

            public bool CanShowItemDropInUI()
            {
                return false;
            }

            public string GetConditionDescription()
            {
                return "Drops in the Underworld in Pre-Hardmode";
            }
        }
        #endregion

        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(ItemDropRule.ByCondition(new DepthsBiomeHardmodeDropRule(), ModContent.ItemType<LivingFog>(), 45, 20, 50));
            globalLoot.Add(ItemDropRule.ByCondition(new DepthsBiomeDropRule(), ModContent.ItemType<RubyRelic>(), 50));
            globalLoot.Add(ItemDropRule.ByCondition(new DepthsBiomeHardmodeDropRuleNoRemix(), ModContent.ItemType<BlueSphere>(), 400));
            globalLoot.Add(ItemDropRule.ByCondition(new DepthsBiomeHardmodeDropRuleRemix(), ModContent.ItemType<WhiteLightning>(), 400));

            globalLoot.RemoveWhere(
            rule => rule is ItemDropWithConditionRule drop
                && drop.itemId == ItemID.LivingFireBlock
                && drop.condition is Conditions.LivingFlames
            );
            globalLoot.RemoveWhere(
            rule => rule is ItemDropWithConditionRule drop
                && drop.itemId == ItemID.Cascade
                && drop.condition is Conditions.YoyoCascade
            );
            globalLoot.RemoveWhere(
            rule => rule is ItemDropWithConditionRule drop
                && drop.itemId == ItemID.HelFire
                && drop.condition is Conditions.YoyosHelFire
            );

            globalLoot.Add(ItemDropRule.ByCondition(new UnderworldHardmodeDropRule(), ItemID.LivingFireBlock, 45, 20, 50));
            globalLoot.Add(ItemDropRule.ByCondition(new UnderworldPrehardmodeONLYDropRule(), ItemID.Cascade, 400));
            globalLoot.Add(ItemDropRule.ByCondition(new UnderworldHardmodeDropRule(), ItemID.HelFire, 400));
        }
    }
}
