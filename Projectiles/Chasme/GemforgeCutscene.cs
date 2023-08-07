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

namespace TheDepths.Projectiles.Chasme
{
	public class GemforgeCutscene : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			NPCID.Sets.MPAllowedEnemies[NPCID.WallofFlesh] = true;
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
			Vector2 vector311 = Projectile.Center + new Vector2(0f, -20f);
			float num1589 = 0.99f;
			if (Math.Abs(player.position.ToTileCoordinates().Y) >= Main.maxTilesY - 210)
            {
				player.AddBuff(ModContent.BuffType<RelicsCurse>(), 5);
			}
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
				Geomancer.PraiseTheRelic = true;
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
			if (Projectile.timeLeft == 225f)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GemforgeScreenflash>(), 0, 0f, Main.myPlayer);
			}
			if (Projectile.timeLeft == 150f)
			{
				Main.player[Main.myPlayer].GetModPlayer<TheDepthsPlayer>().tremblingDepthsScreenshakeTimer = 150;
			}
			if (Projectile.timeLeft == 150)
			{
				Geomancer.TheRelicMadeHimExplode = true;
				Gemforge.RubyRelicIsOnForge = 1;
				SoundEngine.PlaySound(SoundID.NPCDeath10, Projectile.position);
				if (player.whoAmI == Main.myPlayer)
				{
					if (Projectile.Center.X < (Main.maxTilesX * 16) / 2 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						Main.NewText("[c/AF4BFF:Wall of Flesh has awoken!]");
						NPC.NewNPC(new EntitySource_Misc(""), (int)(Projectile.Center.X - 1500f), (int)Projectile.Center.Y, NPCID.WallofFlesh, 0, 1f, 0f, 0f, player.whoAmI);
					}
					else if (Projectile.Center.X > (Main.maxTilesX * 16) / 2 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						Main.NewText("[c/AF4BFF:Wall of Flesh has awoken!]");
						NPC.NewNPC(new EntitySource_Misc(""), (int)(Projectile.Center.X + 1500f), (int)Projectile.Center.Y, NPCID.WallofFlesh, 0, 1f, 0f, 0f, player.whoAmI);
					}
					else
                    {
						NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: NPCID.WallofFlesh);
					}
				}
			}
			if (Projectile.timeLeft == 140)
            {
				Geomancer.TheRelicMadeHimExplode = false;
			}
		}

		/*public override void Kill(int timeLeft)
		{
			Gemforge.RubyRelicIsOnForge = true;
		}*/
	}
}