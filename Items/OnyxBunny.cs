using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Placeable;

namespace TheDepths.Items
{
	public class OnyxBunny : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 5;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Onyx>();
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.width = 12;
			Item.height = 12;
			Item.noUseGraphic = true;
			Item.value = Item.sellPrice(0, 0, 10);
			Item.makeNPC = ModContent.NPCType<NPCs.OnyxBunny>();
		}
	}
}

