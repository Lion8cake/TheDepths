using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;
using TheDepths.Projectiles;
using TheDepths.Dusts;
using Terraria.ID;

namespace TheDepths.Mounts
{
	public class NightmareHorse : ModMount
	{
		public override void SetStaticDefaults() {
			MountData.spawnDust = ModContent.DustType<SmashedHeartDust>();
			MountData.buff = ModContent.BuffType<Buffs.NightmareHorseBuff>();
			MountData.heightBoost = 34;
			MountData.flightTimeMax = 0;
			MountData.fallDamage = 0.2f;
			MountData.runSpeed = 4f;
			MountData.dashSpeed = 12f;
			MountData.acceleration = 0.3f;
			MountData.jumpHeight = 10;
			MountData.jumpSpeed = 8.01f;
			MountData.totalFrames = 16;
			int[] array = new int[MountData.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 30;
			}
			array[3] += 2;
			array[4] += 2;
			array[7] += 2;
			array[8] += 2;
			array[12] += 2;
			array[13] += 2;
			array[15] += 4;
			MountData.playerYOffsets = array;
			MountData.xOffset = 5;
			MountData.bodyFrame = 3;
			MountData.yOffset = 1;
			MountData.playerHeadOffset = 34;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 7;
			MountData.runningFrameDelay = 15;
			MountData.runningFrameStart = 1;
			MountData.dashingFrameCount = 6;
			MountData.dashingFrameDelay = 40;
			MountData.dashingFrameStart = 9;
			MountData.flyingFrameCount = 6;
			MountData.flyingFrameDelay = 6;
			MountData.flyingFrameStart = 1;
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 12;
			MountData.inAirFrameStart = 15;
			MountData.idleFrameCount = 0;
			MountData.idleFrameDelay = 0;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = false;
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				MountData.backTextureGlow = ModContent.Request<Texture2D>("TheDepths/Mounts/NightmareHorse_Glow");
				MountData.backTextureExtra = Asset<Texture2D>.Empty;
				MountData.frontTexture = Asset<Texture2D>.Empty;
				MountData.frontTextureExtra = Asset<Texture2D>.Empty;
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			int mountState = 1;
			bool flag5 = Math.Abs(velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f;
			if (state == 1)
			{
				bool flag6 = false;
				if (flag5)
				{
					mountState = 5;
					if (mountedPlayer.mount._frameExtra < 6)
					{
						flag6 = true;
					}
					mountedPlayer.mount._frameExtra++;
				}
				else
				{
					mountedPlayer.mount._frameExtra = 0;
				}
				if (flag6)
				{
					int type = ModContent.DustType<NightmareEmberDust>();
					Vector2 vector9 = mountedPlayer.Center + new Vector2((float)(mountedPlayer.width * mountedPlayer.direction), 0f);
					Vector2 vector10 = new(40f, 30f);
					float num23 = (float)Math.PI * 2f * Main.rand.NextFloat();
					for (float num24 = 0f; num24 < 14f; num24 += 1f)
					{
						Dust dust5 = Main.dust[Dust.NewDust(vector9, 0, 0, type)];
						Vector2 vector2 = Vector2.UnitY.RotatedBy(num24 * ((float)Math.PI * 2f) / 14f + num23);
						vector2 *= 0.2f * (float)mountedPlayer.mount._frameExtra;
						dust5.position = vector9 + vector2 * vector10;
						dust5.velocity = vector2 + new Vector2(mountedPlayer.mount.RunSpeed - (float)(Math.Sign(velocity.X) * mountedPlayer.mount._frameExtra * 2), 0f);
						dust5.noGravity = true;
						dust5.noLightEmittence = true;
						dust5.scale = 1f + Main.rand.NextFloat() * 0.8f;
						dust5.fadeIn = Main.rand.NextFloat() * 2f;
						dust5.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
					}
				}
			}
			if (flag5 && velocity.Y == 0f)
			{
				int num25 = (int)mountedPlayer.Center.X / 16;
				int num26 = (int)(mountedPlayer.position.Y + (float)mountedPlayer.height - 1f) / 16;
				Tile tile = Main.tile[num25, num26 + 1];
				if (tile != null && tile.HasTile && tile.LiquidType == 0 && WorldGen.SolidTileAllowBottomSlope(num25, num26 + 1))
				{
					if (Main.rand.NextBool(3))
					{
						Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(mountedPlayer.Center.X, mountedPlayer.Center.Y + mountedPlayer.height / 2 - 5), new Vector2(0), ModContent.ProjectileType<ShadowflameEmber1>(), 0, 0);
					}
					else if (Main.rand.NextBool(3))
					{
						Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(mountedPlayer.Center.X, mountedPlayer.Center.Y + mountedPlayer.height / 2 - 5), new Vector2(0), ModContent.ProjectileType<ShadowflameEmber2>(), 0, 0);
					}
					else
					{
						Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(mountedPlayer.Center.X, mountedPlayer.Center.Y + mountedPlayer.height / 2 - 5), new Vector2(0), ModContent.ProjectileType<ShadowflameEmber3>(), 0, 0);
					}
				}
			}

			if (mountState == 5)
			{
				float num27 = Math.Abs(velocity.X);
				mountedPlayer.mount._frameCounter += num27;
				if (num27 >= 0f)
				{
					if (mountedPlayer.mount._frameCounter > (float)mountedPlayer.mount._data.dashingFrameDelay)
					{
						mountedPlayer.mount._frameCounter -= mountedPlayer.mount._data.dashingFrameDelay;
						mountedPlayer.mount._frame++;
					}
					if (mountedPlayer.mount._frame < mountedPlayer.mount._data.dashingFrameStart || mountedPlayer.mount._frame >= mountedPlayer.mount._data.dashingFrameStart + mountedPlayer.mount._data.dashingFrameCount)
					{
						mountedPlayer.mount._frame = mountedPlayer.mount._data.dashingFrameStart;
					}
				}
				else
				{
					if (mountedPlayer.mount._frameCounter < 0f)
					{
						mountedPlayer.mount._frameCounter += mountedPlayer.mount._data.dashingFrameDelay;
						mountedPlayer.mount._frame--;
					}
					if (mountedPlayer.mount._frame < mountedPlayer.mount._data.dashingFrameStart || mountedPlayer.mount._frame >= mountedPlayer.mount._data.dashingFrameStart + mountedPlayer.mount._data.dashingFrameCount)
					{
						mountedPlayer.mount._frame = mountedPlayer.mount._data.dashingFrameStart + mountedPlayer.mount._data.dashingFrameCount - 1;
					}
				}
				return false;
			}
			return true;
		}

		public override void UpdateEffects(Player player)
        {
			player.GetJumpState<NightmareHorseJump>().Enable();
			if (Math.Abs(player.velocity.X) > player.mount.DashSpeed - player.mount.RunSpeed / 2f)
			{
				player.noKnockback = true;
			}
		}
	}

	public class NightmareHorseJump : ExtraJump
	{
		public override Position GetDefaultPosition() => new After(GoatMount);

		public override float GetDurationMultiplier(Player player)
		{
			return 2f;
		}

		public override void UpdateHorizontalSpeeds(Player player)
		{
			player.runAcceleration *= 3f;
			player.maxRunSpeed *= 1.5f;
		}

		public override void OnStarted(Player player, ref bool playSound)
		{
			Vector2 center2 = player.Center;
			Vector2 vector4 = new(50f, 20f);
			float num12 = (float)Math.PI * 2f * Main.rand.NextFloat();
			for (int i = 0; i < 5; i++)
			{
				for (float num13 = 0f; num13 < 14f; num13 += 1f)
				{
					Dust obj = Main.dust[Dust.NewDust(center2, 0, 0, ModContent.DustType<NightmareEmberDust>())];
					Vector2 vector5 = Vector2.UnitY.RotatedBy(num13 * ((float)Math.PI * 2f) / 14f + num12);
					vector5 *= 0.2f * (float)i;
					obj.position = center2 + vector5 * vector4;
					obj.velocity = vector5 + new Vector2(0f, player.gravDir * 4f);
					obj.noGravity = true;
					obj.scale = 1f + Main.rand.NextFloat() * 0.8f;
					obj.fadeIn = Main.rand.NextFloat() * 2f;
					obj.shader = GameShaders.Armor.GetSecondaryShader(player.cMount, player);
				}
			}
		}
	}
}