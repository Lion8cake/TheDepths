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
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Rat];
            Main.npcCatchable[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Velocity = 1f,
                Position = new(1, 2)
            });
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Rat);
            NPC.catchItem = (short)ModContent.ItemType<Items.AlbinoRat>();
            NPC.aiStyle = 7;
            NPC.friendly = true;
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

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public virtual void OnCatchNPC(Player player, Item item)
        {
            item.stack = 1;

            try
            {
                var npcCenter = NPC.Center.ToTileCoordinates();
            }
            catch
            {
                return;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if ((spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths && !Main.remixWorld) || (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths && (spawnInfo.SpawnTileX < Main.maxTilesX * 0.38 + 50.0 || spawnInfo.SpawnTileX > Main.maxTilesX * 0.62) && Main.remixWorld))
            {
                return 1f;
            }
            return 0f;
        }
    }
}
