using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.NPCs.Chasme;

public class ChasmeHead : ChasmeBodyPart
{
	public override bool IsLoadingEnabled(Mod mod) => true;

	protected override Vector2 BaseOffset => new(x: 36, y: -150);

	public override void SetDefaults()
	{
		base.SetDefaults();
		NPC.width = 272;
		NPC.height = 170;
		NPC.defense = 18;
		NPC.lifeMax = 2500;
		NPC.damage = 40;
	}
}