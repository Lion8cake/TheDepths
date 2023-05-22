using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.NPCs.Chasme;

public class ChasmeHandRight : ChasmeHand
{
	public override bool IsLoadingEnabled(Mod mod) => true;

	protected override Vector2 BaseOffset => new(x: 136, y: -89);

	public override void SetDefaults()
	{
		base.SetDefaults();
		NPC.width = 176;
		NPC.height = 130;
    }

	public override void SetStaticDefaults()
	{
		NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
		{
			Hide = true
		};
		NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
	}
}

public class ChasmeHandRightExpert : ChasmeHandRight
{
	public override bool IsLoadingEnabled(Mod mod) => false;

	protected override Vector2 BaseOffset => new(x: 114, y: 95);

	public override void SetStaticDefaults()
	{
		NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
		{
			Hide = true
		};
		NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
	}
}