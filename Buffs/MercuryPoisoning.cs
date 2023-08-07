using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Buffs
{
	public class MercuryPoisoning : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Mercury Radiation");
			Description.SetDefault("Losing life and slowed movement");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<TheDepthsPlayer>().merPoison = true;
			int extra = player.buffTime[buffIndex] / 60;
			player.moveSpeed -= 0.5f;
		}
	}
}
