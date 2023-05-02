using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Items
{
	public class ShalestoneConch : ModItem
	{
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.DemonConch);
		}
	}
}
