using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Projectiles
{
	public class QuicksilverSnowmanRocket : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(810);
			AIType = 810;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			Projectile.Resize(22, 22);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			Color transparent4 = Color.Transparent;
			int num861 = ModContent.DustType<QuicksilverBubble>();
			for (int num862 = 0; num862 < 30; num862++)
			{
				Dust dust55 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, transparent4, 1.5f);
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
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Point pt3 = Projectile.Center.ToTileCoordinates();
				Projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DepthsGlobalProj.SpreadQuicksilver);
			}
		}
	}
}