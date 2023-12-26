using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
    public class MercuryMoss : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.RainbowMoss;
			ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
		}

		public override void SetDefaults()
		{
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.width = 24;
			Item.height = 20;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Blue;
		}

		public override bool? UseItem(Player player)
		{
			Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);

			if (tile.HasTile && player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, Terraria.DataStructures.TileReachCheckSettings.Simple))
			{
				if (tile.TileType == TileID.Stone)
				{
					Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)ModContent.TileType<Tiles.MercuryMoss>();
					WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
					return true;
				}
				else if (tile.TileType == TileID.GrayBrick)
				{
					Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)ModContent.TileType<Tiles.MercuryMossStoneBricks>();
					WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
					return true;
				}
				else if (tile.TileType == TileID.Extractinator || tile.TileType == TileID.ChlorophyteExtractinator)
				{
					return true;
				}
			}
			return false;
		}

		public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
		{
			int ItemType = ItemID.BlueMoss;
			switch (Main.rand.Next(5))
			{
				case 0:
					ItemType = ItemID.BlueMoss;
					break;
				case 1:
					ItemType = ItemID.BrownMoss;
					break;
				case 2:
					ItemType = ItemID.GreenMoss;
					break;
				case 3:
					ItemType = ItemID.PurpleMoss;
					break;
				case 4:
					ItemType = ItemID.RedMoss;
					break;
			}
			resultType = ItemType;
			resultStack = 1;
		}
	}
}