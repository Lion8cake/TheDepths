using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.WorldBuilding;
using TheDepths;
using TheDepths.Tiles;
using TheDepths.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheDepths.Dusts;
using TheDepths.Projectiles.Summons;
using TheDepths.Buffs;
using Terraria.DataStructures;
using Microsoft.CodeAnalysis;
using static Humanizer.In;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace TheDepths.Projectiles.Summons
{
    public class MiniChasmeHand : ModProjectile
	{
		public int MiniChasmeOwner;

		public bool IsOffhand = false;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.TrailingMode[Type] = 0;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
			ProjectileID.Sets.MinionSacrificable[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 12;
			Projectile.aiStyle = -1;
			Projectile.netImportant = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 12;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			int num136 = 0;
			int num137 = 0;
			float num138 = (float)(TextureAssets.Projectile[Projectile.type].Width() - Projectile.width) * 0.5f + (float)Projectile.width * 0.5f;
			SpriteEffects dir = (SpriteEffects)0;
			if (Projectile.spriteDirection == -1)
			{
				dir = (SpriteEffects)1;
			}
			Texture2D value110 = TextureAssets.Projectile[Projectile.type].Value;
			int num401 = value110.Height / Main.projFrames[Projectile.type];
			int y26 = num401 * Projectile.frame;
			int num402 = 0;
			int num403 = 0;
			if (Projectile.ai[0] == 2f)
			{
				num402 = 10;
				num403 = 1;
			}
			else
			{
				num402 = 3;
				num403 = 1;
			}
			for (int num404 = 1; num404 < num402; num404 += num403)
			{
				_ = ref Projectile.oldPos[num404];
				Color newColor4 = lightColor;
				newColor4 = Projectile.GetAlpha(newColor4);
				newColor4 *= (float)(num402 - num404) / 15f;
				Vector2 position25 = Projectile.oldPos[num404] - Main.screenPosition + new Vector2(num138 + (float)num137, (float)(Projectile.height / 2) + Projectile.gfxOffY);
				Main.EntitySpriteDraw(value110, position25, (Rectangle?)new Rectangle(0, y26, value110.Width, num401), newColor4, Projectile.rotation, new Vector2(num138, (float)(Projectile.height / 2 + num136)), Projectile.scale, dir, 0f);
			}
			Main.EntitySpriteDraw(value110, Projectile.position - Main.screenPosition + new Vector2(num138 + (float)num137, (float)(Projectile.height / 2) + Projectile.gfxOffY), (Rectangle?)new Rectangle(0, y26, value110.Width, num401), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(num138, (float)(Projectile.height / 2 + num136)), Projectile.scale, dir, 0f);
			return false;
		}

		public override void AI()
		{
			if (Main.projectile[MiniChasmeOwner].type != ModContent.ProjectileType<MiniChasme>())
			{
				Projectile.Kill();
				return;
			}
			if (Main.player[Projectile.owner].dead)
			{
				Main.player[Projectile.owner].GetModPlayer<TheDepthsPlayer>().miniChasme = false;
			}
			if (Main.player[Projectile.owner].GetModPlayer<TheDepthsPlayer>().miniChasme)
			{
				Projectile.timeLeft = 2;
			}
			Projectile miniChasme = Main.projectile[MiniChasmeOwner];
			if (Projectile.ai[2] > 0)
			{
				Projectile.ai[2]--;
			}
			if (Projectile.ai[2] == 0)
			{
				int handwidthpos = (miniChasme.direction == 1 ? 20 : -20);
				Projectile.position = miniChasme.position + (IsOffhand ? new Vector2(handwidthpos, 20) : new Vector2(handwidthpos, 0));
				Projectile.spriteDirection = miniChasme.spriteDirection;
				Projectile.rotation = 0f;
			}
			else
			{
				float num583 = 2000f;
				float num584 = 800f;
				float num585 = 1200f;
				float num586 = 150f;
				float num587 = 0.05f;
				for (int num589 = 0; num589 < 1000; num589++)
				{
					bool flag20 = true;
					if (num589 != Projectile.whoAmI && Main.projectile[num589].active && Main.projectile[num589].owner == Projectile.owner && flag20 && Math.Abs(Projectile.position.X - Main.projectile[num589].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[num589].position.Y) < (float)Projectile.width)
					{
						if (Projectile.position.X < Main.projectile[num589].position.X)
						{
							Projectile.velocity.X -= num587;
						}
						else
						{
							Projectile.velocity.X += num587;
						}
						if (Projectile.position.Y < Main.projectile[num589].position.Y)
						{
							Projectile.velocity.Y -= num587;
						}
						else
						{
							Projectile.velocity.Y += num587;
						}
					}
				}
				bool flag21 = false;
				if (Projectile.ai[0] == 2f)
				{
					Projectile.ai[1]++;
					Projectile.extraUpdates = 1;
					Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI;
					if (Projectile.ai[1] > 40f)
					{
						Projectile.ai[1] = 1f;
						Projectile.ai[0] = 0f;
						Projectile.extraUpdates = 0;
						Projectile.numUpdates = 0;
						Projectile.netUpdate = true;
					}
					else
					{
						flag21 = true;
					}
				}
				if (flag21)
				{
					return;
				}
				Vector2 center16 = Projectile.position;
				Vector2 zero = Vector2.Zero;
				bool flag22 = false;
				if (Projectile.ai[0] != 1f)
				{
					Projectile.tileCollide = true;
				}
				if (Projectile.tileCollide && WorldGen.SolidTile(Framing.GetTileSafely((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16)))
				{
					Projectile.tileCollide = false;
				}
				NPC ownerMinionAttackTargetNPC3 = miniChasme.OwnerMinionAttackTargetNPC;
				if (ownerMinionAttackTargetNPC3 != null && ownerMinionAttackTargetNPC3.CanBeChasedBy(Projectile))
				{
					float num596 = Vector2.Distance(ownerMinionAttackTargetNPC3.Center, Projectile.Center);
					float num597 = num583 * 3f;
					if (num596 < num597 && !flag22 && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, ownerMinionAttackTargetNPC3.position, ownerMinionAttackTargetNPC3.width, ownerMinionAttackTargetNPC3.height))
					{
						num583 = num596;
						center16 = ownerMinionAttackTargetNPC3.Center;
						flag22 = true;
					}
				}
				if (!flag22)
				{
					for (int num598 = 0; num598 < 200; num598++)
					{
						NPC nPC15 = Main.npc[num598];
						if (nPC15.CanBeChasedBy(Projectile))
						{
							float num600 = Vector2.Distance(nPC15.Center, Projectile.Center);
							if (!(num600 >= num583) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, nPC15.position, nPC15.width, nPC15.height))
							{
								num583 = num600;
								center16 = nPC15.Center;
								zero = nPC15.velocity;
								flag22 = true;
							}
						}
					}
				}
				float num601 = num584;
				if (flag22)
				{
					num601 = num585;
				}
				Player player7 = Main.player[Projectile.owner];
				if (Vector2.Distance(player7.Center, Projectile.Center) > num601)
				{
					Projectile.ai[0] = 1f;
					Projectile.tileCollide = false;
					Projectile.netUpdate = true;
				}
				if (flag22 && Projectile.ai[0] == 0f)
				{
					Vector2 vector117 = center16 - Projectile.Center;
					float num602 = vector117.Length();
					vector117.Normalize();
					if (num602 > 200f)
					{
						float num603 = 14f;
						vector117 *= num603;
						Projectile.velocity = (Projectile.velocity * 40f + vector117) / 41f;
					}
					else
					{
						float num604 = 4f;
						vector117 *= 0f - num604;
						Projectile.velocity = (Projectile.velocity * 40f + vector117) / 41f;
					}
				}
				else
				{
					bool flag24 = false;
					if (!flag24)
					{
						flag24 = Projectile.ai[0] == 1f;
					}
					float num605 = 6f;
					float num606 = 40f;
					if (flag24)
					{
						num605 = 15f;
					}
					Vector2 center17 = Projectile.Center;
					Vector2 vector118 = player7.Center - center17 + new Vector2(0f, -60f);
					float num607 = vector118.Length();
					float num608 = num607;
					if (num607 > 200f && num605 < 8f)
					{
						num605 = 8f;
					}
					if (num605 < Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y))
					{
						num606 = 30f;
						num605 = Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y);
						if (num607 > 200f)
						{
							num606 = 20f;
							num605 += 4f;
						}
						else if (num607 > 100f)
						{
							num605 += 3f;
						}
					}
					if (flag24 && num607 > 300f)
					{
						num605 += 6f;
						num606 -= 10f;
					}
					if (num607 < num586 && flag24 && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					{
						Projectile.ai[0] = 0f;
						Projectile.netUpdate = true;
					}
					if (num607 > 2000f)
					{
						Projectile.position.X = Main.player[Projectile.owner].Center.X - (float)(Projectile.width / 2);
						Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (float)(Projectile.height / 2);
						Projectile.netUpdate = true;
					}
					if (num607 > 70f)
					{
						Vector2 vector119 = vector118;
						vector118.Normalize();
						vector118 *= num605;
						Projectile.velocity = (Projectile.velocity * num606 + vector118) / (num606 + 1f);
					}
					else if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
					{
						Projectile.velocity.X = -0.15f;
						Projectile.velocity.Y = -0.05f;
					}
					if (Projectile.velocity.Length() > num605)
					{
						Projectile.velocity *= 0.95f;
					}
				}
				Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI;
				if (Projectile.ai[1] > 0f)
				{
					Projectile.ai[1] += Main.rand.Next(1, 4);
				}
				if (Projectile.ai[1] > 40f)
				{
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
				if (Projectile.ai[0] == 0f)
				{
					if (Projectile.ai[1] == 0f && flag22 && num583 < 500f)
					{
						Projectile.ai[1]++;
						if (Main.myPlayer == Projectile.owner)
						{
							Projectile.ai[0] = 2f;
							Vector2 v4 = center16 - Projectile.Center;
							v4 = v4.SafeNormalize(Projectile.velocity);
							float num618 = 6f;
							Projectile.velocity = v4 * num618;
							TryInterceptingTarget(center16, zero, num618);
							Projectile.netUpdate = true;
						}
					}
				}
			}
		}

		private void TryInterceptingTarget(Vector2 targetDir, Vector2 targetVelocity, float speed)
		{
			float num = 5f;
			float num2 = 30f;
			float num3 = num2 + num;
			int num5 = 4;
			int num6 = 2;
			bool flag = false;
			int num4 = 2;
			num2 = 40f;
			targetVelocity /= (float)num4;
			for (float num7 = 1f; num7 <= 1.5f; num7 += 0.1f)
			{
				Utils.ChaseResults chaseResults = Utils.GetChaseResults(Projectile.Center, speed, targetDir, targetVelocity);
				if (chaseResults.InterceptionHappens && chaseResults.InterceptionTime <= num3)
				{
					Projectile.velocity = chaseResults.ChaserVelocity;
					if (flag)
					{
						int num8 = (int)Utils.Clamp((float)Math.Ceiling(chaseResults.InterceptionTime) + (float)num6, num5, num2 - 1f) / num4;
						float num9 = num2 / (float)num4 - (float)num8;
						Projectile.ai[1] += num9 * (float)num4;
					}
					break;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
	}
}
