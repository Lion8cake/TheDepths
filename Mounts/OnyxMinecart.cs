using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheDepths.Buffs;
using Terraria.Graphics.Shaders;
using static Terraria.Mount;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheDepths.Mounts
{
	public class OnyxMinecart : ModMount
	{
		public override void SetStaticDefaults()
		{
			MountID.Sets.Cart[Type] = true;

			SetAsMinecart(
				MountData,
				Mod.Find<ModBuff>($"{nameof(OnyxMinecartBuff)}_Left").Type,
				Mod.Find<ModBuff>($"{nameof(OnyxMinecartBuff)}_Right").Type,
				MountData.frontTexture
			);

			MountData.spawnDust = 21;
			MountData.delegations.MinecartDust = DelegateMethods.Minecart.Sparks;
			MountData.delegations.MinecartLandingSound = DelegateMethods.Minecart.LandingSound;
			MountData.delegations.MinecartBumperSound = DelegateMethods.Minecart.BumperSound;
		}

		public static void Sparks(Vector2 dustPosition)
		{
			dustPosition += new Vector2((Main.rand.Next(2) == 0) ? 13 : (-13), 0f).RotatedBy(DelegateMethods.Minecart.rotation);
			int num = Dust.NewDust(dustPosition, 1, 1, ModContent.DustType<Dusts.ShadowflameEmber>(), Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
			Main.dust[num].noGravity = true;
			Main.dust[num].fadeIn = Main.dust[num].scale + 1f + 0.01f * (float)Main.rand.Next(0, 51);
			Main.dust[num].noGravity = true;
			Main.dust[num].velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
			Main.dust[num].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
			Main.dust[num].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
			Main.dust[num].position.Y -= 4f;
			if (Main.rand.Next(3) != 0)
			{
				Main.dust[num].noGravity = false;
			}
			else
			{
				Main.dust[num].scale *= 0.6f;
			}
		}

		public override void UpdateEffects(Player player)
		{
			if (Main.rand.NextBool(10))
			{
				Vector2 randomOffset = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(22f, 10f);
				Vector2 directionOffset = new Vector2(0f, 10f) * player.Directions;
				Vector2 position = player.Center + directionOffset + randomOffset;
				position = player.RotatedRelativePoint(position);
				Dust dust = Dust.NewDustPerfect(position, ModContent.DustType<Dusts.GemOnyxDust>());
				dust.noGravity = true;
				dust.fadeIn = 0.6f;
				dust.scale = 0.4f;
				dust.velocity *= 0.25f;
				dust.shader = GameShaders.Armor.GetSecondaryShader(player.cMinecart, player);
			}
		}

		private static void SetAsMinecart(MountData newMount, int buffToLeft, int buffToRight, Asset<Texture2D> texture, int verticalOffset = 0, int playerVerticalOffset = 0)
		{
			newMount.Minecart = true;
			newMount.delegations = new MountDelegatesData();
			newMount.delegations.MinecartDust = DelegateMethods.Minecart.Sparks;
			newMount.spawnDust = 213;
			newMount.buff = buffToLeft;
			newMount.extraBuff = buffToRight;
			newMount.heightBoost = 10;
			newMount.flightTimeMax = 0;
			newMount.fallDamage = 1f;
			newMount.runSpeed = 13f;
			newMount.dashSpeed = 13f;
			newMount.acceleration = 0.04f;
			newMount.jumpHeight = 15;
			newMount.jumpSpeed = 5.15f;
			newMount.blockExtraJumps = true;
			newMount.totalFrames = 3;
			int[] array = new int[newMount.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 8 - verticalOffset + playerVerticalOffset;
			}
			newMount.playerYOffsets = array;
			newMount.xOffset = 1;
			newMount.bodyFrame = 3;
			newMount.yOffset = 13 + verticalOffset;
			newMount.playerHeadOffset = 14;
			newMount.standingFrameCount = 1;
			newMount.standingFrameDelay = 12;
			newMount.standingFrameStart = 0;
			newMount.runningFrameCount = 3;
			newMount.runningFrameDelay = 12;
			newMount.runningFrameStart = 0;
			newMount.flyingFrameCount = 0;
			newMount.flyingFrameDelay = 0;
			newMount.flyingFrameStart = 0;
			newMount.inAirFrameCount = 0;
			newMount.inAirFrameDelay = 0;
			newMount.inAirFrameStart = 0;
			newMount.idleFrameCount = 0;
			newMount.idleFrameDelay = 0;
			newMount.idleFrameStart = 0;
			newMount.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				newMount.backTexture = Asset<Texture2D>.Empty;
				newMount.backTextureExtra = Asset<Texture2D>.Empty;
				newMount.frontTexture = texture;
				newMount.frontTextureExtra = Asset<Texture2D>.Empty;
				newMount.textureWidth = newMount.frontTexture.Width();
				newMount.textureHeight = newMount.frontTexture.Height();
			}
		}
	}
}
