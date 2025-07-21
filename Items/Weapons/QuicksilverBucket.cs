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
	public class QuicksilverBucket : ModItem
	{
		public override void SetStaticDefaults()
		{
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
			Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
			if (player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, TileReachCheckSettings.Simple))
			{
				if (Worldgen.TheDepthsWorldGen.InDepths(player))
				{
					if (player.itemTime == 0 && player.itemAnimation > 0 && player.controlUseItem)
					{
						if (tile.HasUnactuatedTile)
						{
							bool[] tileSolid = Main.tileSolid;
							if (tileSolid[tile.TileType])
							{
								bool[] tileSolidTop = Main.tileSolidTop;
								if (!tileSolidTop[tile.TileType])
								{
									if (tile.TileType != 546)
									{
										return;
									}
								}
							}
						}
						if (tile.LiquidAmount != 0)
						{
							return;
						}
						SoundEngine.PlaySound(SoundID.SplashWeak, player.position);
						tile.LiquidType = ModLiquidLib.ModLiquidLib.LiquidType<Quicksilver>();
						tile.LiquidAmount = byte.MaxValue;
						Item.stack--;
						player.PutItemInInventoryFromItemUsage(ItemID.EmptyBucket, player.selectedItem);
						WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);
						player.ApplyItemTime(Item);
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
						}
					}
					player.cursorItemIconEnabled = true;
					player.cursorItemIconID = Type;
				}
				else
				{
					if (player.itemTime == 0 && player.itemAnimation == 1 && player.controlUseItem)
					{
						for (int i = 0; i < 12; i++)
						{
							Dust.NewDustDirect(Main.MouseWorld + new Vector2(-4f, -4f), 4, 4, DustID.Smoke, 0f, -1f);
						}
					}
				}
			}
		}
	}
}

