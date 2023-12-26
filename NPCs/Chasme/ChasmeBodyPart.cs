using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.NPCs.Chasme;

/// <summary>
/// IMPORTANT!!! Remember to override <see cref="IsLoadingEnabled"/> so it returns
/// <see langword="true" />. All of the <see cref="NPC.ai"/> array is usable
/// except for the 0th index.
/// </summary>
public abstract class ChasmeBodyPart : ModNPC
{
	/// <summary>
	/// Gets the index of the main NPC this body part should follow, stored on the 0th index of
	/// the ai array
	/// </summary>
	protected int MainNPCIndex
	{
		get => (int)NPC.ai[0];
	}

	protected NPC MainNPC
	{
		get => Main.npc[MainNPCIndex];
	}
    public override void SetStaticDefaults()
    {
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = true;
	}

    /// <summary>
    /// Gets the main position (Center) of the boss this body part should follow
    /// </summary>
    protected Vector2 ChasmePosition
	{
		get
		{
			var mainCenter = Main.npc[MainNPCIndex].Center;
			return new(mainCenter.X, mainCenter.Y);
		}
	}

	/// <summary>
	/// The offset this body part should autoposition itself at, relative to
	/// <see cref="ChasmePosition"/>.
	/// </summary>
	protected virtual Vector2 BaseOffset => new(0, 0);

	/// <summary>
	/// Override to change the value of <see cref="BaseOffset"/> at any given moment, returns
	/// <see cref="BaseOffset"/> by default.
	/// </summary>
	protected virtual Vector2 GetOffset() => BaseOffset;

	public override bool IsLoadingEnabled(Mod mod) => false;

	public bool ShouldFaceTarget = true;

	public override void SetDefaults()
	{
		NPC.HitSound = SoundID.Item70;
		NPC.DeathSound = SoundID.NPCDeath14;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.knockBackResist = 0f;
		NPC.aiStyle = -1;
        //NPC.ScaleStats_UseStrengthMultiplier(0.6f); //dont scale like a regular npc in different gamemodes
    }

	public override void AI()
	{
		if (ShouldFaceTarget) //disable during dashes
		{
			NPC.spriteDirection = NPC.direction = MainNPC.direction;
		}
		Vector2 offset = GetOffset();
		PreAutoPosition();
		if (ShouldAutoPosition)
			AutoPosition(offset);
		PostAutoPosition(offset);


		if (!MainNPC.active)
		{
			NPC.active = false;
		}
		Main.BestiaryTracker.Kills.SetKillCountDirectly(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHandLeft>()], Main.BestiaryTracker.Kills.GetKillCount(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHeart>()]));
		Main.BestiaryTracker.Kills.SetKillCountDirectly(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeBody>()], Main.BestiaryTracker.Kills.GetKillCount(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChasmeHeart>()]));
	}

	/// <summary>
	/// <see langword="true" /> if this NPC should be positioned automatically
	/// in the current tick, <see langword="false" /> otherwise
	/// </summary>
	protected virtual bool ShouldAutoPosition => true;

	protected void AutoPosition(Vector2 offset)
	{
		NPC.Center = ChasmePosition + new Vector2(offset.X * MainNPC.direction, offset.Y);
	}

	protected virtual void PreAutoPosition() { }

	protected virtual void PostAutoPosition(Vector2 offset) { }

    public override bool CheckDead()
    {
        return false;
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

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

        // Retrieve reference to shader
        /*var deathShader = GameShaders.Misc["TheDepths:ChasmeDeath"];  //TODO compile the shader in the effects file that i stole and uncomment/debug this part

        // Reset back to default value.
        deathShader.UseOpacity(1f);
        // We use npc.ai[3] as a counter since the real death.
        if (MainNPC.ai[3] > 30f)
        {
            // Our shader uses the Opacity register to drive the effect. See ExampleEffectDeath.fx to see how the Opacity parameter factors into the shader math. 
            deathShader.UseOpacity(1f - (MainNPC.ai[3] - 30f) / 150f);
        }
        // Call Apply to apply the shader to the SpriteBatch. Only 1 shader can be active at a time.
        deathShader.Apply(null);
		*/
        return true;
    }
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
    }
}
