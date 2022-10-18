using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheDepths.Projectiles.Nohitweapon;

namespace TheDepths.Items.Weapons
{
	public class MiracfulSaber : ModItem
	{
		public int swings = 0;
		public int mode = 0;

		protected override bool CloneNewInstances => true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miracful Saber");
			Tooltip.SetDefault($@"Changes blade color every 14 swings
There are 8 weapon modes in total
[c/A500EC:Amethyst Mode]: Transforms in amethyst whip
[c/F19F01:Topaz Mode]: Shoots 4-6 hot fire every swing
[c/0D6BD8:Sapphire Mode]: Sword now able to freeze enemies!
[c/21B873:Emerald Mode]: Power of nature blesses your weapon...
[c/C3292C:Ruby Mode]: Sword begins to shoot crimson musical notes
[c/DFE6EE:Diamond Mode]: Blade becomes so shiny, so it begins to shoot with sparkles!
[c/D58E30:Amber Mode]: Shoots with amber snakes those able to deal colossal damage
[c/393940:Onyx Mode]: Shoots 5 onyx blasters");
		}

		public override void SetDefaults()
		{
			Item.knockBack = 2f;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.damage = 45;
			Item.crit += 10;
			Item.rare = ItemRarityID.Cyan;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ProjectileID.None;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = true;
			swings = 0;
			mode = 0;
		}

		private void SwingsHahaSoFunny()
		{
			if (mode == 0)
			{
				Item.noUseGraphic = true;
				Item.noMelee = true;
				Item.shoot = ModContent.ProjectileType<MiracfulWhip>();
				Item.shootSpeed = 15f;
			}
			else if (mode == 1)
			{
				Item.noUseGraphic = false;
				Item.noMelee = false;
				Item.shoot = ProjectileID.Fireball;
				Item.shootSpeed = 7f;
			}
			else if (mode == 2)
			{
				Item.shoot = ModContent.ProjectileType<MiracfulSapphire>();
				Item.shootSpeed = 5f;
			}
			else if (mode == 3)
			{
				Item.shoot = ProjectileID.CrystalLeafShot;
				Item.shootSpeed = 10f;
			}
			else if (mode == 4)
			{
				Item.shoot = ModContent.ProjectileType<MiracfulCrimson1>();
				Item.shootSpeed = 5f;
			}
			else if (mode == 5)
			{
				Item.shoot = ModContent.ProjectileType<MiracfulSparkle>();
				Item.shootSpeed = 8f;
			}
			else if (mode == 6)
            {
				Item.shoot = ModContent.ProjectileType<MiracfulSnake>();
				Item.shootSpeed = 6f;
            }
			else if (mode == 7)
			{
				Item.shoot = ProjectileID.BlackBolt;
				Item.shootSpeed = 8f;
            }
			else
			{
				Item.noUseGraphic = false;
				Item.noMelee = false;
				Item.shoot = ProjectileID.None;
				Item.shootSpeed = 0f;
			}

            swings++;
			if (swings > 14)
			{
				mode++;
				if (mode > 7)
				{
					mode = 0;
				}
				swings = 0;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			SwingsHahaSoFunny();

			Vector2 speed = new Vector2();
            switch (mode)
            {
				case 0: // Amethyst
					float num = (float)((Main.rand.NextFloat() - 0.75) * 0.785398185253143);
					Projectile.NewProjectile(source, position, speed, ModContent.ProjectileType<MiracfulWhip>(), damage, knockback, player.whoAmI, 0f, num);
					break;
				case 1: // Topaz
					speed.X *= Main.rand.NextFloat(0.5f, 1.5f);
					speed.Y *= Main.rand.NextFloat(0.5f, 1.5f);
					int[] num2 = new int[6] { 0, 0, 0, 0, 0, 0, };
					for (int i = 0; i < Main.rand.Next(4, 6); i++)
					{
						if (Main.rand.NextBool(2))
							num2[i] = Projectile.NewProjectile(source, position, speed.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(5, 75))), ProjectileID.Fireball, damage, knockback, player.whoAmI);
						else
							num2[i] = Projectile.NewProjectile(source, position, speed.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(-75, -5))), ProjectileID.Fireball, damage, knockback, player.whoAmI);
						Main.projectile[num2[i]].friendly = true;
						Main.projectile[num2[i]].hostile = false;
					}
					break;
				case 2: // Sapphire
					Projectile.NewProjectile(source, position, speed, ModContent.ProjectileType<MiracfulSapphire>(), damage, knockback, player.whoAmI);
					break;
				case 3: // Emerald
					int[] num3 = new int[6] { 0, 0, 0, 0, 0, 0, };
					for (int i = 0; i < Main.rand.Next(4, 6); i++)
					{
						if (Main.rand.NextBool(2))
							num3[i] = Projectile.NewProjectile(source, position, speed.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(5, 75))), ProjectileID.CrystalLeafShot, damage, knockback, player.whoAmI);
						else
							num3[i] = Projectile.NewProjectile(source, position, speed.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(-75, -5))), ProjectileID.CrystalLeafShot, damage, knockback, player.whoAmI);
						Main.projectile[num3[i]].friendly = true;
						Main.projectile[num3[i]].hostile = false;
					}
					break;
				case 4: // Ruby
					speed.X *= Main.rand.NextFloat(0.5f, 1.5f);
					speed.Y *= Main.rand.NextFloat(0.5f, 1.5f);
					if (Main.rand.NextBool(3)) type = ModContent.ProjectileType<MiracfulCrimson1>();
					else if (Main.rand.NextBool(3)) type = ModContent.ProjectileType<MiracfulCrimson2>();
					else type = ModContent.ProjectileType<MiracfulCrimson3>();
					Projectile.NewProjectile(source, position, speed, type, damage, knockback, player.whoAmI);
                    break;
                case 5: // Diamond
                    Projectile.NewProjectile(source, position, speed, ModContent.ProjectileType<MiracfulSparkle>(), damage, knockback, player.whoAmI);
                    break;
				case 6: // Amber
					Projectile.NewProjectile(source, position, speed, ModContent.ProjectileType<MiracfulSnake>(), damage, knockback, player.whoAmI);
					break;
				case 7: // Onyx
					for (int i = 0; i < Main.rand.Next(2, 4); i++)
					{
						if (Main.rand.NextBool(2))
							Projectile.NewProjectile(source, position, speed.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(45, 135))), ProjectileID.BlackBolt, damage, knockback, player.whoAmI);
						else
							Projectile.NewProjectile(source, position, speed.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(-135, -45))), ProjectileID.BlackBolt, damage, knockback, player.whoAmI);
					}
                    Projectile.NewProjectile(source, position, speed, ProjectileID.BlackBolt, damage, knockback, player.whoAmI);
					break;
            }
            return false;
		}

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */ //KEEP IT
		{
			SwingsHahaSoFunny();
			return true;
		}

        public override Color? GetAlpha(Color lightColor)
		{
			Color[] itemNameCycleColors = new Color[]
			{
				new Color(165, 0, 236),
				new Color(241, 159, 1),
				new Color(13, 107, 216),
				new Color(33, 184, 115),
				new Color(195, 41, 44),
				new Color(223, 230, 238),
				new Color(213, 142, 48),
				new Color(57, 57, 64),
			};
			float fade = Main.GameUpdateCount % 80 / 60f;
			int index = (int)(Main.GameUpdateCount / 80 % itemNameCycleColors.Length);
			Color color = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[(index + 1) % itemNameCycleColors.Length], fade);
			return Color.Lerp(color, lightColor, 0.2f);
		}


		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Color[] itemNameCycleColors = new Color[]
			{
				new Color(165, 0, 236),
				new Color(241, 159, 1),
				new Color(13, 107, 216),
				new Color(33, 184, 115),
				new Color(195, 41, 44),
				new Color(223, 230, 238),
				new Color(213, 142, 48),
				new Color(57, 57, 64),
			};
			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.Mod == "Terraria" && line2.Name == "ItemName")
				{
					float fade = Main.GameUpdateCount % 80 / 60f;
					int index = (int)(Main.GameUpdateCount / 80 % itemNameCycleColors.Length);
					line2.OverrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[(index + 1) % itemNameCycleColors.Length], fade);
				}
			}
		}
	}
}
