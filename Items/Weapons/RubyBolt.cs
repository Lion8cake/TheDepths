using TheDepths.Projectiles;
using TheDepths.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Weapons
{
	public class RubyBolt : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 72;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 7;
			Item.width = 48;
			Item.height = 44;
			Item.useTime = 27;
			Item.useAnimation = 27;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 3f;
			Item.value = Item.sellPrice(0, 1, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.RubyBolt>();
			Item.shootSpeed = 7.5f;
		}
	}
}