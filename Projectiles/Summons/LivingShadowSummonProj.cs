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

namespace TheDepths.Projectiles.Summons
{
    public class LivingShadowSummonProj : ModProjectile
{
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Silhouette");
		Main.projFrames[Projectile.type] = 15;
		ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
	}

	public override void SetDefaults()
	{
		Projectile.CloneDefaults(394);
		Projectile.width = 30;
		Projectile.height = 46;
		Projectile.aiStyle = 67;
		Projectile.friendly = true;
		Projectile.scale = 1f;
		Projectile.timeLeft = 18000;
		Projectile.penetrate = -1;
		Projectile.timeLeft *= 5;
		Projectile.minionSlots = 1f;
		DrawOriginOffsetY = -6;
		AIType = 394;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
	}

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		    #region Active check
			if (player.dead || !player.active) {
				player.ClearBuff(ModContent.BuffType<LivingShadowSummonBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<LivingShadowSummonBuff>())) {
				Projectile.timeLeft = 2;
			}
			#endregion
		if (Projectile.localAI[0] >= 800f)
		{
			Projectile.localAI[0] = 0f;
		}
		if (Vector2.Distance(player.Center, Projectile.Center) > 600f)
		{
			Projectile.position.X = player.position.X;
			Projectile.position.Y = player.position.Y;
		}
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
	{
		fallThrough = false;
		return true;
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		return false;
	}
}
}
