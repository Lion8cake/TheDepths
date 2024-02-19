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

namespace TheDepths.Projectiles.Summons
{
    public class MiniChasme : ModProjectile
	{
		int ChasmeleftArm = 0;
		int ChasmerightArm = 0;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
			ProjectileID.Sets.MinionSacrificable[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 20;
			Projectile.height = 22;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (Main.projectile[ChasmeleftArm].type != ModContent.ProjectileType<MiniChasmeHand>())
			{
				int handleft = Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.position, Vector2.Zero, ModContent.ProjectileType<MiniChasmeHand>(), Projectile.damage, 0f, Projectile.owner);
				(Main.projectile[handleft].ModProjectile as MiniChasmeHand).MiniChasmeOwner = Projectile.whoAmI;
				ChasmeleftArm = Main.projectile[handleft].whoAmI;
			}
			if (Main.projectile[ChasmerightArm].type != ModContent.ProjectileType<MiniChasmeHand>())
			{
				int handright = Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.position, Vector2.Zero, ModContent.ProjectileType<MiniChasmeHand>(), Projectile.damage, 0f, Projectile.owner);
				(Main.projectile[handright].ModProjectile as MiniChasmeHand).MiniChasmeOwner = Projectile.whoAmI;
				(Main.projectile[handright].ModProjectile as MiniChasmeHand).IsOffhand = true;
				ChasmerightArm = Main.projectile[handright].whoAmI;
			}
			float num = 0f;
			float num12 = 0f;
			float num23 = 20f;
			float num34 = 40f;
			float num45 = 0.69f;
			if (player.dead)
			{
				player.GetModPlayer<TheDepthsPlayer>().miniChasme = false;
			}
			if (player.GetModPlayer<TheDepthsPlayer>().miniChasme)
			{
				Projectile.timeLeft = 2;
			}			
			float num2 = 0.05f;
			float num3 = Projectile.width;
			for (int m = 0; m < 1000; m++)
			{
				if (m != Projectile.whoAmI && Main.projectile[m].active && Main.projectile[m].owner == Projectile.owner && Main.projectile[m].type == Projectile.type && Math.Abs(Projectile.position.X - Main.projectile[m].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[m].position.Y) < num3)
				{
					if (Projectile.position.X < Main.projectile[m].position.X)
					{
						Projectile.velocity.X -= num2;
					}
					else
					{
						Projectile.velocity.X += num2;
					}
					if (Projectile.position.Y < Main.projectile[m].position.Y)
					{
						Projectile.velocity.Y -= num2;
					}
					else
					{
						Projectile.velocity.Y += num2;
					}
				}
			}
			Vector2 vector = Projectile.position;
			float num4 = 2000f;
			bool flag = false;
			Projectile.tileCollide = true;
			NPC ownerMinionAttackTargetNPC2 = Projectile.OwnerMinionAttackTargetNPC;
			if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(Projectile))
			{
				float num9 = Vector2.Distance(ownerMinionAttackTargetNPC2.Center, Projectile.Center);
				float num10 = num4 * 3f;
				if (num9 < num10 && !flag && (Collision.CanHit(Projectile.Center, 1, 1, ownerMinionAttackTargetNPC2.Center, 1, 1)))
				{
					num4 = num9;
					vector = ownerMinionAttackTargetNPC2.Center;
					flag = true;
				}
			}
			if (!flag)
			{
				for (int num11 = 0; num11 < 200; num11++)
				{
					NPC nPC2 = Main.npc[num11];
					if (nPC2.CanBeChasedBy(Projectile))
					{
						float num13 = Vector2.Distance(nPC2.Center, Projectile.Center);
						if (!(num13 >= num4) && (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, nPC2.position, nPC2.width, nPC2.height)))
						{
							num4 = num13;
							vector = nPC2.Center;
							flag = true;
						}
					}
				}
			}
			int num14 = 500;
			if (flag)
			{
				num14 = 1000;
			}
			if (Vector2.Distance(player.Center, Projectile.Center) > (float)num14)
			{
				Projectile.ai[0] = 1f;
				Projectile.netUpdate = true;
			}
			if (Projectile.ai[0] == 1f)
			{
				Projectile.tileCollide = false;
			}
			bool flag2 = false;
			if (flag2)
			{
				if (Projectile.ai[0] <= 1f && Projectile.localAI[1] <= 0f)
				{
					Projectile.localAI[1] = -1f;
				}
				else
				{
					Projectile.localAI[1] = Utils.Clamp(Projectile.localAI[1] + 0.05f, 0f, 1f);
					if (Projectile.localAI[1] == 1f)
					{
						Projectile.localAI[1] = -1f;
					}
				}
			}
			bool flag3 = false;
			if (Projectile.ai[0] >= 2f)
			{
				Projectile.ai[0] += 1f;
				if (flag2)
				{
					Projectile.localAI[1] = Projectile.ai[0] / num34;
				}
				if (!flag)
				{
					Projectile.ai[0] += 1f;
				}
				if (Projectile.ai[0] > num34)
				{
					Projectile.ai[0] = 0f;
					Projectile.netUpdate = true;
				}
				Projectile.velocity *= num45;
			}
			else if (flag && (flag3 || Projectile.ai[0] == 0f))
			{
				Main.projectile[ChasmeleftArm].ai[2] = 20f;
				Main.projectile[ChasmerightArm].ai[2] = 20f;
				Vector2 v = vector - Projectile.Center;
				float num15 = v.Length();
				v = v.SafeNormalize(Vector2.Zero);
				if (num15 > 200f)
				{
					float num19 = 6f + num12 * num;
					v *= num19;
					float num20 = num23 * 2f;
					Projectile.velocity.X = (Projectile.velocity.X * num20 + v.X) / (num20 + 1f);
					Projectile.velocity.Y = (Projectile.velocity.Y * num20 + v.Y) / (num20 + 1f);
				}
				else if (Projectile.velocity.Y > -1f)
				{
					Projectile.velocity.Y -= 0.1f;
				}
			}
			else
			{
				if (!Collision.CanHitLine(Projectile.Center, 1, 1, player.Center, 1, 1))
				{
					Projectile.ai[0] = 1f;
				}
				float num25 = 6f;
				if (Projectile.ai[0] == 1f)
				{
					num25 = 15f;
				}
				Vector2 center2 = Projectile.Center;
				Vector2 v2 = player.Center - center2 + new Vector2(0f, -60f);
				float num28 = v2.Length();
				if (num28 > 200f && num25 < 9f)
				{
					num25 = 9f;
				}
				if (num28 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					Projectile.netUpdate = true;
				}
				if (num28 > 2000f)
				{
					Projectile.position.X = player.Center.X - (float)(Projectile.width / 2);
					Projectile.position.Y = player.Center.Y - (float)(Projectile.width / 2);
				}
				if (num28 > 70f)
				{
					v2 = v2.SafeNormalize(Vector2.Zero);
					v2 *= num25;
					Projectile.velocity = (Projectile.velocity * 20f + v2) / 21f;
				}
				else
				{
					if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
					{
						Projectile.velocity.X = -0.15f;
						Projectile.velocity.Y = -0.05f;
					}
					Projectile.velocity *= 1.01f;
				}
			}
			Projectile.rotation = Projectile.velocity.X * 0.05f;
			if (Projectile.velocity.X > 0f)
			{
				Projectile.spriteDirection = (Projectile.direction = -1);
			}
			else if (Projectile.velocity.X < 0f)
			{
				Projectile.spriteDirection = (Projectile.direction = 1);
			}
			if (Projectile.ai[1] > 0f)
			{
				Projectile.ai[1] += Main.rand.Next(1, 4);
			}
			int num39 = 90;
			if (Projectile.ai[1] > (float)num39)
			{
				Projectile.ai[1] = 0f;
				Projectile.netUpdate = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle(0, 0, 0, 0);
		}
	}
}
