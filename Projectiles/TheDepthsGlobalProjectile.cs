using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TheDepths.Projectiles
{
	public class TheDepthsGlobalProjectile : GlobalProjectile
	{
		public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
		{
			target.GetModPlayer<TheDepthsPlayer>().noHit = true;
		}
	}

	//Attempted to remove lava dispencement from lava projectiles but due to how projectile kill works in vanilla this will never override it. Ill have to IL edit someone in the future for this
	public class LavaBombGlobal : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return entity.type == ProjectileID.LavaBomb;
		}

		public override void Kill(Projectile projectile, int timeLeft)
		{
			projectile.Resize(22, 22);
			SoundEngine.PlaySound(SoundID.Item14, projectile.position);
			Color transparent4 = Color.Transparent;
			int num861 = 35;
			for (int num862 = 0; num862 < 30; num862++)
			{
				Dust dust55 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, transparent4, 1.5f);
				Dust dust2 = dust55;
				dust2.velocity *= 1.4f;
			}
			for (int num863 = 0; num863 < 80; num863++)
			{
				Dust dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num861, 0f, 0f, 100, transparent4, 1.2f);
				Dust dust2 = dust56;
				dust2.velocity *= 7f;
				dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num861, 0f, 0f, 100, transparent4, 0.3f);
				dust2 = dust56;
				dust2.velocity *= 4f;
			}
			for (int num864 = 1; num864 <= 2; num864++)
			{
				for (int num865 = -1; num865 <= 1; num865 += 2)
				{
					for (int num866 = -1; num866 <= 1; num866 += 2)
					{
						Gore gore8 = Gore.NewGoreDirect(new EntitySource_Misc(""), projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
						Gore gore2 = gore8;
						gore2.velocity *= ((num864 == 1) ? 0.4f : 0.8f);
						gore2 = gore8;
						gore2.velocity += new Vector2(num865, num866);
					}
				}
			}
			if (Main.netMode != 1 && Worldgen.TheDepthsWorldGen.depthsorHell == false)
			{
				Point pt3 = projectile.Center.ToTileCoordinates();
				projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
		}
	}

	public class LavaGrenadeGlobal : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return entity.type == ProjectileID.LavaGrenade;
		}

		public override void Kill(Projectile projectile, int timeLeft)
		{
			projectile.Resize(22, 22);
			SoundEngine.PlaySound(SoundID.Item62, projectile.position);
			Color transparent4 = Color.Transparent;
			int num861 = 35;
			for (int num862 = 0; num862 < 30; num862++)
			{
				Dust dust55 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, transparent4, 1.5f);
				Dust dust2 = dust55;
				dust2.velocity *= 1.4f;
			}
			for (int num863 = 0; num863 < 80; num863++)
			{
				Dust dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num861, 0f, 0f, 100, transparent4, 1.2f);
				Dust dust2 = dust56;
				dust2.velocity *= 7f;
				dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num861, 0f, 0f, 100, transparent4, 0.3f);
				dust2 = dust56;
				dust2.velocity *= 4f;
			}
			for (int num864 = 1; num864 <= 2; num864++)
			{
				for (int num865 = -1; num865 <= 1; num865 += 2)
				{
					for (int num866 = -1; num866 <= 1; num866 += 2)
					{
						Gore gore8 = Gore.NewGoreDirect(new EntitySource_Misc(""), projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
						Gore gore2 = gore8;
						gore2.velocity *= ((num864 == 1) ? 0.4f : 0.8f);
						gore2 = gore8;
						gore2.velocity += new Vector2(num865, num866);
					}
				}
			}
			if (Main.netMode != 1 && Worldgen.TheDepthsWorldGen.depthsorHell == false)
			{
				Point pt3 = projectile.Center.ToTileCoordinates();
				projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
		}
	}

	public class LavaMineGlobal : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return entity.type == ProjectileID.LavaMine;
		}

		public override void Kill(Projectile projectile, int timeLeft)
		{
			projectile.Resize(22, 22);
			SoundEngine.PlaySound(SoundID.Item14, projectile.position);
			Color transparent4 = Color.Transparent;
			int num861 = 35;
			for (int num862 = 0; num862 < 30; num862++)
			{
				Dust dust55 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, transparent4, 1.5f);
				Dust dust2 = dust55;
				dust2.velocity *= 1.4f;
			}
			for (int num863 = 0; num863 < 80; num863++)
			{
				Dust dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num861, 0f, 0f, 100, transparent4, 1.2f);
				Dust dust2 = dust56;
				dust2.velocity *= 7f;
				dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num861, 0f, 0f, 100, transparent4, 0.3f);
				dust2 = dust56;
				dust2.velocity *= 4f;
			}
			for (int num864 = 1; num864 <= 2; num864++)
			{
				for (int num865 = -1; num865 <= 1; num865 += 2)
				{
					for (int num866 = -1; num866 <= 1; num866 += 2)
					{
						Gore gore8 = Gore.NewGoreDirect(new EntitySource_Misc(""), projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
						Gore gore2 = gore8;
						gore2.velocity *= ((num864 == 1) ? 0.4f : 0.8f);
						gore2 = gore8;
						gore2.velocity += new Vector2(num865, num866);
					}
				}
			}
			if (Main.netMode != 1 && Worldgen.TheDepthsWorldGen.depthsorHell == false)
			{
				Point pt3 = projectile.Center.ToTileCoordinates();
				projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
		}
	}

	public class LavaRocketGlobal : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return entity.type == ProjectileID.LavaRocket;
		}

		public override void Kill(Projectile projectile, int timeLeft)
		{
			projectile.Resize(22, 22);
			SoundEngine.PlaySound(SoundID.Item14, projectile.position);
			Color transparent4 = Color.Transparent;
			int num861 = 35;
			for (int num862 = 0; num862 < 30; num862++)
			{
				Dust dust55 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, transparent4, 1.5f);
				Dust dust2 = dust55;
				dust2.velocity *= 1.4f;
			}
			for (int num863 = 0; num863 < 80; num863++)
			{
				Dust dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num861, 0f, 0f, 100, transparent4, 1.2f);
				Dust dust2 = dust56;
				dust2.velocity *= 7f;
				dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num861, 0f, 0f, 100, transparent4, 0.3f);
				dust2 = dust56;
				dust2.velocity *= 4f;
			}
			for (int num864 = 1; num864 <= 2; num864++)
			{
				for (int num865 = -1; num865 <= 1; num865 += 2)
				{
					for (int num866 = -1; num866 <= 1; num866 += 2)
					{
						Gore gore8 = Gore.NewGoreDirect(new EntitySource_Misc(""), projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
						Gore gore2 = gore8;
						gore2.velocity *= ((num864 == 1) ? 0.4f : 0.8f);
						gore2 = gore8;
						gore2.velocity += new Vector2(num865, num866);
					}
				}
			}
			if (Main.netMode != 1 && Worldgen.TheDepthsWorldGen.depthsorHell == false)
			{
				Point pt3 = projectile.Center.ToTileCoordinates();
				projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
		}
	}

	public class LavaSnowmanRocketGlobal : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return entity.type == ProjectileID.LavaSnowmanRocket;
		}

		public override void Kill(Projectile projectile, int timeLeft)
		{
			projectile.Resize(22, 22);
			SoundEngine.PlaySound(SoundID.Item14, projectile.position);
			Color transparent4 = Color.Transparent;
			int num861 = 35;
			for (int num862 = 0; num862 < 30; num862++)
			{
				Dust dust55 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, transparent4, 1.5f);
				Dust dust2 = dust55;
				dust2.velocity *= 1.4f;
			}
			for (int num863 = 0; num863 < 80; num863++)
			{
				Dust dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num861, 0f, 0f, 100, transparent4, 1.2f);
				Dust dust2 = dust56;
				dust2.velocity *= 7f;
				dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num861, 0f, 0f, 100, transparent4, 0.3f);
				dust2 = dust56;
				dust2.velocity *= 4f;
			}
			for (int num864 = 1; num864 <= 2; num864++)
			{
				for (int num865 = -1; num865 <= 1; num865 += 2)
				{
					for (int num866 = -1; num866 <= 1; num866 += 2)
					{
						Gore gore8 = Gore.NewGoreDirect(new EntitySource_Misc(""), projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
						Gore gore2 = gore8;
						gore2.velocity *= ((num864 == 1) ? 0.4f : 0.8f);
						gore2 = gore8;
						gore2.velocity += new Vector2(num865, num866);
					}
				}
			}
			if (Main.netMode != 1 && Worldgen.TheDepthsWorldGen.depthsorHell == false)
			{
				Point pt3 = projectile.Center.ToTileCoordinates();
				projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
		}
	}
}
