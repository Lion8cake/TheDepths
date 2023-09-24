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
            if (Worldgen.TheDepthsWorldGen.InDepths && Collision.LavaCollision(item.position, item.width, item.height))
            {
                ItemID.Sets.IsLavaImmuneRegardlessOfRarity[item.type] = true;
            }
            if (!Worldgen.TheDepthsWorldGen.InDepths && Collision.LavaCollision(item.position, item.width, item.height) && LavaProof == false)
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
            if (!Worldgen.TheDepthsWorldGen.InDepths)
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
            player.GetModPlayer<TheDepthsPlayer>().aAmulet = true;
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
            if (Worldgen.TheDepthsWorldGen.InDepths)
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
            if (Worldgen.TheDepthsWorldGen.InDepths)
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
            if (Worldgen.TheDepthsWorldGen.InDepths)
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
            if (Worldgen.TheDepthsWorldGen.InDepths)
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
            if (Worldgen.TheDepthsWorldGen.InDepths)
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
            if (Worldgen.TheDepthsWorldGen.InDepths)
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
            if (Worldgen.TheDepthsWorldGen.InDepths)
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
            if (Worldgen.TheDepthsWorldGen.InDepths)
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
            if (Worldgen.TheDepthsWorldGen.InDepths)
            {
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.QuicksilverBuckets.QuicksilverAbsorbantSpongeName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.LavaAbsorbantSponge"));
            }
        }
    }

    public class ShellphoneHell : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ShellphoneHell;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths)
            {
                tooltips.RemoveAll(t => t.Text.Contains("everything"));
                tooltips.RemoveAll(t => t.Text.Contains("underworld"));
                tooltips.RemoveAll(t => t.Text.Contains("toggle"));
                tooltips.RemoveAll(t => t.Text.Contains("you"));
                tooltips.Add(new(Mod, "NewDescription", (string)Language.GetOrRegister("Mods.TheDepths.Items.ShellPhoneDepths.Tooltip")));
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.Items.ShellPhoneDepths.DisplayName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.ShellphoneHell"));
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths)
            {
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.Items.ShellPhoneDepths.DisplayName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.ShellphoneHell"));
            }
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths)
            {
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.Items.ShellPhoneDepths.DisplayName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.ShellphoneHell"));
            }
        }
    }

    public class ShellPhoneNameFix : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ShellphoneSpawn;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            item.SetNameOverride((string)Language.GetOrRegister("ItemName.ShellphoneSpawn"));
        }
        public override void UpdateInventory(Item item, Player player)
        {
            item.SetNameOverride((string)Language.GetOrRegister("ItemName.ShellphoneSpawn"));
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            item.SetNameOverride((string)Language.GetOrRegister("ItemName.ShellphoneSpawn"));
        }
    }

    public class ShellPhoneNameFix2 : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.Shellphone;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            item.SetNameOverride((string)Language.GetOrRegister("ItemName.Shellphone"));
        }
        public override void UpdateInventory(Item item, Player player)
        {
            item.SetNameOverride((string)Language.GetOrRegister("ItemName.Shellphone"));
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            item.SetNameOverride((string)Language.GetOrRegister("ItemName.Shellphone"));
        }
    }

    public class ShellPhoneNameFix3 : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ShellphoneOcean;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            item.SetNameOverride((string)Language.GetOrRegister("ItemName.ShellphoneOcean"));
        }
        public override void UpdateInventory(Item item, Player player)
        {
            item.SetNameOverride((string)Language.GetOrRegister("ItemName.ShellphoneOcean"));
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            item.SetNameOverride((string)Language.GetOrRegister("ItemName.ShellphoneOcean"));
        }
    }
}
