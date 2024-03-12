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

namespace TheDepths.Projectiles
{
	public class ShalestoneHand : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 70;
			Projectile.height = 60;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300;
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
				Projectile.alpha -= 4;
			}
			if (Projectile.ai[0] == 1f) //Petting
			{
				Projectile.ai[1]++;
				Projectile.position.X = player.position.X - Projectile.width / 2;
				if (Projectile.alpha <= 0)
				{
					Projectile.velocity.Y = -4f;
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