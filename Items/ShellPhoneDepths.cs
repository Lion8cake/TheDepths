using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Items
{
	public class ShellPhoneDepths : ModItem
	{
        public override void SetStaticDefaults()
        {
			//ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
			ItemID.Sets.CanBeQuickusedOnGamepad[Type] = true;
        }

        public override void SetDefaults() {
			Item.useTurn = true;
			Item.width = 20;
			Item.height = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 90;
			Item.UseSound = SoundID.Item6;
			Item.useAnimation = 90;
			Item.SetShopValues(ItemRarityColor.Yellow8, Item.sellPrice(0, 10));
		}

        public override void UpdateInfoAccessory(Player player)
        {
			player.accWatch = 3;
			player.accDepthMeter = 1;
			player.accCompass = 1;
			player.accFishFinder = true;
			player.accWeatherRadio = true;
			player.accCalendar = true;
			player.accThirdEye = true;
			player.accJarOfSouls = true;
			player.accCritterGuide = true;
			player.accStopwatch = true;
			player.accOreFinder = true;
			player.accDreamCatcher = true;
		}
    }
}
