using Microsoft.Xna.Framework;
using Terraria;
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

	public override void SetDefaults()
	{
		NPC.HitSound = SoundID.NPCHit8;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.knockBackResist = 0f;
		NPC.aiStyle = -1;
	}

	public override void AI()
	{
		NPC.spriteDirection = NPC.direction = MainNPC.direction;
		Vector2 offset = GetOffset();
		PreAutoPosition();
		if (ShouldAutoPosition)
			AutoPosition(offset);
		PostAutoPosition(offset);
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
}
