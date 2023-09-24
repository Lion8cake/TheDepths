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
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using static Terraria.ModLoader.PlayerDrawLayer;
using TheDepths.Projectiles.Chasme;

namespace TheDepths.NPCs.Chasme;

public class ChasmeHand : ChasmeBodyPart
{
	private enum ActionState
	{
		Float,
		Chase,
		Lunge,
		Return
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

	private float shootTimer = 0;

	static int LungeFrequency => 900;

	static float FloatingSpeed => 0.2f;

	static float FloatingAmplitude => 3f;

	private Vector2 DirectionToTarget = Vector2.Zero;

	protected int shootOffset = 0; //so they shoot at different times

	private float magnitude = 1;

	private bool regen;

    public override void SetStaticDefaults()
    {
		NPCID.Sets.TrailCacheLength[Type] = 10;
        NPCID.Sets.TrailingMode[Type] = 1;
    }
    public override void SetDefaults()
	{
		NPC.defense = 14;
		NPC.lifeMax = 350;
		NPC.damage = 35;
		NPC.noTileCollide = true;
		shootTimer = shootOffset;
	}

	protected override bool ShouldAutoPosition => false;

	private bool WithinAngle(float number, float min, float max) //something like this probably exists already
	{
		if (number > min && number < max)
		{
			return true;
		}
		return false;
	}

    protected override void PostAutoPosition(Vector2 offset)
	{
		if (!WithinAngle(NPC.rotation, MathHelper.ToDegrees(-5), MathHelper.ToDegrees(5)))
        {
			NPC.rotation = NPC.rotation.AngleTowards(0, MathHelper.ToRadians(2.5f));
		} else
		{
			NPC.rotation = 0;
		}
		/*for (int i = 0; i < Main.maxNPCs; i++)
		{
			if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<ChasmeHead>())
			{
				if (Main.npc[i].life >= Main.npc[i].lifeMax)
				{
					NPC.life = NPC.lifeMax;
				}
			}
		}*/

		//int halfDamage = (int)(NPC.damage*0.5f);
		if (regen)
		{
			//NPC.damage /= 2;
			NPC.dontTakeDamage = true;
			NPC.life += (int)(NPC.lifeMax / 1200); //10 seconds to regen
			if (NPC.life >= NPC.lifeMax)
			{
				NPC.dontTakeDamage = false;
				NPC.life = NPC.lifeMax;
				regen = false;
			}
		}

        ++ActionStateTimer;

		Player player = Main.player[Main.npc[(int)NPC.ai[0]].target]; //gets the target of the heart
		if (NPC.life > 1)
		{
			switch (DetermineState(AI_State, player))
			{
				case ActionState.Float:
					FloatAI(offset);
					break;
				case ActionState.Chase:
					ChaseAI(offset);
					break;
				case ActionState.Lunge:
					LungeAI(player);
					break;
				case ActionState.Return:
					ReturnAI(BaseOffset);
					break;
				default:
					break;
			}
		}
		else
        {
			switch (DetermineState(AI_State, player))
			{
				case ActionState.Float:
					FloatAI(offset);
					break;
				case ActionState.Chase:
					FloatAI(offset);
					break;
				case ActionState.Lunge:
					FloatAI(offset);
					break;
				case ActionState.Return:
					FloatAI(offset);
					break;
				default:
					break;
			}
		}
	}

	private ActionState DetermineState(ActionState previousState, Player player)
	{
		if (AI_State != ActionState.Return && AI_State != ActionState.Lunge)
		{
			ShouldFaceTarget = true;
		}
		if (previousState != ActionState.Lunge && ActionStateTimer == LungeFrequency/2)
		{
			AI_State = ActionState.Lunge;
			ActionStateTimer = 0;
            DirectionToTarget = NPC.DirectionTo(player.Center);
            return ActionState.Lunge;
		}
		if (previousState == ActionState.Lunge)
		{
			if (ActionStateTimer >= 60)
			{
				foreach (int i in voids)
				{
					Main.projectile[i].ai[0] = 1;
				}
				voids.Clear();
				DirectionToTarget = Vector2.Zero;
				AI_State = ActionState.Return;
				return ActionState.Return;
			}
			return ActionState.Lunge;
		}
		if (previousState == ActionState.Return)
		{
			return previousState;

        }
		return previousState;
	}

	private void FloatAI(Vector2 offset)
	{
		SharedTimer += FloatingSpeed / FloatingAmplitude;

		offset.Y += FloatingAmplitude * MathF.Sin(SharedTimer);


		shootTimer++;
		Attack(Main.player[MainNPC.target], 200, 12, 1, 0.9f);

		AutoPosition(offset);
	}
    public override bool CheckDead()
    {
        if (NPC.life <= 1)
        {
            NPC.life = 1;
			regen = true;
            NPC.netUpdate = true;
            NPC.dontTakeDamage = true;
            return false;
        }
        return false;
    }

    private void ChaseAI(Vector2 offset)
	{
		NPC.velocity = NPC.velocity.RotatedBy(NPC.AngleTo(Main.player[MainNPC.target].Center) / 20);
	}
	List<int> voids = new List<int>();
	private void LungeAI(Player player)
    {
		NPC.velocity = DirectionToTarget * 18;
		if (DirectionToTarget.X * 14 > NPC.Center.X)
		{
			NPC.spriteDirection = 1;
		}
		if (ActionStateTimer % 24 == 0)
		{
			voids.Add(Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<ChasVoid>(), NPC.damage / 3, 8, player.whoAmI, 0, 1));
		}
    }
	private void ReturnAI(Vector2 offset)
	{
		NPC.velocity = NPC.DirectionTo(ChasmePosition + new Vector2(offset.X * MainNPC.direction, offset.Y)) * magnitude;

		magnitude = Math.Clamp(magnitude * 1.03f, 0, 10);

		if (NPC.Center.WithinRange(ChasmePosition + new Vector2(offset.X * MainNPC.direction, offset.Y), 16))
		{
			AI_State = ActionState.Float;
			magnitude = 1;
		}
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
		float offsetX;
		if (AI_State == ActionState.Lunge)
		{
			if (NPC.spriteDirection == 1)
			{
				offsetX = NPC.width / 2;
            } else
			{
				offsetX = NPC.width / 2 * -NPC.spriteDirection;
            }
			Main.instance.LoadNPC(Type);
            Texture2D HandTexture = TextureAssets.Npc[Type].Value;
			Rectangle Source = new Rectangle(0, 0, HandTexture.Width, HandTexture.Height);
			SpriteEffects spriteEffects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new Vector2(HandTexture.Width * 0.5f, NPC.height * 0.5f);
            for (int i = 0; i < NPC.oldPos.Length; i++)
			{
                Vector2 drawPos = NPC.oldPos[i] + new Vector2(offsetX, NPC.height/2) - Main.screenPosition;
                Color color = NPC.GetAlpha(drawColor) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                spriteBatch.Draw(HandTexture, drawPos, null, color, NPC.rotation, drawOrigin, NPC.scale - i / (float)NPC.oldPos.Length / 3, spriteEffects, 0f);
			}
		}
        return true;
    }
    public void Attack(Player target, float fireRate, float speed, float damageModifier, float maxDegrees)
    {
        Vector2 TargetDir = NPC.DirectionTo(target.Center).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-maxDegrees, maxDegrees)));
        if (shootTimer == fireRate + shootOffset) //if timer is the fire rate with a 20% variation
        {
            int a = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Vector2.UnitX*32*NPC.spriteDirection, TargetDir * speed, ModContent.ProjectileType<ChasmeRay>(), (int)((NPC.damage / 3) * damageModifier), 4, 255, 1, 1);
            Main.projectile[a].friendly = false;
            Main.projectile[a].hostile = true;
			Main.projectile[a].tileCollide = false;
            shootTimer = shootOffset;
        }
    }
}