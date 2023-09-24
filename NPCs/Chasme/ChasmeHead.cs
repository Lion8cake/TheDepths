using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Projectiles.Chasme;

namespace TheDepths.NPCs.Chasme;

public class ChasmeHead : ChasmeBodyPart
{

    private const int ActionTimerMax = 90;

    public ref float ActionTimer => ref NPC.localAI[2];

    public bool RegenPhase
    {
        get => NPC.ai[2] == 1f;
        set => NPC.ai[2] = value ? 1f : 0f;
    }
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("chasme head");
        Main.projFrames[Type] = 5;
        NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
        {
            Hide = true
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
    }
    private enum ActionState
    {
        Default,
        Enraged,
        Regen,
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

    public override bool IsLoadingEnabled(Mod mod) => true;

    protected override Vector2 BaseOffset => new(x: 36, y: -150);

    private Vector2 saphireOffset => new Vector2(NPC.width/2, -NPC.height/4);

    private Vector2 EmraldOffset => new Vector2(NPC.width / 4, NPC.height / 6);

    public override void SetDefaults()
    {
        base.SetDefaults();
        NPC.width = 272;
        NPC.height = 170;
        NPC.defense = 18;
        NPC.lifeMax = 2500;
        NPC.damage = 40;
    }
    public override bool CheckDead()
    {
        if (NPC.life <= 1 && AI_State != ActionState.Regen)
        {
            AI_State = ActionState.Regen;
            NPC.life = 1;
            NPC.netUpdate = true;
            NPC.dontTakeDamage = true;
            return false;
        }
        return false;
    }

    public override void AI()
    {
        

        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
        {
            NPC.TargetClosest(); //find target
        }
        Player player = Main.player[NPC.target];


        ++ActionTimer;

        CheckEnragedState();
        switch (AI_State)
        {
            case ActionState.Regen:
                RegenPhase = true;
                NPC.dontTakeDamage = true;
                NPC.life += (int)(NPC.lifeMax / 1200); //10 seconds to regen
                if (NPC.life >= NPC.lifeMax)
                {
                    NPC.life = NPC.lifeMax;
                    AI_State = ActionState.Default;
                    RegenPhase = false;
                }
                break;
            case ActionState.Default:
                NPC.dontTakeDamage = false;
                Attack(player, 160, 9, 1, 0.8f);
                break;
            case ActionState.Enraged:
                NPC.dontTakeDamage = false;
                Attack(player, 100, 12, 1.5f, 1f);
                break;
        }
        base.AI();
    }
    /// <summary>
    /// Shoots the player with "Ruby Rays"
    /// </summary>
    /// <param name="target">Player target</param>
    /// <param name="fireRate">delay between shots in ticks</param>
    /// <param name="damageModifier">Mulitplies damage</param>
    /// <param name="accuracyModifier">Accuracy Calculator magic value should be between 0(low accuracy) and 1(high accuracy)</param>
    public void Attack(Player target, float fireRate, float speed, float damageModifier, float accuracyModifier)
    {
        float maxDegrees = 1-accuracyModifier * 15;
        Vector2 TargetDir = NPC.DirectionTo(target.Center).RotatedByRandom(MathHelper.ToRadians(MathHelper.Lerp(-maxDegrees, maxDegrees, Main.rand.NextFloat())));

        if (ActionTimer == fireRate) //if timer is the fire rate
        {
            int a = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(saphireOffset.X * NPC.spriteDirection, saphireOffset.Y), TargetDir * speed, ModContent.ProjectileType<ChasmeRay>(), (int)((NPC.damage / 3) * damageModifier), 4, MainNPCIndex, 0, 0);
            Main.projectile[a].friendly = false;
            Main.projectile[a].hostile = true;
            ActionTimer = 0;
        }
    }
    public void CheckEnragedState()
    {
        if (AI_State == ActionState.Default && NPC.life <= NPC.lifeMax/2)
        {
            AI_State = ActionState.Enraged;
            ActionTimer = 0;
        }
    }
}