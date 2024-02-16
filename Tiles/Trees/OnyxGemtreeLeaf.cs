using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Tiles.Trees
{
	public class OnyxGemtreeLeaf : ModGore
	{
		public override void SetStaticDefaults() {

			ChildSafety.SafeGore[Type] = true;
			GoreID.Sets.SpecialAI[Type] = 3;
			GoreID.Sets.PaintedFallingLeaf[Type] = true;
		}
	}
}
