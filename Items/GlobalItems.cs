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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld) && Collision.LavaCollision(item.position, item.width, item.height))
            {
                ItemID.Sets.IsLavaImmuneRegardlessOfRarity[item.type] = true;
            }
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld) == false && Collision.LavaCollision(item.position, item.width, item.height) && LavaProof == false)
            {
                ItemID.Sets.IsLavaImmuneRegardlessOfRarity[item.type] = false;
            }
            if (item.type == ItemID.Amethyst || item.type == ItemID.Topaz || item.type == ItemID.Sapphire || item.type == ItemID.Emerald || item.type == ItemID.Ruby || item.type == ItemID.Diamond)
            {
                ItemID.Sets.IsLavaImmuneRegardlessOfRarity[item.type] = true;
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
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
            if ((TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
            {
                item.SetNameOverride((string)Language.GetOrRegister("Mods.TheDepths.Items.ShellPhoneDepths.DisplayName"));
            }
            else
            {
                item.SetNameOverride((string)Language.GetOrRegister("ItemName.ShellphoneHell"));
            }
        }
    }
}
