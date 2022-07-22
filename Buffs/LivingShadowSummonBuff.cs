using TheDepths.Projectiles.Summons;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Buffs
{
	public class LivingShadowSummonBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Silhouette");
		    Description.SetDefault("A figure similar to yourself will attack enemys for you.");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<LivingShadowSummonProj>()] > 0) {
				player.buffTime[buffIndex] = 18000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}