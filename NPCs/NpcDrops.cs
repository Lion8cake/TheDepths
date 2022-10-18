using AltLibrary.Common.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items;
using TheDepths.Items.Placeable;

namespace TheDepths.NPCs
{
    public class NpcDrops : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
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
    }
}
