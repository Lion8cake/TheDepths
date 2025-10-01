using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace TheDepths.Achievements
{
	public class MercuryAngler : ModAchievement
	{
		public CustomFlagCondition FishingInQuicksilver { get; private set; }

		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Challenger);

			FishingInQuicksilver = AddCondition();
		}

		public override Position GetDefaultPosition() => new After("GO_LAVA_FISHING");
	}
}
