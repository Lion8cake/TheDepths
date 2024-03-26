using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Armor;
using TheDepths.Projectiles.Chasme;
using TheDepths.Biomes;
using Terraria.GameContent.Bestiary;
using TheDepths.Worldgen;
using TheDepths.Items;
using TheDepths.Items.Weapons;
using Terraria.GameContent.UI.Elements;
using TheDepths.Projectiles.Summons;
using TheDepths.Projectiles;
using ReLogic.Content;

namespace TheDepths.NPCs.Chasme;

[AutoloadBossHead] // For loading "ChasmeHeart_Head_Boss" automatically
public class ChasmeHeart : ModNPC
{
	int[] ChasmePartIDs = new int[4];

	private int TimesDownedHead = 0;

	public float drawTimer;
	float alpha = 0;
	int fadeTimer = 0;

	public override void SetStaticDefaults()
	{
		NPCID.Sets.BossBestiaryPriority.Add(Type);

		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = true;

		NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
		{
			CustomTexturePath = "TheDepths/NPCs/Chasme/ChasmeSoul",
			Position = new Vector2(0f, 30f),
			PortraitPositionYOverride = 0f
		};
		NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
	}

	public override void SetDefaults()
	{
		NPC.npcSlots = 10f;
		NPC.width = 32;
		NPC.height = 24;
		NPC.aiStyle = -1;
		NPC.defense = 15;//30;
		NPC.lifeMax = 5500;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.knockBackResist = 0f;
		NPC.damage = 50;
		NPC.boss = true;
		NPC.value = 80000;
		NPC.HitSound = SoundID.Item30;
		NPC.DeathSound = SoundID.NPCDeath7;
		NPC.ScaleStats_UseStrengthMultiplier(0.6f); //dont scale like a regular npc in different gamemodes
		if (!Main.dedServ)
		{
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Chasme");
		}
		SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
	}

	public override void AI()
	{
		//ChasmePartID array
		//1 head
		//2 body
		//3 hand left
		//4 hand right

		//NPC AI array
		//ai[0] is the core timer, The timer for when the core gets crammed back into chasme
		//ai[1] is the invicibility timer
		//ai[2] is the delay between shot chasme shadowlashes
		//ai[3]

		//Attacks
		//Head shoots ruby rays that are very similar to the ruby bolt from the ruby staff
		//at 50% health (red eyes) Ruby rays will be 50% bigger, deal more damage and explode into 2 slow to fast moving blazing wheel like projectiles
		//at 25% core health (crying) the head will drop quicksilver tears on the player, these inflict damage and quicksilver poisoning
		//Maybe: Moonlord like lazer for the ruby ray in expert instead of an exploding upgraded ruby ray at 50% health

		//Hands shoot emerald shots when alive
		//Hands lunge at the player every now and then
		//2 hands normal, 4 hands expert, 8 hands legendary

		//Chasme's soul shoots Shadowlash projectiles, she has a small summoning frame when congelling up an orb

		//Legendary mod:
		//8 hands
		//geomancers spawn during the 2nd half of the fight
		//head and core is smaller by 50%, body and hands are bigger by 50%
		//speed and damage are already at max 1.5x increase

		//Extra notes:
		//Hands and Head lose accuracy when the core is out, TBD on how much accuracy is lost
		//Speed is increased by 1.5x when the head starts crying
		//Attack is slowly increased up to a max of 1.5x as the core loses health
		//Core and head become invicible for 2 seconds between transition (core closing only)

		//Head spawning
		if (Main.npc[ChasmePartIDs[1]].type != ModContent.NPCType<ChasmeHead>())
		{
			int head = NPC.NewNPC(new EntitySource_Misc(""), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<ChasmeHead>());
			(Main.npc[head].ModNPC as ChasmeHead).HeartID = NPC.whoAmI;
			ChasmePartIDs[1] = Main.npc[head].whoAmI;
		}
		//Body Spawning
		if (Main.npc[ChasmePartIDs[2]].type != ModContent.NPCType<ChasmeBody>())
		{
			int body = NPC.NewNPC(new EntitySource_Misc(""), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<ChasmeBody>());
			(Main.npc[body].ModNPC as ChasmeBody).HeartID = NPC.whoAmI;
			ChasmePartIDs[2] = Main.npc[body].whoAmI;
		}

		//targetting/getting the correct player
		NPC.TargetClosestUpgraded();
		Player player = Main.player[NPC.target];

		//Movement/despawning
		if (player.dead || NPC.target < 0 || Math.Abs(NPC.Center.X - player.Center.X) / 16f > (float)750)
		{
			NPC.velocity.Y += 0.05f;
			NPC.EncourageDespawn(10);
		}
		else
		{
			float speed = 3f;
			if (NPC.life <= NPC.lifeMax / 4)
				speed *= 1.5f;
			Vector2 direction = NPC.DirectionTo(player.Center + new Vector2(1600 * NPC.Center.X > player.Center.X ? 1 : -1, 0));
			direction *= speed;
			NPC.velocity = (NPC.velocity * (20f - 1) + direction) / 20f;
			if (NPC.velocity == Vector2.Zero)
				NPC.velocity = new Vector2(0.1f, 0.1f); //Make sure the velocity is never 0 (npcs despawn when its 0 for some reason)
		}

		//Alpha to not have 2 hearts
		NPC.alpha += NPC.dontTakeDamage ? -4 : 4;
		if (NPC.alpha >= 255)
			NPC.alpha = 255;
		if (NPC.alpha <= 0)
			NPC.alpha = 0;

		NPC headNPC = Main.npc[ChasmePartIDs[1]];
		NPC bodyNPC = Main.npc[ChasmePartIDs[2]];

		//Stages, goes head -> core -> head 4 times
		bool Headlife3 = NPC.life <= NPC.lifeMax / 4;
		bool Headlife2 = NPC.life <= NPC.lifeMax / 2;
		bool Headlife1 = NPC.life <= (NPC.lifeMax / 4 + NPC.lifeMax / 2);
		if (headNPC.dontTakeDamage && ((Headlife1 && TimesDownedHead == 0) || (Headlife2 && TimesDownedHead == 1) || (Headlife3 && TimesDownedHead == 2) || NPC.ai[0] >= 60 * 20))
		{
			headNPC.life = headNPC.lifeMax;
			NPC.ai[1]++;
			if (NPC.ai[1] >= 2 * 60)
			{
				headNPC.dontTakeDamage = false;
				TimesDownedHead = (Headlife3 ? 3 : (Headlife2 ? 2 : (Headlife1 ? 1 : 0))); //not the best programmer, may cause the head to need to be fought twice in some stages, if this ever happens in testing, CHANGE THIS!!
				NPC.ai[1] = 0;
				NPC.ai[0] = 0;
			}
			NPC.dontTakeDamage = true;
		}
		else
		{
			NPC.dontTakeDamage = !headNPC.dontTakeDamage;
		}
		if (!headNPC.dontTakeDamage)
		{
			drawTimer = 0;
		}
		if (!NPC.dontTakeDamage)
		{
			if (NPC.ai[1] <= 0)
			{
				NPC.ai[0]++;
				if (NPC.ai[0] > 60 * 20)
				{
					NPC.ai[0] = 60 * 20;
				}
			}

			//Spawn chasme shadowlashs
			NPC.ai[2]++;
			if (NPC.ai[2] >= 2 * 60)
			{
				if (Main.netMode != 1)
				{
					int projDamage = 42 / 2; //divided by 2 because projectiles multiply the damage by 2 for some dumbass reason
					Vector2 val = Main.player[NPC.target].Center + new Vector2(NPC.Center.X, NPC.Center.Y - 26);
					Vector2 val2 = NPC.Center + new Vector2(NPC.Center.X, NPC.Center.Y - 26);
					float shootSpeed = (float)Math.Atan2(val2.Y - val.Y, val2.X - val.X);
					Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X, NPC.Center.Y - 26, (float)(Math.Cos(shootSpeed) * 14.0 * -1.0), (float)(Math.Sin(shootSpeed) * 14.0 * -1.0), ModContent.ProjectileType<ShadowLash>(), projDamage, 0f, 0);
				}
				NPC.ai[2] = 0;
			}
		}
	}

	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.ChasmeSoul")
		});
	}

    public override void BossLoot(ref string name, ref int potionType)
    {
		potionType = ItemID.HealingPotion;
    }

	public override bool CheckActive()
	{
		for (int i = 0; 0 < Main.maxPlayers; i++)
		{
			if (Main.player[i].active && (!Main.player[i].dead || !Main.player[i].ghost))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		//Most likely will go unused and removed sometime soon
		//Main.spriteBatch.End();
		//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

		// Retrieve reference to shader //TODO compile the shader in the effects file that i stole and uncomment/debug this part
		// 
		/*var deathShader = GameShaders.Misc["TheDepths:ChasmeDeath"];
		// Reset back to default value.
        deathShader.UseOpacity(1f);
        // We use npc.ai[3] as a counter since the real death.
        if (NPC.ai[3] > 30f)
        {
            // Our shader uses the Opacity register to drive the effect. See ExampleEffectDeath.fx to see how the Opacity parameter factors into the shader math. 
            deathShader.UseOpacity(1f - (NPC.ai[3] - 30f) / 150f);
        }
		// Call Apply to apply the shader to the SpriteBatch. Only 1 shader can be active at a time.
		//deathShader.Apply();
		*/
		return true;
	}

	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

		Texture2D ChasmeSoul = ModContent.Request<Texture2D>("TheDepths/NPCs/Chasme/ChasmeSoul", AssetRequestMode.ImmediateLoad).Value;
		if (NPC.ai[2] >= 60 * 1)
		{
			ChasmeSoul = ModContent.Request<Texture2D>("TheDepths/NPCs/Chasme/ChasmeSoul_Summoning", AssetRequestMode.ImmediateLoad).Value;
		}

		Color color = new(195, 136, 251);
		Vector2 DrawPos = NPC.Center - screenPos + new Vector2(-27, -50);
		Rectangle Source = new Rectangle(0, 0, ChasmeSoul.Width, ChasmeSoul.Height);
		SpriteEffects fx = (NPC.direction == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

		SpriteEffects effects = SpriteEffects.None;
		Vector2 position1 = DrawPos - new Vector2(-27, -50) + new Vector2(0, -30);


		Texture2D texture2D2 = TextureAssets.Extra[98].Value;
		Vector2 origin2 = texture2D2.Size() / 2f;
		float num9 = (float)((double)Utils.GetLerpValue(15f, 30f, drawTimer, true) * (double)Utils.GetLerpValue(240f, 200f, drawTimer, true) * (1.0 + 0.200000002980232 * Math.Cos((double)Main.GlobalTimeWrappedHourly % 30.0 / 0.5 * 6.28318548202515 * 3.0)) * 0.800000011920929);
		Vector2 scale1 = new Vector2(0.5f, 5f) * 2 * num9;
		Vector2 scale2 = new Vector2(0.5f, 2f) * 2 * num9;


		float height = 7;
		drawTimer++;

		if (!NPC.dontTakeDamage)
		{
			if (drawTimer >= 20)
			{
				alpha = (float)(Math.Clamp(0.6375f * Math.Pow((drawTimer - 20), 2), 0, 1));
				height = (float)(-3 * Math.Cos(MathHelper.Pi * (drawTimer - 15) / 45) + 7);
				spriteBatch.Draw(ChasmeSoul, DrawPos - Vector2.UnitY * height, Source, Color.White * alpha, 0, Vector2.Zero, 1, fx, 0f);
			}
			if (drawTimer < 50)
			{
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 1.570796f, origin2, scale1, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 0.0f, origin2, scale2, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 1.570796f, origin2, scale1 * 0.6f, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 0.0f, origin2, scale2 * 0.6f, effects, 0);
			}


			fadeTimer = 45;
		}
		else
		{
			fadeTimer--;
			if (fadeTimer >= 0)
			{
				num9 = (float)((double)Utils.GetLerpValue(15f, 30f, fadeTimer, true) * (double)Utils.GetLerpValue(240f, 200f, fadeTimer, true) * (1.0 + 0.200000002980232 * Math.Cos((double)Main.GlobalTimeWrappedHourly % 30.0 / 0.5 * 6.28318548202515 * 3.0)) * 0.800000011920929);
				scale1 = new Vector2(0.5f, 5f) * 2 * num9;
				scale2 = new Vector2(0.5f, 2f) * 2 * num9;

				Color color1 = new Color(255, 255, 255) * ((float)fadeTimer / 45);
				color = new Color(195, 136, 251) * ((float)fadeTimer / 45);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 1.570796f, origin2, scale1, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 0.0f, origin2, scale2, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 1.570796f, origin2, scale1 * 0.6f, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 0.0f, origin2, scale2 * 0.6f, effects, 0);
				spriteBatch.Draw(ChasmeSoul, DrawPos - Vector2.UnitY * height, Source, color1, 0, Vector2.Zero, 1, fx, 0f);
			}
		}
	}

	public override void OnKill()
	{
		NPC.SetEventFlagCleared(ref TheDepthsWorldGen.downedChasme, -1);

		if (Main.netMode != NetmodeID.MultiplayerClient)
		{
			int CentreX = (int)(NPC.position.X + (12)) / 16;
			int CentreY = (int)(NPC.position.Y + (12)) / 16;
			int HalfLength = 3 + 1;
			for (int k = CentreX - HalfLength; k <= CentreX + HalfLength; k++)
			{
				for (int l = CentreY - HalfLength; l <= CentreY + HalfLength; l++)
				{
					if ((k == CentreX - HalfLength || k == CentreX + HalfLength || l == CentreY - HalfLength || l == CentreY + HalfLength) && !Main.tile[k, l].HasTile)
					{
						Tile tile = Main.tile[k, l];
						Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.ShadowBrick>();
						tile.HasTile = true;
					}
					Main.tile[k, l].LiquidAmount = 0;
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else
					{
						WorldGen.SquareTileFrame(k, l, true);
					}
				}
			}
		}

		if (!Main.hardMode)
		{
			WorldGen.StartHardmode();
		}
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
		//Death animation
		// Slow down
		// Crack small
		// Crack Big
		// Cracks glow white
		// Screen flash
		// Lots of stone and heart gores
		// Drop a pendant
		// Pendant falls after a while, smashing and starting hardmode/spawning the lootbox
	}

	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.ChasmeBag>()));

		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.ChasmeTrophy>(), 10));

		npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.ChasmeRelic>()));

		npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<MidnightHorseshoe>(), 4));

		LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

		notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ShadowChasmeMask>(), 7));
		notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ChasmeSoulMask>(), 7));

		notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.POWHammer>()));

		notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Weapons.ShadeBlade>(), ModContent.ItemType<Items.Weapons.QuartzCannon>(), ModContent.ItemType<Items.Weapons.ShadowClaw>(), ModContent.ItemType<StaffOfAThousandYears>()));

		notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ItemID.WarriorEmblem, ItemID.RangerEmblem, ItemID.SorcererEmblem, ItemID.SummonerEmblem));

		notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(2, ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Diamond, ItemID.Amber, ModContent.ItemType<Items.Placeable.Onyx>()));

		npcLoot.Add(notExpertRule);
	}
}
