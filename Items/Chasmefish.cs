using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class Chasmefish : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 2;
		}

		public override void SetDefaults() {
			Item.DefaultToQuestFish();
		}

		public override bool IsQuestFish() => true;

		public override bool IsAnglerQuestAvailable() => Main.hardMode && (Worldgen.TheDepthsWorldGen.depthsorHell || Worldgen.TheDepthsWorldGen.DrunkDepthsLeft || Worldgen.TheDepthsWorldGen.DrunkDepthsRight);

		public override void AnglerQuestChat(ref string description, ref string catchLocation) {
			description = Language.GetTextValue("Mods.TheDepths.Quest.Description.Chasmefish");
			catchLocation = Language.GetTextValue("Mods.TheDepths.Quest.CaughtLocation.Chasmefish");
		}
	}
}
