using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class IntenseShadowDye : ModItem
	{
		public override void SetStaticDefaults() {
			if (!Main.dedServ) {
				GameShaders.Armor.BindShader(
					Item.type,
					new ArmorShaderData(
						new Ref<Effect>(Mod.Assets.Request<Effect>("Shaders/ShadowedDye",
						AssetRequestMode.ImmediateLoad).Value
					), "DeepShadowDyeShaderPass")
				);
			}
			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() {
			int dye = Item.dye;
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = Item.sellPrice(0, 0, 75);
			Item.rare = ItemRarityID.Orange;
			Item.dye = dye;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ShadowDye, 2);
			recipe.AddTile(TileID.DyeVat);
			recipe.Register();
		}
	}
}
