using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Biomes;

namespace TheDepths.NPCs
{
    public class QuartzCrawler : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Snail];
            Main.npcCatchable[Type] = true;

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Velocity = 1f,
                Position = new(1, 2)
            });
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 12;
            NPC.height = 12;
            NPC.aiStyle = 67;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.npcSlots = 0.5f;
            NPC.noGravity = true;

            NPC.catchItem = (short)ModContent.ItemType<Items.QuartzCrawler>();
            NPC.lavaImmune = true;
            NPC.buffImmune[ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
            NPC.buffImmune[ModContent.BuffType<Buffs.MercuryPoisoning>()] = true;
            AIType = NPCID.Snail;
            AnimationType = NPCID.Snail;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.QuartzCrawler")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if ((spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths && !Main.remixWorld) || (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths && (spawnInfo.SpawnTileX < Main.maxTilesX * 0.38 + 50.0 || spawnInfo.SpawnTileX > Main.maxTilesX * 0.62) && Main.remixWorld))
            {
                return 0.5f;
            }
            return 0f;
        }
    }
}
