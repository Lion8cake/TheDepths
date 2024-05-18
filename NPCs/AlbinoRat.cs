using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Biomes;

namespace TheDepths.NPCs
{
    public class AlbinoRat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Snail];
            Main.npcCatchable[Type] = true;

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;
            NPCID.Sets.ShimmerTransformToNPC[Type] = NPCID.Shimmerfly;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
			{
                Velocity = 1f,
                Position = new Vector2(1f, 2f)
            });
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 14;
            NPC.height = 12;
            NPC.aiStyle = 7;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath4;
            NPC.npcSlots = 0.25f;

            NPC.catchItem = (short)ModContent.ItemType<Items.AlbinoRat>();
            NPC.lavaImmune = true;
            NPC.buffImmune[ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
            NPC.buffImmune[ModContent.BuffType<Buffs.MercuryPoisoning>()] = true;
            AIType = NPCID.Rat;
            AnimationType = NPCID.Rat;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.AlbinoRat")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if ((spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && !Main.remixWorld) || (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && (spawnInfo.SpawnTileX < Main.maxTilesX * 0.38 + 50.0 || spawnInfo.SpawnTileX > Main.maxTilesX * 0.62) && Main.remixWorld))
            {
                return 1f;
            }
            return 0f;
        }
    }
}
