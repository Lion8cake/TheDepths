using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class Relicarp : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 2;
		}

		public override void SetDefaults() {
			Item.DefaultToQuestFish();
		}

		public override bool IsQuestFish() => true;

		public override bool IsAnglerQuestAvailable() => Worldgen.TheDepthsWorldGen.isWorldDepths || Worldgen.TheDepthsWorldGen.DrunkDepthsLeft || Worldgen.TheDepthsWorldGen.DrunkDepthsRight;

		public override void AnglerQuestChat(ref string description, ref string catchLocation) {
			description = Language.GetTextValue("Mods.TheDepths.Quest.Description.Relicarp");
			catchLocation = Language.GetTextValue("Mods.TheDepths.Quest.CaughtLocation.Relicarp");
		}
	}
}
