using TheDepths.Items.Placeable;
using TheDepths.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
	public enum PlantStage : byte
	{
		Planted,
		Growing,
		Grown
	}
	public class ShadowShrub : ModTile
	{
		private const int FrameWidth = 18;

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			if (Main.LocalPlayer.HeldItem.type == ItemID.StaffofRegrowth)
			{
				Main.tileCut[Type] = false;
			}
			else
            {
				Main.tileCut[Type] = true;
			}
			TileID.Sets.SwaysInWindBasic[Type] = true;
			Main.tileNoFail[Type] = true;


			TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);

			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				ModContent.TileType<ShaleBlock>()
			};

			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				TileID.ClayPot,
				TileID.PlanterBox,
				ModContent.TileType<ShadowShrubPlanterBox>()
			};

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Shadow Shrub");
			AddMapEntry(new Color(172, 49, 238), name);

			TileObjectData.addTile(Type);
			DustType = ModContent.DustType<ShadowShrubDust>();
			HitSound = SoundID.Grass;
		}

		public override bool CanPlace(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);

			if (tile.HasTile)
			{
				int tileType = tile.TileType;
				if (tileType == Type)
				{
					PlantStage stage = GetStage(i, j);

					return stage == PlantStage.Grown;
				}
				else
				{
					if (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType] || tileType == TileID.WaterDrip || tileType == TileID.LavaDrip || tileType == TileID.HoneyDrip || tileType == TileID.SandDrip)
					{
						bool foliageGrass = tileType == TileID.Plants || tileType == TileID.Plants2;
						bool moddedFoliage = tileType >= TileID.Count && (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType]);
						bool harvestableVanillaHerb = Main.tileAlch[tileType] && WorldGen.IsHarvestableHerbWithSeed(tileType, tile.TileFrameX / 18);

						if (foliageGrass || moddedFoliage || harvestableVanillaHerb)
						{
							WorldGen.KillTile(i, j);
							if (!tile.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
							{
								NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
							}

							return true;
						}
					}

					return false;
				}
			}

			return true;
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 0)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}

		public override bool Drop(int i, int j)
		{
			PlantStage stage = GetStage(i, j);

			if (stage == PlantStage.Planted)
			{
				return false;
			}

			Vector2 worldPosition = new Vector2(i, j).ToWorldCoordinates();
			Player nearestPlayer = Main.player[Player.FindClosest(worldPosition, 16, 16)];

			int herbItemType = ModContent.ItemType<Items.ShadowShrub>();
			int herbItemStack = 1;

			int seedItemType = ModContent.ItemType<ShadowShrubSeeds>();
			int seedItemStack = 1;

			if (nearestPlayer.active && nearestPlayer.HeldItem.type == ItemID.StaffofRegrowth && stage == PlantStage.Grown)
			{
				herbItemStack = Main.rand.Next(1, 3);
				seedItemStack = Main.rand.Next(1, 6);
			}
			else if (stage == PlantStage.Grown)
			{
				herbItemStack = 1;
				seedItemStack = Main.rand.Next(1, 4);
			}
			else if (stage == PlantStage.Growing)
			{
				herbItemStack = 1;
			}

			var source = new EntitySource_TileBreak(i, j);

			if (herbItemType > 0 && herbItemStack > 0)
			{
				Item.NewItem(source, worldPosition, herbItemType, herbItemStack);
			}

			if (seedItemType > 0 && seedItemStack > 0)
			{
				Item.NewItem(source, worldPosition, seedItemType, seedItemStack);
			}

			return false;
		}

		public override bool IsTileSpelunkable(int i, int j)
		{
			PlantStage stage = GetStage(i, j);

			return stage == PlantStage.Grown;
		}

		public override void RandomUpdate(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			PlantStage stage = GetStage(i, j);

			if (stage != PlantStage.Grown && stage != PlantStage.Growing)
			{
				tile.TileFrameX += FrameWidth;

				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
			}
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = -2;

			if (Main.tile[i, j].TileFrameX != 36 && Main.tile[i, j].TileFrameX == 18 && Main.time >= 11700 && Main.time <= 20700 && !Main.dayTime)
			{
				Main.tile[i, j].TileFrameX = 36;
				offsetY = -2;
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
			}
			else if (Main.tile[i, j].TileFrameX == 36 && Main.tile[i, j].TileFrameX != 18 && Main.dayTime || Main.tile[i, j].TileFrameX == 36 && Main.tile[i, j].TileFrameX != 18 && Main.time >= 20700 && !Main.dayTime)
			{
				Main.tile[i, j].TileFrameX = 18;
				offsetY = -2;
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
			}
		}

        private static PlantStage GetStage(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			return (PlantStage)(tile.TileFrameX / FrameWidth);
		}
	}
}
