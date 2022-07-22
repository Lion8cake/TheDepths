using TheDepths.Tiles;
using TheDepths.Buffs;
using TheDepths.Items.Placeable;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace TheDepths.Items.Accessories
{
	[AutoloadEquip(new EquipType[]
	{
		EquipType.HandsOn,
		EquipType.HandsOff
	})]
	public class AquaGlove : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Increases Melee knockback and inflicts freezing water on attack\n9% Melee Speed and Damage");
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 300000;
			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage(DamageClass.Melee) += 0.09f;
			//player.meleeSpeed += 0.09f;
			player.kbGlove = true;
            player.GetModPlayer<TheDepthsPlayer>().aStone = true;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.MechanicalGlove, 1).AddIngredient(ModContent.ItemType<Items.Accessories.AquaStone>(), 1).AddTile(114).Register();
		}
	}
}