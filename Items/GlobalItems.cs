using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Items.Placeable;
using Terraria.GameContent.Creative;
using System.Collections.Generic;

namespace TheDepths.Items
{
    public class GlobalItems : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (TheDepthsWorldGen.depthsorHell)
            {
                ItemID.Sets.IsLavaImmuneRegardlessOfRarity[item.type] = true;
            }
            if (item.type == ItemID.Amethyst || item.type == ItemID.Topaz || item.type == ItemID.Sapphire || item.type == ItemID.Emerald || item.type == ItemID.Ruby || item.type == ItemID.Diamond)
            {
                ItemID.Sets.IsLavaImmuneRegardlessOfRarity[item.type] = true;
            }
        }
    }
}
