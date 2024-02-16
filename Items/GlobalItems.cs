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

            ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(ItemID.AshBlock, 1, ModContent.ItemType<ShaleBlock>(), 1);
            ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(ItemID.Hellstone, 1, ModContent.ItemType<ArqueriteOre>(), 1);
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
        }

		public override bool IsAnglerQuestAvailable(int type)
		{
            if (Worldgen.TheDepthsWorldGen.depthsorHell && (!Worldgen.TheDepthsWorldGen.DrunkDepthsLeft && !Worldgen.TheDepthsWorldGen.DrunkDepthsRight))
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

    public class DemonConch : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.DemonConch;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (!Worldgen.TheDepthsWorldGen.InDepths(player))
            {
                return true;
            }
            return false;
        }
    }

	public class ShellPhoneUnderworld : GlobalItem
	{
		public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
            return entity.type == ItemID.ShellphoneHell;
		}

		public override bool CanUseItem(Item item, Player player)
		{
			if (!Worldgen.TheDepthsWorldGen.InDepths(player))
			{
				return true;
			}
			return false;
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

    public class LavaBucket : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.LavaBucket;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
            {
                tooltips.RemoveAll(t => t.Text.Contains("lava"));
                tooltips.RemoveAll(t => t.Text.Contains("poured"));
                tooltips.Add(new(Mod, "NewDescription", (string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.QuicksilverBucketDescription")));
                tooltips.Add(new(Mod, "NewDescription", (string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.PouredOut")));
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.QuicksilverBucketName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.LavaBucket"));
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths(player))
            {
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.QuicksilverBucketName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.LavaBucket"));
            }
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
            {
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.QuicksilverBucketName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.LavaBucket"));
            }
        }
    }

    public class BottomlessLavaBucket : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BottomlessLavaBucket;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
            {
                tooltips.RemoveAll(t => t.Text.Contains("lava"));
                tooltips.RemoveAll(t => t.Text.Contains("poured"));
                tooltips.Add(new(Mod, "NewDescription", (string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.BottomlessQuicksilverBucketDescription")));
                tooltips.Add(new(Mod, "NewDescription", (string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.PouredOut")));
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.BottomlessQuicksilverBucketName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.BottomlessLavaBucket"));
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths(player))
            {
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.BottomlessQuicksilverBucketName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.BottomlessLavaBucket"));
            }
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
            {
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.BottomlessQuicksilverBucketName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.BottomlessLavaBucket"));
            }
        }
    }

    public class LavaSponge : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.LavaAbsorbantSponge;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
            {
                tooltips.RemoveAll(t => t.Text.Contains("lava"));
                tooltips.Add(new(Mod, "NewDescription", (string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.QuicksilverAbsorbantSpongeDescription")));
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.QuicksilverAbsorbantSpongeName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.LavaAbsorbantSponge"));
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths(player))
            {
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.QuicksilverAbsorbantSpongeName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.LavaAbsorbantSponge"));
            }
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths(Main.LocalPlayer))
            {
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.QuicksilverAbsorbantSpongeName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.LavaAbsorbantSponge"));
            }
        }
    }
}
