using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Items.Placeable;
using Terraria.GameContent.Creative;
using System.Collections.Generic;
using TheDepths.Buffs;
using Terraria.DataStructures;
using Terraria.Localization;
using System;
using Terraria.GameContent.ItemDropRules;
using System.Linq;
using Terraria.GameContent;
using Terraria.Audio;
using TheDepths.Items.Weapons;
using TheDepths.Tiles.Trees;
using Terraria.ObjectData;

namespace TheDepths.Items
{
    public class GlobalItems : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public bool LavaProof;

        public override void SetDefaults(Item item)
        {
            if (ItemID.Sets.IsLavaImmuneRegardlessOfRarity[item.type] == true)
            {
                LavaProof = true;
            }
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) && Collision.LavaCollision(item.position, item.width, item.height))
            {
                ItemID.Sets.IsLavaImmuneRegardlessOfRarity[item.type] = true;
            }
            if (!Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer) && Collision.LavaCollision(item.position, item.width, item.height) && LavaProof == false)
            {
                ItemID.Sets.IsLavaImmuneRegardlessOfRarity[item.type] = false;
            }
            if (item.type == ItemID.Amethyst || item.type == ItemID.Topaz || item.type == ItemID.Sapphire || item.type == ItemID.Emerald || item.type == ItemID.Ruby || item.type == ItemID.Diamond)
            {
                ItemID.Sets.IsLavaImmuneRegardlessOfRarity[item.type] = true;
            }
        }

		public override void SetStaticDefaults()
		{
            ItemID.Sets.ShimmerTransformToItem[ItemID.Amber] = ItemID.Diamond;
            ItemID.Sets.ShimmerTransformToItem[ItemID.CobaltOre] = ModContent.ItemType<ArqueriteOre>();
            ItemID.Sets.ShimmerTransformToItem[ItemID.Hellstone] = ItemID.PlatinumOre;
            ItemID.Sets.ShimmerTransformToItem[ItemID.AshGrassSeeds] = ModContent.ItemType<NightmareSeeds>();
            ItemID.Sets.ShimmerTransformToItem[ItemID.LivingFireBlock] = ModContent.ItemType<LivingFog>();

            ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(ItemID.AshBlock, 1, ModContent.ItemType<ShaleBlock>(), 1);
            ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(ItemID.Hellstone, 1, ModContent.ItemType<ArqueriteOre>(), 1);
			ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(ItemID.Obsidian, 1, ModContent.ItemType<Quartz>(), 1);
		}

		public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
		{
			if (player.GetModPlayer<TheDepthsPlayer>().HasAquaQuiver && type == ProjectileID.WoodenArrowFriendly)
			{
                type = ModContent.ProjectileType<Projectiles.AquaArrow>();
                damage += 2f;
            }
		}

		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.LockBox)
            {
                itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Accessories.PalladiumShield>(), 7));
            }
			if (item.type == ItemID.WallOfFleshBossBag)
			{
				itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Accessories.HungryLeash>()));
			}
		}

		public override bool IsAnglerQuestAvailable(int type)
		{
            if (Worldgen.TheDepthsWorldGen.isWorldDepths && (!Worldgen.TheDepthsWorldGen.DrunkDepthsLeft && !Worldgen.TheDepthsWorldGen.DrunkDepthsRight))
            {
                if (type == ItemID.DemonicHellfish)
                {
                    return false;
                }
				if (type == ItemID.GuideVoodooFish)
				{
					return false;
				}
				if (type == ItemID.Hungerfish)
				{
					return false;
				}
			}
            return true;
		}

		public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack)
		{
            if (extractinatorBlockType == TileID.ChlorophyteExtractinator && (extractType == ItemID.LavaMoss || extractType == ItemID.ArgonMoss || extractType == ItemID.XenonMoss || extractType == ItemID.KryptonMoss || extractType == ItemID.VioletMoss))
            {
                int ItemType = ItemID.BlueMoss;
                if (Main.rand.Next(100) <= 12)
                {
                    switch (Main.rand.Next(6))
                    {
                        case 0:
                            ItemType = ItemID.LavaMoss;
                            break;
                        case 1:
                            ItemType = ItemID.XenonMoss;
                            break;
                        case 2:
                            ItemType = ItemID.KryptonMoss;
                            break;
                        case 3:
                            ItemType = ItemID.ArgonMoss;
                            break;
                        case 4:
                            ItemType = ItemID.VioletMoss;
                            break;
                        case 5:
                            ItemType = ModContent.ItemType<MercuryMoss>();
                            break;
                    }
                }
                else
                {
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
                }
                resultType = ItemType;
                resultStack = 1;
            }
        }
	}

	public class HellItemBlockig : GlobalItem
	{
		public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
            return entity.type == ItemID.ShellphoneHell || entity.type == ItemID.LavaBucket || entity.type == ItemID.BottomlessLavaBucket || entity.type == ItemID.DemonConch || entity.type == ItemID.LavaAbsorbantSponge;
		}

		public override bool CanUseItem(Item item, Player player)
		{
            if (item.type == ItemID.LavaBucket || item.type == ItemID.BottomlessLavaBucket)
            {
				item.autoReuse = !Worldgen.TheDepthsWorldGen.InDepths(player);
			}
			if (!Worldgen.TheDepthsWorldGen.InDepths(player))
			{
				return true;
			}
            else
            {
                if (item.type == ItemID.LavaBucket || item.type == ItemID.BottomlessLavaBucket)
                {
                    if (player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, TileReachCheckSettings.Simple) && !Main.tile[Player.tileTargetX, Player.tileTargetY].HasTile && (Main.tile[Player.tileTargetX, Player.tileTargetY].LiquidAmount == 0 || Main.tile[Player.tileTargetX, Player.tileTargetY].LiquidType == LiquidID.Lava))
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            Dust.NewDustDirect(Main.MouseWorld + new Vector2(-4f, -4f), 4, 4, DustID.Smoke, 0f, -1f);
                        }
                    }
                }
				return false;
			}
		}
	}

	public class TerrasparkUpgrade : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.TerrasparkBoots;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            player.GetModPlayer<TheDepthsPlayer>().aAmulet2 = true;
            player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;
            player.GetModPlayer<TheDepthsPlayer>().stoneRose = true;
        }
    }

    public class AcornTreeEdit : GlobalItem //Thanks Gabe and the Verdant mod for this
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Acorn;

        public override void HoldItem(Item item, Player player)
        {
            if (CanPlaceAtNight(Main.MouseWorld.ToTileCoordinates(), player)) // Replace placement with lush sapling if it can be placed here
                item.createTile = ModContent.TileType<NightSapling>();
            else if (CanPlaceAtPetrified(Main.MouseWorld.ToTileCoordinates(), player))
                item.createTile = ModContent.TileType<PetrifiedSapling>();
            else if (item.createTile == ModContent.TileType<PetrifiedSapling>() || item.createTile == ModContent.TileType<NightSapling>()) // Otherwise revert only if it's still a lush sapling
                item.createTile = TileID.Saplings;
        }

        /// <summary>
        /// Whether a nightwood sapling can be planted here. Checks the tile below the given coordinates.
        /// </summary>
        public static bool CanPlaceAtNight(Point pos, Player player)
        {
            Tile tile = Main.tile[pos.X, pos.Y + 1];
            bool inRange = player.IsInTileInteractionRange(pos.X, pos.Y + 1, TileReachCheckSettings.Simple);

            return inRange && tile.HasTile && TileObjectData.GetTileData(ModContent.TileType<NightSapling>(), 0).AnchorValidTiles.Contains(tile.TileType);
        }

        /// <summary>
        /// Whether a petrified sapling can be planted here. Checks the tile below the given coordinates.
        /// </summary>
        public static bool CanPlaceAtPetrified(Point pos, Player player)
        {
            Tile tile = Main.tile[pos.X, pos.Y + 1];
            bool inRange = player.IsInTileInteractionRange(pos.X, pos.Y + 1, TileReachCheckSettings.Simple);

            return inRange && tile.HasTile && TileObjectData.GetTileData(ModContent.TileType<PetrifiedSapling>(), 0).AnchorValidTiles.Contains(tile.TileType);
        }
    }
}
