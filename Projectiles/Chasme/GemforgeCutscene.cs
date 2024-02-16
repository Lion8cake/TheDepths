using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using System;
using Terraria.GameContent.Events;
using Terraria.Audio;
using Terraria.DataStructures;
using TheDepths.Buffs;
using TheDepths.Tiles;
using TheDepths.NPCs;
using Terraria.Localization;
using Terraria.Chat;
using TheDepths.NPCs.Chasme;
using System.IO;
using static Humanizer.In;
using static System.Net.WebRequestMethods;
using System.Diagnostics;

namespace TheDepths.Projectiles.Chasme
{
	public class GemforgeCutscene : ModProjectile
	{
		private int ScreenflashTimer = 82;

		public override void SetStaticDefaults()
		{
			NPCID.Sets.MPAllowedEnemies[ModContent.NPCType<ChasmeHeart>()] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.timeLeft = 300; //5 seconds (60 x 5)
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Player player = Main.LocalPlayer;
			for (int dust = 0; dust < 6; dust++)
			{
				Vector2 vector311 = Projectile.Center + new Vector2(0f, -20f);
				float num1589 = 0.99f;
				if (Projectile.timeLeft >= 160f)
				{
					num1589 = 0.79f % 2;
				}
				if (Projectile.timeLeft >= 180f)
				{
					num1589 = 0.58f % 2;
				}
				if (Projectile.timeLeft >= 220f)
				{
					num1589 = 0.43f % 2;
				}
				if (Projectile.timeLeft >= 260f)
				{
					num1589 = 0.33f % 2;
				}
				if (Projectile.timeLeft >= 300f)
				{
					num1589 = 1f % 2;
				}
				for (int num1590 = 0; num1590 < 9; num1590++)
				{
					if (!(Main.rand.NextFloat() < num1589))
					{
						float num1591 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
						float num1592 = Main.rand.NextFloat();
						Vector2 vector312 = vector311 + num1591.ToRotationVector2() * (110f + 600f * num1592);
						Vector2 vector313 = (num1591 - (float)Math.PI).ToRotationVector2() * (14f + 0f * Main.rand.NextFloat() + 8f * num1592);
						Dust dust26 = Dust.NewDustPerfect(vector312, 264, vector313);
						dust26.scale = 0.9f;
						dust26.fadeIn = 1.15f + num1592 * 0.3f;
						dust26.color = new Color(1f, 1f, 1f, num1589) * (1f - num1589);
						dust26.noGravity = true;
						dust26.noLight = true;
					}
				}
			}
			if (Projectile.timeLeft <= 225f && ScreenflashTimer > 0)
			{
				ScreenflashTimer--;
			}
			if (ScreenflashTimer >= 1f && ScreenflashTimer < 82)
			{
				MoonlordDeathDrama.RequestLight(((ScreenflashTimer + 920) - 480f) / 120f, Projectile.Center);
			}
			if (Projectile.timeLeft == 150f)
			{
				Main.player[Main.myPlayer].GetModPlayer<TheDepthsPlayer>().tremblingDepthsScreenshakeTimer = 150;
			}
			if (Projectile.timeLeft == 150)
			{
				Gemforge.RubyRelicIsOnForge = 1;
				SoundEngine.PlaySound(SoundID.DD2_BetsyDeath, Projectile.position);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (Projectile.position.Y / 16f < (float)(Main.maxTilesY - 205) || Main.wofNPCIndex >= 0 || Main.netMode == 1)
					{
						return;
					}
					Player.FindClosest(Projectile.position, 16, 16);
					int num = 1;
					if (Projectile.position.X / 16f > (float)(Main.maxTilesX / 2))
					{
						num = -1;
					}
					bool flag = false;
					int num3 = (int)Projectile.position.X;
					int targetPlayerIndex = 0;
					while (!flag)
					{
						flag = true;
						for (int i = 0; i < 255; i++)
						{
							if (Main.player[i].active && Main.player[i].position.X > (float)(num3 - 1200) && Main.player[i].position.X < (float)(num3 + 1200))
							{
								num3 -= num * 16;
								flag = false;
								targetPlayerIndex = i;
							}
						}
						if (num3 / 16 < 20 || num3 / 16 > Main.maxTilesX - 20)
						{
							flag = true;
						}
					}
					int num4 = (int)Projectile.position.Y;
					int num5 = num3 / 16;
					int num6 = num4 / 16;
					int num7 = 0;
					int num8 = 1000;
					if (!WorldGen.InWorld(num5, num6, 2) || WorldGen.SolidTile(num5, num6) || Main.tile[num5, num6 - num7].LiquidAmount >= 100)
					{
						while (true)
						{
							num8--;
							if (num8 <= 0)
							{
								break;
							}
							try
							{
								if (WorldGen.InWorld(num5, num6 - num7, 2) && !WorldGen.SolidTile(num5, num6 - num7) && Main.tile[num5, num6 - num7].LiquidAmount < 100)
								{
									num6 -= num7;
									break;
								}
								if (WorldGen.InWorld(num5, num6 + num7, 2) && !WorldGen.SolidTile(num5, num6 + num7) && Main.tile[num5, num6 + num7].LiquidAmount < 100)
								{
									num6 += num7;
									break;
								}
								num7++;
							}
							catch
							{
								break;
							}
						}
					}
					int num9 = Main.UnderworldLayer + 10;
					int num10 = num9 + 70;
					if (num6 < num9)
					{
						num6 = num9;
					}
					if (num6 > num10)
					{
						num6 = num10;
					}
					num4 = num6 * 16;
					NPC.NewNPC(NPC.GetBossSpawnSource(targetPlayerIndex), num3, num4, ModContent.NPCType<ChasmeHeart>());
				}
				if (Main.netMode == 0)
				{
					Main.NewText(Language.GetTextValue("Announcement.HasAwoken", Language.GetTextValue("Mods.TheDepths.NPCs.ChasmeBody.DisplayName")), 175, 75);
				}
				else if (Main.netMode == 2)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", Language.GetTextValue("Mods.TheDepths.NPCs.ChasmeBody.DisplayName")), new Color(175, 75, 255));
				}
			}
			if (Projectile.timeLeft <= 140) 
            {
				for (int nPC = 0; nPC < Main.maxNPCs; nPC++)
				{
					NPC npc = Main.npc[nPC];
					if (npc.active && npc.type == ModContent.NPCType<Geomancer>())
					{
						npc.life = -1;
					}
				}
			}
		}

		/*public override void Kill(int timeLeft)
		{
			Gemforge.RubyRelicIsOnForge = true;
		}*/

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(ScreenflashTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			ScreenflashTimer = reader.ReadInt32();
		}
	}
}