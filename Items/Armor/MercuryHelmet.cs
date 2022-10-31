using TheDepths.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Buffs;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MercuryHelmet : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("4% Increased melee critical strike chance");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
			Item.value = 45000;
		}
		
		public override void UpdateEquip(Player player) {
		player.GetCritChance(DamageClass.Generic) += 4;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<MercuryChestplate>() && legs.type == ModContent.ItemType<MercuryGreaves>();
		}

		public override void UpdateArmorSet(Player player) {
			player.setBonus = "3 Defence"
				+"\nMercury cannot infect you";
			player.statDefense += 3;
			player.buffImmune[ModContent.BuffType<MercuryBoiling>()] = true;
		}

		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}