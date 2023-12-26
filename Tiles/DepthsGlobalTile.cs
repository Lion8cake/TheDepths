using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheDepths.Tiles
{
    public class DepthsGlobalTile : GlobalTile
    {
		public override void SetStaticDefaults()
		{
			TileObjectData tileObjectData = TileObjectData.GetTileData(TileID.Sunflower, 0);
			tileObjectData.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<NightmareGrass>()).ToArray();
		}

		public override void Unload()
		{
			TileObjectData tileObjectData = TileObjectData.GetTileData(TileID.Sunflower, 0);
			tileObjectData.AnchorValidTiles = tileObjectData.AnchorValidTiles.Except(new int[] { ModContent.TileType<NightmareGrass>() }).ToArray();
		}
	}
}
