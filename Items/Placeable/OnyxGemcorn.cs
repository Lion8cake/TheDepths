using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheDepths.Items.Placeable
{
	public class OnyxGemcorn : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 5;
		}

		public override void SetDefaults()
		{
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Trees.OnyxGemtreeSapling>();
			Item.width = 22;
			Item.useTurn = true;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.maxStack = 9999;
			Item.useAnimation = 15;
			Item.height = 22;
		}
    }
}
