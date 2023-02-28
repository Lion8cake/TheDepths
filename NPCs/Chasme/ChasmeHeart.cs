using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.NPCs.Chasme;

[AutoloadBossHead] // For loading "ChasmeHeart_Head_Boss" automatically
public class ChasmeHeart : ModNPC
{
	private enum ActionState
	{
		Idle,
		Chase
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
		DisplayName.SetDefault("Chasme");

		NPCID.Sets.BossBestiaryPriority.Add(Type);

		NPCDebuffImmunityData debuffData = new()
		{
			SpecificallyImmuneTo = new int[]
			{
				BuffID.Poisoned,
				BuffID.Confused,
				BuffID.Burning
			}
		};
		NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

		NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
		{
			CustomTexturePath = "TheDepths/NPCs/Chasme_Preview",
			PortraitScale = 0.5f,
			PortraitPositionYOverride = 0f,
		};
		NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
	}

	public override void SetDefaults()
	{
		NPC.npcSlots = 10f;
		NPC.width = 32;
		NPC.height = 24;
		NPC.aiStyle = -1;
		NPC.defense = 30;
		NPC.lifeMax = 5500;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.knockBackResist = 0f;
		NPC.boss = true;
	}

	public bool BodyPartsSpawned
	{
		get => NPC.ai[0] == 1f;
		set => NPC.ai[0] = value ? 1f : -1f;
	}

	public override void AI()
	{
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
				ChaseAI();
				break;
			}
			default:
			{
				break;
			}
		}
	}

	private ActionState DetermineState(ActionState previousState)
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
			default:
			{
				return ActionState.Chase;
			}
		}
	}

	private void ChaseAI()
	{
		float speed = 3f;
		float inertia = 20f;

		Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
		direction.Normalize();
		direction *= speed;

		NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
	}

	private void SpawnBodyParts()
	{
		if (Main.netMode == NetmodeID.MultiplayerClient)
			return;

		var entitySource = NPC.GetSource_FromAI();
		Point spawnPos = NPC.Center.ToPoint();

		NPC.NewNPC
		(
			entitySource,
			spawnPos.X,
			spawnPos.Y,
			ModContent.NPCType<ChasmeHead>(),
			Start: NPC.whoAmI,
			ai0: NPC.whoAmI // Give the body part a reference to the main NPC (this one)
		);

		NPC.NewNPC
		(
			entitySource,
			spawnPos.X,
			spawnPos.Y,
			ModContent.NPCType<ChasmeBody>(),
			Start: NPC.whoAmI,
			ai0: NPC.whoAmI
		);

		NPC.NewNPC
		(
			entitySource,
			spawnPos.X,
			spawnPos.Y,
			ModContent.NPCType<ChasmeHandRight>(),
			Start: NPC.whoAmI,
			ai0: NPC.whoAmI
		);

		NPC.NewNPC
		(
			entitySource,
			spawnPos.X,
			spawnPos.Y,
			ModContent.NPCType<ChasmeHandLeft>(),
			Start: NPC.whoAmI,
			ai0: NPC.whoAmI
		);

		BodyPartsSpawned = true;
	}
}
