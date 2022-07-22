using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Pets.ShadePet
{
	public class ShadePetBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Shade Pet");
			Description.SetDefault(@"""A Shade is floating around with you!""");

			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.buffTime[buffIndex] = 18000;

			int projType = ModContent.ProjectileType<ShadePetProjectile>();

			if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0) {
				var entitySource = player.GetSource_Buff(buffIndex);
				
				Projectile.NewProjectile(entitySource, player.Center, Vector2.Zero, projType, 0, 0f, player.whoAmI);
			}
		}
	}
}
