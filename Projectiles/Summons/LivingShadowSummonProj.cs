using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheDepths;
using TheDepths.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using TheDepths.Dusts;
using TheDepths.Projectiles.Summons;
using TheDepths.Buffs;
using Terraria.DataStructures;

namespace TheDepths.Projectiles.Summons
{
	public class LivingShadowSummonProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silhouette");
			Main.projFrames[Projectile.type] = 15;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			//ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
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
			AIType = 394;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}
		
		public override void AI() {
			Player owner = Main.player[Projectile.owner];

			if (!CheckActive(owner)) {
				return;
			}
		}
		
		private bool CheckActive(Player owner) {
			if (owner.dead || !owner.active) {
				owner.ClearBuff(ModContent.BuffType<LivingShadowSummonBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<LivingShadowSummonBuff>())) {
				Projectile.timeLeft = 2;
			}

			return true;
		}
		
		public override bool MinionContactDamage() {
			return true;
		}
	}
}
