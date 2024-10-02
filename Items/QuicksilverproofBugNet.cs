using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Items.Armor;

namespace TheDepths.Items
{
	public class QuicksilverproofBugNet : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.LavaproofCatchingTool[Item.type] = true;
			ItemID.Sets.CatchingTool[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 48;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.useAnimation = 20;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
		}

		public override bool? CanCatchNPC(NPC target, Player player)
		{
			return null;
		}
	}
}
