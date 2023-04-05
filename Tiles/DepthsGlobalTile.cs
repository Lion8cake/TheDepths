using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace TheDepths.Tiles
{
    internal class DepthsGlobalTile : GlobalTile
    {
        public override void SetStaticDefaults()
        {
            if (TheDepthsWorldGen.depthsorHell)
            {
                //remove lava kill tile here
            }
        }
    }
}
