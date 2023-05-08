using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;

namespace TheDepths.Projectiles
{
	public class QuicksilverRocket : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(799);
			AIType = 799;
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Projectile.Kill();
            return true;
        }

        public override void Kill(int timeLeft)
		{
			Projectile.Resize(22, 22);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			Color transparent4 = Color.Transparent;
			int num861 = 35;
			for (int num862 = 0; num862 < 30; num862++)
			{
				Dust dust55 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31, 0f, 0f, 100, transparent4, 1.5f);
				Dust dust2 = dust55;
				dust2.velocity *= 1.4f;
			}
			for (int num863 = 0; num863 < 80; num863++)
			{
				Dust dust56 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, num861, 0f, 0f, 100, transparent4, 1.2f);
				Dust dust2 = dust56;
				dust2.velocity *= 7f;
				dust56 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, num861, 0f, 0f, 100, transparent4, 0.3f);
				dust2 = dust56;
				dust2.velocity *= 4f;
			}
			for (int num864 = 1; num864 <= 2; num864++)
			{
				for (int num865 = -1; num865 <= 1; num865 += 2)
				{
					for (int num866 = -1; num866 <= 1; num866 += 2)
					{
						Gore gore8 = Gore.NewGoreDirect(new EntitySource_Misc(""), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
						Gore gore2 = gore8;
						gore2.velocity *= ((num864 == 1) ? 0.4f : 0.8f);
						gore2 = gore8;
						gore2.velocity += new Vector2(num865, num866);
					}
				}
			}
			if (Main.netMode != 1 && (TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) < Main.maxTilesX / 2 || TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(Main.LocalPlayer.position.ToTileCoordinates().X) > Main.maxTilesX / 2) && Main.drunkWorld))
			{
				Point pt3 = base.Projectile.Center.ToTileCoordinates();
				Projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
		}
	}
}