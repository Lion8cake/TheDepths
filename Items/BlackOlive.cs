﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class BlackOlive : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(17, 17, 23),
				new Color(16, 16, 22),
				new Color(14, 14, 20)
			};

			ItemID.Sets.IsFood[Type] = true;

			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Ambrosia;
		}

		public override void SetDefaults() {
			Item.DefaultToFood(22, 22, BuffID.WellFed, 18000);
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
		}
    }
}