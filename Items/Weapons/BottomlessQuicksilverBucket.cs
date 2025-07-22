using Microsoft.Xna.Framework;
using System.Security.Cryptography;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Liquids;

namespace TheDepths.Items.Weapons
{
	public class BottomlessQuicksilverBucket : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 12;
			Item.useTime = 5;
			Item.width = 20;
			Item.height = 20;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 10);
			Item.tileBoost += 2;
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
					if (tile.LiquidType != ModLiquidLib.ModLiquidLib.LiquidType<Quicksilver>())
					{
						return;
					}
				}
				SoundEngine.PlaySound(SoundID.SplashWeak, player.position);
				tile.LiquidType = ModLiquidLib.ModLiquidLib.LiquidType<Quicksilver>();
				tile.LiquidAmount = byte.MaxValue;
				WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);
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

