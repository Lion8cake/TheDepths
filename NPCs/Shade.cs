using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheDepths.Items.Banners;
using Terraria.GameContent.ItemDropRules;
using TheDepths.Items;
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;   

namespace TheDepths.NPCs
{
    public class Shade : ModNPC
    {
		public override void SetStaticDefaults()
		{
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
        }

		public override void SetDefaults()
        {
            NPC.width = 56;
            NPC.height = 70;
            NPC.damage = 46;
            NPC.defense = 10;
            NPC.lifeMax = 140;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 300f;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.5f;
			NPC.lavaImmune = true;
            NPC.aiStyle = 22;
            AIType = NPCID.FloatyGross;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<ShadeBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.Shade")
            });
        }

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			target.AddBuff(BuffID.Blackout, 180);
		}

		public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if ((spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && !Main.remixWorld) || (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && (spawnInfo.SpawnTileX < Main.maxTilesX * 0.38 + 50.0 || spawnInfo.SpawnTileX > Main.maxTilesX * 0.62) && Main.remixWorld))
            {
                return 1.3f;
            }
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShadePetItem>(), 500, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RubyRelic>(), 4, 1, 1));
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            if (NPC.life <= 0)
            {
                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 3; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 63);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 62);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61);
                }
            }
        }
    }
}
