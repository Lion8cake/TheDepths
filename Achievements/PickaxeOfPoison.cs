using System.Collections.Generic;
using Terraria.Achievements;
using Terraria.ModLoader;
using TheDepths.Items.Weapons;

namespace TheDepths.Achievements
{
	public class PickaxeOfPoison : ModAchievement
	{
		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Collector);

			AddItemCraftCondition(ModContent.ItemType<MercuryPickaxe>());
		}

		public override Position GetDefaultPosition() => new Before("STILL_HUNGRY");
	}
}
