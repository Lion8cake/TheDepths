using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.NPCs.Chasme;

public class ChasmeHand : ChasmeBodyPart
{
	private enum ActionState
	{
		Float,
		Follow,
		Lunge
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

	private ref float SharedTimer => ref NPC.ai[2];

	private ref float ActionStateTimer => ref NPC.ai[3];

	static int LungeFrequency => 900;

	static float FloatingSpeed => 0.2f;

	static float FloatingAmplitude => 3f;

	public override void SetDefaults()
	{
		NPC.defense = 14;
		NPC.lifeMax = 350;
		NPC.damage = 35;
	}

	protected override bool ShouldAutoPosition => false;

	protected override void PostAutoPosition(Vector2 offset)
	{
		switch (DetermineState(AI_State))
		{
			case ActionState.Float:
				FloatAI(offset);
				break;
			case ActionState.Follow:
				FollowAI(offset);
				break;
			case ActionState.Lunge:
				LungeAI();
				break;
			default:
				break;
		}
	}

	private ActionState DetermineState(ActionState previousState)
	{
		return ActionState.Float;
	}

	private void FloatAI(Vector2 offset)
	{
		SharedTimer += FloatingSpeed / FloatingAmplitude;

		offset.Y += FloatingAmplitude * MathF.Sin(SharedTimer);

		AutoPosition(offset);
	}

	private void FollowAI(Vector2 offset)
	{
		// throw new NotImplementedException();
	}

	private void LungeAI()
	{
		// throw new NotImplementedException();
	}
}