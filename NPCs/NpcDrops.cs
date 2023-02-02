using AltLibrary.Common.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Biomes;
using TheDepths.Items;
using TheDepths.Items.Placeable;

namespace TheDepths.NPCs
{
    public class NpcDrops : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<DepthsBiome>()) || WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
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

        public class LivingFogDrop : IItemDropRuleCondition, IProvideItemConditionDescription
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
                return "";
            }
        }


        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(ItemDropRule.ByCondition(new LivingFogDrop(), ModContent.ItemType<LivingFog>(), 50, 20, 50));
            globalLoot.Add(ItemDropRule.ByCondition(new RubyRelicDrop(), ModContent.ItemType<RubyRelic>(), 50, 1, 1));
        }

        public class RubyRelicDrop : IItemDropRuleCondition, IProvideItemConditionDescription
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
                return "";
            }
        }
    }
}
