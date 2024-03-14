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
using System.Collections.Generic;

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
			Player player = Main.player[Projectile.owner];
			
			Projectile.spriteDirection = -player.direction;
			if (Projectile.ai[0] == 1f) //Petting
			{
				if (Projectile.alpha > 0)
				{
					Projectile.alpha -= 3;
				}
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
				if (player.afkCounter != 0)
				{
					Projectile.position.X = player.position.X - Projectile.width / 2;
				}
			}
			else //Below the player
			{
				if (player.GetModPlayer<TheDepthsPlayer>().isSlamming)
				{
					if (Projectile.alpha > 0)
					{
						Projectile.alpha -= 20;
					}
				}
				Projectile.position = player.position - new Vector2(Projectile.width / 2, -(player.height / 2 + 20));
			}
			if ((player.afkCounter == 0 && Projectile.ai[0] == 1f) || (!player.GetModPlayer<TheDepthsPlayer>().isSlamming && Projectile.ai[0] == 0f))
			{
				if (Projectile.alpha <= 255)
				{
					Projectile.alpha += 20;
				}
				if (Projectile.alpha >= 255)
				{
					Projectile.Kill();
				}
			}
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
		}

		public override bool? CanCutTiles()
		{
			return false;
		}
	}
}