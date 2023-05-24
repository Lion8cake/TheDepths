using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Biomes;
using TheDepths.Items;
using TheDepths.Items.Placeable;
using TheDepths.Items.Weapons;

namespace TheDepths.NPCs
{
    public class NpcDrops : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<DepthsBiome>()) || TheDepthsWorldGen.InDepths)
            {
                pool.Remove(NPCID.Hellbat);
                pool.Remove(NPCID.LavaSlime);
                pool.Remove(NPCID.FireImp);
                pool.Remove(NPCID.Demon);
                pool.Remove(NPCID.VoodooDemon);
                pool.Remove(NPCID.BoneSerpentHead);
                pool.Remove(NPCID.BoneSerpentBody);
                pool.Remove(NPCID.BoneSerpentTail);
                pool.Remove(NPCID.DemonTaxCollector);
                pool.Remove(NPCID.Lavabat);
                pool.Remove(NPCID.RedDevil);
            }
        }
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
            globalLoot.Add(ItemDropRule.ByCondition(new DepthsBiomeHardmodeDropRule(), ModContent.ItemType<BlueSphere>(), 400));

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
