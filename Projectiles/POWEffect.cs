using Terraria.Audio;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TheDepths.Projectiles
{
	public class POWEffect : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults() {
			Projectile.width = 94;
			Projectile.height = 72;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 40;
		}

        public override void AI()
        {
			if (++Projectile.frameCounter >= 6)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 2)
				{
					Projectile.frame = 0;
				}
			}
			if (Projectile.timeLeft < 12.5)
            {
				Projectile.alpha += 20;
            }

			if (Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<POWEffect>()] > 1)
            {
				Projectile.Kill();
            }
        }
    }
}