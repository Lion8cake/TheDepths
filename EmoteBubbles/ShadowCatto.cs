using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.EmoteBubbles
{
	public class ShadowCatto : ModEmoteBubble
	{
		public override void SetStaticDefaults() {
			AddToCategory(EmoteID.Category.Items);
		}

		public override LocalizedText Command => Language.GetOrRegister("Mods.TheDepths.Emotes.ShadowCatEmote");
	}
}
