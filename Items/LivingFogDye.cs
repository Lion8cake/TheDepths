using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class LivingFogDye : ModItem
	{
		public override void SetStaticDefaults() {
			if (!Main.dedServ) {
				GameShaders.Armor.BindShader(
					Item.type, 
					new ArmorShaderData(
						new Ref<Effect>(Mod.Assets.Request<Effect>("Shaders/LivingFogDye", 
						AssetRequestMode.ImmediateLoad).Value
					), "LivingFogDyeShaderPass")
				).UseImage(ModContent.Request<Texture2D>("TheDepths/Shaders/Perlin"));
			}
			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() {
			int dye = Item.dye;
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = Item.sellPrice(0, 1, 50);
			Item.rare = 3;
			Item.dye = dye;
		}
	}
}
