using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.Utilities;
using TheDepths.Buffs;
using TheDepths.Dusts;
using static Terraria.ModLoader.ModContent;

namespace TheDepths.Items.Accessories
{
	public class AquaStone : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Inflicts freezing water on attack");
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 100000;
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<TheDepthsPlayer>().aStone = true;
		}
	}
	
	public class InflictWater : GlobalNPC
    {
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (player.GetModPlayer<TheDepthsPlayer>().aStone)
            {
                npc.AddBuff(BuffType<FreezingWater>(), Main.rand.Next(100, 200));
            }
        }
    }	
}