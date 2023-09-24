using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Worldgen;

namespace TheDepths.EmoteBubbles
{
	public class BossChasme : ModEmoteBubble
	{
		public override void SetStaticDefaults()
		{
			AddToCategory(EmoteID.Category.Dangers);
		}

		public override bool IsUnlocked()
		{
			return Main.hardMode || TheDepthsWorldGen.downedChasme;
		}

		public override LocalizedText Command => Language.GetOrRegister("Mods.TheDepths.Emotes.ChasmeEmote");
	}
}
