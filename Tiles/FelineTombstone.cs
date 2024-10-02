using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheDepths.Tiles
{
    public class FelineTombstone : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			Main.tileSign[Type] = true;
			TileID.Sets.TileInteractRead[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.AvoidedByNPCs[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);

			DustType = DustID.Stone;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(192, 192, 192), name);
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Sign.KillSign(i, j);
		}
	}
}