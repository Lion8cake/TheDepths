using Terraria.ModLoader;
using Terraria.Achievements;

namespace TheDepths
{
    public class DepthsModCalling : ModSystem
    {
		public static readonly Mod? Achievements = ModLoader.TryGetMod("TMLAchievements", out Mod obtainedMod) ? obtainedMod : null;

		public override void PostSetupContent()
		{
            if (Achievements == null)
            {
                return;
            }

            Achievements.Call("AddAchievement", ModContent.GetInstance<TheDepths>(), "MercuryAngler", AchievementCategory.Challenger, "TheDepths/Assets/MercuryAngler", null, false, false, 8f, new string[] { "Event_FishingInQuicksilver" });
            Achievements.Call("AddAchievement", ModContent.GetInstance<TheDepths>(), "MysteriesOfTheDark", AchievementCategory.Explorer, "TheDepths/Assets/MysteriesOfTheDark", null, false, false, 8f, new string[] { "Event_WalkedIntoTheDepths" });
            Achievements.Call("AddAchievement", ModContent.GetInstance<TheDepths>(), "PickaxeOfPoison", AchievementCategory.Collector, "TheDepths/Assets/PickaxeOfPoison", null, false, false, 8f, new string[] { "Craft_" + ModContent.ItemType<Items.Weapons.MercuryPickaxe>() });
            Achievements.Call("AddAchievement", ModContent.GetInstance<TheDepths>(), "HeartSmasher", AchievementCategory.Slayer, "TheDepths/Assets/HeartBreaker", null, false, false, 8f, new string[] { "Kill_" + ModContent.NPCType<NPCs.Chasme.ChasmeHeart>() });
        }
	}
}