using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Tiles.Trees
{
	public class PetrifiedTreeLeaf : ModGore
	{
		public override string Texture => "TheDepths/Tiles/Trees/PetrifiedTree_Leaf";

		public override void SetStaticDefaults() {
			
			GoreID.Sets.SpecialAI[Type] = 3;
		}
	}
}
