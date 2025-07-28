using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using static Humanizer.In;
using TheDepths.Tiles.Trees;
using TheDepths.NPCs;
using TheDepths.Dusts;
using TheDepths.Liquids;
using ModLiquidLib.ModLoader;

namespace TheDepths.Projectiles
{
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
					flag34 = Main.netMode != NetmodeID.MultiplayerClient;
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

		public static bool SpreadQuicksilver(int x, int y)
		{
			if (Vector2.Distance(DelegateMethods.v2_1, new Vector2((float)x, (float)y)) > DelegateMethods.f_1)
			{
				return false;
			}
			if (WorldGen.PlaceLiquid(x, y, (byte)LiquidLoader.LiquidType<Quicksilver>(), byte.MaxValue))
			{
				Vector2 position = new((float)(x * 16), (float)(y * 16));
				int type = ModContent.DustType<QuicksilverBubble>();
				for (int i = 0; i < 3; i++)
				{
					Dust dust = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 1.2f);
					dust.velocity *= 7f;
					Dust dust2 = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 0.8f);
					dust2.velocity *= 4f;
				}
				return true;
			}
			return false;
		}
	}
}
