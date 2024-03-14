using TheDepths.Dusts;
using TheDepths.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using System;
using Terraria.DataStructures;
using TheDepths.Projectiles;
using Terraria.Audio;

namespace TheDepths.Items.Weapons
{
	public class Terminex : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 38;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30; 
			Item.knockBack = 15;
			Item.value = Item.buyPrice(gold: 27);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1; 
			Item.autoReuse = true;
			Item.crit = 4;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox) {
			for (int i = 0; i < 2; i++)
			{
				GetPointOnSwungItemPath(70f, 70f, 0.2f + 0.8f * Main.rand.NextFloat(), player.GetAdjustedItemScale(Item), out var location, out var outwardDirection);
				Vector2 vector = outwardDirection.RotatedBy((float)Math.PI / 2f * (float)player.direction * player.gravDir);
				Dust.NewDustPerfect(location, ModContent.DustType<MercuryFire>(), vector * 4f, 100, default(Color), 2.5f).noGravity = true;
			}
		}

		private void GetPointOnSwungItemPath(float spriteWidth, float spriteHeight, float normalizedPointOnPath, float itemScale, out Vector2 location, out Vector2 outwardDirection)
		{
			Player player = Main.LocalPlayer;
			float num = (float)Math.Sqrt(spriteWidth * spriteWidth + spriteHeight * spriteHeight);
			float num2 = (float)(player.direction == 1).ToInt() * ((float)Math.PI / 2f);
			if (player.gravDir == -1f)
			{
				num2 += (float)Math.PI / 2f * (float)player.direction;
			}
			outwardDirection = player.itemRotation.ToRotationVector2().RotatedBy(3.926991f + num2);
			location = player.RotatedRelativePoint(player.itemLocation + outwardDirection * num * normalizedPointOnPath * itemScale);
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<MercuryExplosion>()] < 20)
			{
				int num403 = Main.rand.Next(20, 31);
				for (int num404 = 0; num404 < num403; num404++)
				{
					Vector2 vector45 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
					vector45.Normalize();
					vector45 *= (float)Main.rand.Next(10, 201) * 0.01f;
					Projectile.NewProjectile(new EntitySource_Misc(""), target.position.X, target.position.Y, vector45.X, vector45.Y, ModContent.ProjectileType<Projectiles.MercuryExplosion>(), 0, 0, Main.LocalPlayer.whoAmI);
				}
				SoundEngine.PlaySound(new SoundStyle("TheDepths/Sounds/Item/deerclops_ice_attack_0") { Volume = 1f, }, target.position);
			}
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.ArqueriteBar>(), 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
