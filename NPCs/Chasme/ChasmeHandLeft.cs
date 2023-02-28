using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.NPCs.Chasme;

public class ChasmeHandLeft : ChasmeHand
{
	public override bool IsLoadingEnabled(Mod mod) => true;

	protected override Vector2 BaseOffset => new(x: 114, y: 95);

	public override void SetDefaults()
	{
		base.SetDefaults();
		NPC.width = 160;
		NPC.height = 166;
	}
}

public class ChasmeHandLeftExpert : ChasmeHandLeft
{
	public override bool IsLoadingEnabled(Mod mod) => false;

	protected override Vector2 BaseOffset => new(x: 114, y: 95);
}