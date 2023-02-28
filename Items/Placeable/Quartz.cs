using TheDepths.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria;
using TheDepths.Items.Weapons;

namespace TheDepths.Items.Placeable
{
	public class Quartz : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Quartz>();
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.White;
			//Item.shoot = ModContent.ProjectileType<Projectiles.QuartzChunk>(); 
		}

        public override bool? CanBeChosenAsAmmo(Item weapon, Player player)
        {
			if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<QuartzCannon>())
			{
				return true;
			}
			return false;
		}
    }
}
