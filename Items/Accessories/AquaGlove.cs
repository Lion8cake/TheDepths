using TheDepths.Tiles;
using TheDepths.Buffs;
using TheDepths.Items.Placeable;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

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
			Tooltip.SetDefault("Increases Melee knockback and inflicts freezing water on attack\n12% increased melee speed and damage"
				+"\nEnables auto swing for melee weapons");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.value = 300000;
			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage(DamageClass.Melee) += 0.12f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
			player.kbGlove = true;
            player.GetModPlayer<TheDepthsPlayer>().aStone = true;
			player.autoReuseGlove = true;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MechanicalGlove, 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Accessories.AquaStone>(), 1);
			recipe.AddTile(114);
			recipe.Register();
		}
	}
}