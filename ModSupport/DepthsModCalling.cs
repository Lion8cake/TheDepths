using Terraria.ModLoader;
using Terraria.Achievements;
using Terraria.Localization;

namespace TheDepths.ModSupport
{
    public class DepthsModCalling : ModSystem
    {
		public static readonly Mod? Achievements = ModLoader.TryGetMod("TMLAchievements", out Mod obtainedMod) ? obtainedMod : null;

        public static bool FargoBoBWSupport = ModLoader.HasMod("FargoSeeds") && ModContent.GetInstance<ModSupport.FargosBoBWConfig>().BothCores;

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

		public override void PostAddRecipes()
		{
			if (!ModLoader.TryGetMod("MusicDisplay", out Mod musicDisplay))
				return;

			LocalizedText modName = Language.GetText("Mods.TheDepths.MusicDisplay.ModName");

			void AddMusic(string path, string name)
			{
				LocalizedText author = Language.GetText("Mods.TheDepths.MusicDisplay.Author");
				LocalizedText displayName = Language.GetText("Mods.TheDepths.MusicDisplay." + name + ".DisplayName");
				musicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, path), displayName, author, modName);
			}

			AddMusic("Sounds/Music/Depths", "Depths");
			AddMusic("Sounds/Music/DepthsOtherworldly", "DepthsOtherworldly");
			AddMusic("Sounds/Music/Chasme", "Chasme");
		}
	}
}