using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Pets.ShadePet
{
	public class ShadePetProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
				.WithOffset(-25, -30f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);
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

			if (!player.dead && player.HasBuff(ModContent.BuffType<ShadePetBuff>())) {
				Projectile.timeLeft = 2;
			}
		}
	}
}
