using Terraria.Audio;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.CodeAnalysis;
using static Humanizer.In;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;

namespace TheDepths.Projectiles
{
	public class SapphireSerpentKite : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Type] = 2;
			ProjectileID.Sets.TrailCacheLength[Type] = 80;
		}

		public override void SetDefaults() {
			Projectile.netImportant = true;
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 60;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.width = Projectile.width;
			Projectile.height = 34;
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D value2 = TextureAssets.Extra[103].Value;
			int num = 15;
			float num12 = 0f;
			int num23 = 10;
			int num33 = 5;
			float num34 = 10f;
			float num35 = 0f;
			int num36 = -14;
			int num37 = -2;
			int num38 = -1;
			int num2 = -1;
			int num3 = 8;
			int num4 = 0;
			int num5 = 1;
			int num6 = 0;
			int num7 = 0;
			bool flag = true;
			bool flag2 = false;

			value2 = ModContent.Request<Texture2D>(Texture + "_Tail").Value;
			num = 7;
			num4 = 7;
			num12 = ((Projectile.spriteDirection == 1) ? ((float)Math.PI / 2f) : (-(float)Math.PI / 2f));
			num23 = 7;
			num34 = 32f;
			num35 += (float)Math.PI / 12f * (float)Projectile.spriteDirection;
			num36 = -20;
			num37 = -6;
			num3 = 16;
				
			SpriteEffects effects = ((SpriteEffects)(Projectile.spriteDirection != 1 ? 1 : 0));
			Rectangle rectangle = value.Frame(Main.projFrames[Projectile.type], 1, Projectile.frame);
			Vector2 origin = rectangle.Size() / 2f;
			Vector2 position = Projectile.Center - Main.screenPosition;
			Color color = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
			Color alpha = Projectile.GetAlpha(color);
			Texture2D value3 = TextureAssets.FishingLine.Value;
			Rectangle value4 = value3.Frame();
			Vector2 origin2 = new((float)(value4.Width / 2), 2f);
			Rectangle rectangle2 = value2.Frame(num);
			int width = rectangle2.Width;
			rectangle2.Width -= 2;
			Vector2 origin3 = rectangle2.Size() / 2f;
			rectangle2.X = width * (num - 1);
			Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);
			Vector2 center = Projectile.Center;
			Vector2.Distance(center, playerArmPosition);
			float num8 = 12f;
			_ = (playerArmPosition - center).SafeNormalize(Vector2.Zero) * num8;
			Vector2 vector = playerArmPosition;
			Vector2 vector3 = center - vector;
			Vector2 velocity = Projectile.velocity;
			if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
			{
				Utils.Swap(ref velocity.X, ref velocity.Y);
			}
			float num9 = vector3.Length();
			float num10 = 16f;
			float num11 = 80f;
			bool flag3 = true;
			if (num9 == 0f)
			{
				flag3 = false;
			}
			else
			{
				vector3 *= 12f / num9;
				vector -= vector3;
				vector3 = center - vector;
			}
			while (flag3)
			{
				float num13 = 12f;
				float num14 = vector3.Length();
				float num15 = num14;
				if (float.IsNaN(num14) || num14 == 0f)
				{
					flag3 = false;
					continue;
				}
				if (num14 < 20f)
				{
					num13 = num14 - 8f;
					flag3 = false;
				}
				num14 = 12f / num14;
				vector3 *= num14;
				vector += vector3;
				vector3 = center - vector;
				if (num15 > 12f)
				{
					float num16 = 0.3f;
					float num17 = Math.Abs(velocity.X) + Math.Abs(velocity.Y);
					if (num17 > num10)
					{
						num17 = num10;
					}
					num17 = 1f - num17 / num10;
					num16 *= num17;
					num17 = num15 / num11;
					if (num17 > 1f)
					{
						num17 = 1f;
					}
					num16 *= num17;
					if (num16 < 0f)
					{
						num16 = 0f;
					}
					num17 = 1f;
					num16 *= num17;
					if (vector3.Y > 0f)
					{
						vector3.Y *= 1f + num16;
						vector3.X *= 1f - num16;
					}
					else
					{
						num17 = Math.Abs(velocity.X) / 3f;
						if (num17 > 1f)
						{
							num17 = 1f;
						}
						num17 -= 0.5f;
						num16 *= num17;
						if (num16 > 0f)
						{
							num16 *= 2f;
						}
						vector3.Y *= 1f + num16;
						vector3.X *= 1f - num16;
					}
				}
				float rotation = vector3.ToRotation() - (float)Math.PI / 2f;
				if (!flag3)
				{
					value4.Height = (int)num13;
				}
				Color color2 = Lighting.GetColor(center.ToTileCoordinates());
				Main.EntitySpriteDraw(value3, vector - Main.screenPosition, value4, color2, rotation, origin2, 1f, (SpriteEffects)0);
			}
			Vector2 vector4 = Projectile.Size / 2f;
			float num18 = Math.Abs(Main.WindForVisuals);
			float num19 = MathHelper.Lerp(0.5f, 1f, num18);
			float num20 = num18;
			if (vector3.Y >= -0.02f && vector3.Y < 1f)
			{
				num20 = Utils.GetLerpValue(0.2f, 0.5f, num18, clamped: true);
			}
			int num21 = num33;
			int num22 = num23 + 1;
			for (int i = 0; i < num5; i++)
			{
				rectangle2.X = width * (num - 1);
				List<Vector2> list = new List<Vector2>();
				Vector2 vector5 = new Vector2(num19 * (float)num3 * (float)Projectile.spriteDirection, (float)Math.Sin(Main.timeForVisualEffects / 300.0 * 6.2831854820251465) * num20) * 2f;
				float num24 = num36 + num6;
				float num25 = num37 + num7;
				switch (i)
				{
					case 1:
						vector5 = new Vector2(num19 * (float)num3 * (float)Projectile.spriteDirection, (float)Math.Sin(Main.timeForVisualEffects / 300.0 * 6.2831854820251465) * num20 + 0.5f) * 2f;
						num24 -= 8f;
						num25 -= 8f;
						break;
					case 2:
						vector5 = new Vector2(num19 * (float)num3 * (float)Projectile.spriteDirection, (float)Math.Sin(Main.timeForVisualEffects / 300.0 * 6.2831854820251465) * num20 + 1f) * 2f;
						num24 -= 4f;
						num25 -= 4f;
						break;
					case 3:
						vector5 = new Vector2(num19 * (float)num3 * (float)Projectile.spriteDirection, (float)Math.Sin(Main.timeForVisualEffects / 300.0 * 6.2831854820251465) * num20 + 1.5f) * 2f;
						num24 -= 12f;
						num25 -= 12f;
						break;
				}
				Vector2 vector6 = Projectile.Center + Utils.RotatedBy(new Vector2(((float)rectangle.Width * 0.5f + num24) * (float)Projectile.spriteDirection, num25), (double)(Projectile.rotation + num35), default(Vector2));
				list.Add(vector6);
				int num26 = num21;
				int num27 = 1;
				while (num26 < num22 * num21)
				{
					if (num38 != -1 && num38 == num27)
					{
						num34 = num2;
					}
					Vector2 vector7 = Projectile.oldPos[num26];
					if (vector7.X == 0f && vector7.Y == 0f)
					{
						list.Add(vector6);
					}
					else
					{
						vector7 += vector4 + Utils.RotatedBy(new Vector2(((float)rectangle.Width * 0.5f + num24) * (float)Projectile.oldSpriteDirection[num26], num25), (double)(Projectile.oldRot[num26] + num35), default(Vector2));
						vector7 += vector5 * (float)(num27 + 1);
						Vector2 vector8 = vector6 - vector7;
						float num28 = vector8.Length();
						if (num28 > num34)
						{
							vector8 *= num34 / num28;
						}
						vector7 = vector6 - vector8;
						list.Add(vector7);
						vector6 = vector7;
					}
					num26 += num21;
					num27++;
				}
				if (flag)
				{
					Rectangle value5 = value3.Frame();
					for (int num29 = list.Count - 2; num29 >= 0; num29--)
					{
						Vector2 vector9 = list[num29];
						Vector2 v = list[num29 + 1] - vector9;
						float num30 = v.Length();
						if (!(num30 < 2f))
						{
							float rotation2 = v.ToRotation() - (float)Math.PI / 2f;
							Main.EntitySpriteDraw(value3, vector9 - Main.screenPosition, value5, alpha, rotation2, origin2, new Vector2(1f, num30 / (float)value5.Height), (SpriteEffects)0);
						}
					}
				}
				for (int num31 = list.Count - 2; num31 >= 0; num31--)
				{
					Vector2 vector10 = list[num31];
					Vector2 vector2 = list[num31 + 1];
					Vector2 v2 = vector2 - vector10;
					v2.Length();
					float rotation3 = v2.ToRotation() - (float)Math.PI / 2f + num12;
					Main.EntitySpriteDraw(value2, vector2 - Main.screenPosition, rectangle2, alpha, rotation3, origin3, Projectile.scale, effects);
					rectangle2.X -= width;
					if (rectangle2.X < 0)
					{
						int num32 = num4;
						if (flag2)
						{
							num32--;
						}
						rectangle2.X = num32 * width;
					}
				}
			}
			Main.EntitySpriteDraw(value, position, rectangle, alpha, Projectile.rotation + num35, origin, Projectile.scale, effects);
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
			Projectile.timeLeft = 60;
			bool flag = false;
			if (player.CCed || player.noItems)
			{
				flag = true;
			}
			else if (player.inventory[player.selectedItem].shoot != Projectile.type)
			{
				flag = true;
			}
			else if (player.pulley)
			{
				flag = true;
			}
			else if (player.dead)
			{
				flag = true;
			}
			if (!flag)
			{
				Vector2 val = player.Center - Projectile.Center;
				flag = val.Length() > 2000f;
			}
			if (flag)
			{
				Projectile.Kill();
				return;
			}
			float num = 4f;
			float num7 = 500f;
			float num8 = num7 / 2f;
			if (Projectile.owner == Main.myPlayer && Projectile.extraUpdates == 0)
			{
				float num14 = Projectile.ai[0];
				if (Projectile.ai[0] == 0f)
				{
					Projectile.ai[0] = num8;
				}
				float num9 = Projectile.ai[0];
				if (Main.mouseRight)
				{
					num9 -= 5f;
				}
				if (Main.mouseLeft)
				{
					num9 += 5f;
				}
				Projectile.ai[0] = MathHelper.Clamp(num9, num, num7);
				if (num14 != num9)
				{
					Projectile.netUpdate = true;
				}
			}
			if (Projectile.numUpdates == 1)
			{
				Projectile.extraUpdates = 0;
			}
			int num10 = 0;
			float cloudAlpha = Main.cloudAlpha;
			float num11 = 0f;
			if (WorldGen.InAPlaceWithWind(Projectile.position, Projectile.width, Projectile.height))
			{
				num11 = Main.WindForVisuals;
			}
			float num12 = Utils.GetLerpValue(0.2f, 0.5f, Math.Abs(num11), clamped: true) * 0.5f;
			switch (num10)
			{
				case 0:
					{
						Vector2 mouseWorld = Main.MouseWorld;
						mouseWorld = Projectile.Center;
						mouseWorld += new Vector2(num11, (float)Math.Sin(Main.GlobalTimeWrappedHourly) + cloudAlpha * 5f) * 25f;
						Vector2 v = mouseWorld - Projectile.Center;
						v = v.SafeNormalize(Vector2.Zero) * (3f + cloudAlpha * 7f);
						if (num12 == 0f)
						{
							v = Projectile.velocity;
						}
						float num13 = Projectile.Distance(mouseWorld);
						float lerpValue = Utils.GetLerpValue(5f, 10f, num13, clamped: true);
						float y = Projectile.velocity.Y;
						if (num13 > 10f)
						{
							Projectile.velocity = Vector2.Lerp(Projectile.velocity, v, 0.075f * lerpValue);
						}
						Projectile.velocity.Y = y;
						Projectile.velocity.Y -= num12;
						Projectile.velocity.Y += 0.02f + num12 * 0.25f;
						Projectile.velocity.Y = MathHelper.Clamp(Projectile.velocity.Y, -2f, 2f);
						if (Projectile.Center.Y + Projectile.velocity.Y < mouseWorld.Y)
						{
							Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, Projectile.velocity.Y + num12 + 0.01f, 0.75f);
						}
						Projectile.velocity.X *= 0.98f;
						float num2 = Projectile.Distance(vector);
						float num3 = Projectile.ai[0];
						if (num2 > num3)
						{
							Vector2 vector3 = Projectile.DirectionTo(vector);
							float num4 = num2 - num3;
							Projectile.Center += vector3 * num4;
							bool num15 = Vector2.Dot(vector3, Vector2.UnitY) < 0.8f || num12 > 0f;
							Projectile.velocity.Y += vector3.Y * 0.05f;
							if (num15)
							{
								Projectile.velocity.Y -= 0.15f;
							}
							Projectile.velocity.X += vector3.X * 0.2f;
							if (num3 == num && Projectile.owner == Main.myPlayer)
							{
								Projectile.Kill();
								return;
							}
						}
						break;
					}
				case 1:
					{
						Vector2 vector2 = Projectile.DirectionTo(vector);
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, vector2 * 16f, 1f);
						if (Projectile.Distance(vector) < 10f && Projectile.owner == Main.myPlayer)
						{
							Projectile.Kill();
							return;
						}
						break;
					}
			}
			Projectile.timeLeft = 2;
			Vector2 vector4 = Projectile.Center - vector;
			int dir = ((vector4.X > 0f) ? 1 : (-1));
			if (Math.Abs(vector4.X) > Math.Abs(vector4.Y) / 2f)
			{
				player.ChangeDir(dir);
			}
			Vector2 vector5 = Projectile.DirectionTo(vector).SafeNormalize(Vector2.Zero);
			if (num12 == 0f && Projectile.velocity.Y > -0.02f)
			{
				Projectile.rotation *= 0.95f;
			}
			else
			{
				float num5 = (-vector5).ToRotation() + (float)Math.PI / 4f;
				if (Projectile.spriteDirection == -1)
				{
					num5 -= (float)Math.PI / 2f * (float)player.direction;
				}
				Projectile.rotation = num5 + Projectile.velocity.X * 0.05f;
			}
			float num6 = Projectile.velocity.Length();
			Projectile.frame = 0;
			Projectile.spriteDirection = player.direction;
		}
	}
}