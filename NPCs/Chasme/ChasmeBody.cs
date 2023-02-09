using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.NPCs.Chasme;

public class ChasmeBody : ChasmeBodyPart
{
	public override bool IsLoadingEnabled(Mod mod) => true;

	protected override Vector2 BaseOffset => new(x: -98, y: 0);

	public override void SetDefaults()
	{
		base.SetDefaults();
		NPC.width = 364;
		NPC.height = 208;
		NPC.defense = 18;
		NPC.lifeMax = 9999; // TO DO: CHANGE THIS
		NPC.damage = 40;
	}
}
