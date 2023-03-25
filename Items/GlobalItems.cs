using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Items.Placeable;
using Terraria.GameContent.Creative;
using AltLibrary.Common.Systems;
using System.Collections.Generic;

namespace TheDepths.Items
{
    public class GlobalItems : GlobalItem
    {


        public override bool? CanBurnInLava(Item item)
        {
            if (WorldBiomeManager.WorldHell == "TheDepths/AltDepthsBiome")
            {
                return false;
            }
            if (item.type == ItemID.Amethyst || item.type == ItemID.Topaz || item.type == ItemID.Sapphire || item.type == ItemID.Emerald || item.type == ItemID.Ruby || item.type == ItemID.Diamond)
            {
                return false;
            }
            return true;
        }
    }
}
