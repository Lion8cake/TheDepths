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

namespace TheDepths.NPCs.Chasme;

[AutoloadBossHead] // For loading "ChasmeHeart_Head_Boss" automatically
public class ChasmeHeart : ModNPC
{
	//TODO add drops also
    /// <summary>
    /// list of body part NPC indexes in this order: Head, Body, Right Hand, Left Hand, Right Hand Expert, Left Hand Expert
    /// </summary>
    int[] bodyParts = new int[6];

    public ref float ActionTimer => ref NPC.localAI[2];

    public float drawTimer;

    int startLife;
	int lastActionState;
	bool open = false;
    Vector2 point = Vector2.Zero;
	float ShootRot;
    float speed = 3f;
    float inertia = 20f;
	bool second;

    private enum ActionState
	{
		Idle,
		Chase,
		Dead
	}

	private uint AI_State_uint
	{
		get => BitConverter.SingleToUInt32Bits(NPC.ai[1]);
		set => NPC.ai[1] = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
	}

	private ActionState AI_State
	{
		get => (ActionState)AI_State_uint;
		set => AI_State_uint = (uint)value;
	}

	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Chasme");

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
		NPC.boss = true;
		NPC.value = 80000;
		NPC.ScaleStats_UseStrengthMultiplier(0.6f); //dont scale like a regular npc in different gamemodes
		if (!Main.dedServ)
		{
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Chasme");
		}
		SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
	}

	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.ChasmeSoul")
			});
	}

	public bool BodyPartsSpawned
	{
		get => NPC.ai[0] == 1f;
		set => NPC.ai[0] = value ? 1f : -1f;
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
		}
		return false;
	}

	public override void AI()
	{
		if (NPC.ai[3] > 0f)
		{
			NPC.dontTakeDamage = true;
			NPC.ai[3] += 1f; // increase our death timer.
		}
		if (NPC.ai[3] >= 180f)
		{
			NPC.life = 0;
			NPC.HitEffect(0, 0);
			NPC.checkDead(); // This will trigger ModNPC.CheckDead the second time, causing the real death.
		}

		if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
		{
			NPC.TargetClosest();
		}

		Player player0 = Main.LocalPlayer;
		float chaspos = NPC.position.X + 40f;
		if (player0.position.Y > (float)((Main.maxTilesY - 250) * 16) && player0.position.X > chaspos - 1920f && player0.position.X < chaspos + 1920f)
		{
			player0.AddBuff(37, 10);
		}

		Player player = Main.player[NPC.target];

		if (player.dead)
		{
			//flee downwards
			NPC.velocity.Y += 0.05f;
			//despawn in 10 ticks outside of player's screen
			NPC.EncourageDespawn(10);
			return;
		}

		if (NPC.life <= NPC.lifeMax / 4 && !second)
		{
			speed *= 1.5f;
			foreach (int i in bodyParts)
			{
				NPC npc = Main.npc[i];
				npc.damage = (int)(npc.damage * 1.5f);

			}
			second = true;
		}
		if (!BodyPartsSpawned)
			SpawnBodyParts();
		NPC.TargetClosestUpgraded();

		switch (DetermineState(AI_State))
		{
			case ActionState.Idle:
				{
					break;
				}
			case ActionState.Chase:
				{
					ChaseAI(player);
					break;
				}
		}





		if ((int)Main.npc[bodyParts[0]].ai[2] == 1 && lastActionState != 1 && !open)
		{

			open = true;
			drawTimer = 0;
			startLife = NPC.life;
			ActionTimer = 1200;
			ShootRot = 0;

		}
		lastActionState = (int)Main.npc[bodyParts[0]].ai[2];

		if (open)
		{
			Vector2 pos = NPC.Center - new Vector2(0, 10);
			Vector2 aim = Vector2.One.RotatedBy(ShootRot);
			pos += aim * 26;
			if (ActionTimer % 3 == 0)
			{
				int a = Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, aim * 30, ProjectileID.Shadowflames, NPC.damage, 3, NPC.whoAmI, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)); //TODO maybe replace this with chlaser that might look cool
				Main.projectile[a].friendly = false;
				Main.projectile[a].hostile = true;

			}
			--ActionTimer;
			NPC.dontTakeDamage = false;
			ShootRot += MathHelper.PiOver4 / 24;


			//shadowflame projectiles



			if (ActionTimer <= 0 || startLife - NPC.life >= 1250)
			{
				open = false;
				NPC.netUpdate = true;
			}
		}
		else
		{
			NPC.dontTakeDamage = true;
		}

	}

	private ActionState DetermineState(ActionState previousState) //i abandond this system
	{
		switch (AI_State)
		{
			case ActionState.Idle:
				{
					return ActionState.Chase;
				}
			case ActionState.Chase:
				{
					return ActionState.Chase;
				}
			case ActionState.Dead:
				{
					return ActionState.Dead;
				}
			default:
				{
					return ActionState.Chase;
				}
		}
	}

	private void ChaseAI(Player player)
	{


		Vector2 direction = NPC.DirectionTo(player.Center + new Vector2(1600 * NPC.Center.X > player.Center.X ? 1 : -1, 0));

		direction *= speed;

		NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
	}


	private void SpawnBodyParts()
	{
		if (Main.netMode == NetmodeID.MultiplayerClient)
			return;

		var entitySource = NPC.GetSource_FromAI();
		Point spawnPos = NPC.Center.ToPoint();

		int a = NPC.NewNPC
		(
			entitySource,
			spawnPos.X,
			spawnPos.Y,
			ModContent.NPCType<ChasmeHead>(),
			Start: NPC.whoAmI,
			ai0: NPC.whoAmI // Give the body part a reference to the main NPC (this one)
		);

		bodyParts[0] = a;

		a = NPC.NewNPC
		(
			entitySource,
			spawnPos.X,
			spawnPos.Y,
			ModContent.NPCType<ChasmeBody>(),
			Start: NPC.whoAmI,
			ai0: NPC.whoAmI
		);

		bodyParts[1] = a;

		a = NPC.NewNPC
		(
			entitySource,
			spawnPos.X,
			spawnPos.Y,
			ModContent.NPCType<ChasmeHandRight>(),
			Start: NPC.whoAmI,
			ai0: NPC.whoAmI
		);

		bodyParts[2] = a;

		a = NPC.NewNPC
		(
			entitySource,
			spawnPos.X,
			spawnPos.Y,
			ModContent.NPCType<ChasmeHandLeft>(),
			Start: NPC.whoAmI,
			ai0: NPC.whoAmI
		);
		bodyParts[3] = a;

		if (Main.expertMode)
		{
			a = NPC.NewNPC
		(
			entitySource,
			spawnPos.X,
			spawnPos.Y,
			ModContent.NPCType<ChasmeHandRightExpert>(),
			Start: NPC.whoAmI,
			ai0: NPC.whoAmI
		);

			bodyParts[4] = a;

			a = NPC.NewNPC
			(
				entitySource,
				spawnPos.X,
				spawnPos.Y,
				ModContent.NPCType<ChasmeHandLeftExpert>(),
				Start: NPC.whoAmI,
				ai0: NPC.whoAmI
			);
			bodyParts[5] = a;
		}




		BodyPartsSpawned = true;
	}
	float alpha = 0;
	int fadeTimer = 0;
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

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

		Texture2D ChasmeSoul = ModContent.Request<Texture2D>("TheDepths/NPCs/Chasme/ChasmeSoul").Value;

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

		if (open)
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
		base.PostDraw(spriteBatch, screenPos, drawColor);
	}
	public override bool CheckDead()
	{
		if (NPC.ai[3] == 0f)
		{
			NPC.ai[3] = 1f;
			NPC.damage = 0;
			NPC.life = 1;
			NPC.dontTakeDamage = true;
			NPC.netUpdate = true;
			return false;
		}
		return true;
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

	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.ChasmeBag>()));

		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.ChasmeTrophy>(), 10));

		npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.ChasmeRelic>()));

		//npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<ShadowHorse>(), 4));

		LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

		notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ShadowChasmeMask>(), 7));
		notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ChasmeSoulMask>(), 7));

		notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.POWHammer>()));

		notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Weapons.ShadeBlade>(), ModContent.ItemType<Items.Weapons.QuartzCannon>(), ModContent.ItemType<Items.Weapons.ShadowClaw>()));

		notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ItemID.WarriorEmblem, ItemID.RangerEmblem, ItemID.SorcererEmblem, ItemID.SummonerEmblem));

		notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(2, ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Diamond, ItemID.Amber, ModContent.ItemType<Items.Placeable.Onyx>()));

		npcLoot.Add(notExpertRule);
	}
}
