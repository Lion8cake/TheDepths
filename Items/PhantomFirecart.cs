using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items
{
	public class PhantomFirecart : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 30000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item79;
			Item.noMelee = true;
			Item.DefaultToMount(ModContent.MountType<Mounts.PhantomFirecart>());
		}
	}
}