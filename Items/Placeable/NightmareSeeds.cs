using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
    public class NightmareSeeds : ModItem
	{
		public override void SetDefaults()
		{
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.width = 16;
			Item.height = 16;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.maxStack = 9999;
			Item.value = 150;
		}

        public override bool? UseItem(Player player)
		{
			Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
			if (tile.HasTile && tile.TileType == ModContent.TileType<Tiles.ShaleBlock>() && player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, Terraria.DataStructures.TileReachCheckSettings.Simple))
			{
				Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)ModContent.TileType<Tiles.NightmareGrass>();
				WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
				return true;
			}
			return false;
		}
    }
}