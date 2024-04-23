using Terraria.Audio;
using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TheDepths.Items.Weapons;

namespace TheDepths.Projectiles
{
	public class MercuryExplosion : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.scale = 1.1f;
		}

        public override void AI()
        {
			Projectile.tileCollide = false;
			Projectile.ai[0] += 1f;
			if (Projectile.ai[1] >= 1f)
			{
				Projectile.ai[0] += 2f;
			}
			float num809 = 260f;
			if (Projectile.ai[0] > num809)
			{
				Projectile.Kill();
				Projectile.ai[0] = num809;
				return;
			}
			float fromValue = Projectile.ai[0] / num809;
			Projectile.Opacity = Utils.Remap(fromValue, 0f, 0.3f, 0f, 1f) * Utils.Remap(fromValue, 0.3f, 1f, 1f, 0f) * 0.7f;
			Projectile.localAI[0] += Projectile.direction;
			Projectile.rotation = (float)Projectile.whoAmI * 0.4002029f + Projectile.localAI[0] * ((float)Math.PI * 2f) / 480f;
			Projectile.velocity *= 0.96f;
			Rectangle rectangle5 = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
			for (int num810 = 0; num810 < 1000; num810++)
			{
				if (num810 == Projectile.whoAmI)
				{
					continue;
				}
				Projectile projectile3 = Main.projectile[num810];
				if (!projectile3.active || projectile3.type < 511 || projectile3.type > 513)
				{
					continue;
				}
				Rectangle value11 = new Rectangle((int)projectile3.position.X, (int)projectile3.position.Y, projectile3.width, projectile3.height);
				if (!rectangle5.Intersects(value11))
				{
					continue;
				}
				Vector2 vector100 = projectile3.Center - Projectile.Center;
				if (vector100 == Vector2.Zero)
				{
					if (num810 < Projectile.whoAmI)
					{
						vector100.X = -1f;
						vector100.Y = 1f;
					}
					else
					{
						vector100.X = 1f;
						vector100.Y = -1f;
					}
				}
				Vector2 vector101 = vector100.SafeNormalize(Vector2.UnitX) * 0.005f;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity - vector101, 0.6f);
				projectile3.velocity = Vector2.Lerp(projectile3.velocity, projectile3.velocity + vector101, 0.6f);
			}
			Vector2 vector102 = Projectile.velocity.SafeNormalize(Vector2.Zero);
			Vector2 pos = Projectile.Center + vector102 * 16f;
			if (Collision.IsWorldPointSolid(pos))
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity - vector102 * 1f, 0.5f);
			}
			if (Projectile.localAI[1] > 0f)
			{
				Projectile.localAI[1]--;
			}
			if (!(Projectile.localAI[1] <= 0f))
			{
				return;
			}
			Projectile.localAI[1] = 15f;
			if (Main.netMode != NetmodeID.Server)
			{
				Player player = Main.player[Projectile.owner];
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Player target = Main.player[i];
					if (target.active && !target.DeadOrGhost && target != player)
					{
						if (target.hostile && player.hostile && (target.team != player.team || target.team == 0))
						{
							if (!target.buffImmune[ModContent.BuffType<Buffs.MercuryContagion>()] && target.Hitbox.Intersects(Projectile.Hitbox))
							{
								target.AddBuff(ModContent.BuffType<Buffs.MercuryContagion>(), 60 * 4);
							}
						}
					}
				}
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}
			for (int num814 = 0; num814 < 200; num814++)
			{
				NPC nPC12 = Main.npc[num814];
				if (nPC12.active && !nPC12.buffImmune[ModContent.BuffType<Buffs.MercuryContagion>()] && nPC12.Hitbox.Intersects(Projectile.Hitbox))
				{
					nPC12.AddBuff(ModContent.BuffType<Buffs.MercuryContagion>(), 60 * 4);
				}
			}
		}
    }
}