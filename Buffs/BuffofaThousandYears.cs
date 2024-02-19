using TheDepths.Projectiles.Summons;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Buffs
{
	public class BuffofaThousandYears : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			TheDepthsPlayer modPlayer = player.GetModPlayer<TheDepthsPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<MiniChasme>()] > 0) {
				modPlayer.miniChasme = true;
			}
			if (!modPlayer.miniChasme) {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else {
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}