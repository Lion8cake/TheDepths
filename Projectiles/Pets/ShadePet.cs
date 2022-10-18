using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Projectiles.Pets
{
	public class ShadePet : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];
			player.zephyrfish = false;
			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			TheDepthsPlayer modPlayer = player.GetModPlayer<TheDepthsPlayer>();
			if (player.dead) {
				modPlayer.ShadePet = false;
			}
			if (modPlayer.ShadePet) {
				Projectile.timeLeft = 2;
			}
		}
	}
}