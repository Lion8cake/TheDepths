using ModLiquidLib.ID;
using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Liquids;

namespace TheDepths.Items.Weapons
{
	public class QuicksilverBucket : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.IsLavaImmuneRegardlessOfRarity[Type] = true;
			ItemID.Sets.AlsoABuildingItem[Type] = true;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.WaterBucket;
			ItemID.Sets.ShimmerTransformToItem[ItemID.HoneyBucket] = Type;
			ItemID.Sets.DuplicationMenuToolsFilter[Type] = true;
			LiquidID_TLmod.Sets.CreateLiquidBucketItem[LiquidLoader.LiquidType<Quicksilver>()] = Type;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void HoldItem(Player player)
		{
			if (!player.JustDroppedAnItem)
			{
				if (player.whoAmI != Main.myPlayer)
				{
					return;
				}
				if (player.noBuilding || !(player.position.X / 16f - (float)Player.tileRangeX - (float)Item.tileBoost <= (float)Player.tileTargetX) || !((player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)Item.tileBoost - 1f >= (float)Player.tileTargetX) || !(player.position.Y / 16f - (float)Player.tileRangeY - (float)Item.tileBoost <= (float)Player.tileTargetY) || !((player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)Item.tileBoost - 2f >= (float)Player.tileTargetY))
				{
					return;
				}
				if (!Main.GamepadDisableCursorItemIcon)
				{
					player.cursorItemIconEnabled = true;
					Main.ItemIconCacheUpdate(Item.type);
				}
				if (!player.ItemTimeIsZero || player.itemAnimation <= 0 || !player.controlUseItem)
				{
					return;
				}
				Tile tile;

				tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
				if (tile.LiquidAmount >= 200)
				{
					return;
				}
				tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
				if (tile.HasUnactuatedTile)
				{
					bool[] tileSolid = Main.tileSolid;
					tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
					if (tileSolid[tile.TileType])
					{
						bool[] tileSolidTop = Main.tileSolidTop;
						tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
						if (!tileSolidTop[tile.TileType])
						{
							tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
							if (tile.TileType != 546)
							{
								return;
							}
						}
					}
				}

				tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
				if (tile.LiquidAmount != 0)
				{
					tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
					if (tile.LiquidType != LiquidLoader.LiquidType<Quicksilver>())
					{
						return;
					}
				}
				SoundEngine.PlaySound(SoundID.SplashWeak, player.position);
				tile.LiquidType = LiquidLoader.LiquidType<Quicksilver>();
				tile.LiquidAmount = byte.MaxValue;
				WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);
				Item.stack--;
				player.PutItemInInventoryFromItemUsage(ItemID.EmptyBucket, player.selectedItem);
				player.ApplyItemTime(Item);
				if (Main.netMode == 1)
				{
					NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
				}
				return;
			}
		}
	}
}

