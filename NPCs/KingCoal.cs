using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheDepths.Items.Banners;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using TheDepths.Items.Armor;
using TheDepths.Items.Placeable;
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;
using TheDepths.Projectiles;

namespace TheDepths.NPCs
{
    public class KingCoal : ModNPC
	{
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 74;
            NPC.damage = 25;
            NPC.defense = 15;
            NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.lavaImmune = true;
            NPC.value = 1200f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<KingCoalBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,

                new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.KingCoal")
            });
        }

		public override void FindFrame(int frameHeight)
		{
			if (NPC.velocity.Y == 0f)
			{
				if (NPC.direction == 1)
				{
					NPC.spriteDirection = 1;
				}
				if (NPC.direction == -1)
				{
					NPC.spriteDirection = -1;
				}
			}
			NPC.frame.Y = 0;
			if (NPC.velocity.Y != 0f)
			{
				NPC.frame.Y += frameHeight * 2;
			}
			else if (NPC.ai[1] > 0f)
			{
				NPC.frame.Y += frameHeight;
			}
		}

		public override void AI()
		{
			Lighting.AddLight(NPC.position, 0.3f, 0.1f, 0.1f);
			NPC.TargetClosest();
			NPC.velocity.X *= 0.93f;
			if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
			{
				NPC.velocity.X = 0f;
			}
			if (NPC.ai[0] == 0f)
			{
				NPC.ai[0] = 500f;
			}
			if (NPC.ai[2] != 0f && NPC.ai[3] != 0f)
			{
				NPC.position += NPC.netOffset;
				SoundEngine.PlaySound(in SoundID.Item8, NPC.position);
				for (int num1274 = 0; num1274 < 50; num1274++)
				{
					Vector2 val6 = new Vector2(NPC.position.X, NPC.position.Y);
					int num1612 = NPC.width;
					int num1613 = NPC.height;
					int num1285 = Dust.NewDust(val6, num1612, num1613, DustID.Torch, 0f, 0f, 100, default, Main.rand.Next(1, 3));
					Dust dust12 = Main.dust[num1285];
					Dust dust87 = dust12;
					dust87.velocity *= 3f;
					if (Main.dust[num1285].scale > 1f)
					{
						Main.dust[num1285].noGravity = true;
					}
				}
				NPC.position -= NPC.netOffset;
				NPC.position.X = NPC.ai[2] * 16f - (float)(NPC.width / 2) + 8f;
				NPC.position.Y = NPC.ai[3] * 16f - (float)NPC.height;
				NPC.netOffset *= 0f;
				NPC.velocity.X = 0f;
				NPC.velocity.Y = 0f;
				NPC.ai[2] = 0f;
				NPC.ai[3] = 0f;
				SoundEngine.PlaySound(in SoundID.Item8, NPC.position);
				for (int num1373 = 0; num1373 < 50; num1373++)
				{
					Vector2 val14 = new Vector2(NPC.position.X, NPC.position.Y);
					int num1628 = NPC.width;
					int num1629 = NPC.height;
					int num1385 = Dust.NewDust(val14, num1628, num1629, DustID.Torch, 0f, 0f, 100, default, Main.rand.Next(1, 3));
					Dust dust4 = Main.dust[num1385];
					Dust dust87 = dust4;
					dust87.velocity *= 3f;
					if (Main.dust[num1385].scale > 1f)
					{
						Main.dust[num1385].noGravity = true;
					}
				}
			}
			NPC.ai[0] += 1f;
			if (NPC.ai[0] == 100f || NPC.ai[0] == 200f || NPC.ai[0] == 300f)
			{
				NPC.ai[1] = 30f;
				NPC.netUpdate = true;
			}
			if (NPC.ai[0] >= 650f && Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.ai[0] = 1f;
				int targetTileX = (int)Main.player[NPC.target].Center.X / 16;
				int targetTileY = (int)Main.player[NPC.target].Center.Y / 16;
				Vector2 chosenTile = Vector2.Zero;
				if (NPC.AI_AttemptToFindTeleportSpot(ref chosenTile, targetTileX, targetTileY))
				{
					NPC.ai[1] = 20f;
					NPC.ai[2] = chosenTile.X;
					NPC.ai[3] = chosenTile.Y;
				}
				NPC.netUpdate = true;
			}
			if (NPC.ai[1] > 0f)
			{
				NPC.ai[1] -= 1f;
				if (NPC.ai[1] == 0f)
				{
					SoundEngine.PlaySound(in SoundID.Item42, NPC.position);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 val = Main.player[NPC.target].Center + new Vector2(NPC.Center.X, NPC.Center.Y);
						Vector2 val2 = NPC.Center + new Vector2(NPC.Center.X, NPC.Center.Y);
						float num10 = (float)Math.Atan2(val2.Y - val.Y, val2.X - val.X);
						int proj = Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X, NPC.Center.Y, (float)(Math.Cos(num10) * 14.0 * -1.0), (float)(Math.Sin(num10) * 14.0 * -1.0), ModContent.ProjectileType<LumpOfCoal>(), 15, 0f, 0);
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
					}
				}
			}
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode && (spawnInfo.Player.ZoneUnderworldHeight && !Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && !Main.remixWorld) || Main.hardMode && (spawnInfo.Player.ZoneUnderworldHeight && !Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && (spawnInfo.SpawnTileX < Main.maxTilesX * 0.38 + 50.0 || spawnInfo.SpawnTileX > Main.maxTilesX * 0.62) && Main.remixWorld))
            {
                return 0.12f;
            }
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Ember>(), 1, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CharredCrown>(), 100, 1, 1));
			//npcLoot.Add(ItemDropRule.Common(ItemID.Ruby, 50, 1, 1));
			npcLoot.Add(ItemDropRule.Food(ItemID.Hotdog, 150));
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
			NPC.ai[1] = 0f;

			if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            if (NPC.life <= 0)
            {
                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("KingCoalGore1").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("KingCoalGore2").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("KingCoalGore2").Type);
                }
            }
        }
    }
}