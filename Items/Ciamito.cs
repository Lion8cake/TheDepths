using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class Ciamito : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(191, 126, 245),
				new Color(228, 194, 255),
				new Color(249, 241, 255)
			};

			ItemID.Sets.IsFood[Type] = true;

			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Ambrosia;
		}

		public override void SetDefaults() {
			Item.DefaultToFood(22, 22, BuffID.WellFed, 18000);
			Item.value = Item.buyPrice(0, 20);
			Item.rare = ItemRarityID.Blue;
		}
    }
}