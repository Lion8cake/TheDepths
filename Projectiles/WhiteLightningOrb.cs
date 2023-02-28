/*using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace TheDepths.Projectiles
{
    public class WhiteLightningOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Name = "White Lightning Orb";
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
			Projectile.penetrate = 10000;
		}
		
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 5;
		}

        public override void AI()
        {			
			if (++Projectile.frameCounter >= 5) {
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 5) {
					Projectile.frame = 0;
				}
			}
            if (Projectile.timeLeft <= 10)
            {
                Projectile.alpha += 25;
            }
        }
	}
}*/
