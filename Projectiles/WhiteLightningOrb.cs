using Microsoft.Xna.Framework;
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
            //Projectile.Name = "White Lightning Orb";
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

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (Main.projectile[i].type == ModContent.ProjectileType<WhiteLightningTo1>() && Main.projectile[i].Hitbox.Intersects(Projectile.Hitbox) && other.Center.DistanceSQ(Projectile.Center) < 19 * 19)
                {
                    Main.projectile[i].Kill();
                }
            }
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<WhiteLightningOrb2>()] > 0)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<WhiteLightningTo2>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<WhiteLightningOrb3>()] > 0)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<WhiteLightningTo3>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
            }
        }
	}

    public class WhiteLightningOrb2 : ModProjectile
    {
        public override string Texture => "TheDepths/Projectiles/WhiteLightningOrb";

        public override void SetDefaults()
        {
            //Projectile.Name = "White Lightning Orb";
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 10000;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 5)
                {
                    Projectile.frame = 0;
                }
            }
            if (Projectile.timeLeft <= 10)
            {
                Projectile.alpha += 25;
            }

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (Main.projectile[i].type == ModContent.ProjectileType<WhiteLightningTo2>() && Main.projectile[i].Hitbox.Intersects(Projectile.Hitbox) && other.Center.DistanceSQ(Projectile.Center) < 19 * 19)
                {
                    Main.projectile[i].Kill();
                }
            }
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<WhiteLightningOrb>()] > 0)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<WhiteLightningTo1>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<WhiteLightningOrb3>()] > 0)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<WhiteLightningTo3>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
            }
        }
    }

    public class WhiteLightningOrb3 : ModProjectile
    {
        public override string Texture => "TheDepths/Projectiles/WhiteLightningOrb";

        public override void SetDefaults()
        {
            //Projectile.Name = "White Lightning Orb";
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 10000;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 5)
                {
                    Projectile.frame = 0;
                }
            }
            if (Projectile.timeLeft <= 10)
            {
                Projectile.alpha += 25;
            }

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (Main.projectile[i].type == ModContent.ProjectileType<WhiteLightningTo3>() && Main.projectile[i].Hitbox.Intersects(Projectile.Hitbox) && other.Center.DistanceSQ(Projectile.Center) < 19 * 19)
                {
                    Main.projectile[i].Kill();
                }
            }
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<WhiteLightningOrb2>()] > 0)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<WhiteLightningTo2>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<WhiteLightningOrb>()] > 0)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<WhiteLightningTo1>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
            }
        }
    }
}
