using Terraria.Audio;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TheDepths.Items.Weapons;

namespace TheDepths.Projectiles
{
	public class ShadeBladeCalculator : ModProjectile
	{
		public override string Texture => "TheDepths/Projectiles/CrystalBall";
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Projectile.timeLeft < 10)
            {
				ShadeBlade.handsReturning = true;
			}
			Projectile.damage = 0;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}
	}
}