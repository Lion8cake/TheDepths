using TheDepths.Tiles;
using TheDepths.Projectiles.GeodeSetBonus;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Items.Placeable;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace TheDepths.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class GeodeHelmet : ModItem
	{ 
		public int timer;
		
		public override void SetStaticDefaults() {
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 5;
			Item.value = 4500;
		}
		
		public override void UpdateEquip(Player player) {
			player.maxMinions++;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<GeodeChestplate>() && legs.type == ModContent.ItemType<GeodeLeggings>();
		}

		public override void UpdateArmorSet(Player player) {
			player.setBonus = Language.GetTextValue("Mods.TheDepths.SetBonus.GeodeHelmet");
			timer++;
			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("GeodeCrystalSummon").Type] < 1 && timer > 20)
			{
				for (int i = 0; i < 6; i++)
				{
					Projectile.NewProjectile(new EntitySource_Misc(""), player.Center.X, player.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("GeodeCrystalSummon").Type, 15, 8f, player.whoAmI, i);
				}
				timer = 0;
			}
			for (int j = 0; j < 1000; j++)
			{
				Projectile projectile = Main.projectile[j];
				if (projectile.active && projectile.owner == player.whoAmI && projectile.type == Mod.Find<ModProjectile>("GeodeCrystalSummon").Type)
				{
					projectile.timeLeft = 2;
				}
			}
		}
		
		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 2);
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ModContent.ItemType<Items.Placeable.Geode>(), 2);
			recipe2.AddIngredient(ItemID.ShadowScale, 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}