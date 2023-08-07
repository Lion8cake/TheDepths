using TheDepths.Projectiles.Summons;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Buffs
{
public class LivingShadowSummonBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Silhouette");
		Description.SetDefault("A figure similar to yourself will attack enemys for you.");
		Main.buffNoTimeDisplay[Type] = true;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex) {
			TheDepthsPlayer modPlayer = player.GetModPlayer<TheDepthsPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summons.LivingShadowSummonProj>()] > 0) {
				modPlayer.livingShadow = true;
			}
			if (!modPlayer.livingShadow) {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else {
				player.buffTime[buffIndex] = 18000;
			}
		}
}
}