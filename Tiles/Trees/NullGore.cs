using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Tiles.Trees
{
	public class NullGore : ModGore
	{
		public override string Texture => "TheDepths/Projectiles/CrystalBall";

		public override void SetStaticDefaults() {
		}

		public override bool Update(Gore gore)
		{
			Gore.goreTime = 0;
			return false;
		}
	}
}
