using Terraria.Achievements;
using Terraria.ModLoader;

namespace TheDepths.Achievements
{
	public class HeartBreaker : ModAchievement
	{
		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Slayer);

			AddNPCKilledCondition(ModContent.NPCType<NPCs.Chasme.ChasmeHeart>());
		}

		public override Position GetDefaultPosition() => new After("STILL_HUNGRY");
	}
}
