using Terraria.Audio;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TheDepths.Items.Weapons;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Humanizer;
using static Humanizer.On;

namespace TheDepths.Projectiles
{
	public class ShalestoneHand : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 70;
			Projectile.height = 60;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.timeLeft = 5000;
		}

        public override void AI()
		{
			//AI[0] is different states
			//AI[1] is petting timer
			//AI[2] unsued
			//AI[3] unused
			Player player = Main.player[Projectile.owner];
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 3;
			}
			if (Projectile.ai[0] == 1f) //Petting
			{
				Projectile.position.X = player.position.X - Projectile.width / 2;
				if (Projectile.alpha <= 0)
				{
					Projectile.ai[1]++;
					if (Projectile.ai[1] < 20)
					{
						Projectile.position.Y -= MathHelper.Lerp(0.1f, 1f, Projectile.ai[1] / 20);
					}
					else if (Projectile.ai[1] < 40)
					{
						Projectile.position.Y += MathHelper.Lerp(0.1f, 1f, Projectile.ai[1] / 40);
					}
					else if (Projectile.ai[1] < 60)
					{
						Projectile.position.Y -= MathHelper.Lerp(0.1f, 1f, Projectile.ai[1] / 60);
					}
					else if (Projectile.ai[1] < 80)
					{
						Projectile.position.Y += MathHelper.Lerp(0.1f, 1f, Projectile.ai[1] / 80);
					}
					else if (Projectile.ai[1] < 100)
					{
						Projectile.position.Y -= MathHelper.Lerp(0.1f, 1f, Projectile.ai[1] / 100);
					}
					else if (Projectile.ai[1] < 120)
					{
						Projectile.position.Y += MathHelper.Lerp(0.1f, 1f, Projectile.ai[1] / 120);
					}
				}
				if (Projectile.ai[1] >= 120)
				{
					if (Projectile.alpha <= 255)
					{
						Projectile.alpha += 6;
					}
					if (Projectile.alpha >= 255)
					{
						Projectile.Kill();
					}
				}
			}
			else //Below the player
			{
				Projectile.position = player.position;
			}
		}

		public override bool? CanCutTiles()
		{
			return false;
		}
	}
}