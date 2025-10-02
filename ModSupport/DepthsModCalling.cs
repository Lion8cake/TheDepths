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
		public static readonly Mod? FargoSeeds = ModLoader.TryGetMod("FargoSeeds", out Mod obtainedMod) ? obtainedMod : null;

		public static readonly Mod? BiomeLavaMod = ModLoader.TryGetMod("BiomeLava", out Mod obtainedMod) ? obtainedMod : null;

		/// <summary>
		/// Checks if fargos best of both worlds is enabled
		/// </summary>
		public static bool FargoBoBW = false;

		/// <summary>
		/// Call this method to update the FargoBoBW bool before using it.
		/// </summary>
		public static void UpdateFargoBoBW()
		{
			FargoBoBW = FargoSeeds != null && ModContent.GetInstance<FargosBoBWConfig>().BothCores;
		}

		public override void PostSetupContent()
		{
			BossChecklistSetup();
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