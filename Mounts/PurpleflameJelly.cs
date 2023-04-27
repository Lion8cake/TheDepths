using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace TheDepths.Mounts
{
	public class PurpleflameJelly : ModMount
	{
		protected class RollercycleSpecificData
		{
			internal static float[] offsets = new float[] { 0, 14, -14 };
		}

		public override void SetStaticDefaults() {
			MountData.heightBoost = 8;
			MountData.runSpeed = 1.5f;
			MountData.dashSpeed = 1f;
			MountData.acceleration = 0.4f;
			MountData.jumpHeight = 4;
			MountData.jumpSpeed = 3f;
			MountData.swimSpeed = 14f;
			MountData.blockExtraJumps = true;
			MountData.flightTimeMax = 0;
			MountData.fatigueMax = 360;
			MountData.usesHover = false;

			MountData.buff = ModContent.BuffType<Buffs.PurpleflameJellyBuff>();

			MountData.spawnDust = ModContent.DustType<Dusts.NightDust>();

			MountData.totalFrames = 4;
			MountData.playerYOffsets = Enumerable.Repeat(16, MountData.totalFrames).ToArray();
			MountData.xOffset = 13;
			MountData.yOffset = 0;
			MountData.playerHeadOffset = 22;
			MountData.bodyFrame = 0;
			MountData.standingFrameCount = 4;
			MountData.standingFrameDelay = 6;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = MountData.standingFrameCount;
			MountData.runningFrameDelay = MountData.standingFrameDelay;
			MountData.runningFrameStart = MountData.standingFrameStart;
			MountData.flyingFrameCount = MountData.standingFrameCount;
			MountData.flyingFrameDelay = MountData.standingFrameDelay;
			MountData.flyingFrameStart = MountData.standingFrameStart;
			MountData.inAirFrameCount = MountData.standingFrameCount; 
			MountData.inAirFrameDelay = MountData.standingFrameDelay; 
			MountData.inAirFrameStart = MountData.standingFrameStart;
			MountData.idleFrameCount = MountData.standingFrameCount;
			MountData.idleFrameDelay = MountData.standingFrameDelay;
			MountData.idleFrameStart = MountData.standingFrameStart;
			MountData.idleFrameLoop = true;
			MountData.swimFrameCount = MountData.standingFrameCount;
			MountData.swimFrameDelay = MountData.standingFrameDelay;
			MountData.swimFrameStart = MountData.standingFrameStart;

			if (!Main.dedServ) {
				MountData.textureWidth = MountData.backTexture.Width() + 20;
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

        public override void UpdateEffects(Player player)
        {
			if (Collision.LavaCollision(player.position, player.width, player.height) || Collision.WetCollision(player.position, player.width, player.height))
			{
				MountData.usesHover = true;
				MountData.flightTimeMax = int.MaxValue;
				MountData.fatigueMax = int.MaxValue;
				MountData.runSpeed = 15f;
				MountData.acceleration = 6f;
			}
			else
			{
				MountData.usesHover = false;
				MountData.flightTimeMax = 0;
				MountData.fatigueMax = 360;
				MountData.runSpeed = 1.5f;
				MountData.acceleration = 0.4f;
			}
			player.buffImmune[ModContent.BuffType<Buffs.MercuryPoisoning>()] = true;
			player.buffImmune[ModContent.BuffType<Buffs.MercuryBoiling>()] = true;
        }
	}
}