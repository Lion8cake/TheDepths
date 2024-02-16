using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class Lamington : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(36, 25, 21),
				new Color(255, 255, 255),
				new Color(213, 214, 218)
			};

			ItemID.Sets.IsFood[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.DefaultToFood(22, 22, BuffID.WellFed3, 43200);
			Item.value = Item.buyPrice(0, 3);
			Item.rare = ItemRarityID.Orange;
		}
	}
}