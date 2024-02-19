using Terraria.ModLoader;
using Terraria.Achievements;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using TheDepths.Items;

namespace TheDepths.ModSupport
{
    public class DepthsModCalling : ModSystem
    {
		public static readonly Mod? Achievements = ModLoader.TryGetMod("TMLAchievements", out Mod obtainedMod) ? obtainedMod : null;

        public static bool FargoBoBWSupport = ModLoader.HasMod("FargoSeeds") && ModContent.GetInstance<ModSupport.FargosBoBWConfig>().BothCores;

        public override void PostSetupContent()
		{
			AchievementSetup();

			BossChecklistSetup();
		}

		private void AchievementSetup()
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

		private void BossChecklistSetup()
		{
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
			{
				return;
			}

			string internalName = "Chasme";

			float weight = 7f;

			Func<bool> downed = () => Worldgen.TheDepthsWorldGen.downedChasme;

			int bossType = ModContent.NPCType<NPCs.Chasme.ChasmeHeart>();

			int spawnItem = ModContent.ItemType<RubyRelic>();

			LocalizedText spawnInfo = Language.GetText("Mods.TheDepths.BossChecklist.SpawnInfo.Chasme");

			LocalizedText displayName = Language.GetText("Mods.TheDepths.NPCs.ChasmeBody.DisplayName");

			List<int> collectibles = new List<int>()
			{
				ModContent.ItemType<Items.Placeable.ChasmeRelic>(),
				ModContent.ItemType<Items.MidnightHorseshoe>(),
				ModContent.ItemType<Items.Placeable.ChasmeTrophy>(),
				ModContent.ItemType<Items.Armor.ChasmeSoulMask>(),
				ModContent.ItemType<Items.Armor.ShadowChasmeMask>(),
				ModContent.ItemType<Items.Placeable.ChasmeMusicBox>()
			};

			var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
				Texture2D texture = ModContent.Request<Texture2D>("TheDepths/NPCs/Chasme/Chasme_Preview").Value;
				Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
				sb.Draw(texture, centered, new Rectangle(0, 0, 522, 408), color, 0f, new Vector2(-110, -40), 0.7f, SpriteEffects.None, 0);
			};

			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>()
				{
					["displayName"] = displayName,
					["spawnItems"] = spawnItem,
					["spawnInfo"] = spawnInfo,
					["collectibles"] = collectibles,
					["customPortrait"] = customPortrait
				}
			);
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