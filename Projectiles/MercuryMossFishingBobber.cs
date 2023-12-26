using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheDepths.Projectiles
{
	public class MercuryMossFishingBobber : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
			Projectile.bobber = true;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			//Projectile.light = 1f;

			DrawOriginOffsetY = -8;
		}

		public override bool PreAI()
		{
			if (Main.rand.NextFloat() < 0.05f)
			{
				Dust dust;
				Vector2 position = Projectile.Center - new Vector2(-6, 12);
				dust = Main.dust[Terraria.Dust.NewDust(position, 4, 4, 261, 0f, 0f, 0, new Color(119, 135, 162), 2f)];
				dust.noGravity = true;
			}
			return true;
		}

		public override void AI()
		{
			AI_061_FishingBobber();
		}

		private void AI_061_FishingBobber()
		{
			Player player = Main.player[Projectile.owner];
			bool flag = true;
			Projectile.timeLeft = 60;
			bool flag2 = false;
			if (player.inventory[player.selectedItem].fishingPole == 0 || player.CCed || player.noItems)
			{
				flag2 = true;
			}
			else if (player.inventory[player.selectedItem].shoot != Projectile.type && !flag)
			{
				flag2 = true;
			}
			else if (player.pulley)
			{
				flag2 = true;
			}
			else if (player.dead)
			{
				flag2 = true;
			}
			if (flag2)
			{
				Projectile.Kill();
				return;
			}
			if (Projectile.ai[1] > 0f && Projectile.localAI[1] != 0f)
			{
				Projectile.localAI[1] = 0f;
				if (!Projectile.lavaWet && !Projectile.honeyWet)
				{
					typeof(Projectile).GetMethod("AI_061_FishingBobber_DoASplash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Projectile, new object[] { });
				}
			}
			if (Projectile.ai[0] >= 1f)
			{
				if (Projectile.ai[0] == 2f)
				{
					Projectile.ai[0] += 1f;
					SoundEngine.PlaySound(in SoundID.Item17, Projectile.position);
					if (!Projectile.lavaWet && !Projectile.honeyWet)
					{
						typeof(Projectile).GetMethod("AI_061_FishingBobber_DoASplash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Projectile, new object[] { });
					}
				}
				if (Projectile.localAI[0] < 100f)
				{
					Projectile.localAI[0] += 1f;
				}
				if (Projectile.frameCounter == 0)
				{
					Projectile.frameCounter = 1;
					typeof(Projectile).GetMethod("ReduceRemainingChumsInPool", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Projectile, new object[] { });
				}
				Projectile.tileCollide = false;
				int num = 10;
				Vector2 vector = new(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num3 = player.position.X + (float)(player.width / 2) - vector.X;
				float num4 = player.position.Y + (float)(player.height / 2) - vector.Y;
				float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
				if (num5 > 3000f)
				{
					Projectile.Kill();
				}
				num5 = 15.9f / num5;
				num3 *= num5;
				num4 *= num5;
				Projectile.velocity.X = (Projectile.velocity.X * (float)(num - 1) + num3) / (float)num;
				Projectile.velocity.Y = (Projectile.velocity.Y * (float)(num - 1) + num4) / (float)num;
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
				if (Main.myPlayer == Projectile.owner)
				{
					Rectangle hitbox = Projectile.Hitbox;
					if (hitbox.Intersects(player.Hitbox))
					{
						Projectile.Kill();
					}
				}
				return;
			}
			bool flag3 = false;
			Vector2 vector2 = new(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
			float num6 = player.position.X + (float)(player.width / 2) - vector2.X;
			float num7 = player.position.Y + (float)(player.height / 2) - vector2.Y;
			Projectile.rotation = (float)Math.Atan2(num7, num6) + 1.57f;
			if ((float)Math.Sqrt(num6 * num6 + num7 * num7) > 900f)
			{
				Projectile.ai[0] = 1f;
			}
			if (Projectile.wet)
			{
				if (Projectile.shimmerWet)
				{
					if (Main.myPlayer == Projectile.owner)
					{
						Main.player[Projectile.owner].AddBuff(353, 60);
					}
					if (Projectile.localAI[2] == 0f)
					{
						Projectile.localAI[2] = 1f;
						SoundEngine.PlaySound(SoundID.Item19, new Vector2((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y)/*, 2*/);
					}
				}
				Projectile.rotation = 0f;
				Projectile.velocity.X *= 0.9f;
				int num8 = (int)(Projectile.Center.X + (float)((Projectile.width / 2 + 8) * Projectile.direction)) / 16;
				int num9 = (int)(Projectile.Center.Y / 16f);
				_ = Projectile.position.Y / 16f;
				int num10 = (int)((Projectile.position.Y + (float)Projectile.height) / 16f);
				if (Projectile.velocity.Y > 0f)
				{
					Projectile.velocity.Y *= 0.5f;
				}
				num8 = (int)(Projectile.Center.X / 16f);
				num9 = (int)(Projectile.Center.Y / 16f);
				float num2 = (float)typeof(Projectile).GetMethod("AI_061_FishingBobber_GetWaterLine", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Projectile, new object[] { num8, num9 });
				if (Projectile.Center.Y > num2)
				{
					Projectile.velocity.Y -= 0.1f;
					if (Projectile.velocity.Y < -8f)
					{
						Projectile.velocity.Y = -8f;
					}
					if (Projectile.Center.Y + Projectile.velocity.Y < num2)
					{
						Projectile.velocity.Y = num2 - Projectile.Center.Y;
					}
				}
				else
				{
					Projectile.velocity.Y = num2 - Projectile.Center.Y;
				}
				if ((double)Projectile.velocity.Y >= -0.01 && (double)Projectile.velocity.Y <= 0.01)
				{
					flag3 = true;
				}
			}
			else
			{
				if (Projectile.velocity.Y == 0f)
				{
					Projectile.velocity.X *= 0.95f;
				}
				Projectile.velocity.X *= 0.98f;
				Projectile.velocity.Y += 0.2f;
				if (Projectile.velocity.Y > 15.9f)
				{
					Projectile.velocity.Y = 15.9f;
				}
			}
			if (Main.myPlayer == Projectile.owner && player.GetFishingConditions().BaitItemType == 2673)
			{
				player.displayedFishingInfo = Language.GetTextValue("GameUI.FishingWarning");
			}
			if (Projectile.ai[1] != 0f)
			{
				flag3 = true;
			}
			if (!flag3)
			{
				return;
			}
			if (Projectile.ai[1] == 0f && Main.myPlayer == Projectile.owner)
			{
				int finalFishingLevel = player.GetFishingConditions().FinalFishingLevel;
				if (Main.rand.Next(300) < finalFishingLevel)
				{
					Projectile.localAI[1] += Main.rand.Next(1, 3);
				}
				Projectile.localAI[1] += finalFishingLevel / 30;
				Projectile.localAI[1] += Main.rand.Next(1, 3);
				if (Main.rand.Next(60) == 0)
				{
					Projectile.localAI[1] += 60f;
				}
				if (Projectile.localAI[1] > 660f)
				{
					Projectile.localAI[1] = 0f;
					Projectile.FishingCheck();
				}
			}
			else if (Projectile.ai[1] < 0f)
			{
				if (Projectile.velocity.Y == 0f || (Projectile.honeyWet && Math.Abs(Projectile.velocity.Y) <= 0.01f))
				{
					Projectile.velocity.Y = (float)Main.rand.Next(100, 500) * 0.015f;
					Projectile.velocity.X = (float)Main.rand.Next(-100, 101) * 0.015f;
					Projectile.wet = false;
					Projectile.lavaWet = false;
					Projectile.honeyWet = false;
				}
				Projectile.ai[1] += Main.rand.Next(1, 5);
				if (Projectile.ai[1] >= 0f)
				{
					Projectile.ai[1] = 0f;
					Projectile.localAI[1] = 0f;
					Projectile.netUpdate = true;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = oldVelocity.X * -0.3f;
			}
			if (Projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 1f)
			{
				Projectile.velocity.Y = oldVelocity.Y * -0.3f;
			}
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			SpriteEffects dir = 0;
			if (Projectile.spriteDirection == -1)
			{
				dir = (SpriteEffects)1;
			}
			if (Projectile.ai[0] <= 1f)
			{
				Texture2D glowMask = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
				Color color = Color.White;
				Main.EntitySpriteDraw(glowMask, new Vector2(Projectile.position.X - Main.screenPosition.X + (glowMask.Width - Projectile.width) * 0.5f + Projectile.width * 0.5f, Projectile.position.Y - Main.screenPosition.Y + (Projectile.height / 2) + Projectile.gfxOffY), (Rectangle?)new Rectangle(0, 0, glowMask.Width, glowMask.Height), Projectile.GetAlpha(color), Projectile.rotation, new Vector2((glowMask.Width - Projectile.width) * 0.5f + Projectile.width * 0.5f, (Projectile.height / 2 + 8)), Projectile.scale, dir, 0f);
			}
		}
	}
}