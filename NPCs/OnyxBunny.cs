using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Dusts;
using TheDepths.Items.Placeable;

namespace TheDepths.NPCs
{
	public class OnyxBunny : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 7;
			Main.npcCatchable[Type] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Velocity = 1f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.TownCritter[Type] = true;
			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Onyx>();
		}

		public override void SetDefaults()
		{
			NPC.width = 18;
			NPC.height = 20;
			NPC.aiStyle = 7;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			AnimationType = NPCID.GemBunnyAmethyst;
			AIType = NPCID.GemBunnyAmethyst;
			NPC.friendly = false;
			NPC.catchItem = ModContent.ItemType<Items.OnyxBunny>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
			new FlavorTextBestiaryInfoElement(Language.GetTextValue("CommonBestiaryFlavor.GemBunny"))
			});
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) //possibly incomplete, needs special rules for special seeds?
		{
			int spawnRangeX = (int)(NPC.sWidth / 16 * 0.7);
			int spawnRangeY = (int)(NPC.sHeight / 16 * 0.7);
			for (int l = 0; l < 255; l++)
			{
				bool spawnThing = false;
				if (!(spawnInfo.Player.ZoneTowerNebula && spawnInfo.Player.ZoneTowerSolar && spawnInfo.Player.ZoneTowerStardust && spawnInfo.Player.ZoneTowerVortex) && ((!Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon) || Main.dayTime) && (!Main.eclipse || !Main.dayTime) && !Main.player[l].ZoneDungeon && !Main.player[l].ZoneCorrupt && !Main.player[l].ZoneCrimson && !Main.player[l].ZoneMeteor && !Main.player[l].ZoneOldOneArmy)
				{
					if (Main.player[l].Center.Y / 16f > (float)Main.UnderworldLayer && (!Main.remixWorld || !((double)(Main.player[l].Center.X / 16f) > (double)Main.maxTilesX * 0.39 + 50.0) || !((double)(Main.player[l].Center.X / 16f) < (double)Main.maxTilesX * 0.61)))
					{
						if (Main.player[l].townNPCs == 1f && Main.rand.NextBool(10))
						{
							spawnThing = true;
						}
						else if (Main.player[l].townNPCs == 2f && Main.rand.NextBool(5))
						{
							spawnThing = true;
						}
						else if (Main.player[l].townNPCs >= 3f && Main.rand.NextBool(3))
						{
							spawnThing = true;
						}
					}
					else if (Main.player[l].townNPCs == 1f)
					{
						if (Main.player[l].ZoneGraveyard && Main.rand.NextBool(10))
						{
							spawnThing = true;
						}
						else if (Main.rand.NextBool(3))
						{
							spawnThing = true;
						}
					}
					else if (Main.player[l].townNPCs == 2f)
					{
						if (Main.player[l].ZoneGraveyard && Main.rand.NextBool(6))
						{
							spawnThing = true;
						}
						else if (!Main.rand.NextBool(3))
						{
							spawnThing = true;
						}
					}
					else if (Main.player[l].townNPCs >= 3f)
					{
						if (Main.player[l].ZoneGraveyard && Main.rand.NextBool(3))
						{
							spawnThing = true;
						}
						else if (!Main.rand.NextBool(30))
						{
							spawnThing = true;
						}
					}
				}

				//int num14 = (int)(Main.player[l].position.X / 16f) - spawnRangeX;
				//int num15 = (int)(Main.player[l].position.X / 16f) + spawnRangeX;
				int num16 = (int)(Main.player[l].position.Y / 16f) - spawnRangeX;
				int num17 = (int)(Main.player[l].position.Y / 16f) + spawnRangeY;
				//int num3 = Main.rand.Next(num14, num15);
				int num4 = Main.rand.Next(num16, num17);
				//int num52 = Main.tile[num3, num4].TileType;
				if (spawnThing && Main.hardMode)
				{
					//if (num52 != 2 && num52 != 477 && num52 != 109 && num52 != 492 && !((double)num4 > Main.worldSurface))
					//{
					//    break;
					//}
					if (Main.raining && num4 <= Main.UnderworldLayer && num4 >= Main.rockLayer)
					{
						if (num4 >= Main.rockLayer && Main.rand.NextBool(35))
						{
							return 1f;
						}
					}
					else if (num4 > Main.UnderworldLayer)
					{
						if (Main.remixWorld && (double)(Main.player[l].Center.X / 16f) > Main.maxTilesX * 0.39 + 50.0 && (double)(Main.player[l].Center.X / 16f) < Main.maxTilesX * 0.61)
						{
							if (Main.rand.NextBool(28))
							{
								return 1f;
							}
						}
					}
					else if (Main.remixWorld)
					{
						if (num4 < Main.rockLayer && num4 > Main.worldSurface)
						{
							if (num4 >= Main.rockLayer && num4 <= Main.UnderworldLayer)
							{
								if (Main.rand.NextBool(28))
								{
									return 1f;
								}
							}
						}
					}
					else if (num4 >= Main.rockLayer && num4 <= Main.UnderworldLayer)
					{
						if (Main.rand.NextBool(28))
						{
							return 1f;
						}
					}
				}
			}
			return 0;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.direction == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Rectangle frame6 = NPC.frame;
			float num35 = 0f;
			float num36 = Main.NPCAddHeight(NPC);
			Vector2 halfSize = new Vector2(TextureAssets.Npc[Type].Width() / 2, TextureAssets.Npc[Type].Height() / Main.npcFrameCount[Type] / 2);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("TheDepths/NPCs/OnyxBunny_Glow").Value, new Vector2(NPC.position.X - screenPos.X + (float)(NPC.width / 2) - (float)TextureAssets.Npc[Type].Width() * NPC.scale / 2f + halfSize.X * NPC.scale, NPC.position.Y - screenPos.Y + (float)NPC.height - (float)TextureAssets.Npc[Type].Height() * NPC.scale / (float)Main.npcFrameCount[Type] + 4f + halfSize.Y * NPC.scale + num36 + num35 + NPC.gfxOffY), frame6, NPC.GetAlpha(Color.White), NPC.rotation, halfSize, NPC.scale, spriteEffects, 0f);
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			Player player = Main.player[NPC.target];
			if (NPC.life > 0)
			{
				for (int num461 = 0; (double)num461 < (double)10.0 / (double)NPC.lifeMax * 20.0; num461++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<GemOnyxDust>(), hit.HitDirection, -1f);
				}
				return;
			}
			for (int num462 = 0; num462 < 10; num462++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<GemOnyxDust>(), 2 * hit.HitDirection, -2f);
			}
			Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity.RotatedByRandom(MathHelper.Pi / 16), Mod.Find<ModGore>("OnyxBunnyGore1").Type);
			Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity.RotatedByRandom(MathHelper.Pi / 16), Mod.Find<ModGore>("OnyxBunnyGore2").Type);
		}
	}
}