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
using Terraria.GameContent.Bestiary;
using TheDepths.Biomes;
using TheDepths.Dusts;

namespace TheDepths.NPCs
{
	public class Archroma : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = 2;
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
		}
		
		public override void SetDefaults() {
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 75;
			NPC.defense = 35;
			NPC.lifeMax = 800;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 150000f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
			AnimationType = NPCID.Crimslime;
			NPC.lavaImmune = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<AchromaBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.Archroma")
			});
		}

		public override void AI()
		{
			bool flag = false;
			if (!Main.dayTime || NPC.life != NPC.lifeMax || (double)NPC.position.Y > Main.worldSurface * 16.0 || Main.slimeRain)
			{
				flag = true;
			}
			if (NPC.life == NPC.lifeMax)
			{
				flag = false;
			}
			if (NPC.ai[2] > 1f)
			{
				NPC.ai[2] -= 1f;
			}
			if (NPC.wet)
			{
				if (NPC.collideY)
				{
					NPC.velocity.Y = -2f;
				}
				if (NPC.velocity.Y < 0f && NPC.ai[3] == NPC.position.X)
				{
					NPC.direction *= -1;
					NPC.ai[2] = 200f;
				}
				if (NPC.velocity.Y > 0f)
				{
					NPC.ai[3] = NPC.position.X;
				}
				if (NPC.velocity.Y > 2f)
				{
					NPC.velocity.Y *= 0.9f;
				}
				else if (NPC.directionY < 0)
				{
					NPC.velocity.Y -= 1.5f;
				}
				NPC.velocity.Y -= 0.5f;
				if (NPC.velocity.Y < -18f)
				{
					NPC.velocity.Y = -18f;
				}
				if (NPC.ai[2] == 1f && flag)
				{
					NPC.TargetClosest();
				}
			}
			NPC.aiAction = 0;
			if (NPC.ai[2] == 0f)
			{
				NPC.ai[0] = -100f;
				NPC.ai[2] = 1f;
				NPC.TargetClosest();
			}
			if (NPC.velocity.Y == 0f)
			{
				if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
				{
					NPC.position.X -= NPC.velocity.X + (float)NPC.direction;
				}
				if (NPC.ai[3] == NPC.position.X)
				{
					NPC.direction *= -1;
					NPC.ai[2] = 200f;
				}
				NPC.ai[3] = 0f;
				NPC.velocity.X *= 0.8f;
				if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
				{
					NPC.velocity.X = 0f;
				}
				if (flag)
				{
					NPC.ai[0] += 1f;
				}
				NPC.ai[0] += 1f;
				NPC.ai[0] += 2f;
				float num27 = -250f;
				int num28 = 0;
				if (NPC.ai[0] >= 0f)
				{
					num28 = 1;
				}
				if (NPC.ai[0] >= num27 && NPC.ai[0] <= num27 * 0.5f)
				{
					num28 = 2;
				}
				if (NPC.ai[0] >= num27 * 2f && NPC.ai[0] <= num27 * 1.5f)
				{
					num28 = 3;
				}
				if (num28 > 0)
				{
					NPC.netUpdate = true;
					if (flag && NPC.ai[2] == 1f)
					{
						NPC.TargetClosest();
					}
					if (num28 == 3)
					{
						NPC.velocity.Y = -8f;
						NPC.velocity.Y -= 2f;
						NPC.velocity.X += 3 * NPC.direction;
						NPC.velocity.X += 0.5f * (float)NPC.direction;
						NPC.ai[0] = -200f;
						NPC.ai[3] = NPC.position.X;
					}
					else
					{
						NPC.velocity.Y = -6f;
						NPC.velocity.X += 2 * NPC.direction;
						NPC.velocity.X += 2 * NPC.direction;
						NPC.ai[0] = -120f;
						if (num28 == 1)
						{
							NPC.ai[0] += num27;
						}
						else
						{
							NPC.ai[0] += num27 * 2f;
						}
					}
				}
				else if (NPC.ai[0] >= -30f)
				{
					NPC.aiAction = 1;
				}
			}
			else if (NPC.target < 255 && ((NPC.direction == 1 && NPC.velocity.X < 3f) || (NPC.direction == -1 && NPC.velocity.X > -3f)))
			{
				if (NPC.collideX && Math.Abs(NPC.velocity.X) == 0.2f)
				{
					NPC.position.X -= 1.4f * (float)NPC.direction;
				}
				if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
				{
					NPC.position.X -= NPC.velocity.X + (float)NPC.direction;
				}
				if ((NPC.direction == -1 && (double)NPC.velocity.X < 0.01) || (NPC.direction == 1 && (double)NPC.velocity.X > -0.01))
				{
					NPC.velocity.X += 0.2f * (float)NPC.direction;
				}
				else
				{
					NPC.velocity.X *= 0.93f;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.hardMode && (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && !Main.remixWorld) || Main.hardMode && (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && (spawnInfo.SpawnTileX < Main.maxTilesX * 0.38 + 50.0 || spawnInfo.SpawnTileX > Main.maxTilesX * 0.62) && Main.remixWorld))
			{
				return 0.02f;
			}
			return 0f;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 10; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<MercurySparkleDust>());
				}
			}
		}
	}
}