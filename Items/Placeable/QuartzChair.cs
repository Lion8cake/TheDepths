using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable
{
	public class QuartzChair : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quartz Chair");
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 0;
			Item.createTile = Mod.Find<ModTile>("QuartzChair").Type;
		}
	}
}
