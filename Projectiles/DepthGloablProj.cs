using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using static Humanizer.In;
using TheDepths.Tiles.Trees;
using TheDepths.NPCs;

namespace TheDepths.Projectiles
{
    public class LavaBomb : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ProjectileID.LavaBomb;
        }

        public override bool PreKill(Projectile projectile, int timeLeft)
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
			if (Main.netMode != NetmodeID.MultiplayerClient && !Worldgen.TheDepthsWorldGen.InDepths(Main.player[projectile.owner]))
			{
				Point pt3 = projectile.Center.ToTileCoordinates();
				projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
			return false;
        }
	}

	public class LavaGrenade : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return entity.type == ProjectileID.LavaGrenade;
		}

		public override bool PreKill(Projectile projectile, int timeLeft)
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
			if (Main.netMode != NetmodeID.MultiplayerClient && !Worldgen.TheDepthsWorldGen.InDepths(Main.player[projectile.owner]))
			{
				Point pt3 = projectile.Center.ToTileCoordinates();
				projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
			return false;
		}
	}

	public class LavaMine : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return entity.type == ProjectileID.LavaMine;
		}

		public override bool PreKill(Projectile projectile, int timeLeft)
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
			if (Main.netMode != NetmodeID.MultiplayerClient && !Worldgen.TheDepthsWorldGen.InDepths(Main.player[projectile.owner]))
			{
				Point pt3 = projectile.Center.ToTileCoordinates();
				projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
			return false;
		}
	}

	public class LavaRocket : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return entity.type == ProjectileID.LavaRocket;
		}

		public override bool PreKill(Projectile projectile, int timeLeft)
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
			if (Main.netMode != NetmodeID.MultiplayerClient && !Worldgen.TheDepthsWorldGen.InDepths(Main.player[projectile.owner]))
			{
				Point pt3 = projectile.Center.ToTileCoordinates();
				projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
			return false;
		}
	}

	public class LavaSnowmanRocket : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return entity.type == ProjectileID.LavaSnowmanRocket;
		}

		public override bool PreKill(Projectile projectile, int timeLeft)
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
			if (Main.netMode != NetmodeID.MultiplayerClient && !Worldgen.TheDepthsWorldGen.InDepths(Main.player[projectile.owner]))
			{
				Point pt3 = projectile.Center.ToTileCoordinates();
				projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(pt3, 3f, DelegateMethods.SpreadLava);
			}
			return false;
		}
	}

	public class DepthsGlobalProj : GlobalProjectile
	{
		public override void AI(Projectile projectile)
		{
			if (projectile.aiStyle == 6)
			{
				bool flag23 = projectile.type == 1019;
				bool flag34 = Main.myPlayer == projectile.owner;
				if (flag23)
				{
					flag34 = Main.netMode != 1;
				}
				if (flag34 && (flag23))
				{
					int num988 = (int)(projectile.position.X / 16f) - 1;
					int num999 = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
					int num1010 = (int)(projectile.position.Y / 16f) - 1;
					int num1021 = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;
					if (num988 < 0)
					{
						num988 = 0;
					}
					if (num999 > Main.maxTilesX)
					{
						num999 = Main.maxTilesX;
					}
					if (num1010 < 0)
					{
						num1010 = 0;
					}
					if (num1021 > Main.maxTilesY)
					{
						num1021 = Main.maxTilesY;
					}
					Vector2 vector57 = default(Vector2);
					for (int num1032 = num988; num1032 < num999; num1032++)
					{
						for (int num1043 = num1010; num1043 < num1021; num1043++)
						{
							vector57.X = num1032 * 16;
							vector57.Y = num1043 * 16;
							if (!(projectile.position.X + (float)projectile.width > vector57.X) || !(projectile.position.X < vector57.X + 16f) || !(projectile.position.Y + (float)projectile.height > vector57.Y) || !(projectile.position.Y < vector57.Y + 16f) || !Main.tile[num1032, num1043].HasTile)
							{
								continue;
							}
							Tile tile = Main.tile[num1032, num1043];
							if (tile.TileType == ModContent.TileType<OnyxGemtreeSapling>())
							{
								if (Main.remixWorld && num1043 >= (int)Main.worldSurface - 1 && num1043 < Main.maxTilesY - 20)
								{
									OnyxGemtreeSapling.AttemptToGrowOnyxFromSapling(num1032, num1043, underground: false);
								}
								OnyxGemtreeSapling.AttemptToGrowOnyxFromSapling(num1032, num1043, num1043 > (int)Main.worldSurface - 1);
							}
							if (tile.TileType == ModContent.TileType<PetrifiedSapling>())
							{
								if (Main.remixWorld && num1043 >= (int)Main.worldSurface - 1 && num1043 < Main.maxTilesY - 20)
								{
									PetrifiedSapling.AttemptToGrowPetrifiedFromSapling(num1032, num1043);
								}
								PetrifiedSapling.AttemptToGrowPetrifiedFromSapling(num1032, num1043);
							}
							if (tile.TileType == ModContent.TileType<NightSapling>())
							{
								if (Main.remixWorld && num1043 >= (int)Main.worldSurface - 1 && num1043 < Main.maxTilesY - 20)
								{
									NightSapling.AttemptToGrowNightmareFromSapling(num1032, num1043);
								}
								NightSapling.AttemptToGrowNightmareFromSapling(num1032, num1043);
							}
						}
					}
				}
			}
		}

		public override void PostAI(Projectile projectile)
		{
			if (projectile.type == ProjectileID.PurificationPowder && Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int n = 0; n < Main.maxNPCs; n++)
				{
					NPC nPC2 = Main.npc[n];
					if (!nPC2.active)
					{
						continue;
					}
					if (nPC2.type == ModContent.NPCType<CrystalBoundTaxCollector>())
					{
						if (projectile.Hitbox.Intersects(nPC2.Hitbox))
						{
							nPC2.Transform(441);
						}
					}
				}
			}
		}
	}
}
